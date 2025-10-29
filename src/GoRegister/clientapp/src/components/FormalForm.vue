<template>
  <div>
    <div v-if="!isReviewScreen">


      <!-- <div class="formal-form-wrapper">
        <div class="formal-form-topbar">
          <language-select v-if="lookupData && lookupData.languages && lookupData.languages.length"
            :languages="lookupData.languages" v-model="model.preferredLanguage" />
        </div>
      </div> -->


      <div class>
        <form-generator :v="$v.model" :schema="schemas[model.registrationTypeId]" :schemas="schemas" :model="model"
          :modelPath="'model'" :hideButtons="isAdmin" @on-submit="submit" :isAdmin="isAdmin" />
      </div>
      <div v-if="allowGuest" class>
        <div class>
          <hr />
          <h4>Guests</h4>
          <div v-if="hasGuests" class>
            <div v-for="(guest, key, i) in model.guests" :key="i" class="card mb-3">
              <div class="card-body">
                <div class="clearfix">
                  <div class="float-right">
                    <button @click="removeGuest(key)" class="btn btn-outline-danger">
                      Remove
                    </button>
                  </div>
                  <h5>Guest {{ i + 1 }}</h5>
                </div>
                <form-generator :schema="schemas[guest.registrationTypeId]" :schemas="schemas" :model="guest"
                  modelPath="model" :v="$v.model.guests[key]" v-if="!guestFormInModal" />
              </div>
            </div>
          </div>
          <div v-if="canStillAddGuest" class>
            <input type="button" value="Add guest" @click="addGuest" class="btn btn-outline-primary" />
          </div>
        </div>
      </div>
    </div>
    <div v-if="isReviewScreen">
      <!-- <FormSummary
        :schema="schemas[model.registrationTypeId]"
        :model="model"
        modelPath="model"
      /> -->
    </div>
    <div v-if="!isAdmin">
      <!-- <button @click="review" class="btn btn-primary" v-if="!isReviewScreen">
        Review
      </button>
      <button @click="submit" class="btn btn-primary" v-if="isReviewScreen">
        Submit
      </button> -->
    </div>
    <!--<div v-else>
      <hr />
      <button @click="submit" class="btn btn-primary">Update</button>
    </div>-->
    <div class="modal-mask" v-if="editGuestModel" id tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel"
      aria-hidden="true">
      <div class="modal-wrapper">
        <div class="modal-dialog">
          <div class="modal-content">
            <div class="modal-header">
              <h5 class="modal-title" id="exampleModalLabel">Edit Guest</h5>
              <button type="button" class="close" v-on:click="editGuestModel = null" aria-label="Close">
                <span aria-hidden="true">&times;</span>
              </button>
            </div>
            <div class="modal-body">
              <form-generator :schema="schemas[editGuestModel.registrationTypeId]" :schemas="schemas"
                :model="editGuestModel" modelPath="model" :v="$v.model.editGuest" />
            </div>
            <div class="modal-footer">
              <button v-on:click="saveGuestFromModal" type="button" class="btn btn-primary">
                Save
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="modal fade" id="staticBackdrop" data-backdrop="static" data-keyboard="false" tabindex="-1"
      aria-labelledby="staticBackdropLabel" aria-hidden="true">
      <div class="modal-dialog modal-xl">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title" id="staticBackdropLabel">Modal title</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
              <span aria-hidden="true">&times;</span>
            </button>
          </div>
          <div class="modal-body"></div>
          <div class="modal-footer">
            <button type="button" class="btn btn-primary">Save</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import FormGenerator from "./FormGenerator";
import FormSummary from "./FormSummary";
import {
  //getFieldSummaryValue,
  shouldDisplayField,
} from "../utils/fieldsHelper";
import { generateVuelidateConfiguration } from "../utils/validationHelper";
import axios from "axios";

const components = {
  FormGenerator,
  FormSummary
};

