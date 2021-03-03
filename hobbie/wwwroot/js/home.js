$(".btn-delete").click(function () {
    let id = $(this).attr("target");
    let dialog = confirm("Are you sure to delete the data?");
    if (dialog) {
        $.ajax({
            url: "/api/user/delete/" + id,
            type: "DELETE",
            dataType: "json",
            success: function (result) {
                if (result.code == 1) {
                    location.href = location.origin;
                }
                else {
                    alert(result.message);
                }
            },
        });
    }
});