$(document).ready(function () {
    var element = document.getElementById("script-tpnreports-datatable");
    var postUrl = element.attributes["data-postUrl"].value;
    var locationURL = (window.location.href).replace("Index", "");
    var actionUrl = new URL(locationURL);

    TPNReportController.init(postUrl, actionUrl);
});

var TPNReportController = function () {

    const init = function (postUrl, actionUrl) {
        var table = $("#dataTable").DataTable({
            stateSave: true,
            autoWidth: true,
            language: {
                processing: "Loading TPN Report Requests...",
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
            order: [[2, 'desc']],

            // Columns Setups
            columns: [
                //{ data: "id" },
                {
                    data: "clientName",
                    render: $.fn.dataTable.render.text()

                },
                {
                    data: "tpnCountry",
                    render: $.fn.dataTable.render.text()

                },
                { data: "dateRequested" },
                {
                    data: "reportStatus",
                    "sortable": false
                },
                {
                    data: "reportType",
                    "sortable": false
                },
                {
                    "title": "Actions",
                    "data":  "downloadPath",
                    "searchable": false,
                    "sortable": false,
                    "render": function (downloadPath, type, full, meta) {

                        if (downloadPath != "") {
                            return `
                                <a href="${downloadPath}" class="btn btn-primary btn-sm" >Download</a>                         
                                
                        `;
                        } 
                        else {
                            return ``;
                        }
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