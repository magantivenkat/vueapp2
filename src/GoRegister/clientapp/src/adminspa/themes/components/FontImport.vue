<template>
  <div>
    <ul>
      <li v-for="(font, i) in fonts" :key="i">
        {{ font.name }}
      </li>
    </ul>
    <div>
      <div class="form-group">
        <label for="">Select a Google Font</label>
        <select class="form-control" v-model="selectedGoogleFont">
          <option v-for="font in googleFonts" :key="font" :value="font">
            {{ font }}
          </option>
        </select>
      </div>
      <div v-if="this.selectedGoogleFont">
        <div class="form-group">
          <label>Select one or more styles</label>
          <div v-for="(variant, i) in currentGoogleFontObject.variants" :key="i" class="form-check">
            <input
              class="form-check-input"
              v-model="selectedGoogleFontStyles"
              type="checkbox"
              :value="variant"
              :id="'variant-' + variant"
            />
            <label class="form-check-label" :for="'variant-' + variant">
              {{ getVariantDisplayName(variant) }}
            </label>
          </div>
        </div>
        <div class="mb-3">
          <button @click="addFont" class="btn btn-outline-primary mr-2" type="button">Add</button>
          <span class="text-muted"> - not selecting any styles will add them all</span>
        </div>
      </div>
    </div>
    <span class="text-muted"
      ><a href="https://fonts.google.com/" target="_blank">Click here</a> to find and preview all the Google fonts that
      are available</span
    >
  </div>
</template>

<script>
import googleFonts from "@/shared/googlefonts.js";

export default {
  props: {
    fonts: {
      type: Array,
      required: true,
    },
  },
  created() {
    this.googleFonts = Object.keys(googleFonts);
  },
  data: () => ({
    selectedGoogleFont: null,
    selectedGoogleFontStyles: [],
  }),
  methods: {
    getVariantDisplayName(variant) {
      if (variant.endsWith("i")) {
        return variant.slice(0, -1) + " italic";
      }

      return variant;
    },
    addFont() {
      let selectedVariants = this.selectedGoogleFontStyles.slice();
      if (selectedVariants.length === 0) {
        selectedVariants = this.currentGoogleFontObject.variants.slice();
      }

      const font = {
        name: this.selectedGoogleFont,
        variants: selectedVariants,
        fontType: "google",
        link: "",
      };

      if (font.fontType === "google") {
        const hasItalics = selectedVariants.some((v) => v.endsWith("i"));
        const weights = [];
        for (let i = 0; i < selectedVariants.length; i++) {
          const v = selectedVariants[i];
          let weight = v;
          let isVariantItalic = "0";
          if (hasItalics) {
            if (v.endsWith("i")) {
              isVariantItalic = "1";
              weight = v.substring(0, v.length - 1);
            }
            weights.push(`${isVariantItalic},${weight}`);
          } else {
            weights.push(weight);
          }
        }

        const weightString = weights.join(";");
        const fontNameForLink = font.name.replace(" ", "+");
        const fontTypesRequired = hasItalics ? "ital,wght" : "wght";
        const link = `https://fonts.googleapis.com/css2?family=${fontNameForLink}:${fontTypesRequired}@${weightString}&display=swap`;
        font.link = link;
      }

      this.$emit("font-added", font);

      this.selectedGoogleFontStyles = [];
      this.selectedGoogleFont = null;
    },
  },
  computed: {
    currentGoogleFontObject() {
      console.log(this.googleFonts);

      if (!this.selectedGoogleFont) return null;

      const googleFont = googleFonts[this.selectedGoogleFont];
      return {
        name: this.selectedGoogleFont,
        variants: googleFont.variants.split(","),
      };
    },
  },
};
</script>