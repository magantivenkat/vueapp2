export default function() {

    const $ = window.jQuery;

    var element = document.getElementById("page-config");  
    var dataPostUrl = element.attributes["data-posturl"].value;

    $(function () {
            var table = $('#dataTable').DataTable({
                searching: false,
                info: false,
                paging: false,
                rowReorder: true,
                columnDefs: [
                    { targets: 1, visible: false },
                    { orderable: true, className: 'reorder', targets: 0 },
                    { orderable: false, targets: '_all' }
                ]
            });

            var postUrl = dataPostUrl;

            table.on('row-reorder.dt', function(e, diff, edit) {

                var positions = {};

                var result = 'Reorder started on row: ' + edit.triggerRow.data()[0] + '<br>';

                for (var i = 0, ien = diff.length; i < ien; i++) {
                    var rowData = table.row(diff[i].node).data();

                    result += rowData[2] +
                        ' updated to be in position ' +
                        diff[i].newData +
                        ' (was ' +
                        diff[i].oldData +
                        ')<br>';

                    positions[rowData[1]] = diff[i].newData;
                }

                $.ajax({
                    url: postUrl,
                    method: 'POST',
                    async: true,
                    data: { positions },
                    success: function(response) {
                        //alert("Done");
                        //toastr.success("The section order was succesfully updated.");
                    },
                    error: function(msg) {
                        alert(msg.val);
                        // toastr.error("The section order was not updated, something unexpected happened.");
                    }
                });

                $('#result').html(result);
            });
    })

}
