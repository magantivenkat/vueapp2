export default {
    pages(state){
        return state.pages;
    },
    internalFields(state) {
        const fields = state.internalFields.map((id) => state.fields.find(f => f.id === id &&
            f.isInternal
        ));
        return removeUndefined(fields)
    },
    delegateFields(state) {
        const fields = state.fieldsWithoutPage.map((id) => state.fields.find(f => f.id === id &&
            f.isInternal == false
        ));
        return removeUndefined(fields)
    },
    getFieldsWithOptions(state) {
        return state.fields.filter(f => !f.isDeleted && f.fieldTypeId === 2) // should we have a property hasOptions here instead?
    },
    registrationTypes(state) {
        return state.registrationTypes
    }
}

function removeUndefined(array) {
    return array.filter(a => a !== undefined)
}