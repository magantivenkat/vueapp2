export default {
  init(state, model) {
    state.fields = model.fields;
    state.registrationTypes = model.registrationTypes;
    state.fieldsWithoutPage = model.fieldsWithoutPage;
    state.internalFields = model.internalFields;
    state.blankFields = model.blankFields;
    state.fieldTypes = model.fieldTypes;
    state.pages = model.pages;
    state.id = model.id;
    state.formType = model.formType;
    state.formName = model.formName;
    state.isReviewPageHidden = model.isReviewPageHidden;
    state.submitButtonText = model.submitButtonText;
    state.isProjectLive = model.isProjectLive;
  },
  setPages(state, payload){
    state.pages = payload
  },
  setFields(state, payload) {
    state.fields = payload
  },
  setFieldsWithoutPages(state, payload) {
    state.fieldsWithoutPage = payload
  },
  setInternalFieldOrder(state, payload) {
    state.internalFields = payload
  },
  setIsInternalActive(state, payload) {
    state.isInternalActive = payload
  },
  setFormName(state, payload) {
    state.formName = payload
  },
  setSubmitButtonText(state, payload) {
    state.submitButtonText = payload
  },
  setIsReviewPageHidden(state, payload) {
    state.isReviewPageHidden = payload
  }
}