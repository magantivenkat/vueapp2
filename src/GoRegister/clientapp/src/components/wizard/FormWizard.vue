<!--MRF Changes: Hide next and prev button, nevigation menu from User MRF Form page
Modified Date: 18th October 2022
Modified By: Mandar.Khade@amexgbt.com
Team member: Harish.Rane@amexgbt.com
JIRA Ticket No: GoRegister / GOR - 238 - New-->

<template>
    <div class="form-wizard">
        <!--<ul class="list-unstyled form-wizard-steps clearfix" v-if="!hideSteps">
          <slot
            name="step"
            v-for="(step, index) in steps"
            :step="step"
            :index="index"
          >
            <WizardStep
              v-if="showWizard"
              :step="step"
              @click.native="navigateToStep(index)"
              @keyup.enter.native="navigateToStep(index)"
              :index="index"
              :stepIndex="stepIndex"
              :isDisabled="completedStep >= index || isAdmin"
            />
          </slot>
        </ul>-->
        <div>
            <slot></slot>
        </div>
        <div class="mt-2">
            <div class="clearfix" v-if="!hideButtons">
                <slot name="footer">
                    <div class="d-flex justify-content-center m-md-4">
                        <!--<span
      @click="prevStep"
      @keyup.enter="prevTab"
      v-if="displayPrevButton"
      role="button"
      tabindex="0"
    >
      <slot name="previous">
        <button class="btn btn-outline-primary" tabindex="-1">
          {{ previousButtonText }}
        </button>
      </slot>
    </span>-->

                        <span @click="submit"
                              @keyup.enter="submit"
                              role="button"
                              tabindex="0">
                            <slot name="submit">
                                <button class="btn btn-primary" tabindex="-1">
                                    {{ submitButtonText }}
                                </button>
                            </slot>
                        </span>
                       &nbsp;&nbsp;
                        <span @click="cancel"
                              @keyup.enter="cancel"
                              role="button"
                              tabindex="0">
                            <slot name="cancel">
                                <button class="btn btn-primary" tabindex="-1">
                                    {{ cancelButtonText }}
                                </button>
                            </slot>
                        </span>

                    </div>

                    <!--<div class="float-right">

                      <span
                        @click="nextStep"
                        @keyup.enter="nextStep"
                        role="button"
                        tabindex="0"
                        v-else
                      >
                        <slot name="next">
                          <button class="btn btn-primary" tabindex="-1">
                            {{ nextButtonText }}
                          </button>
                        </slot>
                      </span>
                    </div>-->
                </slot>
            </div>
        </div>
    </div>
</template>

<script>
    //import WizardStep from "./WizardStep";

    import flatpickr from "flatpickr";
    import 'flatpickr/dist/flatpickr.min.css'

    document.addEventListener('DOMContentLoaded', function () {
        let tomorrow = new Date();
        tomorrow.setDate(tomorrow.getDate() + 1);
        flatpickr("#EventStartDate",
            {
                minDate: 'today',
                onChange: (selectedDates, dateStr, instance) => {                 
                  flatpickr("#EventEndDate",
                  {
                      minDate: dateStr,
                  });
                }   
            });

        flatpickr("#EventEndDate",
            {
                minDate: tomorrow,
            });
    });

