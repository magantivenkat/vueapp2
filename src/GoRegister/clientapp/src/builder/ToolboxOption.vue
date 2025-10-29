<template>
  <div>
      <div class="card toolbox-option">
      <span>{{fieldType.name}}</span>
      <button
        @click="addField(fieldType)"
        type="button"
        class="btn btn-primary"
        :disabled="isUnique"
      >
        <strong> &plus; </strong>
      </button>
    </div>
  </div>
</template>

<script>
export default {
  props: ["fieldType", "pageId", "click"],
  data() {
    return {
      text: "",
    };
  },
  methods: {
    addField(fieldType) {
      this.$store.dispatch("addField", {
        fieldType: fieldType,
        name: this.text,
        pageId: this.pageId,
      });
      this.text = "";
      this.click();
    },
  },
  computed: {
    isUnique() {
      return (
        this.fieldType.isUnique &&
         this.$store.state.fields.some((f) => f.fieldTypeId == this.fieldType.fieldTypeId && !f.isDeleted)
      );
    },
  },
};
</script>

<style scoped>
.toolbox-option {
  margin-bottom: 0.5rem;
  padding: 0.5rem;
  display: grid;
  grid-template-columns: auto 40px;
}
.toolbox-option span {
  padding-top: 7px;
}
</style>