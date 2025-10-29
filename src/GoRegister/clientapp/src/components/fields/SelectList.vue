<template>
  <InputWrapper :context="context">
    <select class="form-control" v-model="proxy" :disabled="readonly">
      <option v-if="placeholderText" :value="0">{{ placeholderText }}</option>
      <option v-if="defaultOption" value :selected="valueIsEmpty" disabled hidden>{{ defaultOptionText }}</option>
      <option
        v-for="(opt, index) in options"
        :key="index"
        :id="uuid"
        :value="opt.value"
      >
        {{ opt.text || opt.name }}
      </option>
    </select>
  </InputWrapper>
</template>

<script>
import inputMixin from "../InputMixin";
import InputWrapper from "./InputWrapper.vue";

export default {
  mixins: [inputMixin],
  components: { InputWrapper },
  props: {
    options: {
      type: Array,
    },
    defaultOption: [String, Boolean],
    placeholderText: String,
    value: [String, Number],
    model: Object,
  },
  data() {
    return {
      uuid: Math.random().toString(16).substr(2),
    };
  },
  computed: {
    proxy: {
      get() {
        if(this.valueIsEmpty) return '';
        return this.value;
      },
      set(value) {
        console.log(value);
        this.$emit("input", value);
      },
    },
    defaultOptionText() {
      if(this.defaultOption) {
        if(typeof this.defaultOption === 'string' || this.defaultOption instanceof String) {
          return this.defaultOption;
        } else {
          return "";
        }
      } else {
        return "";
      }
    },
    valueIsEmpty() {
      return  this.value === undefined ||
              this.value === '' ||
              this.value === null ||
              this.value === false;
    }
  },
};
</script>