export default {
  components: {
    //WizardStep,
  },
  provide() {
    return {
      addStep: this.addStep,
    };
  },
  props: {
    //showWizard: Boolean,
    hideButtons: Boolean,
    //isAdmin: Boolean,
    //hidePreviousButtons: Boolean,
    //hideSteps: Boolean,
    //previousButtonText: {
    //  type: String,
    //  default: "Previous",
    //},
    //nextButtonText: {
    //  type: String,
    //  default: "Next",
    //},
    submitButtonText: {
      type: String,
      default: "Submit",
      },
      cancelButtonText: {
          type: String,
          default: "Cancel",
      },
    beforeSubmit: Function
  },
  data() {
           
    return {
      steps: [],
      stepIndex: 0,
      completedStep: 0,
      mywindow:''
    };
  },
  methods: {
    addStep(item) {
      const index = this.$slots.default.indexOf(item.$vnode);
      this.steps.splice(index, 0, item);
    },
    //nextStep() {
    //  if (this.stepIndex === this.steps.length) return;
    //  const index = this.stepIndex + 1;

    //  const currentStep = this.steps[this.stepIndex];
    //  const newStep = this.steps[index];

    //  if(currentStep.beforeNext) {
    //    if(!currentStep.beforeNext(currentStep, newStep)) {
    //      return;
    //    }
    //  }

    //  currentStep.active = false;
    //  this.stepIndex = index;
    //  newStep.active = true;

    //  // log highest completed step
    //  if(this.stepIndex > this.completedStep)
    //    this.completedStep = this.stepIndex

    //  this.$emit('afterPageChange', newStep, index);
    //},
    //prevStep() {
    //  if (this.stepIndex === 0) return;

    //  this.navigateToStep(this.stepIndex - 1);
    //},
    submit() {
      //if(this.beforeSubmit) {
      //  if(!this.beforeSubmit()) {
      //    return;
      //  }
      //}

      //this.$emit('on-submit');

        //this.$swal({
        //    title: 'Are you sure you want to submit the request?',
        //    text: '',
        //    type: 'warning',
        //    showCancelButton: true,
        //    confirmButtonText: 'Yes',
        //    cancelButtonText: 'No',
        //    showCloseButton: true,
        //    showLoaderOnConfirm: true
        //}).then((result) => {
        //    if (result.value) {
        //        if (this.beforeSubmit) {
        //            if (!this.beforeSubmit()) {
        //                return;
        //            }
        //        }
        //        this.$emit('on-submit')
        //    }
        //})


        if (this.beforeSubmit) {
            if (!this.beforeSubmit()) {
                return;
            }

            let swal;
            if (typeof window !== 'undefined' && typeof document !== 'undefined') {
                swal = require('sweetalert2').default;
            }

            swal.fire({
                title: 'Are you sure you want to submit the request?',
                text: '',
                type: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Yes',
                cancelButtonText: 'No',
                showCloseButton: true,
                showLoaderOnConfirm: false
            }).then((result) => {
                if (result.value) {
                    if (this.beforeSubmit) {
                        if (!this.beforeSubmit()) {
                            return;
                        }
                    }
                    this.$emit('on-submit')
                }
            })
        }
      },
      cancel() {
          let swal;
          if (typeof window !== 'undefined' && typeof document !== 'undefined') {
              swal = require('sweetalert2').default;
          }

          swal.fire({
              title: 'Are you sure you want to cancel the request?',
              text: '',
              type: 'warning',
              showCancelButton: true,
              confirmButtonText: 'Yes',
              cancelButtonText: 'No',
              showCloseButton: true,
              showLoaderOnConfirm: false
          }).then((result) => {
              if (result.value) {
                  location.reload();
              }
          })

        //window.close();
      }
    //navigateToStep(index) {
    //  if (index > this.completedStep && !this.isAdmin) return

    //  const currentStep = this.steps[this.stepIndex];
    //  const newStep = this.steps[index];

    //  currentStep.active = false;
    //  this.stepIndex = index;
    //  newStep.active = true;

    //  this.$emit('afterPageChange', newStep, index);
    //},
  },
  computed: {
  //  isLastStep() {
  //    return this.stepIndex === this.steps.length - 1;
  //  },
  //  displayPrevButton() {
  //    return !this.hidePreviousButtons && this.stepIndex !== 0;
  //  },
  },
  mounted() {
    this.steps[this.stepIndex].active = true;
  }
};
</script>

<style>
    .form-wizard .form-wizard-steps {
        display: flex;
    }

        .form-wizard .form-wizard-steps > li {
            -webkit-box-flex: 1;
            -ms-flex: 1;
            flex: 1;
            -webkit-box-align: center;
            -ms-flex-align: center;
            align-items: center;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap;
            -ms-flex-positive: 1;
            flex-grow: 1;
        }

        .form-wizard .form-wizard-steps > li {
            padding: 0;
            position: relative;
        }

            .form-wizard .form-wizard-steps > li .form-wizard-stepname {
                color: #595959;
                font-size: 16px;
                margin-bottom: 5px;
            }

            .form-wizard .form-wizard-steps > li .form-wizard-info {
                color: #999;
                font-size: 14px;
            }

            .form-wizard .form-wizard-steps > li .form-wizard-dot {
                position: absolute;
                width: 30px;
                height: 30px;
                display: block;
                background: #f5f5f5;
                top: 45px;
                left: 50%;
                margin-top: -15px;
                margin-left: -15px;
                border-radius: 50%;
                border: none;
            }

                .form-wizard .form-wizard-steps > li .form-wizard-dot:after {
                    content: " ";
                    width: 14px;
                    height: 14px;
                    background-color: var(--form-wizard-dot-color, #007bff);
                    border-radius: 50px;
                    position: absolute;
                    top: 8px;
                    left: 8px;
                    opacity: 0;
                }

            .form-wizard .form-wizard-steps > li.active > .form-wizard-dot,
            .form-wizard .form-wizard-steps > li.completed > .form-wizard-dot {
                background-color: var(--form-wizard-dot-bg-color, #94c0ee);
            }

                .form-wizard .form-wizard-steps > li.active > .form-wizard-dot:after,
                .form-wizard .form-wizard-steps > li.completed > .form-wizard-dot:after {
                    opacity: 1;
                }

            .form-wizard .form-wizard-steps > li .progress {
                position: relative;
                border-radius: 0px;
                height: 8px;
                box-shadow: none;
                margin: 20px 0;
                left: 50%;
            }

                .form-wizard .form-wizard-steps > li .progress > .progress-bar {
                    width: 0px;
                    box-shadow: none;
                    background-color: var(--form-wizard-dot-bg-color, #94c0ee);
                }

            .form-wizard .form-wizard-steps > li.completed > .progress > .progress-bar {
                width: 100%;
            }

            .form-wizard .form-wizard-steps > li.active > .progress > .progress-bar {
                /* width: 50%; */
            }

            .form-wizard
            .form-wizard-steps > li:first-child.active > .progress > .progress-bar {
                width: 0%;
            }

            .form-wizard
            .form-wizard-steps > li:last-child.active > .progress > .progress-bar {
                width: 100%;
            }

            .form-wizard .form-wizard-steps > li:first-child > .progress {
                left: 50%;
                /* width: 50%; */
            }

            .form-wizard .form-wizard-steps > li:last-child > .progress {
                /* width: 50%; */
                display: none;
            }

            .form-wizard .form-wizard-steps > li.disabled a.form-wizard-dot {
                pointer-events: none;
            }
    /*END Form Wizard*/
</style>