export default {
  props: {
    model: {
      type: Object,
      required: true,
    },
    allowGuest: Boolean,
    guestFormInModal: Boolean,
    modelPath: String,
    schema: {
      type: Object,
      required: true,
    },
    schemas: Object,
    submitUrl: String,
    isAdmin: Boolean
  },
  components,
  data() {
    return {
      fg: this,
      isBringingGuest: false,
      guestKeyCount: 0,
      editGuestModel: null,
      isReviewScreen: false,
    };
  },
  mounted() {
    this.guestKeyCount = Object.keys(this.model.guests).length;
  },
  methods: {
    getFieldDisplayName(field) {
      return field.name;
    },
    getValidation(fieldId) {
      var v = this.$v.model[fieldId];
      if (v) return v;

      return {};
    },
    addGuest() {
      if (this.guestFormInModal) {
        this.editGuestModel = {
          id: null,
          isGuest: true,
          registrationTypeId: this.model.registrationTypeId,
          model: {},
        };
      } else {
        this.$set(this.model.guests, this.guestCount, {
          id: null,
          isGuest: true,
          registrationTypeId: this.model.registrationTypeId,
          model: {},
        });
        this.guestKeyCount++;
      }
    },
    saveGuestFromModal() {
      this.$set(this.model.guests, this.guestCount, { ...this.editGuestModel });
      this.guestKeyCount++;
      this.editGuestModel = null;
    },
    removeGuest(key) {
      this.$delete(this.model.guests, key);
    },
    buildFormDisplaySchema(schema, model) {
      let fieldsToDisplay = [];
      for (let index = 0; index < schema.fields.length; index++) {
        const field = schema.fields[index];
        if (shouldDisplayField(field.key, schema, model)) {
          fieldsToDisplay.push(field.key);
        }
      }
      return {
        fieldsToDisplay: fieldsToDisplay,
      };
    },
    submit: function () {
      // only validate on frontend
      if (!this.isAdmin && this.$v.$invalid) {
        return alert("Form invalid");
      }


      axios
        .post(this.submitUrl, { user: this.model })
        .then((response) => {
          let swal;
          if (typeof window !== 'undefined' && typeof document !== 'undefined') {
            swal = require('sweetalert2').default;
          }

          if (response.data.validationContext.isValid) {
            swal.fire({
              title: 'Thank you! Your request has been submitted',
              text: 'A Meeting Planning team will contact you shortly',
              confirmButtonText: 'Close',
              showLoaderOnConfirm: false
            }).then((result) => {
              if (result.value) {
                this.$emit("submission-success");
              }
            })
          }
          else {
            swal.fire({
              title: '',
              text: response.data.validationContext.errors['423'],
              confirmButtonText: 'Close',
              icon: 'error'
            })
          }
        }
        ).catch((error) => {
          let swal;
          if (typeof window !== 'undefined' && typeof document !== 'undefined') {
            swal = require('sweetalert2').default;
          }
          swal.fire({
            title: "",
            text: "There is an issue submitting this meeting request form, please contact your Meeting Planning team for further assistance",
            icon: 'error'
          })
        }
        );
    },
    review: function () {
      this.validateDisplay();
      if (!this.$v.$invalid) {
        this.isReviewScreen = true;
      }
    },
    validateDisplay() {
      this.$v.$touch();
    },
  },
  computed: {
    dataModel() {
      if (this.modelPath) {
        return this.model[this.modelPath];
      }

      return this.model;
    },
    guestCount() {
      return Object.keys(this.model.guests).length;
    },
    hasGuests() {
      return this.guestCount > 0;
    },
    canStillAddGuest() {
      return /* how many guests are */ 2 > this.guestCount;
    },
    displaySchema() {
      let displaySchema = {
        guests: [],
      };

      // apply rules about which fields to display for user
      displaySchema.user = this.buildFormDisplaySchema(
        this.schemas[this.model.registrationTypeId],
        this.model.model
      );

      // apply rules which fields to display for guest
      for (const [key, guest] of Object.entries(this.model.guests)) {
        displaySchema.guests[key] = this.buildFormDisplaySchema(
          this.schemas[guest.registrationTypeId],
          guest.model
        );
      }

      return displaySchema;
    },
  },
  validations() {
    let validationModel = {
      model: {},
      guests: {},
      editGuest: {
        model: {},
      },
    };

    // for (let index = 0; index < this.fields.length; index++) {
    //   const field = this.fields[index];
    //   validationModel.model[field.id] = {};
    // }

    let generateValidation = function (schema, displaySchema) {
      let vuelidateConfig = {};
      for (let index = 0; index < schema.fields.length; index++) {
        const field = schema.fields[index];
        // is this field being displayed, so do we need to display it?
        if (!displaySchema.fieldsToDisplay.includes(field.key)) continue;

        if (field.validations) {
          vuelidateConfig[field.key] = generateVuelidateConfiguration(field);
        }
      }

      return vuelidateConfig;
    };

    validationModel.model = generateValidation(
      this.schemas[this.model.registrationTypeId],
      this.displaySchema.user
    );

    if (this.allowGuest) {
      for (const [key, guest] of Object.entries(this.model.guests)) {
        validationModel.guests[key] = {
          model: generateValidation(
            this.schemas[guest.registrationTypeId],
            this.displaySchema.guests[key]
          ),
        };
      }
    }

    return { model: validationModel };
  },
};
</script>

<style>
.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}
</style>