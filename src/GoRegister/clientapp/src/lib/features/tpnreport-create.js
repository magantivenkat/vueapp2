export default function () {

    const $ = window.jQuery;

    var $addTPNReportForm = $('#AddTPNReportForm');

    $(function () {
        $('#btnSubmit').click(function (e) {

            e.preventDefault();

            $addTPNReportForm.validate();

            if ($addTPNReportForm.valid()) {

                var model = { clientId: $('#Client').val(), TPNCountry: $('#Country').val() }

                let Swal;
                if (typeof window !== 'undefined' && typeof document !== 'undefined') {
                    Swal = require('sweetalert2').default;
                }

                $.ajax({
                    url: "/admin/TPNReport/Save",
                    method: "post",
                    headers: {
                        'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                    },
                    data: model,
                    success: function () {
                        $addTPNReportForm.removeClass("d-none");
                        $('#Client').val("");
                        $('#Country').val("");
                        Swal.fire("TPN report request submitted successfully");
                    },
                    error: function () {
                    }
                });
            }
        });
    });
}