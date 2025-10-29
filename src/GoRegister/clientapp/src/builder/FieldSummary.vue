<template>
  <div
    class="card mb-3 relative"
    v-bind:class="{ hidden: field.isHidden }"
    style="padding: 8px 20px"
  >
    <div @click="select" class="handle" style="line-height: 40px">
      <span class="text-overflow">{{ displayName }}</span>
      <span class="float-right" v-if="isEditing">
          <span v-if="isStandardField">
              <button v-on:click="deleteField" class="btn btn-outline-danger" v-if="canDeleteField">
                  Delete
              </button>
          </span>
      </span>
    </div>

    <div v-if="isEditing">
      <FieldEditor :field="field" />
    </div>
    <span style="position: absolute; bottom: 5px; right: 10px"
      >[{{ fieldTypeName }}]</span
    >
  </div>
</template>

<script>
import FieldEditor from "./FieldEditor";

export default {
  components: { FieldEditor },
  props: {
    pageId: {
      type: Number,
      required: true,
    },
    field: {
      type: Object,
      required: true,
    },
    rulesList: {
      type: Array,
    },
  },
  data() {
    return {
        isEditing: this.field.isExpanded
     
    };
  },
  methods: {
    select() {
      this.isEditing = !this.isEditing;
    },
    deleteField() {
      this.$store.dispatch("deleteField", {
        pageId: this.pageId,
        fieldId: this.field.id,
      });
    },
  },
  computed: {
    displayName() {
      if (this.field.fieldTypeId == 14) {
        return this.field.name ? this.field.name.replace(/(<([^>]+)>)/gi, "") : "~~ Field name ~~" ;
      }

      return this.field.name ? this.field.name : "~~ Field name ~~";
    },
    fieldTypeName() {
      return this.$store.state.fieldTypes[this.field.fieldTypeId].name;
    },
    isProjectLive() {
      return this.$store.state.isProjectLive;
    },
    canDeleteField() {
      return this.isProjectLive ? this.field.isNew : true;
      },
      isStandardField() {
          return !(this.field.isStandardField);
      }
  },
};
</script>

<style scoped>
.text-overflow {
  text-overflow: ellipsis;
  overflow: hidden;
  white-space: nowrap;
}
.hidden {
  border: 3px solid #c3e4c3;
}
.hidden::after {
  content: "Internal";
  position: absolute;
  top: -7px;
  left: 16px;
  font-size: 14px;
  background: #5cd15c;
  padding: 0 8px;
  color: white;
}

/* .card.mb-2.relative.hidden */
</style>