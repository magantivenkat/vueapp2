<template>
  <div>
    <div class="form-group" v-if="displayRegistrationTypes">
      <label>Registration Types this template will apply to:</label>
      <div
        v-for="(rs, i) in registrationTypes"
        :key="i"
        class="form-check"
      >
        <input
          class="form-check-input"
          v-model="model.registrationTypes"
          type="checkbox"
          :value="rs.value"
          :id="'regtype-' + rs.value"
        />
        <label class="form-check-label" :for="'regtype-' + rs.value">
          {{ rs.name }}
        </label>
      </div>
    </div>
    <div class="form-group">
      <label>Email Body</label>
      <ckeditor v-model="model.bodyHtml" :config="config" :editor="editor" />
    </div>
    <div v-if="!isLayout" class="form-group">
      <div class="form-check">
        <input
          class="form-check-input"
          type="checkbox"
          v-model="model.HasTextBody"
          id="firstTemplateText"
        />
        <label class="form-check-label" for="firstTemplateText">
          Has text template
        </label>
      </div>
    </div>
    <div class="form-group" v-if="model.HasTextBody">
      <text-area        
        v-model="model.bodyText"
        label="Text Template"
        :rows="5"
      />
    </div>
  </div>
</template>

<script>
   
import { getCkEditorConfig } from "@/shared/ckeditor";
import CKEditor from '@ckeditor/ckeditor5-vue2';
import ClassicEditor from '@ckeditor/ckeditor5-editor-classic/src/classiceditor';

const config = getCkEditorConfig(true);

    export default {
        components: { ckeditor: CKEditor.component },
        props: {
            model: {
                type: Object,
            },
            isLayout: Boolean,
            displayRegistrationTypes: Boolean,
            registrationTypes: Array,
        },
        data: () => ({
            config,
            editor: ClassicEditor
        }),
        async created() {
            this.config = getCkEditorConfig(true);
        }
    };
</script>