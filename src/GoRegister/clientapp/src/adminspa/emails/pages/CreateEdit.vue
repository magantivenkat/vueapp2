<template>
  <div v-if="loaded">
    <h1>{{ getPageTitle }} {{ model.emailTypeName }}</h1>
    <hr />
    <formal :vuelidate="$v" @submit="handleSubmit">

        <div  style="padding-bottom:20px;">
            <text-input v-model="model.name" label="Name"  />
        </div>
        <div style="padding-bottom: 20px;">
            <text-input v-model="model.subject" label="Subject" />
        </div>

        <div style="padding-bottom: 20px;">
            <text-input v-model="model.cc" label="CC" />
        </div>
        <div style="padding-bottom: 20px;">
            <text-input v-model="model.bcc" label="BCC" />
        </div>
        <div style="padding-bottom: 20px;">
            <SelectList v-model="model.layoutId" :options="viewModel.layouts"
                        placeholderText="No Layout"
                        label="Please select a layout"></SelectList>
        </div>


            <hr class="mb-4" />
            <div v-if="displayTemplatesAsList" class="mt-2">
                <div class="float-right" v-show="model.templates.length === 1">
                    <div class="form-check">
                        <input class="form-check-input"
                               type="checkbox"
                               v-model="enableTemplatesPerRegTypes"
                               id="enableTemplatesPerRegTypes" />
                        <label class="form-check-label" for="enableTemplatesPerRegTypes">
                            Disable templates per registration type
                        </label>
                    </div>
                </div>
                <h3>Templates</h3>
                <div v-for="(template, i) in model.templates"
                     :key="i"
                     class="card mb-2 email-template-list-item"
                     @click="openTemplateEditor(i)">
                    <div v-if="i === model.defaultTemplateIndex"
                         class="font-italic mt-2 ml-2">
                        Default Template
                    </div>
                    <div class="card-body">
                        <button class="btn btn-outline-primary float-right" type="button">
                            Edit
                        </button>
                        <button type="button"
                                @click.stop="makeDefault(i)"
                                v-show="i !== model.defaultTemplateIndex"
                                class="
                btn-make-default-template btn btn-outline-info
                float-right
                mr-2
              ">
                            Make Default Template
                        </button>
                        <div v-if="i === model.defaultTemplateIndex">
                            Applies tsdasdao:
                            <span v-for="(regType, j) in unusedRegTypes"
                                  :key="j"
                                  class="btn btn-sm btn-secondary mr-2">
                                {{ regType.name }}
                            </span>
                        </div>
                        <div v-else>
                            Applies to:
                            <span v-for="(regType, j) in getRegTypes(template.registrationTypes)"
                                  :key="j"
                                  class="btn btn-sm btn-secondary mr-2">
                                {{ regType.name }}
                            </span>
                        </div>
                    </div>
                </div>
                <div>
                    <button @click.prevent="addTemplate" class="btn btn-outline-primary">
                        Add Template
                    </button>
                </div>
            </div>
            <div v-else>
                <div class="form-group">
                    <div class="form-check">
                        <input class="form-check-input"
                               type="checkbox"
                               v-model="enableTemplatesPerRegTypes"
                               id="enableTemplatesPerRegTypes" />
                        <label class="form-check-label" for="enableTemplatesPerRegTypes">
                            Enable multiple templates per registration type
                        </label>
                    </div>
                </div>
                <EditTemplate :model="firstTemplate" />
            </div>
            <hr />
            <div class="form-group">
                <input type="submit" class="btn btn-primary" value="Save" />{{ " " }}
                <input type="button"
                       class="btn btn-default"
                       value="Preview"
                       @click="previewEmail()" />
            </div>
    </formal>
    <Modal v-if="editTemplateModel.index !== null" :show="true">
      <ModalHeader
        title="Edit Template"
        v-on:close="editTemplateModel.index = null"
      />
      <ModalBody>
        <EditTemplate
          :model="editTemplateModel.data"
          :displayRegistrationTypes="
            editTemplateModel.index !== model.defaultTemplateIndex
          "
          :registrationTypes="viewModel.registrationTypes"
        />
      </ModalBody>
      <ModalFooter confirmText="Finish" v-on:confirm="updateTemplate" />
    </Modal>
  </div>
