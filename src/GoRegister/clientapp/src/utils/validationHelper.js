import { required, minLength, email } from "vuelidate/lib/validators";

const generateVuelidateConfiguration = function (field) {
  const validationModel = {};

  if (!field.validations) return validationModel;

  if (field.validations.minLength) {
    validationModel.minLength = minLength(field.validations.minLength);
  }

  if (field.validations.required)
    validationModel.required = required;

  if (field.validations.email)
    validationModel.email = email;

  return validationModel;
}

export { generateVuelidateConfiguration };