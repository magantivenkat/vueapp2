<template>
  <Overlay v-bind:show="show" v-on:close="$emit('close')">
    <div class="modal-wrapper">
      <div class="modal-dialog modal-dialog-scrollable" v-bind:class="modalSize">
        <div class="modal-content">
          <slot></slot>
        </div>
      </div>
    </div>
  </Overlay>
</template>

<script>
import Overlay from "../Overlay";

export default {
  props: {
    show: Boolean,
    close: Function,
    size: {
      type: String,
      default: "lg",
    },
  },
  components: { Overlay },
  computed: {
    modalSize() {
      return `modal-dialog modal-${this.size}`;
    },
  },
};
</script>

<style>
.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}
</style>