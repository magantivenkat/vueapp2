<template>
  <div class="container">
    <div class="row">
      <div class="col">
        <h2>Field Types</h2>
        <ToolboxOption
          v-for="(fieldType, i) in nonPresentationFields"
          :key="i"
          :fieldType="fieldType"
          :pageId="pageId"
          :click="click"
        />
      </div>

      <div class="col">
        <h2>Presentation Types</h2>
        <ToolboxOption
          v-for="(fieldType, i2) in presentationFields"
          :key="i2"
          :fieldType="fieldType"
          :pageId="pageId"
          :click="click"
        />
      </div>
    </div>
  </div>
</template>

<script>
import ToolboxOption from "./ToolboxOption";

export default {
  props: ["pageId", "click"],

  components: { ToolboxOption },
  methods: {
    filterFields(fieldTypes, isForPresentation) {
      const fields = [];
      for (const key in fieldTypes) {
        const field = fieldTypes[key];

        // show field for all formTypes
        if (field.formTypes.length === 0 && field.isForPresentation == isForPresentation) {
          fields.push(field);
        }

        // show field if coresponds to current formType
        if (field.formTypes.length > 0 && field.formTypes.includes(this.formType)) {
          if (field.isForPresentation == isForPresentation) {
            fields.push(field);
          }
        }
      }

      return fields;
    },
  },
  computed: {
    formType() {
      return this.$store.state.formType;
    },
    fieldTypes() {
      return this.$store.state.fieldTypes;
    },
    nonPresentationFields() {
      const fields = this.filterFields(this.fieldTypes, false);
      return fields;
    },
    presentationFields() {
      const fields = this.filterFields(this.fieldTypes, true);
      return fields;
    },
  },
};
</script>