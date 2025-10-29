/*  MRF Changes : Remove validation on Create Button on Create MRF page
    Modified Date : 16th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-213 
    
    MRF Changes : Get prefix on client dropdown change
    Modified Date :  01st November 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-228
    
    */

export default function () {

    const $ = window.jQuery;
    const flatpickr = window.flatpickr;

    $(function () {

            $(':radio[name=EmailType]').change(function () {

                var clientEmailSection = $('#section-emailtype-client');
                var customEmailSection = $('#section-emailtype-custom');
                var emailAddress = $('#EmailAddress');
                var customEmailAddress = $('#CustomEmailAddress');

                var value = $(this).val();

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
            });

            $('[name=ProjectUrlFormat]').on('change', function () {
                var urlFormat = $('[name=ProjectUrlFormat]:checked').val(),
                    subDomainSection = $('#section-subdomain'),
                    urlPathSection = $('#section-urlpath'),
                    subDomainInput = $('#Subdomain'),
                    urlPathInput = $('#PathPrefix');

                if (urlFormat == 0) {
                    subDomainSection.removeClass('d-none')
                    urlPathSection.addClass('d-none')
                    subDomainInput.attr('required', 'required')
                    urlPathInput.removeAttr('required', 'required')
                } else {
                    subDomainSection.addClass('d-none')
                    urlPathSection.removeClass('d-none')
                    subDomainInput.removeAttr('required', 'required')
                    urlPathInput.attr('required', 'required')
                }
            })

            var pathPrefixTimer;
            $('#PathPrefix').on('keyup', function () {
                // reset timer, UI errors etc
                //Commented by Mandar Khade to remove validation on create button
                 $('input[type="submit"]').attr('disabled', 'disabled')
                $('#tenant-error').addClass('d-none');
                clearTimeout(pathPrefixTimer);

                pathPrefixTimer = setTimeout(function () {
                    var path = $('#PathPrefix').val()

                    $.ajax({
                        url: "/admin/Projects/ProjectExists?prefix=" + path,
                        success: function (pathExists) {
                            pathExists ? $('#tenant-error').removeClass('d-none') : $('input[type="submit"]').removeAttr('disabled', 'disabled')
                        }
                    })

                }, 1500);

            })

            $('[name="ProjectUrlFormat"]').on('change', function () {
                getDomainUrls()
                console.log('callinf getDomains')
            })

        $('#ClientId').on('change', function () {

            if ($("#ClientId option:selected").val() != 0) {
                var $themeList = $('#ProjectTheme'),
                    $emailList = $('#EmailAddress'),
                    clientName = $("#ClientId option:selected").text(),
                    clientId = $("#ClientId option:selected").val()


                //If client changes remove client specific themes & emails

                $themeList.children().remove("optgroup")
                $emailList.children().remove("option")

                // fetch client specific domains and insert into selectList
                getDomainUrls()

                // fetch client specific themes and insert into selectList
                $.ajax({
                    url: "/admin/Admin/GetClientThemes?clientId=" + clientId,
                    success: function (data) {
                        if (data.length > 0) {
                            var selectListHtml = '<optgroup label="' + clientName + ' Themes">'
                            data.map(t => selectListHtml += '<option value="' + t.value + '">' + t.text + '</option>')
                            selectListHtml += '<optgroup>'

                            // add client themes to list
                            $themeList.append(selectListHtml)
                        }
                    }
                });

                // fetch client specific emails and insert into selectList
                //$.ajax({
                //    url: "/admin/Client/ClientEmails?clientId=" + clientId,
                //    success: function (data) {
                //        var emailListHtml
                //        data.map(email => emailListHtml += '<option value="' + email.email + '">' + email.email + '</option>')

                //        // add client themes to list
                //        $emailList.append(emailListHtml)
                //    }
                //});


                $.ajax({
                    url: "/admin/Client/ClientDetails?clientId=" + clientId,
                    success: function (data) {

                        $('#PathPrefix').val(data);


                    }
                });
            }
            else {
                $('#PathPrefix').val('');

            }
        })

            function getDomainUrls() {

                var $urlPathHost = $('#UrlPathHost'),
                    $SubdomainHost = $('#SubdomainHost'),
                    clientId = $("#ClientId option:selected").val(),
                    isSubdomainHost = $('input[name="ProjectUrlFormat"]:checked').val() == "0"


                //If client changes remove client specific themes & emails

                if (clientId == "--Select Client--") return false

                $.ajax({
                    url: `/admin/Domain/ListDomains?clientId=${clientId}`,
                    success: function (data) {
                        var domains = data.filter(d => d.isSubdomainHost == isSubdomainHost),
                            domainListHtml = ""

                        domains.map(domain => domainListHtml += '<option value="' + domain.id + '">' + domain.host + '</option>')

                        if (isSubdomainHost) {
                            $SubdomainHost.children().remove("option")
                            $SubdomainHost.append(domainListHtml)
                        } else {
                            $urlPathHost.children().remove("option")
                            $urlPathHost.append(domainListHtml)
                        }
                    }
                });

            }

            flatpickr("input[type='datetime-local']",
            {
                defaultDate: "today",
                altInput: true,
                enableTime: true,
                altFormat: "d/m/Y H:i",
                dateFormat: "Y-m-dTH:i",
                time_24hr: true
            });

        //Increment archive date to be 3 months after end date
        flatpickr("#ArchiveDate",
            {
                altInput: true,
                enableTime: true,
                defaultDate: new Date().fp_incr(90),
                altFormat: "d/m/Y H:i",
                dateFormat: "Y-m-dTH:i",
                time_24hr: true
            });

        //Increment delete data date to be 3 months after end date
        flatpickr("#DeleteDataDate",
            {
                altInput: true,
                enableTime: true,
                defaultDate: new Date().fp_incr(180),
                altFormat: "d/m/Y H:i",
                dateFormat: "Y-m-dTH:i",
                time_24hr: true
            });

    })

}