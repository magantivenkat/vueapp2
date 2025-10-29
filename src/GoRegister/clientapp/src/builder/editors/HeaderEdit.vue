<template>
  <div>
    <div class="form-group">
      <label>Header</label>
      <input v-model="field.name" type="text" class="form-control" />
    </div>
  </div>
</template>

<script>
export default {
  props: {
    field: {
      type: Object,
      required: true,
    },
  },
};
</script>