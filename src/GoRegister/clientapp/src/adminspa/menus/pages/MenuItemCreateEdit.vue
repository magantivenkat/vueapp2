<template>
  <div v-if="loaded">
    <h1>{{ getPageTitle }} Menu Item</h1>
    <hr />
    <formal :vuelidate="$v" @submit="handleSubmit">
      <select-list
        :options="viewModel.menuItemTypes"
        v-model="model.menuItemType"
        label="Menu Item Type"
      ></select-list>
      <div v-if="model.menuItemType != null">
        <select-list
          v-if="isType(_menuItemType.customPage)"
          :options="viewModel.customPages"
          v-model="model.customPageId"
          label="Custom Pages"
        ></select-list>

        <text-input v-model="model.label" label="Label" />
        <span v-if="isType(_menuItemType.customPage)" class="text-muted"
          >Leave this blank to use the Custom Page title as the menu label</span
        >
        <text-input v-model="model.description" label="Description" />

        <text-input
          v-if="isType(_menuItemType.link)"
          v-model="model.anchorLink"
          label="Absolute URL"
        />

        <div class="form-group">
          <div class="form-check">
            <input
              class="form-check-input"
              type="checkbox"
              v-model="model.openInNewTab"
              id="openInNewTab"
            />
            <label class="form-check-label" for="openInNewTab">
              Open link in new tab
            </label>
          </div>
        </div>

        <text-input v-model="model.fragment" label="Hash Parameter" />
        <text-input v-model="model.cssClass" label="HTML Class" />

        <h5>Display Rules</h5>
        <div v-if="isType(_menuItemType.customPage)">
          The display rules will be inherited from the custom page display rules
        </div>
        <div v-else-if="isType(_menuItemType.register) || isType(_menuItemType.viewRegistration)">
          The display rules will be based on the current attendee's registration status
        </div>
         <div v-else-if="isType(_menuItemType.sessions)">
          This menu item will be displayed to confirmed attendees
        </div>
        <div v-else>
          <div class="row">
            <div class="col">
              <div class="form-group">
                <label for="">Registration Types</label>
                <select
                  class="custom-select"
                  multiple
                  v-model="model.registrationTypes"
                >
                  <option
                    v-for="(rt, i) in viewModel.registrationTypes"
                    :value="rt.value"
                    :key="i"
                  >
                    {{ rt.text }}
                  </option>
                </select>
              </div>
            </div>
            <div class="col">
              <div class="form-group">
                <label for="">Registration Statuses</label>
                <select
                  class="custom-select"
                  multiple
                  v-model="model.registrationStatuses"
                >
                  <option value="0">Not Invited</option>
                  <option value="1">Invited</option>
                  <option value="2">Confirmed</option>
                  <option value="3">Declined</option>
                  <option value="4">Cancelled</option>
                  <option value="5">Waiting</option>
                </select>
              </div>
            </div>
          </div>
        </div>
      </div>

      <hr />
      <div class="form-group">
        <input type="submit" class="btn btn-primary" value="Save" />
      </div>
    </formal>
  </div>
</template>
<script>
import { required, requiredIf, url } from "vuelidate/lib/validators";
import Formal from "@/components/Formal.vue";
import SelectList from "@/components/fields/SelectList.vue";
import { menuItemType } from "../enums.js";
import {
  getMenuItemEditModel,
  getMenuItemCreateModel,
  createMenuItem,
  updateMenuItem,
} from "../api";

export default {
  components: { Formal, SelectList },
  props: {
    id: Number,
  },
  async created() {
    await this.fetchData();
    this._menuItemType = menuItemType;
  },
  data() {
    return {
      model: {},
      viewModel: {},
      loaded: false,
    };
  },
  methods: {
    async fetchData() {
      const $this = this;
      if (this.isEdit) {
        getMenuItemEditModel(this.id).then((result) => {
          this.model = result.model;
          delete result.model;
          this.viewModel = result;
          this.loaded = true;
        });
      } else {
        getMenuItemCreateModel().then((result) => {
          $this.model = result.model;
          this.model.menuItemType = null; // bit of a hack to make them select a property
          delete result.model;
          $this.viewModel = result;
          $this.loaded = true;
        });
      }
    },
    async handleSubmit() {
      if (this.isEdit) {
        updateMenuItem(this.model).then(() => {
          this.$root.$bvToast.toast(`Your menu item has been created.`, {
            title: "Saved",
            autoHideDelay: 3000,
            variant: "success",
          });
          this.$router.push({
            name: "menu-list",
          });
        });
      } else {
        createMenuItem(this.model).then((result) => {
          this.$root.$bvToast.toast(`Your menu item has been updated.`, {
            title: "Saved!",
            autoHideDelay: 3000,
            variant: "success",
          });
          this.$router.push({
            name: "menu-list",
          });
        });
      }
    },
    isType(menuItemType) {
      return this.model.menuItemType === menuItemType;
    }
  },
  computed: {
    isEdit() {
      return this.id ? true : false;
    },
    getPageTitle() {
      return this.isEdit ? "Edit" : "Create";
    },
  },
  validations() {
    return {
      model: {
        menuItemType: { required },
        label: {
          required: requiredIf(function (form) {
            return this.model.menuItemType !== 0; // is custom page link
          }),
        },

        // custom page
        customPageId: {
          required: requiredIf(function (form) {
            return this.model.menuItemType === 0; // is custom page link
          }),
        },

        // link
        anchorLink: {
          required: requiredIf(function (form) {
            return this.model.menuItemType === 1; // is link
          }),
          url,
        },
      },
    };
  },
  watch: {
    async $route() {
      await this.fetchData();
    },
  },
};
</script>

<style>
</style>