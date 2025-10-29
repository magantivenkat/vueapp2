<template>
  <InputWrapper :context="context">
    <input
      :type="inputType"
      :name="context.id"
      v-model.lazy="proxy"
      :class="ifInvalidClass"
      :readonly="readonly"
      :id="context.id"
      :placeholder="placeholder"
      class="form-control"
    />
  </InputWrapper>
</template>

<script>
import inputMixin from "../InputMixin";
import InputWrapper from "./InputWrapper.vue";

export default {
  mixins: [inputMixin],
  components: { InputWrapper },
  props: {
    value: {
      type: String,
      default: "",
    },
    inputType: { type: String, default: "text" },
    model: Object, 
    placeholder: String
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
    },
  },
};
</script>