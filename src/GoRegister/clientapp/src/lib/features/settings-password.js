export default function() {

    const $ = window.jQuery;

    $(function () {

                $(':checkbox[name=AllowAnonymousAccess]').change(function () {
                    HandleFieldVisibility();

                });

                HandleFieldVisibility();

                function HandleFieldVisibility() {
                    var passwordSection = $('#section-password');
    
                    if ($('#AllowAnonymousAccess').is(':checked')) {
                        passwordSection.removeClass('d-none');//visible
                    }
                    else {
                        passwordSection.addClass('d-none');//not visible
                    }
                }
    })
}