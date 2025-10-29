<template>
  <div>
    <div>
      <div
        v-for="(filter, i) in selectedFilters"
        :key="i"
        @click="openEditFilterModal(i)"
        style="opacity: 1"
      >
        <filter-summary :filters="filters" :model="filter" :index="i"></filter-summary>
      </div>
      <button class="btn btn-outline-primary" v-on:click="openAddFilterModal">
        Add filters
      </button>
    </div>
    <Modal
      v-show="showEditFilterModal" 
      @close="setShowEditFilterModal = false"
    >
      <ModalHeader title="Filter" v-on:close="showEditFilterModal = false" />
      <ModalBody>
        <div>
          <select
            v-model="editFilter.model.key"
            v-on:change="setFilterModel"
            class="form-control"
          >
            <option
              v-for="(filter, i) in filters"
              :key="i"
              :value="filter.key"
            >
              {{ filter.name }}
            </option>
          </select>
        </div>
        <div v-if="editFilter.model.key != ''">
          <div v-if="editFilter.viewModel.type === 'ForeignKey'" class="form-group">
            <select
              v-model="editFilter.model.values"
              multiple
              class="form-control"
              style="height: inherit"
            >
              <option
                v-for="(option, i) in editFilter.viewModel.options"
                :key="i"
                :value="option.value"
              >
                {{ option.name }}
              </option>
            </select>
          </div>
          <div v-else-if="editFilter.viewModel.type === 'String'">
            <input
              type="text"
              v-model="editFilter.model.value"
              class="form-control"
            />
          </div>
          <div v-else-if="editFilter.viewModel.type === 'Date'">
            <select v-model="editFilter.model.operation" class="form-control">
              <option value="=">Equal to</option>
              <option value=">=">After</option>
              <option value="<=">Before</option>
            </select>
            <input
              type="date"
              v-model="editFilter.model.value"
              class="form-control"
            />
          </div>
        </div>
      </ModalBody>
      <ModalFooter confirmText="Save" v-on:confirm="saveEditFilter" />
    </Modal>
  </div>
</template>

<script>
import FilterSummary from './FilterSummary.vue';
export default {
  components: { FilterSummary },
  props: {
    filters: {
      type: Array,
      required: true,
    },
    selectedFilters: {
      type: Array,
      required: true,
    },
  },
  data: () => ({
    showEditFilterModal: false,
    editFilter: {
        model: {
          key: ""
        }
      },
  }),
  methods: {
    getFriendlyNameForOption: function (filter, value) {
      return filter.options.find(function (e) {
        return e.value === value;
      }).name;
    },
    openAddFilterModal: function () {
      this.editFilter = {
        index: -1,
        model: {
          key: ""
        },
        viewModel: {}
      };
      this.showEditFilterModal = true;
    },
    openEditFilterModal: function (index) {
      var model = Object.assign({}, this.selectedFilters[index]);
      this.editFilter = {
        index: index,
        model: model,
      };
      this.showEditFilterModal = true;
    },
    setFilterModel: function () {
      this.editFilter.viewModel = Object.assign(
        {},
        this.filters.find((e) => e.key === this.editFilter.model.key)
      );
      this.editFilter.model = {
        key: this.editFilter.model.key
      };
    },
    saveEditFilter() {
      this.$emit("filterSaved", {model: this.editFilter.model, index: this.editFilter.index });
      this.showEditFilterModal = false;
      this.editFilter = {
        model: { key: "" },
        index: -1
      }
    }
  },
};
</script>