<template>
  <div>
    <div class="item-group">
      <div class="card add-field" @click="displayFieldOptions = true">
        + Add Field
      </div>
    </div>
    <Modal v-show="displayFieldOptions">
      <ModalHeader title="New Field" v-on:close="displayFieldOptions = false" />
      <ModalBody>        
        <Toolbox :pageId="pageId" :click="() => displayFieldOptions = false" />
      </ModalBody>
      <ModalFooter confirmText="Close" v-on:confirm="() => displayFieldOptions = false" />
    </Modal>
  </div>
</template>
<script>
import Toolbox from "../../builder/Toolbox.vue";
import Modal from "../../ui/modal/Modal.vue";
import ModalBody from "../../ui/modal/ModalBody.vue";
import ModalFooter from "../../ui/modal/ModalFooter.vue";
import ModalHeader from "../../ui/modal/ModalHeader.vue";
export default {
  data() {
    return {
      displayFieldOptions: false,
    };
  },
  methods: {},
  components: { Modal, ModalHeader, ModalBody, ModalFooter, Toolbox },
  props: ["pageId"],
};
</script>
<style scoped>
.item-group {
  padding-left: 1rem;
}
.add-field {
  cursor: pointer;
  border: 3px dashed #82c4ac;
  padding: 8px 20px;
  color: #82c4ac;
  text-align: center;
}
</style>