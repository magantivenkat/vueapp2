// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.]const items = document.querySelectorAll(".accordion button");

function setupAccordion() {
    const items = document.querySelectorAll(".accordion button");

    function toggleAccordion() {
        const itemToggle = this.getAttribute('aria-expanded');

        for (i = 0; i < items.length; i++) {
            items[i].setAttribute('aria-expanded', 'false');
        }

        if (itemToggle == 'false') {
            this.setAttribute('aria-expanded', 'true');
        }
    }

    items.forEach(item => item.addEventListener('click', toggleAccordion));
}


function setupPrivacyPolicy() {
    $("#privacy-form").submit(function () {
        var acceptedPrivacyRadioButton = document.getElementById("AcceptedPrivacyPolicy");
        if (acceptedPrivacyRadioButton.checked == true) {
            document.getElementById("PrivacyError").style.display = "none";
            document.getElementById("PrivacyError").style.background = "#ccffcc";
            document.getElementById("AcceptedPrivacyPolicy").style.borderColor = "#ccffcc";
        }
        else {
            document.getElementById("PrivacyError").style.display = "block";
            document.getElementById("PrivacyError").style.color = "#e35152";
            document.getElementById("AcceptedPrivacyPolicy").style.borderColor = "#e35152";
            return false;
        }
        $("#privacy-modal").modal('hide');

        $.ajax({
            url: $(this).attr('action'),
            type: $(this).attr('method'),
            dataType: 'json',
            data: $(this).serialize(),
        }).always(function () {
            $("#privacy-modal").modal('hide');
        });


        return false;
    });

    $.ajax({
        url: $("#cfg-privacy-policy").data("url"),
        method: "get",
        success: function () {
            $("#privacy-modal").modal('hide');
        },
        error: function () {
            $("#privacy-modal").modal('show');
        }
    })
}


setupAccordion();
//setupPrivacyPolicy()