<template>
  <div class="modal-footer">
    <button v-on:click="$emit('confirm')" type="button" class="btn btn-primary">
      {{ confirmText }}
    </button>
  </div>
</template>

<script>
export default {
  props: ["confirm", "confirmText", "cancel"],
};
</script>