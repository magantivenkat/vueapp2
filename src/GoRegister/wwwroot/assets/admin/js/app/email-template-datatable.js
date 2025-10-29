var EmailTemplateController = function () {

    const init = function (postUrl, actionUrl) {
        var table = $("#dataTable").DataTable({
            stateSave: true,
            autoWidth: true,
            language: {
                processing: "Loading templates...",
                zeroRecords: "No matching records found"
            },
            processing: true,
            serverSide: true,
            paging: true,
            searching: { regex: true },
            initComplete: function () {
                $(".dataTables_filter input").unbind();
                $(".dataTables_filter input").bind("keyup", function (e) {
                    const code = e.keyCode || e.which;
                    if (code === 13) {
                        table.search(this.value).draw();
                    }
                });
            },
            ajax: {
                url: postUrl,
                type: "POST",
                contentType: "application/json",
                async: true,
                headers: {
                    'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                },
                dataType: "json",
                data: function (d) {
                    return JSON.stringify(d);
                }
            },
            // Columns Setups
            columns: [
                {
                    data: "name",
                    render: $.fn.dataTable.render.text()
                },
                { data: "invitationList" },
                { data: "registrationType" },
                { data: "registrationStatus" },
                {
                    "title": "Actions",
                    "data": "id",
                    "searchable": false,
                    "sortable": false,
                    "render": function (templateId, type, full, meta) {
                        return `
                                <a href="${actionUrl}Preview/${templateId}" class="btn btn-success btn-sm">Preview</a>
                                <a href="${actionUrl}Send/${templateId}" class="btn btn-success btn-sm">Send</a>
                                <a href="${actionUrl}Edit/${templateId}" class="btn btn-primary btn-sm">Edit</a>
                                <a href="${actionUrl}Details/${templateId}" class="btn btn-info btn-sm">Details</a>
                                <button type="button" onclick="DeleteRow(${templateId}, '${full.name}', '${actionUrl}')" class="btn btn-danger btn-sm">Delete</button>
                        `;
                    }
                }
            ]
        });
    };

    return {
        init: init
    };

}();

function DeleteRow(id, name, actionUrl) {
    Swal.fire({
        title: `Are you sure you want to delete ${name}?`,
        //text: "You won't be able to revert this!",
        icon: "error",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#3085d6",
        confirmButtonText: "Yes, do it!"
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "POST",
                url: actionUrl + "Delete/" + id,
                data: {
                    __RequestVerificationToken: $("#RequestVerificationToken").val(),
                    id: id
                },
                dataType: "json",
                success: function (response) {
                    const table = $("#dataTable").DataTable();
                    table.row($(this).parents("tr"))
                        .remove()
                        .draw();
                },
                error: function (msg) {
                    alert(msg.val);
                }
            });
        }
    });
}

