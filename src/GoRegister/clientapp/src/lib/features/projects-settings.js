export default function() {

    const $ = window.jQuery;
    const flatpickr = window.flatpickr;

    $(function () {

        var clientId = $('#ClientId');
        var $emailList = $('#EmailAddress');

        $emailList.children().remove("option");
        // fetch client specific emails and insert into selectList
        $.ajax({
            url: "/admin/Client/ClientEmails?clientId=" + clientId.val(),
            success: function (data) {
                var emailListHtml
                data.map(email => emailListHtml += '<option value="' + email.email + '">' + email.email + '</option>')

                // add client themes to list
                $emailList.append(emailListHtml)
                SetEmailSelected();
            }
        });

        $(':radio[name=EmailType]').change(function () {

            HandleEmailVisibility();
        });

        HandleEmailVisibility();

        function SetEmailSelected() {
            var $emailList = $('#EmailAddress');
            var selectedEmail = $('#SelectedEmail');
    
            $emailList.val(selectedEmail.val());
        }
    
        function HandleEmailVisibility() {
            var clientEmailSection = $('#section-emailtype-client');
            var customEmailSection = $('#section-emailtype-custom');
            var emailAddress = $('#EmailAddress');
            var customEmailAddress = $('#CustomEmailAddress');
    
            var value = $("input[type=radio][name=EmailType]:checked").val();
    
            const ProjectEmailType = {
                CustomEmail: "CustomEmail",
                ClientEmail: "ClientEmail"
            }
    
            if (value == ProjectEmailType.CustomEmail) {
                customEmailSection.removeClass('d-none');//visible
                clientEmailSection.addClass('d-none');//not visible
    
                customEmailAddress.attr('required', 'required')
                emailAddress.removeAttr('required', 'required')
            }
            else if (value == ProjectEmailType.ClientEmail) {
                clientEmailSection.removeClass('d-none');//visible
                customEmailSection.addClass('d-none');//not visible
    
                emailAddress.attr('required', 'required')
                customEmailAddress.removeAttr('required', 'required')
            }
        }
    
        flatpickr("input[type='datetime-local']", {
            altInput: true,
            enableTime: true,
            altFormat: "d/m/Y H:i",
            dateFormat: "Y-m-dTH:i",
            time_24hr: true
        });

    })

}
