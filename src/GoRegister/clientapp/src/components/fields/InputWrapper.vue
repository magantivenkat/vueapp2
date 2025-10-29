<template>
  <div class="form-group" :class="validationClassForGroup">
      <div class="col-md-12 col-sm-12 m-2">
          <div class="col-md-4 col-sm-12" style="float:left; position:relative;">
              <span v-if="v.required != context.IsMandatory" class="required">*</span>
              
              <label :for="context.id" v-if="renderLabel">
                  {{ context.label }}
              </label>

          </div>
          <span v-if="context.helpTextBefore"
                v-text="context.helpTextBefore"
                class="form-text text-muted" />
          <div class="col-md-8 col-sm-12" style="float: right; position: relative;">
              <slot></slot>
          </div>
          <span v-if="context.helpTextAfter" v-text="context.helpTextAfter" class="form-text text-muted" />
          <div class="col-md-4 col-sm-12" style="float:left; position:relative;"></div>
          <div v-if="v.$error" class="col-md-8 col-sm-12 invalid-feedback was-validated" style="float: right; position: relative;">
              <div v-if="hasError(v.required)">{{ validationErrorName }} is required</div>
              <div v-if="hasError(v.minLength)">Please enter at least {{ v.$params.minLength.min }} characters</div>
              <div v-if="hasError(v.email)">Please enter a valid email address</div>
          </div>
      </div>
      </div>
</template>

<script>
export default {
  props: {
    id: String,
    renderLabel: {
      type: Boolean,
      default: true,
    },
    context: {
      type: Object,
      required: true,
    },
  },
  methods: {
    hasError(validation) {
      return validation !== undefined && !validation;
    },
  },
  computed: {
    v() {
      return this.context.v;
    },
    validationErrorName() {
      return this.context.mergedValidationName();
    },
    validationClassForGroup() {
      if (this.v.$error) return "form-group-invalid";
      return "";
    },
    shouldRenderLabel() {
      return this.renderLabel && this.context.label !== '';
    }
  },
};
</script>