<template>
  <div>
    <table class="table gr-form-review-table">
      <template v-for="(field, i) in fields">
        <formElementWrapper
          :key="i"
          :fg="fg"
          :fieldId="field.key"
          :model="model.model"
          :schema="schema"
        >
          <component :field="field" :is="field.summaryTemplate" :model="model.model"></component>
        </formElementWrapper>
      </template>
    </table>
  </div>
</template>

<script>
import fieldComponents from "../utils/fieldsLoader";
import formElementWrapper from "./FormElementWrapper";
//import { shouldDisplayField } from "../utils/fieldsHelper";

const components = {
  ...fieldComponents,
  formElementWrapper,
};

export default {
  props: {
    model: {
      type: Object,
      required: true,
    },
    modelPath: String,
    schema: {
      type: Object,
      required: true,
    },
  },
  components,
  data() {
    return {
      fg: this,
    };
  },
  methods: {
    getFieldDisplayName(field) {
      return field.name;
    },
    getValidationModel(fieldid) {
      if (this.v.model && this.v.model[fieldid]) {
        return this.v.model[fieldid];
      }

      return {};
    },
  },
  computed: {
    dataModel() {
      if (this.modelPath) {
        return this.model[this.modelPath];
      }

      return this.model;
    },
    fields() {
      return this.schema.fields.filter((e) => !e.isForPresentation);
    },
  },
};
</script>