/*   MRF Changes : Share Link functionality
    Modified Date : 02nd Nov 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-242 */


export default function() {
    
    const $ = window.jQuery;
    const Swal = window.Swal;
    

        var element = document.getElementById("page-config");      

        var myVar = element.attributes["data-userid"].value;
        var postUrl = element.attributes["data-posturl"].value;
        var afterpostUrl = element.attributes["data-afterpostUrl"].value;
      
        $("#deleteDelegate").click(function () {
          
            Swal.fire({
                title: `Are you sure you want to delete ${name}?`,
                text: "This is permanent and cannot be undone",
                icon: "error",
                showCancelButton: true,
                confirmButtonColor: "#d33",
                cancelButtonColor: "#3085d6",
                confirmButtonText: "Confirm"
            }).then((result) => {
                //alert(result.value);
                if (result.value) {
                    Swal.fire({
                        title: 'Type the word delete below to confirm',
                        input: 'text',
                        inputAttributes: {
                            autocapitalize: 'off',
                            id: 'confirmtxt'
                        },
                        showCancelButton: true,
                        confirmButtonText: 'Confirm',
                        showLoaderOnConfirm: true,
                    }).then((result) => {
                        if (result.value) {
                            const inputxt = Swal.getPopup().querySelector('#confirmtxt').value;

                            if (inputxt == 'delete') {
                                $.ajax(
                                    {
                                        type: "POST", //HTTP POST Method
                                        url: postUrl, // Controller/View
                                        headers: {
                                            'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                                        },
                                        data: { //Passing data
                                            Id: myVar, //Reading text box values using Jquery
                                            apurl: afterpostUrl
                                        },
                                        success: function (data, textStatus, jQxhr) {
                                            window.location.href = afterpostUrl;                                              
                                        },
                                        error: function (jqXhr, textStatus, errorThrown) {
                                            Swal.fire("Something went wrong when deleting the attendee");
                                        }
                                    });
                            }
                            else {
                                Swal.fire('Sorry, please try again!');
                            }
                        }
                    })
                }

            })

        })

    $("#shareLinkDelegate").click(function () {
      
        $.ajax(
            {
                type: "POST", //HTTP POST Method
                url: postUrl, // Controller/View
                headers: {
                    'RequestVerificationToken': document.querySelector('[name="__RequestVerificationToken"]').value
                },
                data: { //Passing data
                    Id: myVar, //Reading text box values using Jquery
                    apurl: afterpostUrl
                },
                success: function (data, textStatus, jQxhr) {                 
                    Swal.fire("MRF URL shared successfully");
                },
                error: function (jqXhr, textStatus, errorThrown) {
                    Swal.fire("Something went wrong when sharing the MRF URL");
                }
            });
    })
 
}