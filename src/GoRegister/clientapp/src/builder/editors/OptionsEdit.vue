<template>
  <div>
    <div class="" v-if="!field.multiSelect">
      <label>Input Type</label>
      <div class="checkbox">
        <label>
          <input v-model="field.singleSelectType" :value="0" type="radio" />
          Radio (recommended)
        </label>
        <label>
          <input
            v-model="field.singleSelectType"
            :value="1"
            type="radio"
            class="ml-2"
          />
          Select List
        </label>
      </div>
    </div>

    <div v-if="!hasOptions"><strong>No options have been added</strong></div>
    <div>
      <span
        class="text-blue bold pointer"
        v-on:click="showBulkLoadOptions = true"
        >Bulk load options</span
      >
    </div>
    <draggable v-model="field.options" handle=".handle">
      <div v-for="(opt, i) in field.options" :key="i" class>
        <div class="form-group pt-1" v-if="!opt.isDeleted">
          <div class="input-group">
            <div class="input-group-prepend handle">
              <span class="input-group-text">
                <span class="fa fa-arrows-alt" aria-hidden="true"></span>
              </span>
            </div>
            <input type="text" class="form-control" v-model="opt.description" />
            <div class="input-group-append">
              <span class="input-group-text">
                <span class="fa fa-ellipsis-v" aria-hidden="true"></span>
              </span>
              <span class="input-group-text">
                <span
                  @click="removeOption(opt.id)"
                  class="fa fa-trash"
                  aria-hidden="true"
                ></span>
              </span>
            </div>
          </div>
        </div>
      </div>
    </draggable>
    <div class="pt-1">
      <form @submit.prevent="addOption(newOption)">
        <input
          v-model="newOption"
          type="text"
          class="form-control"
          placeholder="Add option"
        />
      </form>
    </div>
    <Modal v-show="showBulkLoadOptions">
      <ModalHeader
        title="Bulk Load Options"
        v-on:close="showBulkLoadOptions = false"
      />
      <ModalBody>
        <textarea
          v-model="bulkNewOptions"
          class="form-control"
          name=""
          id=""
          rows="10"
          placeholder="Each line is a new option"
        ></textarea>
      </ModalBody>
      <ModalFooter confirmText="Add Options" v-on:confirm="bulkAddOptions" />
    </Modal>
  </div>
</template>

<script>
import draggable from "vuedraggable";
import { uuid } from "../utils";

export default {
  components: { draggable },
  props: {
    field: { type: Object, required: true },
  },
  data() {
    return {
      newOption: "",
      bulkNewOptions: "",
      showBulkLoadOptions: false,
    };
  },
  methods: {
    bulkAddOptions() {
      var ks = this.bulkNewOptions.split("\n");

      ks.forEach((option) => this.addOption(option));

      this.showBulkLoadOptions = false;
      this.bulkNewOptions = "";
    },
    addOption(newOption) {
      if (!newOption.trim()) return;
      let option = {
        description: newOption,
        additionalInformation: "",
        capacity: null,
        isDeleted: false,
        id: uuid(),
      };
      this.field.options.push(option);

      // tidy up
      this.newOption = "";
    },
    removeOption(id) {
      console.log(id);
      this.field.options.find((e) => e.id === id).isDeleted = true;
    },
  },
  computed: {
    hasOptions: function () {
      return this.field.options.filter((e) => !e.isDeleted).length > 0;
    },
  },
};
</script>

<style>
.builder-option {
  background-color: #f5f5f5;
  padding-left: 5px;
  padding-right: 5px;
}
</style>