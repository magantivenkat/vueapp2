<template>
  <div>
    <h1>Review</h1>
    <hr />
    <div v-if="loaded">
      <div class="mb-3">
        <button @click="send" class="btn btn-primary">Send {{ emails.length }} email(s)</button>
      </div>
      <table class="table table-striped">
        <thead>
          <tr>
            <th>Email</th>
            <th>Subject</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="email in emails" :key="email.id">
            <td v-text="email.to"></td>
            <td v-text="email.subject"></td>
            <td><a :href="email.previewUrl" class="btn btn-sm btn-outline-primary" target="_blank">Preview</a></td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import { getPreviewEmails, sendPreviewEmails } from "../api";

export default {
  props: {
    id: String,
  },
  data: () => ({
    loaded: false,
    emails: []
  }),
  created() {
    this.fetchData();
  },
  methods: {
    fetchData() {
      getPreviewEmails(this.id).then((data) => {
        this.emails = data.emails;
        this.loaded = true;
      }).catch((error) => {});
    },
    send() {
      sendPreviewEmails(this.id).then(data => {
        this.$router.push({ name: "email-sent", params: { id: this.id } });
      }).catch(error => console.log(error));
    },
    getUrl(id) {
      return this.$router.options.base + "/previewemail/" + id;
    }
  },
};
</script>