/*
MRF Changes: 
Add Activate/Deactivate confirmation popup
Modified Date: 15 January 2024
Modified By: Vishal.Gaikwad@amexgbt.com
JIRA Ticket No: GoRegisterMRF / GOR - 327
*/
export default function () {

    const $ = window.jQuery;

    $(function () {
        
        $('#dataTable').DataTable({
            "order": [[2, "desc"]]
        });

        $('#dataTable').on('click', '.toggleBtn', function (e) {
            const toggleBtn = this;
            //soft delete MRF form with specified id and change button name to Activate
            var $tr = $(toggleBtn).closest('tr'),
                valName = $tr.find("td:eq(0)").text(),
                projectid = $tr.data('projectid')

            let Swal;
            if (typeof window !== 'undefined' && typeof document !== 'undefined') {
                Swal = require('sweetalert2').default;
            }

            Swal.fire({
                title: `Are you sure you want to ${$(toggleBtn).text()} ${valName} ?`,
                icon: "error",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Yes, do it!"
            }).then((result) => {
                if (result.value) {
                    $.ajax({
                        url: "/admin/Projects/ManageMRFDelete/",
                        data: {
                            projectid: projectid
                        },
                        headers: {
                            'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                        },
                        method: "delete",
                        success: function (success) {
                            if (success) {
                                Swal.fire(
                                    {
                                        title: `MRF form ${valName} ${$(toggleBtn).text()}d successfully.`
                                    });  
                                
                                $(toggleBtn).text(($(toggleBtn).text().trim() === "Activate" ? "Deactivate" : "Activate"));                               
                            }
                            else
                                Swal.fire("MRF form Activate/Deactivate failed.");
                        }
                    })
                }
            });
        });
    })
}