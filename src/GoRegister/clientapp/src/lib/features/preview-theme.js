export default function() {

    const $ = window.jQuery;

    var element = document.getElementById("layout-config");
    if(!element) return;

    var postUrl = element.attributes["data-postUrl"].value;
    var projPath = element.attributes["data-proj-path"].value;
    var windowPath = element.attributes["data-window-path"].value;

    $(function () {                

    $('#site-preview-link').on('click', function () {

        // get select list
        $.ajax({
            url: postUrl,
            method: "GET",
            success: function (data) {
                $('#SiteLink').html(data)
            }
        })
    });

    $('#btnPreviewTestDelegate').on('click', function () {
        var $modal = $(this).closest('.modal'),
            value = $modal.find('#SiteLink').val()

        var cookieValue = encodeURIComponent(value);
        document.cookie = "PreviewAs=" + cookieValue + ";expires=" + new Date(new Date().setMinutes(new Date().getMinutes() + 5)) + ";path="+projPath

        $modal.modal('toggle')
        window.open(`${windowPath}?delegateId=${value}`, '_blank')
    })

    })
}