<template>
  <div class="card mb-2">
        <div class="card-body">
          <div v-if="id" class="float-right">
            <router-link class="btn btn-outline-primary mr-2" :to="{ name: 'email-edit', params: { emailId: id } }">Edit</router-link>
            <router-link class="btn btn-primary" :to="{ name: 'email-send', params: { id: id } }">Send</router-link>
          </div>
          <button v-else @click="setup" class="float-right btn btn-primary">
            Setup
          </button>
          
          <h3>{{name}}</h3>
        </div>
      </div>
</template>

<script>
export default {
  props: {
    name: String,
    id: Number,
    emailTypeId: Number
  },
  methods: {
    setup() {
      this.$router.push({ name: "email-create", params: { emailTypeId: this.emailTypeId } });
    }
  }
}
</script>