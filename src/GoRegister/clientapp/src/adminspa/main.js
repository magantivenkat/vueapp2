/*
MRF Changes: Add new package for popup alertbox
Modified Date: 18th October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New
*/

// import the vue instance
import Vue from "vue";
// import the App component
import App from "./App";
// import the vue router
import VueRouter from "vue-router";
import routes from "./routes";
import Formal from "../components/Formal";
import Vuelidate from "vuelidate";
import axios from "axios";
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue';

//Make alert for MRF form page
import VueSweetalert2 from 'vue-sweetalert2';

// Make BootstrapVue available throughout your project
Vue.use(BootstrapVue)
// Optionally install the BootstrapVue icon components plugin
Vue.use(IconsPlugin)

Vue.use(Vuelidate);
Vue.use(Formal);

import VueSanitize from "vue-sanitize";

const defaultOptions = {
    allowedTags: ['abbr', 'a', 'acronym', 'address', 'area', 'article', 'aside', 'audio', 'b', 'bdi', 'bdo', 'big', 'blockquote', 'br', 'button', 'canvas', 'caption', 'center', 'cite', 'code', 'col', 'colgroup', 'datalist', 'dd', 'del', 'details', 'dfn', 'dir', 'div', 'dl', 'dt', 'em', 'fieldset', 'figcaption', 'footer', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'header', 'ins', 'hr', 'i', 'img', 'kbd', 'label', 'legend', 'li', 'map', 'mark', 'menu', 'meter', 'nav', 'ol', 'output', 'p', 'pre ', 'q', 's', 'samp', 'section', 'small', 'source', 'span', 'strike', 'strong', 'sub', 'summary', 'sup', 'table', 'tbody', 'td', 'tfoot', 'th', 'thead', 'time', 'tr', 'track', 'tt', 'u', 'ul', 'var', 'video'],
    allowedAttributes: {
        'table': ['style', 'cellpadding', 'cellspacing', 'border', 'align'],
        'td': ['style', 'cellpadding', 'cellspacing', 'border'],
        'tr': ['style', 'cellpadding', 'cellspacing', 'border'],
        'img': ['src'],
        'font': ['face']
    }
};

Vue.use(VueSanitize, defaultOptions);

//Make alert for MRF form page
Vue.use(VueSweetalert2);

const appElementId = "spa-app";

export default function() {
  const appElement = document.getElementById(appElementId);
  if (appElement) {
    const baseUrl = appElement.dataset.baseUrl;
    const baseApiUrl = appElement.dataset.baseApiUrl;
    // tell vue to use the router
    Vue.use(VueRouter);

    Vue.prototype.$baseProjectUrl = appElement.dataset.baseProjectUrl;

    axios.defaults.baseURL = baseApiUrl;

    const router = new VueRouter({
      routes, // short for routes: routes
      mode: "history",
      base: baseUrl,
      projectUrl: appElement.dataset.baseProjectUrl,
      scrollBehavior (to, from, savedPosition) {
        if (savedPosition) {
          return savedPosition
        } else {
          return { x: 0, y: 0 }
        }
      }
    });

    router.beforeEach((to, from, next) => {
      // if (to.path.startsWith("/project/") && to.params.projectId) {
      //   console.log("project: " + to.params.projectId);
      // }
      next();
    });

    new Vue({
      // define the selector for the root component
      el: `#${appElementId}`,
      render: (h) => h(App),
      // declare components that the root component can access
      components: { App },
      // pass in the router to the Vue instance
      router,
    }); //.$mount('#admin-app') // mount the router on the app
  }
}
