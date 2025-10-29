<!--MRF Changes: Hide nevigation menu from User MRF Form page
Modified Date: 18th October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New-->


<!--<template>
  <li :class="[activeClass, completedClass]">
    <div class="text-center bs-wizard-stepname" v-text="title"></div>
    <div class="progress">
      <div class="progress-bar gr-form-wizard-progress"></div>
    </div>
    <button class="form-wizard-dot gr-form-wizard-dot" :disabled="!isDisabled"></button>
  </li>
</template>-->
<script>
    export default {
        //props: {
        //  step: {
        //    type: Object,
        //  },
        //  index: Number,
        //  stepIndex: Number,
        //  isDisabled: Boolean
        //},
        //computed: {
        //  activeClass() {
        //    return this.step.active ? "active" : "";
        //  },
        //  completedClass() {
        //    return this.stepIndex > this.index ? "completed" : "";
        //  },
        //  title() {
        //    return this.step.title ? this.step.title : `Page ${this.index + 1}`;
        //  }
        //},
    };
</script>