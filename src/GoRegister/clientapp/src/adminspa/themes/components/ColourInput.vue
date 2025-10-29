<template>
  <div>
    <div class="form-group">
      <label>{{ variable.model.label }}</label>
      <div class="input-group">
        
        <input
          type="text"
          class="form-control"
          @input="handle"
          :value="value"
        />
        <div class="input-group-append">
          <span class="input-group-text">
            <input type="color" :value="variable.calculated" @change="handle" />
          </span>
        </div>
      </div>
    </div>
    
  </div>
</template>

<script>
export default {
  props: {
    value: {
      type: String,
    },
    label: String,
    calculated: String,
    variable: {
      type: Object,
      required: true
    }    
  },
  methods: {
    handle(e) {
      this.$emit("input", e.target.value);
    },
  },
};
</script>