<template>
  <div>
    <form @submit.prevent="formSubmitted" v-if="renderFormTag">
      <slot />
    </form>
    <slot v-else />
  </div>
</template>
<script>
export default {
  props: {
    vuelidate: {
      type: Object,
      default() {
        return false;
      },
    },
    nestedModel: {
      type: [String],
      default: null
    },
    renderFormTag: {
      type: Boolean,
      default: true,
    },
  },
  provide() {
    return {
      vuelidate: this.vuelidate,
      formalConfig: {
        nestedModel: this.nestedModel
      }
    };
  },
  methods: {
    formSubmitted() {
      if (this.vuelidate) {
        this.vuelidate.$touch();
        if (this.vuelidate.$invalid) {
          //TODO: display something here?
          return;
        }
      }
      this.$emit("submit");
    },
  },
};
</script> 