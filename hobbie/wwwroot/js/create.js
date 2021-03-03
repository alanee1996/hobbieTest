var createFunction = {
    data: {
        hobbies: []
    },
    init: function () {
        $("#createForm").submit(function (e) {
            e.preventDefault();
            let data = {
                id: parseInt($("#id").val()),
                name: $("#name").val(),
                hobbies: createFunction.data.hobbies.map(c => {
                    let h = {};
                    h.hobbie = c.hobbie;
                    return h;
                })
            };
            $.ajax({
                url: "/api/user/create",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(data),
                success: function (result) {
                    if (result.code == 1) {
                        location.href = location.origin;
                    }
                    else {
                        alert(result.message);
                    }
                },
                error: function (e) {
                    console.log(e);
                    alert("error " + e);
                }
            });
        });

        $("#btn-add-hobbie").click(function () {
            let container = document.getElementById("hb-container");
            let template = document.getElementById("template").cloneNode(true);
            let index = createFunction.data.hobbies.length == 0 ? createFunction.data.hobbies.length + 1 : createFunction.data.hobbies[createFunction.data.hobbies.length - 1].index + 1;
            $(template).removeAttr("id").attr("target", index);
            $(template).removeClass("d-none");
            $($(template).find("input")[0]).attr("name", index).attr("required", true);
            $($(template).find("input")[0]).change(function () {
                if ($(this).val() != '') createFunction.data.hobbies.find(h => h.index == index).hobbie = $(this).val();
                else createFunction.data.hobbies.find(h => h.index == index).hobbie = null;
            });
            container.appendChild(template);
            createFunction.data.hobbies.push({
                index: index,
                hobbie: null,
            });
        });
    },
    discardHobbie: function (target) {
        let hobbie = $(target).closest('.hobbie');
        let index = parseInt($(hobbie).attr("target"));
        createFunction.data.hobbies = createFunction.data.hobbies.filter(h => h.index != index);
        $(hobbie).remove();
    }
};

$(document).ready(function () {
    createFunction.init();
});