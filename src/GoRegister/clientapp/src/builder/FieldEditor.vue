<template>
  <div class="mt-3">
    <div v-if="field.isForPresentation" class="form">
      <div class="row">
        <div class="col-md-8">
          <component
            :is="fieldTypeConfig.template"
            :field="field"
            v-if="fieldTypeConfig.template"
          ></component>
        </div>
        <div class="col">
          <div class="form-group">
            <label>Width</label>
            <div class="checkbox">
              <label>
                <input v-model="field.cols" :value="12" type="radio" />
                100%
              </label>
              <label>
                <input
                  v-model="field.cols"
                  :value="6"
                  type="radio"
                  class="ml-2"
                />
                50%
              </label>
              <label>
                <input
                  v-model="field.cols"
                  :value="4"
                  type="radio"
                  class="ml-2"
                />
                33.3%
              </label>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div v-else class="form">
      <div class="row">
        <div class="col-md-4">
          <div class>
            <input
              v-model="field.name"
              type="text"
              class="form-control"
              placeholder="Name"
              @change="updateDataTag"
            />
          </div>
          <div class="form-group">
            <label>Data Tag</label>
            <input
              v-model="field.dataTag"
              :readonly="isDataTagReadonly"
              type="text"
              class="form-control"
            />
          </div>
          <div class="form-group" v-if="isRegistrationTypesEnabled">
            <label>Registration Types</label>
            <div
              v-for="(regType, key) in registrationTypes"
              :key="key"
              class="checkbox"
            >
              <label>
                <input
                  v-model="field.registrationTypes"
                  :value="key"
                  type="checkbox"
                />
                {{ regType.name }}
              </label>
            </div>
          </div>
        </div>
        <div class="col-md-2 border-left border-grey">
            <div class="form-group">
                <div class="form-check">
                    <input v-model="field.isHidden"
                           class="form-check-input"
                           type="checkbox" />
                    <label class="form-check-label">Hidden </label>
                </div>
            </div>
            <div class="form-group">
                <div class="form-check">
                    <input v-model="field.isReadOnly"
                           class="form-check-input"
                           type="checkbox" />
                    <label class="form-check-label">Readonly</label>
                </div>
            </div>
            <div class="form-group">
                <div class="form-check">
                    <input v-model="field.isMandatory"
                           class="form-check-input"
                           type="checkbox" />
                    <label class="form-check-label">Required</label>
                </div>
            </div>
            <div class="form-group">
                <div class="form-check" v-if="DisplayTPNList(field.dataTag)">
                    <input v-model="field.allowTPNCountries"
                           class="form-check-input"
                           type="checkbox" />
                    <label class="form-check-label">Allow TPN Countries</label>
                </div>
            </div>
        </div>
        <div class="col-md-4 border-left border-grey">
          <component
            :is="fieldTypeConfig.template"
            :field="field"
            v-if="fieldTypeConfig.template"
          ></component>
        </div>
      </div>
    </div>
    <div class="mt-4">
      <button @click="showMore = !showMore" class="btn btn-outline-primary">
        + More
      </button>
      <button @click="showRulesModal = true" class="btn btn-outline-primary">
        Display Rules
      </button>
            <FieldRules @resetOptions="resetOptions" :fieldId="field.id" :rules="field.rules" />
    </div>

    <div v-show="showMore">
      <div class="row">
        <div class="col-md-8">
          <div class="form-group">
            <label>Help Text After</label>
            <input
              v-model="field.helpTextAfter"
              type="text"
              class="form-control"
            />
          </div>
          <div class="form-group">
            <label>Help Text Before</label>
            <input
              v-model="field.helpTextBefore"
              type="text"
              class="form-control"
            />
          </div>
          <div class="form-group">
            <label>Reporting Header</label>
            <input
              v-model="field.reportingHeader"
              type="text"
              class="form-control"
            />
          </div>
          <div class="form-group">
            <label>Validation Name</label>
            <input
              v-model="field.validationName"
              type="text"
              class="form-control"
            />
          </div>
        </div>
        <div class="col">
          <div class="form-group">
            <label>Width</label>
            <div class="checkbox">
              <label>
                <input v-model="field.cols" :value="12" type="radio" />
                100%
              </label>
              <label>
                <input
                  v-model="field.cols"
                  :value="6"
                  type="radio"
                  class="ml-2"
                />
                50%
              </label>
              <label>
                <input
                  v-model="field.cols"
                  :value="4"
                  type="radio"
                  class="ml-2"
                />
                33.3%
              </label>
            </div>
          </div>
        </div>
      </div>
    </div>
    <Modal v-show="showRulesModal">
      <ModalHeader title="Rules" v-on:close="showRulesModal = false" />
      <ModalBody>
        <div class="form-group">
          <div>
            <h4>New Rule</h4>
            <label
              >Display field <strong>'{{ field.name }}'</strong> when:</label
            >
            <select
              v-model="chosenRuleId"
              class="form-control"
              @change="chosenOptions = []"
            >
              <option :value="null"></option>
              <option
                :key="key"
                v-for="(rule, key) in fieldsWithOptions.filter(
                  (f) => f.id !== field.id
                )"
                :value="key"
              >
                {{ rule.name }}
              </option>
            </select>
            <div v-if="chosenRuleId != null">
              <p>Has these options:<br /></p>
              <label
                class="form-check-inline"
                :key="key"
                v-for="(option, key) in fieldsWithOptions[chosenRuleId].options"
                ><input
                  v-model="chosenOptions"
                  type="checkbox"
                  :value="option ? option.id : null"
                  :checked="isChecked()"
                />
                {{ option.description }}
              </label>
            </div>
          </div>
          <div v-if="field.rules.length > 0">
            <h4>Current Rules</h4>
            <FieldRules @resetOptions="resetOptions" :fieldId="field.id" :rules="field.rules" />
          </div>
        </div>
      </ModalBody>
      <ModalFooter confirmText="Save Rule" v-on:confirm="saveRules" />
    </Modal>
  </div>
