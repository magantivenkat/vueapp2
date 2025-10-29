<template>
  <SummaryRow :label="field.label" :value="value" />
</template>

<script>
import SummaryRow from "../SummaryRow";

export default {
  components: { SummaryRow },
  props: {
    field: {
      type: Object,
      required: true,
    },
    model: {
      type: Object,
      required: true,
    },
  },
  computed: {
    value() {
      let optionId = this.model[this.field.id];
      if (!optionId) return "";

      return this.field.options.find((e) => e.value === optionId).text;
    },
  },
};
</script>