export default function () {

    const $ = window.jQuery;

    $(function () {
        
        if ($('#Id').val() == "0") {
            $('#TPNCountry').prop("disabled", false);
        }
        else {
            $('#TPNCountry').prop("disabled", true);
        }
                
        var $addTPNClientEmailForm = $('#AddTPNClientEmailForm');
        var $cancelAddTPNClientEmailButton = $('#AddTPNClientEmailCancelButton')

        $cancelAddTPNClientEmailButton.on('click', function () {
            $addTPNClientEmailForm.removeClass("d-none");
            $("#divtpnEmailExist").addClass("d-none");
        })
              
        $addTPNClientEmailForm.on('submit', function (e) {
           
            e.preventDefault();

            console.log("Save TPN Country Client Email")

            $addTPNClientEmailForm.validate();
            if ($addTPNClientEmailForm.valid()) {
                               
                var model = { Id: $('#Id').val(), clientId: $('#ClientId').val(), TPNemail: $('#TPNEmail').val(), TPNCountry: $('#TPNCountry').val(), ClientUuid: $('#ClientUuid').val(), FormAction: $('#FormAction').val()}

                let Swal;
                if (typeof window !== 'undefined' && typeof document !== 'undefined') {
                    Swal = require('sweetalert2').default;
                }

                $.ajax({
                    url: "/admin/Client/TPNCountryEmail",
                    method: "post",
                    headers: {
                        'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                    },
                    data: model,
                    success: function (email) {
                        if (email == null) {
                            $("#divtpnEmailExist").removeClass("d-none");
                        }
                        else {                           
                            $("#divtpnEmailExist").addClass("d-none");
                            $addTPNClientEmailForm.removeClass("d-none");
                            $('#TPNEmail').val("");
                            $('#TPNCountry').val("");
                            $('#id').val("");
                            Swal.fire("TPN country client email saved successfully");
                        }             

                    },
                    error: function () {
                        $("#divtpnEmailExist").removeClass("d-none");
                    }
                })
                // Reset client UI
                         
            }
        })       
    
    })

}
