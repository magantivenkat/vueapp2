$(".js-delete-action").click(function (e) {
    e.preventDefault();
    const formId = $(this).closest("form").attr("id");

    bootbox.confirm({
        message: "Are you sure you want to delete this?",
        buttons: {
            confirm: {
                label: "Yes, delete it!",
                className: "btn-danger"
            },
            cancel: {
                label: "Cancel",
                className: "btn-primary"
            }
        },
        callback: function (result) {
            if (result) {
                $(`#${formId}`).submit();
            }
        }
    });
});

function deleteItem(form) {
    $(form).parents("tr").fadeOut("slow", function () {
        $(this).remove();
    });
}