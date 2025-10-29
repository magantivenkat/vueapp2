<!--MRF Changes : Destination
  Modified Date : 11th November 2022
  Modified By : Mandar.Khade@amexgbt.com
  Team member : Harish.Rane@amexgbt.com
  JIRA Ticket No : GoRegister/GOR-240-new01-->


<template>
    <InputWrapper :context="context">

        <vue-suggest class="asdad" pattern="\w+"
                     v-model="proxy"
                     :list="getList"
                     :max-suggestions="10"
                     :min-length="3"
                     :debounce="700"
                     :filter-by-query="false"
                     :controls="{
          selectionUp: [38, 33],
          selectionDown: [40, 34],
          select: [13, 36],
          showList: [40],
          hideList: [27, 35]
        }"
                     :mode="mode"
                     :nullable-select="true"
                     ref="suggestComponent"
                     placeholder="Search information..."
                     value-attribute="id"
                     display-attribute="description"
                     @suggestion-click="onSuggestClick"
                     @select="onSuggestSelect"
                     @hover="onSuggestHover"
                     @focus="onFocus"
                     @blur="onBlur"
                     @request-start="onRequestStart"
                     @request-done="onRequestDone"
                     @request-failed="onRequestFailed"
                     @show-list="onShowList"
                     @hide-list="onHideList">
            <!-- <input type="text"> -->

            <div class="g">
                <input placeholder="Search your destination..." id="myInput" type="text">
            </div>

            <!--<test-input placeholder="Search information..." />-->

            <div slot="suggestion-item" slot-scope="scope" :title="scope.suggestion.description">
                <div class="text">
                    <span v-html="boldenSuggestion(scope)"></span>
                </div>

            </div>

            <div class="misc-item" slot="misc-item-below"  v-if="loading">
                <span>Loading...</span>
            </div>
        </vue-suggest>

    </InputWrapper>
</template>

<script>
    import inputMixin from "../InputMixin";
    import InputWrapper from "./InputWrapper.vue";
    import VueSuggest from 'vue-simple-suggest/lib';
    import TestInput from './TestInput.vue';

    const key = "eyJpc3MiOiJtZWV0aW5ncy1jZW50cmFsIiwiYXVkIjoibWVldGluZ19yZXF1ZXN0cyIsInN1YiI6Im1yZiIsImV4cCI6MTc2NTI3OTI4NiwiYWxnIjoiSFMyNTYifQ.e30.FK_lS_S3FU7zkPoNcX853UwFgHU2r3OX8k-E5MtYY9o";
    export default {
        mixins: [inputMixin],
        components: {
            InputWrapper, VueSuggest
        },
        data() {
            return {
                selected: null,
                model: null,
                mode: 'select',
                loading: false
            }
        },
        methods: {
            boldenSuggestion(scope) {
                if (!scope) return scope;
                const { suggestion, query } = scope;
                let result = this.$refs.suggestComponent.displayProperty(suggestion);
                if (!query) return result;
                const texts = query.split(/[\s-_/\\|\\.]/gm).filter(t => !!t) || [''];
                return result.replace(new RegExp('(.*?)(' + texts.join('|') + ')(.*?)', 'gi'), '$1<b>$2</b>$3');
            },
            goto(url) {
                window.open(url, '_blank').focus()
            },

            onFocus(e) {

            },
            onBlur(e) {

            },
            onShowList() {

            },
            onHideList() {

            },
            onSuggestClick(suggest, e) {

            },
            onSuggestSelect(suggest) {

                this.selected = suggest;

            },
            onSuggestHover(suggestion) {

            },
            onRequestStart(value) {
                this.loading = true

            },
            onRequestDone(e) {
                this.loading = false

            },
            onRequestFailed(e) {
                this.loading = false

            },
            getList(inputValue) {
                return new Promise((resolve, reject) => {
                    var inputString = encodeURIComponent(inputValue);
                    let url = `https://meetingscentral-beta.amexgbt.com/api/v1/mrf/destinations?query=${inputString}`
                    this.$refs.suggestComponent.clearSuggestions()

                    fetch(url,
                        {
                            method: "GET",
                            headers: {
                                "Content-type": "application/json;charset=UTF-8",
                                "Authorization": key
                            }
                        }
                    ).then(response => {
                        if (!response.ok) {
                            reject()
                        }
                        response.json().then(json => {
                            var result = [];

                            for (let i = 0; i < json.length; i++) {
                                result.push({  "id": json[i]["id"], "name": json[i]["name"], "description": json[i]["description"] });
                            }

                            resolve(result)

                        }).catch(e => {
                            reject(e)
                        })
                    }).catch(error => {
                        this.loading = false
                        reject(error)
                    })
                })
            }
        },
        props: {

        },
        computed: {
            proxy: {
                get() {
                    return this.value;
                },
                set(value) {
                    this.$emit("input", value);
                },
            },

        }
    };
</script>

<style>

    .vue-simple-suggest-enter-active.suggestions,
    .vue-simple-suggest-leave-active.suggestions {
        transition: opacity .2s;
    }

    .vue-simple-suggest-enter.suggestions,
    .vue-simple-suggest-leave-to.suggestions {
        opacity: 0 !important;
    }

    .vue-simple-suggest.designed .input-wrapper input {
        display: block;
        width: 100%;
        padding: 10px;
        border: 2px solid #000000 !important;
        border-radius: 3px;
        color: black;
        background: white;
        outline: none;
        -webkit-transition: all .1s;
        transition: all .1s;
        -webkit-transition-delay: .05s;
        transition-delay: .05s;
    }
</style>
