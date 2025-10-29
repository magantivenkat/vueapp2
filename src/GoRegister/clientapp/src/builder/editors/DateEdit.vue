<template>
  <div>
     <div class="form-group">
      <label>Type</label>
      <select v-model="field.pickerType" class="form-control">
        <option value="0">Date</option>
        <option value="1">Date and Time</option>
      </select>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    field: {
      type: Object,
      required: true
    }
  }
};
</script>