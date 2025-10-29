<template>
  <div class="container">

    <!-- <header class="form-header">
      <language-select v-if="lookupData.languages" :languages="lookupData.languages" v-model="model.preferredLanguage" />
    </header> -->
    <!-- <h1>Register</h1> -->
    <div class="bs-managed-form">
      <formal-form
        v-if="formSchemas"
        :schema="getSchemaForUser(model)"
        :schemas="formSchemas"
        :model="model"
        :modelPath="'model'"
        :allowGuest="false"
        :guestFormInModal="true"
        :submitUrl="submitUrl"
        @submission-success="reloadPage"
        :lookupData="lookupData"
      />
    </div>
  </div>
</template>

<script>
import Vue from "vue";
import Vuelidate from "vuelidate";
import axios from "axios";
//import FormGenerator from "./components/FormGenerator.vue";
import FormalForm from "./components/FormalForm";

Vue.use(Vuelidate);

export default {
  name: "App",
  components: {
    FormalForm
  },
  data: function () {
    return {
      model: {},
      formSchemas: null,
      submitUrl: "",
      previewUrl: "",
      previewId: "",
      isPreview: false,
      reloadUrl: "",
      lookupData: {}
    };
  },
  beforeMount() {
    var element = document.getElementById("form-app");
    const dataModel = element.dataset.model;
    if(dataModel) {
      let model = JSON.parse(dataModel);
      this.formSchemas = model.formSchemas;
      this.model = model.user;
      this.lookupData = model.lookupData || {};
    }
    
    this.model.guests = {};
    this.submitUrl = element.attributes["data-submit-url"].value;

    this.reloadUrl = element.dataset.reloadUrl;

 

    const previewModelData = element.dataset.previewModel;
    if(previewModelData) {
      const previewModel = JSON.parse(previewModelData);
      this.previewId = previewModel.id;
      this.model = previewModel.user;
    }

    this.previewUrl = element.dataset.previewUrl;
    this.isPreview = element.dataset.isPreview;
    const $this = this;
    if(this.isPreview) {
      window.addEventListener("storage", (ev) => {
        const k = `formpreview-${$this.previewId}`;
        console.log(k);
        if (ev.key.indexOf(k) !== -1) {
          console.log("hey this is storage")
          this.getPreview(JSON.parse(ev.newValue));
        }
      });
    }
    
  },
  computed: {},
  methods: {
    reloadPage() {
      if(this.reloadUrl)
        window.location.href = this.reloadUrl;
      else
        window.location.reload();
    },
    getSchemaForUser(user) {
      return this.formSchemas[user.registrationTypeId];
    },
    getPreview(model) {
      axios.post(this.previewUrl, model)
      .then(e => {
        this.formSchemas = e.data;
      });
    },
  },
};
</script>

<style>
.text-blue {
  color: #28a5ff;
}

.pointer {
  cursor: pointer;
}

.bold {
  font-weight: bold;
}

.relative {
  position: relative;
}
</style>
