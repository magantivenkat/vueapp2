export default function() {

     const $ = window.jQuery;

     $(function () {

        console.log("client create edit");

        var $addClientEmailButton = $('#AddClientEmailButton'),
            $addClientEmailForm = $('#AddClientEmailForm'),
            $cancelAddClientEmailButton = $('#AddClientEmailFormCancelButton')
            

        $addClientEmailButton.on('click', function () {
            $addClientEmailButton.addClass("d-none")
            //$addClientEmailForm.removeClass("d-none")
        })

        $cancelAddClientEmailButton.on('click', function () {
            $addClientEmailButton.removeClass("d-none")
            //$addClientEmailForm.addClass("d-none")
            $("#divEmailExist").addClass("d-none");
        })

        
        $addClientEmailForm.on('submit', function (e) {
            e.preventDefault();
            console.log("Save Email")

            $addClientEmailForm.validate();
            if ($addClientEmailForm.valid()) {             
                var model = { clientId: $('#Id').val(), email: $('#Email').val() }

                // Submit new email to server, if added, add to list of emails with id
                $.ajax({
                    url: "/admin/Client/Email",
                    method: "post",
                    headers: {
                        'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                    },
                    data: model,
                    success: function (email) {
                        //alert(email);
                        if (email == null) {
                            $("#divEmailExist").removeClass("d-none");
                        }
                        else {
                            $("#divEmailExist").addClass("d-none");
                            $('table tbody').append('<tr><td>' + email.email + '</td><td><i class="fa fa-pencil-alt client-theme-edit"></i></td><td><i data-emailId="' + email.id + '" class="fa fa-trash client-theme-delete"></i></td></tr>')
                        }
                    },
                    error: function () {
                        $("#divEmailExist").removeClass("d-none");
                    }
                })
                // Reset client UI
                $addClientEmailButton.removeClass("d-none")
                //$addClientEmailForm.addClass("d-none")
            }
        })


        $('table').on('click', '.fa-pencil-alt', function () {
          var  $tr = $(this).closest('tr'),
                value = $tr.first().text(),
                emailId = $tr.data('emailid')

            // get current text value
            $tr.replaceWith('<tr data-emailId="' + emailId + '"><td><input type="text" class="form-control" value="' + value + '" /><button class="btn btn-success"><i class="fa fa-check"></i></button><button class="btn btn-danger"><i class="fa fa-times"></i></button></td><td></td><td></td></tr>')

        })

         $('table').on('click', '.fa-check', function () {
           var $tr = $(this).closest('tr'),
                email = $tr.find('input').val(),
                emailId = $tr.data('emailid')
            //console.log("Save to db and return to original view", emailId, value)
            $.ajax({
                url: "/admin/Client/Email",
                method: "post",
                headers: {
                    'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: { id: emailId, email: email, clientId: $('#Id').val() },
                success: function (email) {
                    $tr.replaceWith('<tr data-emailId="' + email.id + '"><td>' + email.email + '</td><td><i class="fa fa-pencil-alt client-theme-edit"></i></td><td><i  class="fa fa-trash client-theme-delete"></i></td></tr>')
                }
            })
        })

        
        $('table').on('click', '.fa-times', function () {
           var $tr = $(this).closest('tr'),
                value = $tr.find('input').val(),
                emailId = $tr.data('emailid')

            $tr.replaceWith('<tr data-emailId="' + emailId + '"><td>' + value + '</td><td><i class="fa fa-pencil-alt client-theme-edit"></i></td><td><i  class="fa fa-trash client-theme-delete"></i></td></tr>')
        })
                 

        $('table').on('click', '.fa-trash', function () {
            // delete client email with specified id and remove row
           var $tr = $(this).closest('tr'),
                emailId = $tr.data('emailid')

            $.ajax({
                url: "/admin/Client/Email/",
                data: {
                    emailId: emailId
                },
                headers: {
                    'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                },
                method: "delete",
                success: function (email) {
                    if (email) {
                        $tr.remove()
                    }
                }
            })

        })


    })

}
