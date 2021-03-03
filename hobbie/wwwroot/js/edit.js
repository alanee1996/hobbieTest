var editFunction = {
    init: function () {
        $("#editForm").submit(function (e) {
            e.preventDefault();
            let data = {
                id: parseInt($("#id").val()),
                name: $("#name").val(),
            };
            $.ajax({
                url: "/api/user/edit",
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
            $(template).removeAttr("id");
            $(template).removeClass("d-none");
            $($(template).find("input")[0]).attr("required", true);
            container.appendChild(template);
        });
    },
    saveHobbie: function (target) {
        let hobbie = $(target).closest('.hobbie');
        let id = $(hobbie).attr("target");
        let userId = $("#id").val();
        let url = id ? "/api/user/hobbie/edit" : "/api/user/hobbie/add";
        let data = {
            id: id ? parseInt(id) : 0,
            userId: parseInt(userId),
            hobbie: $($(hobbie).find("input")[0]).val()
        };
        $.ajax({
            url: url,
            type: id ? "PUT" : "POST",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (result) {
                alert(result.message);
                if (result.code == 1) {
                    $(hobbie).attr("target", result.data.id);
                }

            },
            error: function (e) {
                console.log(e);
                alert("error " + e);
            }
        });
    },
    deleteHobbie: function (target) {
        let hobbie = $(target).closest('.hobbie');
        let id = $(hobbie).attr("target");
        let userId = $("#id").val();
        if (id) {
            let dialog = confirm("Are you sure to remove hobbie?");
            if (dialog) {
                $.ajax({
                    url: "/api/user/hobbie/delete/" + userId + "/" + id,
                    type: "DELETE",
                    dataType: "json",
                    success: function (result) {
                        if (result.code == 1) {
                            $(hobbie).remove();
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
            }
        }
        else {
            $(hobbie).remove();
        }
    }
};

$(document).ready(function () {
    editFunction.init();
});