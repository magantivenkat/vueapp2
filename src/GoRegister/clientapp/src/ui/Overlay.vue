<template>
  <div
    class="modal-mask"
    v-show="show"
    id=""
    tabindex="-1"
    role="dialog"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
    v-on:click="$emit('close')"
  >
    <slot></slot>
  </div>
</template>

<script>
export default {
  props: ["show", "close"],
};
</script>

<style scoped>
.modal-mask {
  position: fixed;
  z-index: 9950;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}
</style>