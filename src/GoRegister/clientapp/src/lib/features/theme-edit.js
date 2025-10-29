import { initCkeditor } from "@/shared/ckeditor";

export default function() {
  
  document.querySelectorAll(".js-html-editor").forEach(e => initCkeditor(e,'true'));


  window.$('[data-codemirror]').each(function (i, el) {
    let $el = window.$(el);
    let mode = window.$el.data("codemirror");
    window.CodeMirror.fromTextArea(el, {
        lineNumbers: true,
        mode: mode
    });
});

}