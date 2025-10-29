import {
  uuid
} from "../utils";
import {
  cloneDeep
} from 'lodash'
import axios from "axios";

export default {
  init(context, url) {
    axios
      .get(url)
      .then(function (response) {
        context.commit('init', response.data);
      })
      .catch(function (error) {
        alert(error.message);
      });
  },
  addPage({state, commit}){
    const pages = [...state.pages, {
      id: uuid(),
      name: "New Page",
      fields: [],
      isDeleted: false
    }]

    commit('setPages', pages)
  },
  deletePage({state, commit}, pageId){
    // set pages and fields to isdeleted
    const pages = [...state.pages]
    const fields = [...state.fields]
    const page = pages.find(p => p.id == pageId)
    const pageIndex = pages.indexOf(page)

    pages[pageIndex].isDeleted = true

    pages[pageIndex].fields.map(fieldId => fields.find(f => f.id == fieldId).isDeleted = true)
    pages[pageIndex].fields = []

    commit('setPages', pages)
    commit('setFields', fields)

  },
  setPageName({state, commit}, payload){
    const pages = [...state.pages]
    const page = pages.find(p => p.id == payload.pageId)
    const pageIndex = pages.indexOf(page)
    pages[pageIndex].name = payload.value
    commit('setPages', pages)
  },
  setPages({commit}, pages) {
    commit('setPages', pages)
  },
  addField({
    commit,
    state
  }, payload) {
    const fields = [...state.fields]
    const pages = [...state.pages]

    // set up basics
    const {
      fieldType,
      name,
      pageId 
    } = payload

    // TODO: NEED TO ADD FIELD TO PAGE THAT isDelete == false
    const blankField = cloneDeep(fieldType.blankField);
    const overrideName = fieldType.overrideName !== "" ? fieldType.overrideName : fieldType.name
    let field = {
      ...blankField,
      isForPresentation:fieldType.isForPresentation,
      name: fieldType.overrideName ? overrideName : name,
      dataTag: fieldType.dataTagFixed.length > 0 ? fieldType.dataTagFixed : "",
      isNew: true
    };
    field.id = uuid();
    field.isInternal = state.isInternalActive;
    field.isExpanded = true;

    fields.push(field)
    pages.filter(p => !p.isDeleted)[pageId].fields.push(field.id)

    commit('setFields', fields)
    commit('setPages', pages)
    
  },
  deleteField({
    commit,
    state
  }, payload) {
    const pages = [...state.pages]
    const fields = [...state.fields]
    const field = fields.find((f) => f.id == payload.fieldId);
    const fieldIndex = fields.indexOf(field);
    fields[fieldIndex].isDeleted = true;
    fields[fieldIndex].rules = [];

    if (state.isInternalActive) {
      let internalFields = [...state.internalFields]
      internalFields = internalFields.filter(
      (f) => f != field.id
    );
     commit('setInternalFieldOrder', internalFields)
    } else {
      const updatePage = pages[payload.pageId]
      const updateFields = updatePage.fields.filter(
        (f) => f !== field.id
      )
      updatePage.fields = updateFields
      pages[payload.pageId] = updatePage

     commit('setPages', pages)
   }   

    commit('setFields', fields)
    
  },
  reorderDelegateFields({
    commit
  }, fields) {
    const reorderedIds = fields.map(f => {
      return f.id
    })

    // reorder ids
    //commit('setFields', fields)
    commit('setFieldsWithoutPages', reorderedIds)
  },
  reorderInternalFields({
    commit,
    state
  }, fields) {
    console.log(state)
    const reorderedIds = fields.map(f => {
      return f.id
    })

    // reorder ids
    //commit('setFields', fields)
    commit('setInternalFieldOrder', reorderedIds)
  },
  save({
    commit,
    state
  }) {

    console.log(commit, state)

  },
  saveRules({
    commit,
    getters,
    state
  }, payload) {
    const updateFields = [...state.fields]
    const updateField = updateFields.find(
      (f) => f.id == payload.fieldId
    );
    const fieldIndex = updateFields.indexOf(updateField);
    updateField.rules = []

    const fieldRules = {
      fieldId: getters.getFieldsWithOptions[payload.chosenRuleId].id,
      optionIds: payload.chosenOptions,
    };



    // need to update existing rule if exists
    const ruleToUpdate = updateField.rules.find(
      (r) => r.fieldId == getters.getFieldsWithOptions[payload.chosenRuleId].id
    );
    const ruleIndex = updateField.rules.indexOf(ruleToUpdate);

    if (payload.chosenOptions.length == 0) {
      // if optionIds = empty. delete rule
      updateFields[fieldIndex].rules.splice(ruleIndex, 1);
    } else if (ruleToUpdate) {
      updateFields[fieldIndex].rules[ruleIndex] = fieldRules;
    } else {
      updateFields[fieldIndex].rules.push(fieldRules);
    }

    commit("setFields", updateFields)

  }
}