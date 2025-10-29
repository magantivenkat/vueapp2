<template>
  <div>
    <div>
      <h3>{{ vm.displayName }}</h3>
      <div
        class="form-check"
        v-for="(select, index) in vm.selects"
        :key="index"
      >
        <input
          class="form-check-input"
          type="checkbox"
          :value="select.key"
          v-model="model.selects"
        />
        <label class="form-check-label">
          {{ select.displayName }}
        </label>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    model: Object,
    groupBys: Array,
  },
  computed: {
    vm() {
      return this.groupBys.find((e) => e.key == this.model.key);
    },
  },
};
</script>