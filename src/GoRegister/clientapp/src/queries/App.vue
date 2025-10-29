<style>
.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: table;
  transition: opacity 0.3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}
</style>

<template>
  <div>
    <div class="row">
      <div class="col-md-4">
        <div>
          <div class="form-check form-check-inline">
            <input
              class="form-check-input"
              type="radio"
              v-model.number="type"
              id="report-type-delegates"
              value="0"
            />
            <label class="form-check-label" for="report-type-delegates"
              >Attendee Report</label
            >
          </div>
          <div class="form-check form-check-inline">
            <input
              class="form-check-input"
              type="radio"
              v-model.number="type"
              id="report-type-summary"
              value="1"
            />
            <label class="form-check-label" for="report-type-summary"
              >Summary Report</label
            >
          </div>
        </div>
        <hr />
        <!--Delegate report-->
        <div v-if="type === 0">
          <h5>Selected columns</h5>
          <ul>
            <li v-for="(rf, index) in selectedFields" :key="index">
              {{ getReportFieldForKey(rf).text }}
            </li>
          </ul>
          <span v-show="!selectedFields.length">No columns selected</span>
          <button
            @click="showSelectFieldsModal = true"
            class="btn btn-outline-primary"
          >
            Add columns
          </button>
        </div>

        <!--Summary report-->
        <div v-if="type === 1">
          <div>
            <div v-for="(gb, index) in selectedGroupBys" :key="index">
              <GroupByEditor :model="gb" :groupBys="groupBys"></GroupByEditor>
            </div>
            <label for="">Start from</label>
            <select v-model="groupByStartFrom" class="form-control">
              <option
                v-for="(gb, index) in groupBys"
                :key="index"
                :value="gb.key"
              >
                {{ gb.displayName }}
              </option>
            </select>
            <button @click="addGroupBy">Add</button>
          </div>
          <!-- <div v-for="(gb, index) in selectedGroupBysAsObject" :key="index">
            <select v-model="gb.value" class="form-control">
              <option
                v-for="(select, index) in reportFields"
                :key="index"
                :value="select.value"
              >
                {{ select.text }}
              </option>
            </select>
          </div>
          <button class="btn btn-outline-primary" @click="addGroupBy">
            Add group by
          </button> -->
        </div>
        <hr />
        <div>
          <filter-list
            :filters="reportFilters"
            :selectedFilters="selectedFilters"
            @filterSaved="saveFilter"
          ></filter-list>
          <!-- <div
            v-for="(filter, i) in selectedFilters"
            :key="i"
            @click="openEditFilterModal(i)"
            style="opacity: 1"
          >
            <template v-if="filter.type === 'ForeignKey'">
              <div>
                {{ filter.name }}:
                <span>
                  <button
                    v-for="(val, i) in filter.values"
                    :key="i"
                    class="btn btn-sm btn-outline-info ml-1"
                    disabled
                  >
                    {{ getFriendlyNameForOption(filter, val) }}
                  </button>
                </span>
              </div>
            </template>
            <template v-else>
              <div>{{ filter.name }}</div>
            </template>
          </div>
          <button
            class="btn btn-outline-primary"
            v-on:click="openAddFilterModal"
          >
            Add filters
          </button> -->
        </div>
        <hr />
        <!--Delegate report-->
        <div v-if="type === 0">
          <div class="form-group">
            <label class="d-block" for="orderByKey">Sort by</label>
            <div style="display: flex; justify-content: right">
              <select
                id="orderByKey"
                v-model="orderByKey"
                class="form-control"
                style="flex-grow: 1"
              >
                <option value></option>
                <option
                  v-for="(orderby, i) in reportFields"
                  :key="i"
                  :value="orderby.value"
                >
                  {{ orderby.text }}
                </option>
              </select>
              <select
                v-model="orderByDirection"
                class="form-control"
                style="width: initial"
              >
                <option value="0">Asc</option>
                <option value="1">Desc</option>
              </select>
            </div>
          </div>
        </div>
        <div v-if="type === 1">
          <div class="form-check">
            <label class="form-check-label" for="defaultCheck1">
              <input
                class="form-check-input"
                type="checkbox"
                v-model="groupBySkipRowsWithZeroData"
              />
              Exclude rows with no data
            </label>
          </div>
        </div>
        <hr />
        <div class="form-group">
          <label>Report Name</label>
          <input v-model="name" type="text" class="form-control" />
        </div>

        <div class="mb-2"></div>
        <button v-on:click="refreshData" type="button" class="btn btn-primary">
          Run
        </button>
        <button
          v-on:click="exportReport"
          type="button"
          class="btn btn-outline-primary"
        >
          Export
        </button>
        <button
          v-on:click="saveReport"
          type="button"
          class="btn btn-outline-primary"
        >
          Save Report
        </button>
      </div>
      <div class="col-md-8">
        <div>
          <div class="float-right">
            <button
              v-if="id"
              @click="getSheetsLink"
              class="btn btn-outline-primary"
            >
              Export to Sheets
            </button>
          </div>
          <div class="clearfix"></div>
          <hr />
        </div>
        <div id="js-report-data"></div>
        <div v-if="summaryResults">
          <div>
            <div class="form-check form-check-inline">
              <input
                class="form-check-input"
                type="radio"
                v-model="summaryResultsDisplay"
                id="summary-results-display-table"
                value="table"
              />
              <label
                class="form-check-label"
                for="summary-results-display-table"
                >Table</label
              >
            </div>
            <div class="form-check form-check-inline">
              <input
                class="form-check-input"
                type="radio"
                v-model="summaryResultsDisplay"
                id="summary-results-display-pie"
                value="pie"
              />
              <label class="form-check-label" for="summary-results-display-pie"
                >Pie</label
              >
            </div>
            <div class="form-check form-check-inline">
              <input
                class="form-check-input"
                type="radio"
                v-model="summaryResultsDisplay"
                id="summary-results-display-bar"
                value="bar"
              />
              <label class="form-check-label" for="summary-results-display-bar"
                >Bar</label
              >
            </div>
          </div>
          <div v-if="summaryResultsDisplay === 'table'">
            <table class="table">
              <thead>
                <tr>
                  <th v-for="(header, i) in summaryResults.headers" :key="i">
                    {{ header }}
                  </th>
                </tr>
              </thead>
              <tr v-for="(row, i) in summaryResults.results" :key="i">
                <td v-for="(itemKey, j) in Object.keys(row)" :key="j">
                  {{ row[itemKey] }}
                </td>
              </tr>
            </table>
          </div>
          <div v-else-if="summaryResultsDisplay === 'pie'">
            <PieChartDisplay :chartData="chartData" />
          </div>
          <div v-else-if="summaryResultsDisplay === 'bar'">
            <BarChartDisplay :chartData="chartData" />
          </div>
        </div>
      </div>
    </div>
    <Modal v-bind:show="showSelectFieldsModal">
      <ModalHeader
        title="Select columns"
        v-on:close="showSelectFieldsModal = false"
      />
      <ModalBody>
        <div class="container-fluid">
          <div class="row">
            <div class="col">
              <div class="mb-2">
                <button
                  class="btn-sm btn-outline-primary"
                  @click="selectAllFields"
                >
                  Select all
                </button>
              </div>
              <div
                v-for="(select, i) in reportFields"
                :key="i"
                class="form-check"
              >
                <input
                  v-model="selectedFields"
                  :value="select.value"
                  :id="'check-' + select.value"
                  class="form-check-input"
                  type="checkbox"
                />
                <label
                  class="form-check-label"
                  :for="'check-' + select.value"
                  >{{ select.text }}</label
                >
              </div>
            </div>
            <div class="col">
              <draggable
                :list="selectedFields"
                class="list-group"
                ghost-class="ghost"
              >
                <div
                  class="list-group-item"
                  v-for="select in selectedFields"
                  :key="select"
                >
                  {{ getReportFieldForKey(select).text }}
                </div>
              </draggable>
              <span v-show="!selectedFields.length">No columns selected</span>
            </div>
          </div>
        </div>
      </ModalBody>
      <ModalFooter
        confirmText="Finish"
        v-on:confirm="showSelectFieldsModal = false"
      />
    </Modal>

    <!-- <Modal
      v-bind:show="showEditFilterModal"
      @close="setShowEditFilterModal = false"
    >
      <ModalHeader title="Filter" v-on:close="showEditFilterModal = false" />
      <ModalBody>
        <div>
          <select
            v-model="editFilter.key"
            v-on:change="setFilterModel"
            class="form-control"
          >
            <option
              v-for="(filter, i) in reportFilters"
              :key="i"
              :value="filter.key"
            >
              {{ filter.name }}
            </option>
          </select>
        </div>
        <div v-if="editFilter.key != ''">
          <div v-if="editFilter.model.type === 'ForeignKey'" class="form-group">
            <select
              v-model="editFilter.model.values"
              multiple
              class="form-control"
              style="height: inherit"
            >
              <option
                v-for="(option, i) in editFilter.model.options"
                :key="i"
                :value="option.value"
              >
                {{ option.name }}
              </option>
            </select>
          </div>
          <div v-else-if="editFilter.model.type === 'String'">
            <input
              type="text"
              v-model="editFilter.model.value"
              class="form-control"
            />
          </div>
          <div v-else-if="editFilter.model.type === 'Date'">
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
      <ModalFooter confirmText="Save" v-on:confirm="saveFilter" />
    </Modal> -->

    <Modal v-bind:show="sheetsShareUrl">
      <ModalHeader
        title="Google Sheets Share"
        v-on:close="sheetsShareUrl = false"
      />
      <ModalBody>
        <div class="container-fluid">
          <div class="row">
            <div class="col">
              <div class="form-group">
                <textarea
                  class="form-control"
                  v-model="sheetsShareUrl"
                  rows="3"
                ></textarea>
              </div>
            </div>
          </div>
        </div>
      </ModalBody>
      <ModalFooter confirmText="Finish" v-on:confirm="sheetsShareUrl = false" />
    </Modal>
  </div>
