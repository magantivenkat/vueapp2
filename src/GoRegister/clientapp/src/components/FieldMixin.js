export default {
  props: {
    cols: { type: Number, default: 12 },
    type: String,
    id: {
      type: [Number, String],
    },
    isForPresentation: {
      type: Boolean,
      default: false,
    },
    summaryTemplate: String,
  },
  inject: {
    vuelidate: { default: false },
    formalConfig: { default: false }
  },
};