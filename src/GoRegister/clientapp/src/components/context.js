/* MRF Changes: Reduce field size on User MRF Form page
Modified Date: 18th October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New */


import get from "lodash.get";

export default {
  context() {
    return {
      label: this.label || "",
      helpTextBefore: this.helpTextBefore,
      helpTextAfter: this.helpTextAfter,
      validatePropertyName: this.validatePropertyName,
      cols: this.cols || 12,
      //cols: this.cols || 7,
      readonly: this.readonly || false,
      v: this.v,
      validationName: this.validationName,
      getColumnsClass: getColumnsClass,
      mergedValidationName: getValidationName,
      id: this.id,
      dataTag: this.dataTag
    };
  },
  v
};

function getColumnsClass() {
  return `col-md-${this.cols}`;
}

function getValidationName() {
  return this.validationName || this.label;
}

function v() {
  const def = {
    $invalid: false,
    $dirty: false,
    $anyDirty: false,
    $error: false,
    $anyError: false,
    $pending: false,
    $touch: function () { },
    $params: []
  };

  if (this.vuelidate && this.formalConfig) {
    const vuelidateModel = this.formalConfig.nestedModel ?
      this.vuelidate[this.formalConfig.nestedModel] :
      this.vuelidate;

    if(this.validatePropertyName) {
      return get(vuelidateModel, this.validatePropertyName, def);
    }
    
    if (this.$options._parentVnode.data.model) {
      return get(vuelidateModel, this.$options._parentVnode.data.model.expression, def);
    }
  }

  return def;
}