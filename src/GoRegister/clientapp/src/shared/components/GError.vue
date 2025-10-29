<template>
  <div class="error-list">
    <div v-for="(error, i) in errorList" :key="i" class="alert alert-danger" role="alert">
      {{ error }}
    </div>
  </div>
</template>

<script>
export default {
  props: {
    result: [Object, Boolean],
    displayAllErrors: Boolean
  },
  computed: {
    errorList() {
      if(!this.result) return [];

      const errors = [];
      const rootKey = this.result.errors[""];
      if(rootKey && rootKey.length > 0) {
        errors.push(...rootKey);
      }

      return errors;
    },
    shouldDisplayError() {
      if(!this.result) return false;

      

      return this.result && this.result.errors.errors[""] > 0;
    }
  }
}
</script>