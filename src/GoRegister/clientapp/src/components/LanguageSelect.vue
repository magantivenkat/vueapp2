<template>

  <div class="formal-form-wrapper">
      <div class="language-select">
        <!-- <label class="form-label" for="language">Select Language:</label> -->
        <select v-model="proxy" class="form-control form-control-sm lang-select">
          <option v-if="defaultOption" value :selected="valueIsEmpty" disabled hidden>{{ defaultOptionText }}</option>
          <option v-for="(lang, index) in languages" :key="index" :id="uuid" :value="lang.value">
            {{ lang.text || lang.value }}
          </option>
          <!-- <option v-for="language in languages" :key="language.value" :value="language.value">
        {{ language.text }}
      </option> -->
        </select>
      </div>
    </div>
</template>


<script>
export default {
  name: "LanguageSelect",
  props: {
    languages: {
      type: Array,
      required: true
    },
    value: {
      type: String,
      default: "en-us" // Set default value to en-us
    },
    defaultOption: [String, Boolean],
    placeholderText: String
  },
  data() {
    return {
      uuid: Math.random().toString(16).substr(2),
    };
  },
  computed: {
    proxy: {
      get() {
        // If value is empty, try to default to en-us
        if (this.valueIsEmpty) {
          const enUsOption = this.languages.find(lang => 
            lang.value?.toLowerCase() === 'en-us'
          );
          return enUsOption ? enUsOption.value : '';
        }
        return this.value;
      },
      set(value) {
        console.log(value);
        this.$emit("input", value);
      },
    },
    defaultOptionText() {
      if (this.defaultOption) {
        if (typeof this.defaultOption === 'string' || this.defaultOption instanceof String) {
          return this.defaultOption;
        } else {
          return "";
        }
      } else {
        return "";
      }
    },
    valueIsEmpty() {
      return this.value === undefined ||
        this.value === '' ||
        this.value === null ||
        this.value === false;
    }
  },
  watch: {
    languages: {
      handler(newLanguages) {
        if (this.valueIsEmpty && newLanguages.length > 0) {
          // Prefer en-us if available, else use first language
          const enUsOption = newLanguages.find(lang => 
            lang.value?.toLowerCase() === 'en-us'
          );
          this.$emit("input", enUsOption ? enUsOption.value : newLanguages[0].value);
        }
      },
      immediate: true
    }
  },
  mounted() {
    if (this.valueIsEmpty && this.languages.length > 0) {
      // Prefer en-us if available, else use first language
      const enUsOption = this.languages.find(lang => 
        lang.value?.toLowerCase() === 'en-us'
      );
      this.$emit("input", enUsOption ? enUsOption.value : this.languages[0].value);
    }
  },
};

</script>

<style scoped>
.language-select {
  /* display: flex;
  align-items: center;
  justify-content: flex-end;
  flex-wrap: nowrap;
  gap: 0.5rem;
  min-height: 2rem; */
  position: absolute;
  margin: 81px 0px 0px 895px;
} 

.lang-select {
  max-width: 150px;
  width: 130px;
  font-size: 0.8rem;
  height: 32px;
  padding: 4px 8px;
}

.formal-form-wrapper {
    margin-right: auto;
    margin-left: auto;
    max-width: 1025px;
    position: relative;
}

.formal-form-topbar {
  position: absolute;
}

.formal-form-topbar .language-select select {
  float: right;      /* Optional: floats the select */
  margin: 20px 40px 0 10px; /* Adjust as needed: top right bottom left */
}

</style>

