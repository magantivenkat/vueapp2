/*  MRF Changes : Remove registration status column
    Modified Date : 30th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rane @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-236   */

$(document).ready(function () {    
    var element = document.getElementById("script-delegates");
        
    var postUrl = element.attributes["data-postUrl"].value;
    //var actionUrl = $(location).attr("href");
    var actionUrl = window.location.href;
    actionUrl = actionUrl.substring(0, actionUrl.indexOf('Index'));
    DelegateController.init(postUrl, actionUrl);
});

var DelegateController = function () {

    const escapeHtmlRender = $.fn.dataTable.render.text().display;
    const testLabel = `<span class="badge badge-warning">Test</span>`;

    const init = function (postUrl, actionUrl) {

        var table = $("#dataTable").DataTable({
            stateSave: true,
            autoWidth: true,
            language: {
                processing: "Loading Delegates...",
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
            columnDefs: [
                { targets: '_all', render: $.fn.dataTable.render.text() }
            ],
            // Columns Setups
            columns: [
                {
                    "data": "name",
                    //Commented by Mandar Khade to remove Status column for GOR-236
                    //"render": function (name, type, delegate, meta) {                        
                    //    return delegate.isTest ? `${escapeHtmlRender(name)} ${testLabel}` : `${escapeHtmlRender(name)}`;                        
                    //},
                },
                { data: "email" },
                //Commented by Mandar Khade to remove Status column for GOR-236
                //{ data: "registrationType" },                
                //{ data: "registrationStatus" },
                {
                    "title": "Actions",
                    "data": "id",
                    "searchable": false,
                    "sortable": false,
                    //"render": function (delegateId, type, delegate, meta) {
                    //    return `
                    //            <a href="${ actionUrl}Manage/${delegateId}" class="btn btn-info btn-sm">Manage</a>
                    //            <a href="${actionUrl}Edit/${delegateId}" class="btn btn-info btn-sm">Edit</a>
                    //            <a href="${ actionUrl}Audit/${delegateId}" class="btn btn-info btn-sm">Audit</a>
                    //    `;
                    //}
                    "render": function (delegateId, type, delegate, meta) {
                        return `
                                <a href="${actionUrl}Edit/${delegateId}" class="btn btn-info btn-sm">View</a>                                
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
