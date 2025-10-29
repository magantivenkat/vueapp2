<template>
  <div>
    <template v-if="viewModel.type === 'ForeignKey'">
      <div>
        {{ viewModel.name }}:
        <span>
          <button
            v-for="(val, i) in model.values"
            :key="i"
            class="btn btn-sm btn-outline-info ml-1"
            disabled
          >
            {{ getFriendlyNameForOption(val) }}
          </button>
        </span>
      </div>
    </template>
    <template v-else-if="viewModel.type === 'String'">
      <div>{{ viewModel.name }} = {{ model.value }}</div>
    </template>
  </div>
</template>

<script>
export default {
  props: {
    filters: Array,
    model: Object,
    index: Number,
  },
  methods: {
    getFriendlyNameForOption: function (value) {
      return this.viewModel.options.find(function (e) {
        return e.value === value;
      }).name;
    },
  },
  computed: {
    viewModel() {
      return this.filters.find((e) => e.key === this.model.key);
    },
  },
};
</script>