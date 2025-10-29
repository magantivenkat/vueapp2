<template>
  <div :class="columnClass">
    <slot></slot>
  </div>
</template>

<script>
export default {
  props: {
    cols: {
      type: Number,
      default: 12,
    },
  },
  computed: {
    columnClass() {
      return `col-md-${this.cols}`;
    },
  },
};
</script>