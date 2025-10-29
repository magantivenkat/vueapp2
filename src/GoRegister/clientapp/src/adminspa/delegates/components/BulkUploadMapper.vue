<template>
  <div>
    <div v-if="upload">
      <b-alert
            variant="success"
            dismissible
            show
            >Bulk Upload file was successfully uploaded</b-alert
          >
    </div>
    <div v-else>
      <div v-if="validationResult">
        <template v-if="validationResult.isValid">
          <b-alert
            variant="success"
            dismissible
            :show="validationResult.isValid"
            >Bulk Upload file is valid and can be uploaded</b-alert
          >
        </template>
        <template v-else>
          <b-alert
            variant="danger"
            v-for="(error, i) in validationResult.errors"
            :key="i"
            show
          >
            {{ error.message }}
            <ul>
              <li v-for="(detail, i) in error.errorDetails" :key="i">
                {{ detail }}
              </li>
            </ul>
          </b-alert>
        </template>
      </div>
    <div>
      <p v-if="model.registrationTypeStatus === 0">
        All attendees will be uploaded with the default Registration Type
      </p>
      <div v-else-if="model.registrationTypeStatus === 1">
        Attendees will be uploaded with the Registration Type specified in the upload sheet
      </div>
      <div v-else-if="model.registrationTypeStatus === 2">
        <select-list label="Please select a Registration Type" v-model="model.registrationTypeId" :options="viewModel.registrationTypes" defaultOption="Please select"></select-list>
      </div>
    </div>
      <table class="table table-striped">
        <thead>
          <th>Column</th>
          <th>Registration Field</th>
        </thead>
        <tbody>
          <tr v-for="(col, i) in model.headerMappings" :key="i">
            <td class="align-middle">{{ col.columnName }}</td>
            <td>
              <select v-model="col.fieldId" class="form-control">
                <option :value="null">Select field</option>
                <option
                  v-for="(field, fi) in viewModel.fields"
                  :key="fi"
                  :value="field.id"
                >
                  {{ field.name }}
                </option>
              </select>
            </td>
          </tr>
        </tbody>
      </table>
      <div>
        <button
          @click="submitValidation"
          type="button"
          class="btn btn-outline-primary mr-2"
        >
          Validate
        </button>
        <button @click="submit" type="button" class="btn btn-primary">
          Validate and Upload
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  props: {
    mappingModel: Object,
  },
  data: () => ({
    model: {
      registrationTypeStatus: 0,
      registrationTypeId: null,
      registrationTypeColumnIndex: null,
      headerMappings: [],
      emailFieldId: null,
    },
    viewModel: {},
    validationResult: null,
    upload: null,
  }),
  created() {
    this.viewModel = this.mappingModel.viewModel;
    this.model = this.mappingModel.model;
  },
  methods: {
    submitValidation() {
      const $this = this;
      axios
        .post("/delegates/validateBulkUpload", this.model)
        .then(function (response) {
          $this.validationResult = response.data;
        })
        .catch(function (error) {});
    },
    submit() {
      const $this = this;
      axios
        .post("/delegates/executeBulkUpload", this.model)
        .then(function (response) {
          if (response.data.isValid) {
            $this.validationResult = null;
            $this.upload = {};
          } else {
            $this.validationResult = response.data.validationResult;
          }
        })
        .catch(function (error) {});
    }
  },
};
</script>