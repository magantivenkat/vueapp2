<template>
  <div>
    <div class="row">
      <div class="col">
        <div class="form-group mb-4">
          <Autocomplete
            :search="search"
            placeholder="Search Users"
            aria-label="Search Users"
            :debounce-time="500"
            :get-result-value="getResultValue"
            @submit="onSubmit"
            ref="search-users"
          ></Autocomplete>
        </div>
        <div class="form-group">
          <label>Emails</label>
          <textarea
            v-model="emailList"
            class="form-control"
            rows="5"
          ></textarea>
        </div>
        <div class="form-group">
          <button
            :disabled="!emailList"
            @click="addEmailList"
            class="btn btn-outline-primary"
          >
            Add
          </button>
        </div>
      </div>
      <div class="col ml-4">
        <div class="row">
          <div class="col">
            <div class="form-group">
              <label>Registration Statuses</label>
              <div
                v-for="(rs, i) in viewModel.registrationStatuses"
                :key="i"
                class="form-check"
              >
                <input
                  class="form-check-input"
                  type="checkbox"
                  :value="rs.id"
                  :id="'regstatus-' + rs.id"
                />
                <label class="form-check-label" :for="'regstatus-' + rs.id">
                  {{ rs.name }}
                </label>
              </div>
            </div>
          </div>
          <div class="col">

          </div>
        </div>
        <div>
          <hr />
          <h5>Custom Filters</h5>
          <div>
            <FilterList :filters="viewModel.filters" :selectedFilters="filters" @filterSaved="saveFilter" /> 
          </div>
          <hr />
          <div class="form-group">
            <button @click="customSearch" class="btn btn-outline-primary">Add filtered users</button>
          </div>
        </div>
      </div>
    </div>
    <hr />
    <div>
      <div class="mb-3">
        <button :disabled="users.length === 0" @click="generateEmails" class="btn btn-lg btn-primary">
          {{ sendEmailsButtonText }}
        </button>
      </div>
      <table class="table table-striped">
        <thead>
          <tr>
            <td>Name</td>
            <td>Email</td>
            <td>Registration Type</td>
            <td>Registration Status</td>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(user, i) in users" :key="i">
            <td>{{ user.fullName }}</td>
            <td>{{ user.email }}</td>
            <td>{{ user.registrationType }}</td>
            <td>{{ user.registrationStatus }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>


<script>
import Autocomplete from "@trevoreyre/autocomplete-vue";
import * as searchApi from "../searchapi";
import { generatePreviewEmails, getSendEmailModel } from "../api";
import FilterList from '@/queries/components/FilterList.vue';

export default {
  components: {
    Autocomplete,
    FilterList
  },
  props: {    
    id: String,
  },
  data: () => ({
    users: [],
    emailList: "",
    selectedRegistrationStatuses: [],
    filters: [],
    viewModel: {
      filters: [],
      registrationStatuses: [
        {
          id: 0,
          name: "Not Invited",
        },
        {
          id: 1,
          name: "Invited",
        },
        {
          id: 2,
          name: "Confirmed",
        },
        {
          id: 3,
          name: "Declined",
        },
        {
          id: 4,
          name: "Cancelled",
        },
      ],
    },
  }),
  async created() {
    await this.fetchData();
  },
  methods: {
    fetchData() {
      getSendEmailModel(this.id)
        .then(data => {
          this.viewModel.filters = data.filters;
        })
    },
    addUser(user) {
      if (this.users.some((e) => e.id === user.id)) {
        return;
      }

      this.users.unshift(user);
    },
    search(input) {
      return searchApi.searchUsers(input);
    },
    customSearch() {
      searchApi.customSearch(this.filters)
        .then(data => {
          this.filters = []

          if(!data.results.length) {
            return;
          }

          for (let index = 0; index < data.results.length; index++) {
            const user = data.results[index];
            this.users.push({
              id: user.id,
              fullName: `${user.firstName} ${user.lastName}`,
              firstName: user.firstName,
              lastName: user.lastName,
              email: user.email,
              registrationType: user.name,
              registrationStatus: user.description
            })
          }
        })
    },
    getResultValue(result) {
      return result.fullName;
    },
    onSubmit(result) {
      this.addUser(result);
    },
    saveFilter(filter) {
      if(filter.index >= 0) {
        this.filters.$set(filter.index, filter.model);
      } else {
        this.filters.push(filter.model);
      }
    },
    addEmailList() {
      if (this.emailList === "") return;

      const emails = this.emailList.split(/\r?\n/);

      searchApi.getUsersIdsFromEmails(emails).then((result) => {
        this.users.push(...result.users);
        this.emailList = "";
      });
    },
    async addFilteredUsers() {
      
    },
    generateEmails() {
      generatePreviewEmails({
          userIds: this.users.map(e => e.id),
          emailId: this.id
        }).then((result) => {
          this.$router.push({ name: "email-preview", params: { id: result } });
        }).catch((error) => {
          alert(error)
        })
    }
  },
  computed: {
    sendEmailsButtonText() {
      return this.users.length
        ? `Review emails to ${this.users.length} users`
        : "No users selected";
    },
  },
};
</script>