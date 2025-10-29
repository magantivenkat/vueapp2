<template>
  <div class="container">
    <div class="bs-managed-form">
      <formal-form
        :schema="getSchemaForUser(model)"
        :schemas="formSchemas"
        :model="model"
        :modelPath="'model'"
        :allowGuest="false"
        :guestFormInModal="true"
        :submitUrl="submitUrl"
        :isAdmin="true"
        @submission-success="reloadPage"
      />
    </div>

    <div v-if="displayReview">
      <ul></ul>
    </div>
  </div>
</template>

<script>
import Vue from "vue";
import Vuelidate from "vuelidate";
//import FormGenerator from "./components/FormGenerator.vue";
import FormalForm from "../components/FormalForm";

Vue.use(Vuelidate);

export default {
  name: "App",
  components: {
    FormalForm,
  },
  data: function () {
    return {
      displayReview: false,
      model: {},
      formSchemas: {},
      submitUrl: "",
    };
  },
  beforeMount() {
    var element = document.getElementById("delegates-app");
    let model = JSON.parse(element.attributes["data-model"].value);
    this.formSchemas = model.formSchemas;
    this.model = model.user;
    this.model.guests = {};
    this.submitUrl = element.attributes["data-submit-url"].value;
  },
  computed: {},
  methods: {
    reloadPage() {
      window.location.reload();
    },
    getSchemaForUser(user) {
      return this.formSchemas[user.registrationTypeId];
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
