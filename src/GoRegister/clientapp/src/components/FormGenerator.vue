<template>
  <div>
    <Formal :vuelidate="$v.model" :renderFormTag="false" nestedModel="model">
      <FormWizard
        :showWizard="showWizard"
        :hideButtons="hideButtons"
        :isAdmin="isAdmin"
        :hideSteps="schema.hideSteps"
        @afterPageChange="handlePageChange"
        :beforeSubmit="beforeSubmit"
        @on-submit="handleSubmit"
        :submitButtonText="submitButtonText"
      >
        <FormStep
          v-for="(page, pageIndex) in schema.pages"
          :key="pageIndex"
          :title="page.title"
          :id="page.id"
          class="row"
          :beforeNext="validatePage"
        >
          <template v-for="(field, i) in fields">
            <formElementWrapper
              :key="i"
              :fg="fg"
              :fieldId="field.key"
              :model="model.model"
              :schema="schema"
              v-if="field.pageId === page.id"
            >
              <FieldWrapper :cols="field.cols">
                <component
                  v-bind="field"
                  v-model="model.model[field.id]"
                  :validatePropertyName="field.id"
                  :is="field.type"
                  :key="i"
                  :model="model"
                ></component>
              </FieldWrapper>
            </formElementWrapper>
          </template>
        </FormStep>
        <FormStep v-if="schema.enableReview" title="Review">
          <FormSummary :schema="schema" :model="model" modelPath="model" />
        </FormStep>
      </FormWizard>
    </Formal>
  </div>
</template>

<script>
import fieldComponents from "../utils/fieldsLoader";
import { shouldDisplayField } from "../utils/fieldsHelper";
import { generateVuelidateConfiguration } from "../utils/validationHelper";

// components
import formElementWrapper from "./FormElementWrapper";
import FieldWrapper from "./FieldWrapper";
import FormalLabel from "./FormalLabel";
import FormWizard from "./wizard/FormWizard";
import FormStep from "./wizard/FormStep";
import Formal from "./Formal.vue";
import FormSummary from "./FormSummary";

const components = {
  ...fieldComponents,
  formElementWrapper,
  FormalLabel,
  FieldWrapper,
  Formal,
  FormWizard,
  FormStep,
  FormSummary,
};

export default {
  props: {
    hideButtons: Boolean,
    isAdmin: Boolean,
    model: {
      type: Object,
      required: true,
    },
    modelPath: String,
    schema: {
      type: Object,
      required: true,
    },
    schemas: Object,
    v: {
      type: Object,
      required: true,
    },
  },
  components,
  data() {
    return {
      fg: this,
      currentPage: null
    };
  },
  created() {
    this.currentPage = { page: this.schema.pages[0], index: 0 };
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
    
    handlePageChange(page, index) {
      console.log(index);
      this.currentPage = { page: this.schema.pages[index], index };
    },
    buildFormDisplaySchema(schema, model) {
      let fieldsToDisplay = [];
      for (let index = 0; index < schema.fields.length; index++) {
        const field = schema.fields[index];
        if(!this.shouldValidateFullModel && field.pageId !== this.currentPage.page.id) continue;
        if (shouldDisplayField(field.key, schema, model)) {
          fieldsToDisplay.push(field.key);
        }
      }
      return {
        fieldsToDisplay: fieldsToDisplay,
      };
    },
    validatePage() {
      this.$v.$touch();
      if (this.$v.$invalid) {
        return false;
      }

      return true;
    },
    beforeSubmit() {
      return this.validatePage();
    },
    handleSubmit() {
      this.$emit("on-submit");
    }
  },
  computed: {
    submitButtonText () {
      return this.schema.submitButton ? this.schema.submitButton : "Submit";
    },
    showWizard () {
      return this.schema.enableReview || this.schema.pages.length > 1;
    },
    dataModel() {
      if (this.modelPath) {
        return this.model[this.modelPath];
      }

      return this.model;
    },
    fields() {
      return this.schema.fields;
    },
    shouldValidateFullModel() {
      let totalPages = this.schema.pages.length;
      //if(this.enableReview)
      totalPages++;
      return this.currentPage.index + 1 >= totalPages;
    },
    displayedFields() {
      let fieldsToDisplay = [];
      for (let index = 0; index < this.fields.length; index++) {
        const field = this.fields[index];
        if (shouldDisplayField(field.key, this.schema, this.model)) {
          fieldsToDisplay.push(field);
        }
      }

      return fieldsToDisplay;
    },
    displaySchema() {
      let displaySchema = {
        guests: [],
      };

      // apply rules about which fields to display for user
      displaySchema.user = this.buildFormDisplaySchema(
        this.schemas[this.model.registrationTypeId],
        this.model.model
      );

      // apply rules which fields to display for guest
      for (const [key, guest] of Object.entries(this.model.guests)) {
        displaySchema.guests[key] = this.buildFormDisplaySchema(
          this.schemas[guest.registrationTypeId],
          guest.model
        );
      }

      return displaySchema;
    }
  },
  validations() {
    let validationModel = {
      model: {},
      guests: {},
      editGuest: {
        model: {},
      },
    };

    // for (let index = 0; index < this.fields.length; index++) {
    //   const field = this.fields[index];
    //   validationModel.model[field.id] = {};
    // }

    let generateValidation = function (schema, displaySchema) {
      let vuelidateConfig = {};
      for (let index = 0; index < schema.fields.length; index++) {
        const field = schema.fields[index];
        // is this field being displayed, so do we need to display it?
        if (!displaySchema.fieldsToDisplay.includes(field.key)) continue;

        if (field.validations) {
          vuelidateConfig[field.key] = generateVuelidateConfiguration(field);
        }
      }

      return vuelidateConfig;
    };

    validationModel.model = generateValidation(
      this.schemas[this.model.registrationTypeId],
      this.displaySchema.user
    );

    return { model: validationModel };
  },
};
</script>