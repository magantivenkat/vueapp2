<template>
  <SummaryRow :label="field.label" :value="model[field.id]" />
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
};
</script>