import formalLabel from "./FormalLabel.vue";
import context from "./context";
import FieldMixin from "./FieldMixin";

export default {
  components: { formalLabel },
  mixins: [FieldMixin],
  props: {
    // v: {
    //   type: Object
    // },
    label: {
      type: [Boolean, String],
      default: false
    },
    helpTextBefore: String,
    helpTextAfter: String,
    validationName: String,
    readonly: Boolean,
    validatePropertyName: [String, Number],
    dataTag: String
  },
  data() {
    return {
    }
  },
  computed: {
    ...context,
    isInvalid() {
      return this.v && this.v.$errors;
    }
  }
}