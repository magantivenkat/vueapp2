<template>
  <label
    :for="id"
    v-text="label"
    v-if="label"
  />
</template>

<script>
export default {
  props: {
    label: {
      type: String,
      required: true 
    },
    id: {
      type: Number,
      required: true 
    }
  }
}
</script>