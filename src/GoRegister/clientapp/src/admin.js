// Packages
import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'


// Components
import App from './builder/App.vue'
import Queries from './queries/App.vue'
import DelegatesApp from "./delegates/App.vue"
import Emails from "./adminspa/main.js"

import Modal from './ui/modal/Modal.vue'
import ModalHeader from './ui/modal/ModalHeader.vue'
import ModalBody from './ui/modal/ModalBody.vue'
import ModalFooter from './ui/modal/ModalFooter.vue'

// State
import registrationState from './builder/store/state'
import registrationActions from './builder/store/actions'
import registrationMutations from './builder/store/mutations'
import registrationGetters from './builder/store/getters'

// gr
import { setupGrConfig } from "./shared/grconfig";
import gradmin from "./lib/gradmin";


Vue.config.productionTip = false
Vue.use(Vuex)
axios.defaults.headers.post['RequestVerificationToken'] = document.querySelector('[name="__RequestVerificationToken"]').value
axios.defaults.headers.put['RequestVerificationToken'] = document.querySelector('[name="__RequestVerificationToken"]').value
axios.defaults.headers.delete['RequestVerificationToken'] = document.querySelector('[name="__RequestVerificationToken"]').value


const store = new Vuex.Store({
  state: registrationState,
  actions: registrationActions,
  mutations: registrationMutations,
  getters: registrationGetters
})


export default function () {

  setupGrConfig();
  gradmin();

  // Global Components
  Vue.component('Modal', Modal)
  Vue.component('ModalHeader', ModalHeader)
  Vue.component('ModalBody', ModalBody)
  Vue.component('ModalFooter', ModalFooter)

  // Mounting Individual Apps
  if (document.getElementById("report-app")) {
    new Vue({
      render: h => h(Queries),
    }).$mount('#report-app')
  }

  if (document.getElementById("builder-app")) {
    new Vue({
      render: h => h(App),
      store: store
    }).$mount('#builder-app')
  }

  if(document.getElementById("delegates-app")) {
    new Vue({
      render: h => h(DelegatesApp),
      store: store
    }).$mount('#delegates-app')
  }

  Emails()
}