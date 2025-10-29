<template>
  <div>
    <h1>Sending emails</h1>
    <hr />
    <div v-if="total == 0"> 
      Sending emails
      <b-progress height="2rem" class="mt-2" :max="total" show-value>
        <b-progress-bar :value="100" variant="default" :label-html="`<span>Sending...</span>`" animated></b-progress-bar>
      </b-progress>
    </div>
    <div v-else> 
      {{toSend > 0 ? "Sending emails" : "Emails sent"}}
      <b-progress height="2rem" class="mt-2" :max="total" show-value>
        <b-progress-bar :value="sent" variant="success" :label-html="`<span>Sent: ${sent}</span>`"></b-progress-bar>
        <b-progress-bar :value="toSend" variant="default" :label-html="`<span>Sending: ${toSend}</span>`" animated></b-progress-bar>
        <b-progress-bar :value="failed" variant="danger" :label-html="`<span>Failed: ${failed}</span>`"></b-progress-bar>
      </b-progress>
    </div>
  </div>
</template>

<script>
import axios from "axios";

export default {
  
  data() {
    return {
      batchGuidId: null,
      toSend: null,
      sent: null,
      failed: null,
      total: null,
      pollInterval: null
    }
  },
  mounted() {
    var guid = this.$route.params.id
    this.batchGuidId = guid
    this.fetchEmailsToSend()
    //check if the status is completed, if not fetch data every 10minutes
    if(this.toSend != 0) {
      this.pollInterval = setInterval(this.fetchEmailsToSend, 5000) //save reference to the interval
      setTimeout(() => {clearInterval(this.pollInterval)}, 5000 * 12) //stop polling after a minute
    }
  },
  methods: {
    async fetchEmailsToSend() {
      var {data} = 
      await axios.get('/api/emails/CheckBatchStatus', {
        params: {
          batchId: this.batchGuidId
        }
      })

      this.toSend = data.toSend
      this.sent = data.sent
      this.failed = data.failed
      this.total = data.total

      if(data.toSend == 0 && data.total > 0) {
        clearInterval(this.pollInterval) //won't be polled anymore 
        this.toSend = 0; 
      }

    }
  },
  
}
</script>