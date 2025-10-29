<template>
  <InputWrapper :context="context">
    <textarea
      :name="context.id"
      v-model.lazy="proxy"
      class="form-control"
      :rows="rows"
      :readonly="readonly"
      :id="context.id"    
      :placeholder="placeholder"
      :class="ifInvalidClass"

    />
  </InputWrapper>
</template>

<script>
import inputMixin from "../InputMixin";
import InputWrapper from "./InputWrapper";

export default {
  mixins: [inputMixin],
  components: { InputWrapper },
  props: {
    value: {
      type: String,
          default: ""
    },
    rows: {
      type: Number,
      default: 3,
    },
      model: Object,
      placeholder: String
      
  },
  data() {
    return {};
  },
  computed: {
      proxy: {
          get() {
              return this.value;
          },
          set(value) {
              this.context.v.$touch();
              this.$emit("input", value);
          },
      },
      ifInvalidClass() {
          return this.isInvalid ? "is-invalid" : "";
      }
  }
};
</script>