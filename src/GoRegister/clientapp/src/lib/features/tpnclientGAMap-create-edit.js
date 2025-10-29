export default function () {

    const $ = window.jQuery;

    $(function () {

        if ($('#Id').val() == "0") {
            $('#TPNCountry').prop("disabled", false);
        }
        else {
            $('#TPNCountry').prop("disabled", true);
        }

        var $tpnClientGAMapForm = $('#TPNClientGAMapForm');
        var $btnCancel = $('#btnCancel')

        $btnCancel.on('click', function () {
            $tpnClientGAMapForm.removeClass("d-none");
            $("#divMappingExist").addClass("d-none");
        })

        $tpnClientGAMapForm.on('submit', function (e) {

            e.preventDefault();

            $tpnClientGAMapForm.validate();

            if ($tpnClientGAMapForm.valid()) {

                var model = { Id: $('#Id').val(), clientId: $('#ClientId').val(), TPNCountry: $('#TPNCountry').val(), ClientUuid: $('#ClientUuid').val(), AdminUserId: $('#AdminUser').val(), GAMEmail: $('#GAMEmail').val(), ReportFrequency: $('#ReportFrequency').val(), FormAction: $('#FormAction').val() }

                let Swal;
                if (typeof window !== 'undefined' && typeof document !== 'undefined') {
                    Swal = require('sweetalert2').default;
                }

                $.ajax({
                    url: "/admin/Client/SaveTPNClientGAMapping",
                    method: "post",
                    headers: {
                        'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                    },
                    data: model,
                    success: function (map) {
                        if (map == null) {
                            $("#divMappingExist").removeClass("d-none");
                        }
                        else {
                            $("#divMappingExist").addClass("d-none");
                            $tpnClientGAMapForm.removeClass("d-none");
                            $('#ReportFrequency').val("");
                            $('#GAMEmail').val("");
                            $('#AdminUser').val("");
                            $('#TPNCountry').val("");
                            $('#id').val("");
                            Swal.fire("TPN client GA mapping saved successfully");
                        }

                    },
                    error: function () {
                        $("#divMappingExist").removeClass("d-none");
                    }
                })
                // Reset client UI

            }
        })

    })

}