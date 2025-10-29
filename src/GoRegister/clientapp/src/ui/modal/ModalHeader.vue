<template>
  <div class="modal-header">
    <h5 class="modal-title" id="exampleModalLabel">{{ title }}</h5>
    <button
      type="button"
      class="close"
      @click="$emit('close')"
      aria-label="Close"
    >
      <span aria-hidden="true">&times;</span>
    </button>
    <slot></slot>
  </div>
</template>

<script>
export default {
  props: ["title", "close"],
};
</script>