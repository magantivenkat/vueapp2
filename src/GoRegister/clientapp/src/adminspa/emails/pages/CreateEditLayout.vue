<template>
  <div v-if="loaded">
    <h1>{{ getPageTitle }} Layout</h1>
    <hr />
    <formal @submit="handleSubmit">
      <text-input v-model="model.name" label="Layout Name" />

      <EditTemplate :model="firstTemplate" isLayout />
      <p class="small m-0">
        <span class="strong">Note: </span> Layouts must contain the datatag
        <code>{{ email_content }}</code> in the body.
      </p>
      <div class="form-group">
        <input type="submit" class="btn btn-primary" value="Save" />{{ " " }}
        <input
          type="button"
          class="btn btn-default"
          value="Preview"
          @click="previewEmail()"
        />
      </div>
    </formal>
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
      email_content: "{{email_content}}",
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
      isLayout: this.$route.query.isLayout,
    };
  },
  methods: {
    async fetchData() {
      const $this = this;
      if (this.isEdit) {
        getEditTemplateModel(this.emailId, true).then((result) => {
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
      if (!this.firstTemplate.bodyHtml.includes("{{email_content}}")) {
        this.$bvToast.toast(`You must include {{email_content}} in the body.`, {
          title: "Error!",
          autoHideDelay: 3000,
          variant: "danger",
        });
        return;
      }

      try {
        const emailId = await postEmailModel(this.model, true);
        if (emailId) {
          this.$bvToast.toast(`Your changes have been saved.`, {
            title: "Saved!",
            autoHideDelay: 3000,
            variant: "success",
          });
        }
        this.$router.push({ name: "email-edit-layout", params: { emailId } });
      } catch (ex) {
        alert(ex);
      }
    },
    async previewEmail() {
      var html = await axios.post(`/emails/PreviewEmailTemplate`, {
        emailtemplateId: this.model.layoutId,
        emailBody: this.firstTemplate.bodyHtml,
      });
      var win = window.open("", "_blank");
      win.document.body.innerHTML = html.data;

    },
  },
  computed: {
    firstTemplate() {
      return this.model.templates[0];
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