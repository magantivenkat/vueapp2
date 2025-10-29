export default function() {

    const $ = window.jQuery;

    $(function () {
       
       $('.js-registration-type-multiple-select2').select2({
           placeholder: {
               id: '-1', // the value of the option
               text: 'Select at least one type'
           }
       });
       $('.js-registration-status-multiple-select2').select2({
           placeholder: {
               id: '-1', // the value of the option
               text: 'Select at least one status '
           }
       });
    })

}
