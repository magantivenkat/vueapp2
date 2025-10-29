import { Bar } from "vue-chartjs";

export default {
  extends: Bar,
  props: {
    chartData: {
      type: Object,
      default: null,
    },
    options: {
      type: Object,
      default() {
        return {
          legend: {
            display: false
          },
          responsive: true,
          maintainAspectRatio: false
        };
      },
    },
  },
  mounted() {
    this.renderChart(this.chartData, this.options);
  },
};