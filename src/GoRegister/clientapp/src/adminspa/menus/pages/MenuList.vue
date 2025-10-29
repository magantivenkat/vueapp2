<template>
  <div>
    <div class="row">
      <div class="col-8">
        <div class="float-right">
          <router-link
            class="btn btn-primary mr-2"
            :to="{ name: 'menu-item-create' }"
            >Add Menu Item</router-link
          >
        </div>
        <h2>Menu</h2>
        <draggable
          :list="model.menuItems"
          class="list-group"
          ghost-class="ghost"
          @start="dragging = true"
          @end="dragging = false"
        >
          <div
            class="list-group-item d-flex justify-content-between align-items-center pointer"
            v-for="(menuItem, i) in model.menuItems"
            :key="i"
            v-bind="menuItem"
          >
            <div>
              <span class="fa fa-arrows-alt" aria-hidden="true"></span>
              {{ menuItem.label || menuItem.customPageLabel }}
            </div>
            <div class="">
              <router-link
                class="btn btn-outline-primary mr-2"
                :to="{ name: 'menu-item-edit', params: { id: menuItem.id } }"
                >Manage</router-link
              >
              <button type="button"
                class="btn btn-outline-danger mr-2"
                @click="deleteItem(menuItem.id)"
                >Delete</button
              >
            </div>
          </div>
        </draggable>
        <hr />
        <div class="mt-2">
          <button @click="save" class="btn btn-primary">Save</button>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { getMenu, saveMenu, deleteMenuItem } from "../api";
import draggable from "vuedraggable";

export default {
  components: { draggable },
  data: () => ({
    model: {
      menuItems: [],
    },
  }),
  async created() {
    this.fetchData();
  },
  methods: {
    fetchData() {
      const $this = this;

      getMenu().then((result) => {
        $this.model.menuItems = result.items;
      });
    },
    save() {
      saveMenu({ order: this.model.menuItems.map((item) => item.id) })
        .then((result) => {
          this.model.menuItems = result.items;
          this.$bvToast.toast(`Menu order has been updated`, {
            title: "Saved",
            autoHideDelay: 3000,
            variant: "success",
          });
        })
        .catch((result) => alert("Error"));
    },
    deleteItem(id) {
      if(confirm("Are you sure you want to delete this menu item?")) {
        deleteMenuItem({ order: this.model.menuItems.map((item) => item.id) })
        .then((result) => {
          let removeIndex = this.model.menuItems.map(item => item.id).indexOf(id);
          ~removeIndex && this.model.menuItems.splice(removeIndex, 1);
          this.$bvToast.toast(`Menu item removed`, {
            title: "Success",
            autoHideDelay: 3000,
            variant: "success",
          });
        })
        .catch((result) => this.$bvToast.toast(`Couldn't delete the menu item. Please refresh the page and try again`, {
            title: "Error",
            autoHideDelay: 3000,
            variant: "danger",
          }));
      }
    }
  },
};
</script>