<template>
  <InputWrapper :context="context">
    <label
      v-for="(opt, index) in options"
      :key="index"
      class="form-check mb-0"
      :class="isInlineClass"
    >
      <input
        class="form-check-input"
        type="radio"
        :id="uuid"
        :value="opt.value"
        :disabled="opt.disabled"
        v-model="proxy"
      />
      <span class="form-check-label" :for="uuid">{{ opt.text }}</span>
    </label>
  </InputWrapper>
</template>

<script>
import inputMixin from "../InputMixin";
import InputWrapper from "./InputWrapper.vue";

export default {
  mixins: [inputMixin],
  components: { InputWrapper },
  props: {
    inline: {
      type: Boolean,
      default: false,
    },
    value: [String, Number],
    model: Object,
    options: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      uuid: Math.random().toString(16).substr(2),
    };
  },
  computed: {
    isInlineClass() {
      return this.inline ? "form-check-inline" : "";
    },
    processedOptions() {
      if(this.readonly) return this.options;
      let value = this.value;

      return this.options.map(option => {
        if(option.value === value) return option;

        option.disabled = true;
        return option;
      })
    },
    proxy: {
      get() {
        return this.value;
      },
      set(value) {
        console.log(value)
        this.$emit("input", value);
      }
    }
  },
};
</script>