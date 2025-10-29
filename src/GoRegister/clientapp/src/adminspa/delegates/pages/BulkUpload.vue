<template>
  <div>
    <h1>Bulk Upload</h1>
    <hr />
    <div v-if="state === 'upload'">
      <g-error :result="error" />
      <dl>
        <dt>
          <label>Select file</label>
        </dt>
        <dd>
          <input
            type="file"
            ref="file"
            v-on:change="handleFileUpload()"
          />
          <span></span>
        </dd>
      </dl>
      <div>
        <ul>
          <li>Only the first sheet will be processed</li>
          <li>Row 1 should contain the field headers</li>
          <li>On the next page you will be able to map columns to fields</li>
          <li>If your event has multiple Registration Types you may add a column with the header <strong>Registration Type</strong> and fill this with the attendees Registration Type</li>
        </ul>
      </div>
      <button class="btn btn-primary" v-on:click="submitFile()">Next</button>
      <hr />
      <a class="btn btn-outline-primary" :href="`${$baseProjectUrl}/delegates/delegatetemplate`">Bulk upload template</a>
    </div>
    <div v-if="state === 'map'">
      <BulkUploadMapper :mappingModel="mappingModel"></BulkUploadMapper>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import BulkUploadMapper from "../components/BulkUploadMapper";
import GError from "@/shared/components/GError";

export default {
  components: {
    BulkUploadMapper,
    GError
  },
  data() {
    return {
      file: "",
      state: "upload",
      mappingModel: null,
      error: false
    };
  },

  methods: {
    submitFile() {
      let formData = new FormData();
      formData.append("file", this.file);
      const $this = this;
      axios
        .post("/delegates/bulkupload", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        })
        .then(function (response) {
          $this.mappingModel = response.data;
          $this.state = "map";
        })
        .catch(function (error) {
          console.log(error.response)
          $this.error = error.response.data;
        });
    },
    handleFileUpload() {
      this.file = this.$refs.file.files[0];
    },
  },
};
</script>