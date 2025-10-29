import { initCkeditor } from "@/shared/ckeditor";

export default function(dataset) {
  const $ = window.jQuery;
  
  console.log("hey!")

  $(function() {
    $("form").on("submit", function() {
      if (!$("form").valid()) {
        return false;
      }
    });

    $(document).ajaxStart(function() {
      $("#version-loading").show();
    });

    $(document).ajaxStop(function() {
      $("#version-loading").hide();
    });

    $("#version-modal-container").on("show.bs.modal", function(event) {
      var container = $("#customPageVersionDetailsView");

      const clickedLink = event.relatedTarget;

      const customPageVersionId = clickedLink.id;

      //const splitUrl = $(location)
      //  .attr("href")
      //  .split("/Edit")[0];
      const splitUrl = (window.location.href).split("/Edit")[0];

      const actionUrl = `${splitUrl}/CustomPageVersions/${customPageVersionId}`;

      $.get(actionUrl, function(data) {
        container.html(data);
      });
    });

    $("#version-modal-container").on("hidden.bs.modal", function() {
      $("#customPageVersionDetailsView").html("");
    });

    $(document).ajaxStart(function() {
      $("#loading").show();
    });

    $(document).ajaxStop(function() {
      $("#loading").hide();
    });

    $("#audit-modal-container").on("show.bs.modal", function(event) {
      var container = $("#customPageAuditDetailsView");

      const clickedLink = event.relatedTarget;

      const customPageAuditId = clickedLink.id;

      //const splitUrl = $(location)
      //  .attr("href")
        //  .split("/Edit")[0];
      const splitUrl = (window.location.href).split("/Edit")[0];

      const actionUrl = `${splitUrl}/CustomPageAudits/${customPageAuditId}`;

      $.get(actionUrl, function(data) {
        container.html(data);
      });
    });

    $("#audit-modal-container").on("hidden.bs.modal", function() {
      $("#customPageAuditDetailsView").html("");
    });
  });

  $(document).ready(function() {
    $(".js-registration-type-multiple-select2").select2({
      placeholder: {
        id: "-1", // the value of the option
        text: "Select at least one type",
      },
    });
    $(".js-registration-status-multiple-select2").select2({
      placeholder: {
        id: "-1", // the value of the option
        text: "Select at least one status ",
      },
    });


    const Swal = window.Swal;
    $(document).on("click", ".js-delete-action", function (e) {
      e.preventDefault();
      Swal.fire({
          title: "Are you sure you want to delete this?",
          //text: "You won't be able to revert this!",
          icon: "error",
          showCancelButton: true,
          confirmButtonColor: "#d33",
          cancelButtonColor: "#3085d6",
          confirmButtonText: "Yes, delete it!"
      }).then((result) => {
          if (result.value) {
              var $form = $('#js-delete-form-' + dataset.id);
              $form.submit();
          }
      });

    });
  });
    
  document.querySelectorAll(".js-html-editor").forEach(e => initCkeditor(e));
}
