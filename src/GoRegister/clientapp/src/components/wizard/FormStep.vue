<template>
  <div v-if="active">
    <slot :active="active"></slot>
  </div>
</template>

<script>
export default {
  inject: ['addStep'],
  props: {
    title: String,
    beforeNext: {
      type: Function
    }
  },
  data() {
    return {
      active: false
    }
  },
  mounted() {
    this.addStep(this);
  },
};
</script>