const shouldDisplayField = (fieldId, schema, model) => {

  let ruleHierarchyValid = true;
  let fieldSchema = getField(fieldId, schema);
  if (fieldSchema.rules && fieldSchema.rules.length) {
    var fieldsToCheck = [];
    getRulesForField(fieldSchema.key, schema, fieldsToCheck);

    for (let index = 0; index < fieldsToCheck.length; index++) {
      const field = getField(fieldsToCheck[index], schema);
      if(!field.rules) continue;

      for (let ri = 0; ri < field.rules.length; ri++) {
        const rule = field.rules[ri];

        if (rule.values.includes(model[rule.id])) continue;

        ruleHierarchyValid = false;
        break;
      }

      if(!ruleHierarchyValid) break;
    }
  }

  //console.log(fieldId + " is " + ruleHierarchyValid)
  return ruleHierarchyValid;
}

const getRulesForField = (fieldId, schema, fieldsToCheck) => {
  let fieldSchema = getField(fieldId, schema);
  fieldsToCheck.push(fieldSchema.key);

  if(!fieldSchema.rules) return;

  for (let index = 0; index < fieldSchema.rules.length; index++) {
    const rule = fieldSchema.rules[index];
    fieldsToCheck.push(rule.id);
    getRulesForField(rule.id, schema, fieldsToCheck);
  }
}

const getFieldSummaryValue = (field, model) => {
  var val = model[field.key];
  if(!val) return "";

  return val;
}

const getField = (fieldId, schema) => {
  return schema.fields.find(el => el.key === fieldId);
}

export { shouldDisplayField, getFieldSummaryValue };