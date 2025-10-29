import Vue from 'vue'
import App from './App.vue'
import LanguageSelect from './components/LanguageSelect.vue'
import axios from 'axios'

Vue.config.productionTip = false
axios.defaults.headers.post['RequestVerificationToken'] = document.querySelector('[name="__RequestVerificationToken"]').value
axios.defaults.headers.put['RequestVerificationToken'] = document.querySelector('[name="__RequestVerificationToken"]').value
axios.defaults.headers.delete['RequestVerificationToken'] = document.querySelector('[name="__RequestVerificationToken"]').value

export default function() {
  if(document.getElementById("form-app")) {
    new Vue({
      render: h => h(App),
    }).$mount('#form-app')
  }

  document.addEventListener('DOMContentLoaded', function() {
    const el = document.getElementById('language-app');
    if (!el) return;

    const raw = el.getAttribute('data-languages');   
    let languages = [];
    try {
      languages = JSON.parse(raw) || [];
       console.log(languages);
    } catch (e) {
      console.error('Failed to parse languages JSON:', e);
    }

    const defaultLang = "en-us";

    new Vue({
      render: h => 
        h(LanguageSelect, {
          props: {
            languages,
            value: defaultLang
          }
        })
    }).$mount('#language-app')
  });

}

