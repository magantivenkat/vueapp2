export default function() {

    const $ = window.jQuery;

    var element = document.getElementById("page-config");  
    var dataPostUrl = element.attributes["data-posturl"].value;    

    $(function () {
        var $AddUserProjMap = $('#AddUserProjMap');

        $AddUserProjMap.on('submit', function (e) {
            e.preventDefault();

            var model = { userId: $('#UserId').val(), projectId: $('#ProjectId').val() }
                
                $.ajax({
                    url: dataPostUrl,
                    method: "post",
                    headers: {
                        'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                    },
                    data: model,
                    success: function (user) {                        
                        $('#divShareUser').html(user)
                        
                    },
                    error: function () {
                        $("#divUserExist").removeClass("d-none");
                    }
                })            
        })

    })

}