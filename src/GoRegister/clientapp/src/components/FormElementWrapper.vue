 <!-- <template >
 <div v-show="shouldDisplay">
    <slot></slot>
  </div>
</template>--> 

<script>
import { shouldDisplayField } from "../utils/fieldsHelper";

export default {
  props: {
    fieldId: String,
    schema: {
      type: Object,
      required: true,
    },
    model: Object,
  },
  data() {
    return {};
  },
  computed: {
    shouldDisplay: function () {
      
      return shouldDisplayField(this.fieldId, this.schema, this.model);
    },
  },
  render() {
    return this.shouldDisplay ? this.$slots.default : "";
  },
};
</script>