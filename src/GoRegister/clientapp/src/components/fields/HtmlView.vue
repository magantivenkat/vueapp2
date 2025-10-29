<template>
  <div v-html="title"></div>
</template>

<script>
import mixin from "../FieldMixin";

export default {
  mixins: [mixin],
  props: {
    title: String,
  },
};
</script>