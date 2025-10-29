import { grconfig } from "@/shared/grconfig";

import ClassicEditor from "@ckeditor/ckeditor5-editor-classic/src/classiceditor";
import GeneralHtmlSupport from '@ckeditor/ckeditor5-html-support/src/generalhtmlsupport';
import HtmlEmbed from '@ckeditor/ckeditor5-html-embed/src/htmlembed';
import Essentials from "@ckeditor/ckeditor5-essentials/src/essentials";
import Autoformat from "@ckeditor/ckeditor5-autoformat/src/autoformat";
import Bold from "@ckeditor/ckeditor5-basic-styles/src/bold";
import Italic from "@ckeditor/ckeditor5-basic-styles/src/italic";
import BlockQuote from "@ckeditor/ckeditor5-block-quote/src/blockquote";
import Heading from "@ckeditor/ckeditor5-heading/src/heading";
import Image from "@ckeditor/ckeditor5-image/src/image";
import ImageUpload from "@ckeditor/ckeditor5-image/src/imageupload";
import Link from "@ckeditor/ckeditor5-link/src/link";
import List from "@ckeditor/ckeditor5-list/src/list";
import Paragraph from "@ckeditor/ckeditor5-paragraph/src/paragraph";
import Alignment from "@ckeditor/ckeditor5-alignment/src/alignment";
import SimpleUploadAdapter from "@ckeditor/ckeditor5-upload/src/adapters/simpleuploadadapter";
import SourceEditing from '@ckeditor/ckeditor5-source-editing/src/sourceediting';
import ImageResize from '@ckeditor/ckeditor5-image/src/imageresize';
import ImageStyle from "@ckeditor/ckeditor5-image/src/imagestyle";
import ImageToolbar from "@ckeditor/ckeditor5-image/src/imagetoolbar"
import DataTags from "./DataTags";
import Table from '@ckeditor/ckeditor5-table/src/table';
import TableToolbar from '@ckeditor/ckeditor5-table/src/tabletoolbar';
import FindAndReplace from '@ckeditor/ckeditor5-find-and-replace/src/findandreplace';
import Font from '@ckeditor/ckeditor5-font/src/font';
import ListStyle from '@ckeditor/ckeditor5-list/src/liststyle';
import AutoLink from '@ckeditor/ckeditor5-link/src/autolink';

import sanitize from 'sanitize-html';

const initCkeditor = function (element, includeDatatags) {

  ClassicEditor.create(element, getCkEditorConfig(includeDatatags))

    .then((editor) => {

    })
    .catch((error) => {
      console.error(error.stack);
    });
}

const getCkEditorConfig = function (tags) {

  let toolbarDatatags = tags ? "DataTags" : "";

  const config = {

    extraPlugins: [DataTags],

    plugins: [
      GeneralHtmlSupport,
      HtmlEmbed,
      Essentials,
      Autoformat,
      Bold,
      Italic,
      BlockQuote,
      Heading,
      Image,
      ImageUpload,
      Link,
      List,
      Paragraph,
      Alignment,
      SimpleUploadAdapter,
      SourceEditing,
      ImageResize,
      ImageStyle,
      ImageToolbar,
      DataTags,
      Table,
      TableToolbar,
      FindAndReplace,
      Font,
      ListStyle,
      AutoLink
    ],

    htmlSupport: {
      allow: [
        {
          name: /.*/,
          attributes: true,
          classes: true,
          styles: true
        }
      ]
    },
    //https://ckeditor.com/docs/ckeditor5/latest/features/html-embed.html
    htmlEmbed: {
      showPreviews: true,
      sanitizeHtml: (inputHtml) => {
        // Strip unsafe elements and attributes, e.g.:
        // the `<script>` elements and `on*` attributes.
        const outputHtml = sanitize(inputHtml);

        return {
          html: outputHtml,
          // true or false depending on whether the sanitizer stripped anything.
          hasChanged: true
        };
      }
    },

    heading: {
      options: [
        { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
        { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
        { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
        { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' }
      ]
    },

    image: {
      // You need to configure the image toolbar, too, so it uses the new style buttons.
      toolbar: ['imageTextAlternative', '|', 'imageStyle:alignLeft', 'imageStyle:alignCenter', 'imageStyle:alignRight'],

      styles: [
        // This represents an image aligned to the left.
        'alignLeft',
        // This represents an image aligned to the right.
        'alignRight',
        // This represents an image aligned to the center.
        'alignCenter'
      ]
    },

    toolbar: {
      items: ['insertTable', '|', 'heading', "undo", "redo", "|", 'fontSize', 'fontFamily', 'fontColor', 'fontBackgroundColor', "bold", "italic", "alignment", "|", 'bulletedList', 'numberedList', '|', "imageUpload", "resizeImage", "|", 'findAndReplace', 'link', '|', "sourceEditing", 'htmlEmbed', `${toolbarDatatags}`]
      , shouldNotGroupWhenFull: true
    },

    table: {
      contentToolbar: ['tableColumn', 'tableRow', 'mergeTableCells']
    },

    simpleUpload: {
      // The URL that the images are uploaded to.
      uploadUrl: grconfig.baseProjectUrl + "/pages/CkEditorFileUpload",

      headers: {
        RequestVerificationToken: document.querySelector(
          '[name="__RequestVerificationToken"]'
        ).value,
      },
    },
  }

  return config;
}

export {
  getCkEditorConfig,
  initCkeditor
}