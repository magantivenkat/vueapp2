<template>
  <div>
    <InputWrapper :context="context">
      <flat-pickr
        :id="context.dataTag"
        class="form-control"
        :disabled="readonly"
        v-model.lazy="proxy"
        :config="{enableTime: pickerType ? true: false}"
      />
    </InputWrapper>
  </div>
</template>

<script>
import mixin from "../InputMixin";
import InputWrapper from "./InputWrapper.vue";
import FlatPickr from "vue-flatpickr-component";
import "flatpickr/dist/flatpickr.css";

export default {
  components: {
    FlatPickr,
    InputWrapper,
  },
  mixins: [mixin],
  props: {
    value: {
      type: String,
      default: "",
    },
    label: String,
    pickerType: Number,
    readonly: Boolean
  },
  computed: {
    proxy: {
      get() {
        return this.value;
      },
      set(value) {
        this.$emit("input", value);
      },
    },
  },
};
</script>