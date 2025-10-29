<template>
  <div>
    <div class="mb-2">
      <span class="h1">{{ formName }}</span>
      <button
        @click="showSettingsModal = !showSettingsModal"
        class="btn float-right"
      >
        <i class="fa fa-cog"></i>
      </button>
    </div>
    <div>
      <div class>
        <button @click="submit" :disabled="isSaving" class="btn btn-primary">
          Save
        </button>
        <!--<button class="btn btn-success float-right" @click="addPage">
          New Page
        </button>-->
      </div>
      <hr />
      <div class="row">
        <div class="col-12">
          <DraggablePages v-model="pages" />
        </div>
      </div>
      <hr />
      <div class="mb-2">
        <button @click="submit" class="btn btn-primary" :disabled="isSaving">
          Save
        </button>
        <!--<button class="btn btn-success float-right" @click="addPage">
          New Page
        </button>-->
      </div>
    </div>
    <Modal v-show="showSettingsModal">
      <ModalHeader
        title="Settings"
        v-on:close="showSettingsModal = !showSettingsModal"
      />
      <ModalBody>
        <div class="form-group">
          <label>Form Name</label>
          <input v-model="formName" type="text" class="form-control" />
        </div>
        <div class="form-group">
          <label>Submit Button Text</label>
          <input v-model="submitButtonText" type="text" class="form-control" />
        </div>
        <div class="form-group">
          <div class="form-check">
            <input v-model="isReviewPageHidden" class="form-check-input" type="checkbox" />
            <label class="form-check-label" for="firstTemplateText">
              Hide Review Page
            </label>
          </div>
        </div>
      </ModalBody>
      <ModalFooter confirmText="Close" v-on:confirm="showSettingsModal = !showSettingsModal" />
    </Modal>

    <Modal v-show="showNewFieldModal">
      <ModalHeader title="New Field" v-on:close="showNewFieldModal = false" />
      <ModalBody>
        <input
          v-model="newField.name"
          type="text"
          placeholder="Name"
          class="form-control"
        />
        <select v-model="newField.field" class="form-control">
          <option
            :key="i"
            v-for="(fieldType, i) in fieldTypes"
            :value="fieldType"
          >
            {{ fieldType.name }}
          </option>
        </select>
      </ModalBody>
      <ModalFooter confirmText="Add" v-on:confirm="addField" />
    </Modal>
  </div>
</template>

<script>
import axios from "axios";
import debounce from "lodash/debounce";
import { uuid } from "./utils";

// components
import DraggablePages from "../ui/draggable/draggable-pages"; // https://sortablejs.github.io/Vue.Draggable/#/nested-with-vmodel

export default {
  name: "App",
  components: {
    DraggablePages,
  },
  data() {
    return {
      newPage: {},
      newField: {},
      quickField: {
        text: "",
        fields: [],
        pageId: 0,
        capitalise: true,
      },
      importText: "",
      importModel: null,
      importRegTypeMode: "all",
      showInternalFields: false,
      isSaving: false,
      showNewFieldModal: false,
      showSettingsModal: false,
      previewId: "",
    };
  },
  created() {
    this.reloadPreviewDebounced = debounce(this.reloadPreview, 2000);
    this.previewId = uuid();
    // temporarily removing this while we update the site preview
    // const $sitePreviewEl = document.getElementById("site-preview-link");
    // $sitePreviewEl.setAttribute(
    //   "href",
    //   `/${$sitePreviewEl.dataset.prefix}/form/preview/${this.previewId}`
    // );

    // pull field data from here
    this.getDataUrl = document
      .getElementById("builder-app")
      .getAttribute("data-get-url");
    this.postDataUrl = document
      .getElementById("builder-app")
      .getAttribute("data-post-url");
  },
  mounted: function () {
    const url = this.getDataUrl;
    this.$store.dispatch("init", url);
  },
  methods: {
    addPage() {
      this.$store.dispatch("addPage");
    },
    addField() {
      const { name, field } = this.newField;

      this.$store.dispatch("addField", {
        fieldType: field,
        name: name,
      });

      this.showNewFieldModal = false;
      this.newField = {};
    },
    deleteField(id) {
      this.$store.dispatch("deleteField", id);
    },
    submit() {
      // what validations do we need to do?
      // this.$store.dispatch("save")
      var form = {
        formName: this.$store.state.formName,
        isReviewPageHidden: this.$store.state.isReviewPageHidden,
        submitButtonText: this.$store.state.submitButtonText
      }

      this.isSaving = true;
      axios
        .post(this.postDataUrl, {
          form: form,
          fields: this.$store.state.fields,
          pages: this.$store.state.pages,
          fieldsWithoutPage: this.$store.state.fieldsWithoutPage,
          internalFields: this.$store.internalFields,
        })
        .then(() => {
          alert("Saved successfully");
          //this.isSaving = false;
          // replace with api get
          window.location.reload(false);
        })
        .catch((resp) => {
          alert(resp.message);
          this.isSaving = false;
        });
    },
    reloadPreview: debounce(function () {
      console.log("debounced yay");
      localStorage.setItem(
        `formpreview-${this.previewId}`,
        JSON.stringify({
          fields: this.$store.state.fields,
          pages: this.$store.state.pages,
          id: this.$store.state.id,
        })
      );
    }, 1000),
  },
  computed: {
    formName: {
      get() {
        if(this.$store.state.formName) return this.$store.state.formName

        switch (this.formType) {
          case 0:
            return "MRF FORM";
          case 1:
            return "Decline";
          case 2:
            return "Cancel";
          default:
            return "";
        }
      },
      set(value) {
        this.$store.commit("setFormName", value);
      },
    },
    isReviewPageHidden: {
      get() { return this.$store.state.isReviewPageHidden },
      set(value) {
        this.$store.commit("setIsReviewPageHidden", value);
      },
    },
    submitButtonText: {
      get() { return this.$store.state.submitButtonText },
      set(value) {
        this.$store.commit("setSubmitButtonText", value);
      },
    },
    pages: {
      get() {
        return this.$store.state.pages.filter((p) => p.isDeleted == false);
      },
      set(value) {
        this.$store.dispatch("setPages", value);
      },
    },
    fieldTypes() {
      return this.$store.state.fieldTypes;
    },
    formType() {
      return this.$store.state.formType;
    },
    delegateFields: {
      get() {
        return this.$store.getters.delegateFields;
      },
      set(value) {
        this.$store.dispatch("reorderDelegateFields", value);
      },
    },
    internalFields: {
      get() {
        return this.$store.getters.internalFields;
      },
      set(value) {
        this.$store.dispatch("reorderInternalFields", value);
      },
    },
    fieldsWithOptions() {
      return this.$store.getters.getFieldsWithOptions;
    },
    state() {
      return this.$store.state;
    },
  },
  watch: {
    state: {
      deep: true,
      handler: function (oldSate, newState) {
        this.reloadPreviewDebounced();
      },
    },
  },
};
</script>

<style>
[v-cloak] {
  display: none;
}

.sortable-ghost .card {
  border: dashed 4px green;
}

.handle {
  cursor: pointer;
}

.btn.btn-secondary {
  background: #50bcac;
}
</style>
