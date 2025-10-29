

$(document).ready(function () {
    var element = document.getElementById("script-clients-datatable");
    var postUrl = element.attributes["data-postUrl"].value;
    // var actionUrl = $(location).attr("href").replace("Index", "");

    var locationURL = (window.location.href).replace("Index", "");
    var actionUrl = new URL(locationURL);

    ClientController.init(postUrl, actionUrl);
});

var ClientController = function () {

    const init = function (postUrl, actionUrl) {
        var table = $("#dataTable").DataTable({
            stateSave: true,
            autoWidth: true,
            language: {
                processing: "Loading Clients...",
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
            order: [[1, 'desc']],
            // Columns Setups
            columns: [
                //{ data: "id" },
                {
                    data: "name",
                    render: $.fn.dataTable.render.text()
                },               
                //{ data: "email" },
                //{ data: "registrationType" },
                //{ data: "invitationList" },
                //{ data: "registrationStatus" },
                {
                    "title": "Actions",
                    "data": "id",
                    "searchable": false,
                    "sortable": false,
                    "render": function (clientId, type, full, meta) {
                        //<a href="${ actionUrl }Details/${delegateId}" class="btn btn-info btn-sm">Details</a>
                        return `
                                <a href="/admin/Client/TPNView?id=${clientId}" class="btn btn-primary btn-sm">View</a>                            
                                
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
                url: actionUrl + "Delete?id=" + id,
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
