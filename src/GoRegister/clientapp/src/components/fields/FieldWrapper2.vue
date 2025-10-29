<template>
  <div :class="columnClass">
    <InputWrapper :context=context v-if=renderInputWrapper>
      <slot></slot>
    </InputWrapper>
    <div v-else><slot></slot></div>
  </div>
</template>

<script>
import InputWrapper from "./InputWrapper";

export default {
  components: { InputWrapper },
  props: {
    context: {
      type: Object,
      required: true
    },
    renderInputWrapper:{ type: Boolean, default: true }
  },
  computed: {
    columnClass: function() {
      return `col-md-${this.context.cols}`; 
    }
  }
};
</script>