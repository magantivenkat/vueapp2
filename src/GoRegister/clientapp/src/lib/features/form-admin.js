
export default function() {
    
    const $ = window.jQuery;

    $(function () {
        $('#custom-form-name').on('keyup', function () {
            if ($(this).val().length > 2) {
                $('#custom-form-button').removeAttr('disabled')
            } else {
                $('#custom-form-button').attr('disabled', 'disabled')
            }
        })                
    })

}