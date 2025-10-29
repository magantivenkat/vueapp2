
$(".js-delete-action").click(function (e) {
    e.preventDefault();
    Swal.fire({
        title: "Are you sure you want to delete this?",
        //text: "You won't be able to revert this!",
        icon: "error",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.value) {
            const formId = $(this).closest("form").attr("id");
            $(`#${formId}`).submit();

        }
    });

});

function deleteItem(form) {
    $(form).parents("tr").fadeOut("slow", function () {
        $(this).remove();
    });
}