

$(document).ready(function () {
    var element = document.getElementById("script-session-index");

    var postUrl = element.attributes["data-postUrl"].value;
    //var actionUrl = $(location).attr("href");
    var actionUrl = window.location.href;
    actionUrl = actionUrl.substring(0, actionUrl.indexOf('Index'));
    SessionsController.init(postUrl, actionUrl);
});


var SessionsController = function () {

    const init = function (postUrl, actionUrl) {
        var table = $("#dataTable").DataTable({
            stateSave: true,
            autoWidth: true,
            language: {
                processing: "Loading Sessions...",
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
                //{ data: "id" },
                {
                    "data": "id",
                    "render": function (sessionId, type, full, meta) {
                        return `<a href="${actionUrl}Edit/${sessionId}">${full.name}</a>`
                    }
                },
                {
                    "data": "isOptional",
                    "render": function (sessionId, type, full, meta) {
                        return full.isOptional ? "Optional" : "Required"
                    }
                },
                {
                    "data": "sessionCategory",
                    "render": function (sessionId, type, full, meta) {
                        return full.sessionCategory ? `${full.sessionCategory.name}` : ''
                    }
                },
                {
                    data: "dateStartUtc",
                    "render": function (sessionId, type, full, meta) {
                        return pretifyDate(full.dateStartUtc)
                    }
                },
                {
                    "data": "id",
                    "render": function (sessionId, type, full, meta) {
                        return `<a href="${actionUrl}Delegates/${sessionId}">${full.registrationCount}/${full.capacity + full.capacityReserved}</a>`
                    }
                },
                //{ data: "registrationStatus" },
                {
                    "title": "Actions",
                    "data": "id",
                    "searchable": false,
                    "sortable": false,
                    "render": function (sessionId, type, full, meta) {
                        return `
                                <a href="${actionUrl}Edit/${sessionId}" class="btn btn-info btn-sm">Edit</a>
                                <!--<a href="${actionUrl}Audit/${sessionId}" class="btn btn-info btn-sm">Audit</a>-->
                                <!--button type="button" onclick="DeleteRow(${sessionId}, '${full.name}', '${actionUrl}')" class="btn btn-danger btn-sm">Delete</button-->
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

function pretifyDate(date) {
    var days = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun']
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    var d = new Date(date)

    return `${days[d.getDay() - 1]}, ${d.getDate() + getDaySuffix(d.getDate())} ${months[d.getMonth()]} ${d.getFullYear()} ${d.toLocaleTimeString()}`
}

function getDaySuffix(day) {
    switch (day) {
        case 1:
        case 21:
        case 31:
            return "st";
        case 2:
        case 22:
            return "nd";
        case 3:
        case 23:
            return "rd";
        default:
            return "th";
    }
}

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