</template>

<script>
import draggable from "vuedraggable";
import axios from "axios";
import PieChartDisplay from "./PieChartDisplay";
import BarChartDisplay from "./BarChartDisplay";
import FilterList from "./components/FilterList.vue";
import GroupByEditor from "./components/GroupByEditor";

export default {
  data() {
    return {
      type: null,
      selectedGroupBys: [],
      selectedGroupBysAsObject: [],
      showEditFilterModal: false,
      showSelectFieldsModal: false,
      editFilter: {
        key: "",
      },
      id: null,
      name: "",
      exportDataUrl: "",
      getDataUrl: "",
      getSheetsUrl: "",
      selectedFields: [],
      selectedFilters: [],
      reportFilters: [],
      reportFields: [],
      groupByStartFrom: "",
      groupBys: [],
      summaryResults: null,
      summaryResultsDisplay: "table",
      sheetsShareUrl: false,
      groupBySkipRowsWithZeroData: false,
    };
  },
  created() {
    var initialData = JSON.parse(
      document.getElementById("report-app").getAttribute("data-initial-data")
    );

    this.type = initialData.type;
    this.selectedFilters = initialData.selectedFilters;
    this.selectedFields = initialData.selectedFields;
    this.orderByDirection = initialData.orderByDirection;
    this.orderByKey = initialData.orderByKey;
    this.reportFilters = initialData.reportFilters;
    this.reportFields = initialData.reportFields;
    this.groupByStartFrom = initialData.groupByStartFrom;
    this.groupBys = initialData.groupBys;
    this.id = initialData.id;
    this.name = initialData.name;
    this.groupBySkipRowsWithZeroData = initialData.groupBySkipRowsWithZeroData;

    this.exportDataUrl = document
      .getElementById("report-app")
      .getAttribute("data-exportData");
    this.getDataUrl = document
      .getElementById("report-app")
      .getAttribute("data-getData");
    this.getSummaryDataUrl = document
      .getElementById("report-app")
      .getAttribute("data-getSummaryData");
    this.saveReportUrl = document
      .getElementById("report-app")
      .getAttribute("data-saveReportUrl");
    this.getSheetsUrl = document
      .getElementById("report-app")
      .getAttribute("data-sheetsShareUrl");
    // this is a hack because you cant edit array items directly in vue
    this.selectedGroupBysAsObject = initialData.selectedGroupBys.map(function (
      e
    ) {
      return { value: e };
    });
  },
  components: {
    draggable,
    PieChartDisplay,
    BarChartDisplay,
    FilterList,
    GroupByEditor,
  },
  methods: {
    PieChartDisplaytShowEditFilterModal(value) {
      this.showEditFilterModal = value;
    },
    getModelToSend: function () {
      return {
        type: this.type,
        selectedFilters: this.selectedFilters,
        selectedFields: this.selectedFields,
        orderByDirection: this.orderByDirection,
        orderByKey: this.orderByKey,
        groupByStartFrom: this.groupByStartFrom,
        selectedGroupBys: this.selectedGroupBys,
        groupBySkipRowsWithZeroData: this.groupBySkipRowsWithZeroData,
      };
    },
    refreshData: function () {
      let model = this.getModelToSend();

      if (this.type == 0) {
        axios.post(this.getDataUrl, model).then((res) => {
          document.getElementById("js-report-data").innerHTML = res.data;
        });
      } else {
        axios.post(this.getSummaryDataUrl, model).then((res) => {
          this.summaryResults = res.data;
        });
      }
    },
    exportReport: function () {
      var model = this.getModelToSend();
      // put export data into the data section
      window.location = this.exportDataUrl + "?query=" + JSON.stringify(model);
    },
    openAddFilterModal: function () {
      this.editFilter = {
        key: "",
        index: -1,
        model: {},
      };
      this.showEditFilterModal = true;
    },
    openEditFilterModal: function (index) {
      var model = Object.assign({}, this.selectedFilters[index]);
      this.editFilter = {
        key: model.key,
        index: index,
        model: model,
      };
      this.showEditFilterModal = true;
    },
    addGroupBy() {
      var gb = this.groupBys.find((e) => e.key === this.groupByStartFrom);
      this.selectedGroupBys.push(Object.assign({}, gb.model));
      this.groupByStartFrom = "";
    },
    // saveFilter: function () {
    //   if (!this.validateFilter()) return;

    //   if (this.editFilter.index >= 0) {
    //     this.selectedFilters[this.editFilter.index] = this.editFilter.model;
    //   } else {
    //     this.selectedFilters.push(this.editFilter.model);
    //   }

    //   this.showEditFilterModal = false;
    // },
    saveFilter(filter) {
      if (filter.index >= 0) {
        this.selectedFilters.$set(filter.index, filter.model);
      } else {
        this.selectedFilters.push(filter.model);
      }
    },
    validateFilter: function () {
      if (!this.editFilter.key) return false;

      if (this.editFilter.model.type === "ForeignKey") {
        if (!this.editFilter.model.values.length) return false;
      }

      return true;
    },
    setFilterModel: function () {
      this.editFilter.model = Object.assign(
        {},
        this.reportFilters.find((e) => e.key === this.editFilter.key)
      );
    },
    getFriendlyNameForOption: function (filter, value) {
      return filter.options.find(function (e) {
        return e.value === value;
      }).name;
    },
    getReportFieldForKey: function (key) {
      return this.reportFields.find(function (item) {
        return item.value === key;
      });
    },
    selectAllFields: function () {
      this.selectedFields = this.reportFields.map(function (e) {
        return e.value;
      });
    },
    saveReport() {
      let model = this.getModelToSend();
      model.id = this.id;
      model.name = this.name;
      axios
        .post(this.saveReportUrl, model)
        .then((res) => {
          alert("Saved successfully");
        })
        .catch((res) => {
          alert("There was a problem saving the form");
        });
    },
    getSheetsLink() {
      axios.get(this.getSheetsUrl).then((res) => {
        this.sheetsShareUrl = res.data.url;
      });
    },
  },
  computed: {
    summaryFields: function () {
      return this.reportFields.filter(function (item) {
        return item;
      });
    },
    disableButton: function () {
      return this.selectedFields.length < 1;
    },
    chartData() {
      let data = {
        datasets: [],
        labels: [],
      };

      if (!this.summaryResults) {
        return;
      }

      let dataset = {
        data: [],
        backgroundColor: [
          "#3e95cd",
          "#8e5ea2",
          "#3cba9f",
          "#e8c3b9",
          "#c45850",
          "#c45850",
        ],
      };

      for (let index = 0; index < this.summaryResults.results.length; index++) {
        const result = this.summaryResults.results[index];
        let keys = Object.keys(result);
        data.labels.push(result[keys[0]]);
        dataset.data.push(result[keys[1]]);
      }

      data.datasets.push(dataset);

      return data;
    },
    //selectedReportFields: function () {
    //    var $this = this;
    //    return this.reportFields.filter(function (item) {
    //        return $this.selectedFields.includes(item.value);
    //    })
    //}
  },
};
</script>

