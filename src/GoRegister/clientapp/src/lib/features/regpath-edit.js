// Any plugins you want to use has to be imported
import { initCkeditor } from "@/shared/ckeditor";

export default function() {
    document.querySelectorAll(".js-html-editor").forEach(e => initCkeditor(e,'true'));
}
