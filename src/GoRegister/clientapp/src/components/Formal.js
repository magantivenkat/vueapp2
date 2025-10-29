import fieldComponents from "../utils/fieldsLoader";
import formElementWrapper from "./FormElementWrapper";
import FieldWrapper from "./FieldWrapper";
import FormalLabel from "./FormalLabel";

const components = {
  ...fieldComponents,
  formElementWrapper,
  FormalLabel,
  FieldWrapper
};

class Formal {
  constructor() {
    this.options = {},
    this.defaults = {
      components: components
    }
  }

  install(Vue) {
    Vue.prototype.$formal = this;
    this.options = this.defaults;
    //merge(options || {});
    for (var componentName in this.options.components) {
      Vue.component(componentName, this.options.components[componentName])
    }
  }
}

export default new Formal();