</template>
<script>
import { required } from "vuelidate/lib/validators";
import Formal from "@/components/Formal.vue";
import EditTemplate from "./EditTemplate";
import axios from "axios";
import {
  postEmailModel,
  getCreateTemplateModel,
  getEditTemplateModel,
} from "../api";

export default {
  components: { Formal, EditTemplate },
  props: {
    emailId: Number,
    emailTypeId: Number,
  },
  async created() {
    await this.fetchData();
  },
  data() {
    return {
      model: {},
      viewModel: {},
      editTemplateModel: {
        data: {},
        index: null,
        registrationTypes: [],
      },
      enableTemplatesPerRegTypes: false,
      loaded: false,
      previewEmailAs: false,
    };
  },
  methods: {
    async fetchData() {
      const $this = this;
      if (this.isEdit) {
        getEditTemplateModel(this.emailId).then((result) => {
          $this.model = result.model;
          $this.viewModel = result.viewModel;
          $this.loaded = true;
        });
      } else {
        getCreateTemplateModel(this.emailTypeId).then((result) => {
          $this.model = result.model;
          $this.viewModel = result.viewModel;
          $this.loaded = true;
        });
      }
    },
    async handleSubmit() {
      try {
        const emailId = await postEmailModel(this.model);
        if (emailId) {
          this.$bvToast.toast(`Your changes have been saved.`, {
            title: "Saved!",
            autoHideDelay: 3000,
            variant: "success",
          });
        }
        this.$router.push({ name: "email-edit", params: { emailId } });
      } catch (ex) {
        alert(ex);
      }
    },
    async previewEmail() {
      var html = await axios.post(`/emails/PreviewEmailTemplate`, {
        emailtemplateId: this.model.layoutId,
        emailBody: this.model.templates[0].bodyHtml,
      });
      var win = window.open("", "_blank");
      //win.document.body.innerHTML = html.data;

        const htmlMRF = this.$sanitize(html.data);
        win.document.body.innerHTML = htmlMRF;

      // this.previewEmailAs = !this.previewEmailAs
    },
    getRegTypes(regTypeIds) {
      return this.viewModel.registrationTypes.filter((rt) => {
        if (regTypeIds.includes(rt.value)) return rt;
      });
    },
    openTemplateEditor(i) {
      const template = this.model.templates[i];
      this.editTemplateModel.data = { ...template };
      this.editTemplateModel.index = i;
      this.editTemplateModel.registrationTypes = [
        ...this.unusedRegTypes,
        ...this.getRegTypes(template.registrationTypes),
      ];
    },
    updateTemplate() {
      this.$set(
        this.model.templates,
        this.editTemplateModel.index,
        this.editTemplateModel.data
      );
      this.editTemplateModel.index = null;
    },
    makeDefault(index) {
      this.model.defaultTemplateIndex = index;
      this.model.templates[index].registrationTypes = [];
    },
    addTemplate() {
      this.model.templates.push({
        id: null,
        bodyHtml: "",
        bodyText: "",
        HasTextBody: false,
        registrationTypes: [],
      });
    },
  },
  computed: {
    displayTemplatesAsList() {
      return this.enableTemplatesPerRegTypes || this.model.templates.length > 1;
    },
    firstTemplate() {
      return this.model.templates[0];
    },
    unusedRegTypes() {
      let usedRegTypes = [];
      for (let index = 0; index < this.model.templates.length; index++) {
        const template = this.model.templates[index];
        usedRegTypes.push(...template.registrationTypes);
      }

      return this.viewModel.registrationTypes.filter((rt) => {
        if (!usedRegTypes.includes(rt.value)) return rt.name;
      });
    },
    isEdit() {
      return this.emailId ? true : false;
    },
    getPageTitle() {
      return this.isEdit ? "Edit" : "Create";
    },
  },
  validations() {
    return {
      model: {
        subject: { required },
      },
    };
  },
  watch: {
    async $route() {
      await this.fetchData();
    },
  },
};
</script>

<style>
.btn-make-default-template {
  display: none;
}

.email-template-list-item:hover .btn-make-default-template {
  display: initial;
}
</style>