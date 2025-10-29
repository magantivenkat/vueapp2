<template>
  <draggable
    v-bind="dragOptions"
    tag="div"
    class="item-container"
    :list="list"
    :value="value"
    @input="emitter"
  >
    <div
      class="item-group relative"
      :key="el.id"
      v-for="(el, pageId) in realValue"
    >
      <span class="delete" @click="deletePage(el)">&times;</span>
      <input
        type="text"
        class="h4 border-0"
        v-model="el.name"
        @change="(e) => updatePageName(e, el.id)"
      />
      <DraggableFields v-model="el.fields" :pageId="pageId" />
    </div>
  </draggable>
</template>

<script>
import draggable from "vuedraggable";
import DraggableFields from "./draggable-fields";
import Swal from "sweetalert2";

export default {
  name: "nested-test",
  methods: {
    emitter(value) {
      this.$emit("input", value);
    },
    updatePageName(e, pageId) {
      this.$store.dispatch("setPageName", {
        pageId: pageId,
        value: e.target.value,
      });
    },
    deletePage(page) {
      Swal.fire({
        title: `Are you sure you want to delete the "${page.name}" page?`,
        icon: "question",
        confirmButtonText: "Yes",
        showCancelButton: true,
      }).then((result) => {
        if (result.isConfirmed) {
          this.$store.dispatch("deletePage", page.id);
        }
      });
    },
  },
  components: {
    draggable,
    DraggableFields,
  },
  computed: {
    dragOptions() {
      return {
        animation: 0,
        group: "pages",
        disabled: false,
        ghostClass: "ghost",
      };
    },
    // this.value when input = v-model
    // this.list  when input != v-model
    realValue() {
      return this.value ? this.value : this.list;
    },
  },
  props: {
    value: {
      required: false,
      type: Array,
      default: null,
    },
    list: {
      required: false,
      type: Array,
      default: null,
    },
  },
};
</script>



<style scoped>
.item-group {
  min-height: 50px;
  list-style: none;
  padding: 1rem;
  background: white;
  margin-bottom: 2rem;
}

.delete {
  position: absolute;
  right: 25px;
  top: 7px;
  font-size: 26px;
  color: #da3838;
  font-weight: bold;
}
/* .dragArea li {
} */
</style>