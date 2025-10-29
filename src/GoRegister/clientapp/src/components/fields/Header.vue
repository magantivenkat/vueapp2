<template>
  <h2>{{ title }}</h2>
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