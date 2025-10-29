<template>
  <div v-if="rules.length > 0">
    <span>Displays when: </span>
    <div
      class="badge badge-pill badge-light pointer"
      :key="key"
      v-for="(rule, key) in fieldName"
      @click="removeRule(rule)"
    >
      {{ rule.name }} =>
      <span :key="key" v-for="(option, key) in rule.options"
        >{{ option.description }},
      </span>
      <strong>&times;</strong>
    </div>
  </div>
</template>


<script>
export default {
  props: ["rules", "fieldId", "resetOptions"],
  methods: {
    removeRule: function (rule) {
      this.$emit('resetOptions');
      this.$store.dispatch("saveRules", {
        fieldId: this.fieldId,
        chosenRuleId: [],
        chosenOptions: [],
      });
    },
  },
  computed: {
    fieldName() {
      // const $ = this;

      return this.rules.map((r) => {
        // match r.id to one of the ruleList
        const field = this.fieldsWithOptions.find((rl) => rl.id == r.fieldId);

        return {
          name: field.name,
          options: r.optionIds.map((o) => field.options.find((f) => f.id == o)),
        };
      });
    },
    fieldsWithOptions() {
      return this.$store.getters.getFieldsWithOptions;
    },
  },
};
</script>