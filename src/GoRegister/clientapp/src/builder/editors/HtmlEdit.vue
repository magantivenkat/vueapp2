<template>
  <div>
    <div class="form-group">
      <label>Html Editor</label>
      <ckeditor v-model="field.name" :config="config" :editor="editor" />
    </div>
  </div>
</template>

<script>
import { getCkEditorConfig } from "@/shared/ckeditor";
import CKEditor from '@ckeditor/ckeditor5-vue2';
import ClassicEditor from '@ckeditor/ckeditor5-editor-classic/src/classiceditor';

const config = getCkEditorConfig();

export default {
  components: { ckeditor: CKEditor.component },
  props: {
    field: {
      type: Object,
      required: true,
    },
  },
  data: () => ({
     config,
    editor: ClassicEditor
  }),
};
</script>