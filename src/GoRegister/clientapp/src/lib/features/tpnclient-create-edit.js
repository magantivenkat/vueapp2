
export default function () {

    const $ = window.jQuery;

    $(function () {

        var $addTPNClientEmailForm = $('#AddTPNClientEmailForm');


        $('.deleteBtn').click(function () {

            // delete client email with specified id and remove row

            var $tr = $(this).closest('tr'),
                valCountry = $tr.find("td:eq(0)").text(), // get current row 1st TD value
                valemail = $tr.find("td:eq(1)").text(),
                emailId = $tr.data('emailid')

            let Swal;
            if (typeof window !== 'undefined' && typeof document !== 'undefined') {
                Swal = require('sweetalert2').default;
            }

            Swal.fire({
                title: `Are you sure you want to delete ${valCountry} and ${valemail} ?`,
                icon: "error",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Yes, do it!"
            }).then((result) => {
                if (result.value) {
                    $.ajax({
                        url: "/admin/Client/TPNCountryEmailDel/",
                        data: {
                            emailId: emailId
                        },
                        headers: {
                            'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                        },
                        method: "delete",
                        success: function (email) {
                            if (email) {
                                Swal.fire("TPN country client email deleted successfully.");
                                $tr.remove();
                            }
                        }
                    })
                }
            });
        });


        $('.deleteBtnMapping').click(function () {

            // delete client email with specified id and remove row

            var $tr = $(this).closest('tr'),
                valCountry = $tr.find("td:eq(0)").text(), // get current row 1st TD value
                valemail = $tr.find("td:eq(1)").text(),
                mappingId = $tr.data('mappingid')

            let Swal;
            if (typeof window !== 'undefined' && typeof document !== 'undefined') {
                Swal = require('sweetalert2').default;
            }

            Swal.fire({
                title: `Are you sure you want to delete mapping of ${valCountry} and ${valemail} ?`,
                icon: "error",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Yes, do it!"
            }).then((result) => {
                if (result.value) {
                    $.ajax({
                        url: "/admin/Client/TPNClientGAMappingDelete/",
                        data: {
                            mappingId: mappingId
                        },
                        headers: {
                            'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                        },
                        method: "delete",
                        success: function (email) {
                            if (email) {
                                Swal.fire("TPN Client GA mapping deleted successfully.");
                                $tr.remove();
                            }
                        }
                    })
                }
            });
        });

    });
}