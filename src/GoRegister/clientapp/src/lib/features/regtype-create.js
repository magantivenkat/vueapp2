export default function() {

    const $ = window.jQuery;

    $(function () {

        $('#Name').on('keyup', function () {
                $('#new-reg-path-name').val($(this).val())
        })

        $(':radio[name=RegPath]').on('change', function () {

            var $new = $('#reg-path-new'),
                $existing = $('#reg-path-existing')

            var val = $(this).val()

            if (val === "new") {
                $('select').prop('selectedIndex', 0)
                $new.removeClass("d-none")
                $existing.addClass("d-none")
            } else {
                $new.addClass("d-none")
                $existing.removeClass("d-none")
            }
        })
    })

}