</template>

<script>
// import { getters } from "./store";
import { fieldEditorComponents } from "./editorLoader";

import FieldRules from "./FieldRules";

const components = {
  ...fieldEditorComponents,
  FieldRules,
};

export default {
  components,
  data() {
    return {
      showRulesModal: false,
      chosenRuleId: null,
      chosenOptions: [],
    };
  },
  props: {
    field: { type: Object, required: true },
    showMore: { type: Boolean, required: false, default: false },
    // rulesList: { type: Array },
  },
  mounted: function () {
    this.chosenRuleId = this.chosenRule;
    this.chosenOptions = this.chosenRuleOptions;
  },
  methods: {
    isChecked() {
      return true;
    },
    resetOptions() {
      this.chosenOptions = []
      this.chosenRuleId = null
    },
    saveRules() {
      this.$store.dispatch("saveRules", {
        fieldId: this.field.id,
        chosenRuleId: this.chosenRuleId,
        chosenOptions: this.chosenOptions,
      });

      this.showRulesModal = false;
    },
    updateDataTag() {
      if (this.field.isNew) {
        this.field.dataTag = this.field.name.replace(/\W/g, "");
      }
      },
      DisplayTPNList(dataTag) {
          if (dataTag == 'ServicingCountry') {
             return true;
          }
          else {
              return false;
          }
      },
  },
  computed: {
    chosenRule() {
      return this.field.rules.length > 0
        ? this.fieldsWithOptions.findIndex(
            (r) => r.id == this.field.rules[0].fieldId
          )
        : null;
    },
    chosenRuleOptions() {
      return this.field.rules.length > 0 ? this.field.rules[0].optionIds : null;
    },
    registrationTypes() {
      return this.$store.getters.registrationTypes;
    },
    fieldsWithOptions() {
      return this.$store.getters.getFieldsWithOptions;
    },
    isRegistrationTypesEnabled() {
      return Object.keys(this.$store.getters.registrationTypes).length > 1;
    },
    fieldTypeConfig() {
      return this.$store.state.fieldTypes[this.field.fieldTypeId];
    },
    isDataTagReadonly() {
      return true;
      //return this.fieldTypeConfig.dataTagFixed.length > 0;
    },
  },
};
</script>