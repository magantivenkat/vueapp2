<template>
  <div>
    <div class="row">
      <div class="col-8">
        <h2>Registration Emails</h2>
        <list-email-template
          v-for="(tpl, i) in model.registrationEmails"
          :key="i"
          v-bind="tpl"
        ></list-email-template>
        <hr />
        <router-link class="btn btn-outline-primary float-right mr-2"
                     :to="{ name: 'email-create', params: { emailTypeId: 6 } }">Add</router-link>

          <h2>Custom Emails</h2>
         
          

           
        <list-email-template 
          v-for="(tpl, i) in model.customEmails"
          :key="i"
          v-bind="tpl"
        ></list-email-template>
      </div>
      <div class="col">
        <h2>Layouts</h2>
        <div class="card mb-2">
          <div class="card-body">
              <router-link
                v-for="(et, i) in model.emailLayouts"
                :key="i"
                class="d-block mb-2"
                :to="{
                  name: 'email-edit-layout',
                  params: { emailId: et.id }
                }"
                >{{ et.name }}</router-link
              >
              <router-link
                class="btn btn-block btn-primary mt-3"
                :to="{ name: 'email-create-layout'}"
                >Add Email Layout</router-link
              >
              <!-- <button class="btn btn-block btn-primary">
                Add Email Layout
              </button> -->
          </div>
        </div>
        <h2>History</h2>
        <div class="card mb-2">
          <div class="card-body font-italic">Coming soon</div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { getTemplates } from "../api";
import ListEmailTemplate from "../components/ListEmailTemplate.vue";

export default {
  components: { ListEmailTemplate },
  data: () => ({
    model: {
      registrationTemplates: [],
      customTemplates: [],
    },
  }),
  async created() {
    this.fetchData();
  },
  methods: {
    fetchData() {
      const $this = this;

      getTemplates().then((result) => {
        $this.model = result;
      });
    },
  },
};
</script>