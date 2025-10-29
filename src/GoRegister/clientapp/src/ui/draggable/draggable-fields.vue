﻿<!--MRF Changes : Added validation to restrict to add new field 
    Modified Date : 27th September 2022
    Modified By : Mandar.Khade@amexgbt.com
    Team member : Harish.Rane@amexgbt.com
    JIRA Ticket No : GoRegister/GOR-232 - revised -->    


<template>
    <draggable v-bind="dragOptions"
               tag="div"
               class="item-container"
               :list="list"
               :value="value"
               @input="emitter">
        <div class="item-group" :key="el.id" v-for="el in realValue">
            <FieldSummary :field="el" :pageId="pageId" />
        </div>
        <AddField :pageId="pageId" v-if="validation" />
    </draggable>
</template>

<script>
    import draggable from "vuedraggable";
    import FieldSummary from '../../builder/FieldSummary'
    import AddField from '../../components/fields/AddField'

    export default {
        name: "nested-test",
        methods: {
            emitter(value) {
                this.$emit("input", value);
                // Todo: The below is a terrible way of doing things ...
                // but this is the only way I could get this to work at the moment.
                // But it does work!
                this.$store.commit("setPages", this.$store.state.pages);
            }
        },
        components: {
            draggable,
            FieldSummary,
            AddField
        },

        computed: {
            dragOptions() {
                return {
                    animation: 0,
                    group: "fields",
                    disabled: false,
                    ghostClass: "ghost"
                };
            },
            // this.value when input = v-model
            // this.list  when input != v-model
            realValue() {
                let fieldList = []
                console.log(this.value)
                this.value.forEach(f => {
                    fieldList.push(this.$store.state.fields.find(field => field.id == f))
                })

                return fieldList;
                //return this.value ? this.value : this.list;
            },
            //Validation on addition of only +5 fields in existing template (Current field count 19)
            validation() {
                let flag = true;
                let fieldList1 = []
                this.value.forEach(f => {
                    fieldList1.push(this.$store.state.fields.find(field => field.id == f))
                })
                if (fieldList1.length >= 25) {
                    flag = false;
                }
                return flag;
            }
        },
        props: {
            pageId: {
                required: true,
                type: Number
            },
            value: {
                required: false,
                type: Array,
                default: null
            },
            list: {
                required: false,
                type: Array,
                default: null
            }
        }
    };
</script>



<style scoped>
    .item-group {
        padding-left: 1rem;
    }

    .add-field {
        cursor: pointer;
        border: 3px dashed #82c4ac;
        padding: 8px 20px;
        color: #82c4ac;
        text-align: center;
    }
</style>



