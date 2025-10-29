    <!--MRF Changes : Remove Menu option from theme submenu
    Modified Date : 20th September 2022
    Modified By : Mandar.Khade @amexgbt.com
    Team member : Harish.Rame @amexgbt.com
    JIRA Ticket No : GoRegister/GOR-216 -->   


<template>
    <div>
        <div class="preview-variable"></div>
        <div v-if="loaded">
            <v-style id="theme-preview-styles">
                .preview-variable {
                {{ variableStyleString }}
                }
            </v-style>
            <div class="d-flex justify-content-between align-items-center">
                <h1>Theme</h1>
                <div>
                    <button @click="reloadPreview" class="btn btn-outline-primary mr-2">Refresh Preview</button>
                    <button @click="save" class="btn btn-primary">Save</button>
                </div>
            </div>
            <hr />
            <span class="text-muted">
                If you preview the site you will see your changes here automatically appear on the site. If you change page in
                the preview you can hit the refresh preview to update the new page with your current updates.
            </span>
            <div class="row">
                <div class="col-md-6" :key="rerenderKey">
                    <div>
                        <b-card no-body>
                            <b-tabs pills card vertical>
                                <b-tab title="General" active>
                                    <b-card-text>
                                        <colour-input v-model="model.variables.bodyBg"
                                                      :variable="variableMap['bodyBg']" />
                                        <div class="form-group">
                                            <label>Font</label>
                                            <select class="form-control" v-model="model.variables.bodyFont">
                                                <option v-for="(font, i) in model.fonts" :key="i" :value="font.name">
                                                    {{ font.name }}
                                                </option>
                                            </select>
                                        </div>
                                        <colour-input v-model="model.variables.bodyColor"
                                                      :variable="variableMap['bodyColor']" />
                                        <colour-input v-model="model.variables.hColor"
                                                      :variable="variableMap['hColor']" />
                                        <div class="form-group">
                                            <label>Header Font</label>
                                            <select class="form-control" v-model="model.variables.hFont">
                                                <option v-for="(font, i) in model.fonts" :key="i" :value="font.name">
                                                    {{ font.name }}
                                                </option>
                                            </select>
                                        </div>
                                        <h4>Colours</h4>
                                        <colour-input v-model="model.variables.primary" :variable="variableMap['primary']" />
                                        <colour-input v-model="model.variables.primaryLight" :variable="variableMap['primaryLight']" />
                                        <colour-input v-model="model.variables.primaryDark" :variable="variableMap['primaryDark']" />
                                    </b-card-text>
                                </b-tab>
                                <b-tab title="Links">
                                    <b-card-text>
                                        <colour-input v-model="model.variables.linkColor"
                                                      :variable="variableMap['linkColor']" />
                                        <colour-input v-model="model.variables.linkHoverColor"
                                                      :variable="variableMap['linkHoverColor']" />
                                        <div class="form-group">
                                            <label for="">Link Hover Decoration</label>
                                            <input type="text" v-model="model.variables.linkHoverDecoration" class="form-control" />
                                        </div>
                                    </b-card-text>
                                </b-tab>
                                <b-tab title="Logo">
                                    <b-card-text>
                                        <img v-if="model.logoUrl" :src="model.logoUrl" class="img-fluid" />
                                        <dl>
                                            <dt>
                                                <label>Select file</label>
                                            </dt>
                                            <dd>
                                                <input type="file" ref="file" v-on:change="handleFileUpload()" accept="image/png, image/gif, image/jpeg" />
                                                <span></span>
                                            </dd>
                                        </dl>
                                        <div></div>
                                        <button class="btn btn-outline-primary" v-on:click="submitFile()" :disabled="file === null">
                                            Upload
                                        </button>&nbsp;
                                        <button class="btn btn-outline-primary" v-on:click="deleteFile()" :disabled="delfile === null"> 
                                            Delete
                                        </button>
                                    </b-card-text>
                                </b-tab>
                                <!--Commented by Mandar Khade for MRF requirement on 20th Sept 2022 -->
                                <!--<b-tab title="Menu">
                                  <b-card-text>
                                    <colour-input v-model="model.variables.navBgColor" :variable="variableMap['navBgColor']" />
                                    <colour-input v-model="model.variables.navItemColor" :variable="variableMap['navItemColor']" />
                                    <colour-input v-model="model.variables.navItemColorHover" :variable="variableMap['navItemColorHover']" />
                                  </b-card-text>
                                </b-tab>-->
                                <b-tab title="Forms">
                                    <b-card-text>
                                        <colour-input v-model="model.variables.labelColor" :variable="variableMap['labelColor']" />
                                        <h4>Wizard</h4>
                                        <colour-input v-model="model.variables.formWizardDotColor"
                                                      :variable="variableMap['formWizardDotColor']" />
                                        <colour-input v-model="model.variables.formWizardDotBgColor"
                                                      :variable="variableMap['formWizardDotBgColor']" />
                                    </b-card-text>
                                </b-tab>
                            </b-tabs>
                        </b-card>
                    </div>
                </div>
                <div class="col-md-6">
                    <div>
                        <b-tabs content-class="mt-3" fill>
                            <b-tab title="CSS" active>
                                <codemirror v-model="model.themeCss" :options="cmOptions" />
                            </b-tab>
                            <b-tab title="Header and Footer">
                                <div class="form-group">
                                    <label>Header HTML</label>
                                    <ckeditor v-model="model.headerHtml" :config="ckConfig" :editor="ckEditor" />
                                </div>
                                <div class="form-group">
                                    <label for="">Footer HTML</label>
                                    <ckeditor v-model="model.footerHtml" :config="ckConfig" :editor="ckEditor" />
                                </div>
                            </b-tab>
                            <b-tab title="Scripts">
                                <div class="form-group">
                                    <label>Header Scripts</label>
                                    <codemirror v-model="model.headerScripts" :options="cmOptionsScript" />
                                </div>
                                <div class="form-group">
                                    <label>Footer Scripts</label>
                                    <codemirror v-model="model.footerScripts" :options="cmOptionsScript" />
                                </div>
                                <span class="text-muted">Please note this will not live update in the preview</span>
                            </b-tab>
                            <b-tab title="Fonts">
                                <font-import :fonts="model.fonts" v-on:font-added="addFont"></font-import>
                            </b-tab>
                        </b-tabs>
                    </div>
                </div>
            </div>

            <div></div>
        </div>
    </div>
</template>
<script>
    import debounce from "lodash/debounce";
    import axios from "axios";

    import ColourInput from "../components/ColourInput.vue";
    import FontImport from "../components/FontImport.vue";
    import VStyle from "../components/VStyle.vue";

    import { getCkEditorConfig } from "@/shared/ckeditor";
    import CKEditor from "@ckeditor/ckeditor5-vue2";
    import ClassicEditor from "@ckeditor/ckeditor5-editor-classic/src/classiceditor";

    // codemirror
    import { codemirror } from "vue-codemirror";
    import "codemirror/lib/codemirror.css";
    import "codemirror/mode/javascript/javascript.js";
    import "codemirror/mode/css/css.js";

    import { get, save } from "../api.js";
   
    export default {
        components: {
            ColourInput,
            codemirror,
            ckeditor: CKEditor.component,
            FontImport,
            VStyle,
        },
        data: () => ({
            model: {
                variables: {},
                css: "",
                footerHTML: "",
                headerHTML: "",
                fonts: [],
            },
            viewModel: {
                // variables: {
                //   h1color: {
                //     key: "h1color",
                //     variable: "--h1-color",
                //     label: "Heading 1 Colour",
                //   },
                //   h2color: {
                //     key: "h2color",
                //     variable: "--h2-color",
                //     label: "Heading 2 Colour",
                //   },
                //   h3color: {
                //     key: "h3color",
                //     variable: "--h3-color",
                //     label: "Heading 3 Colour",
                //   },
                //   bodyColor: {
                //     key: "bodyColor",
                //     variable: "--h4-color",
                //     label: "Heading 4 Colour",
                //   },
                //   font: {
                //     key: "h5color",
                //     variable: "--h5-color",
                //     label: "Heading 5 Colour",
                //   },
                //   h6color: {
                //     key: "h6color",
                //     variable: "--h6-color",
                //     label: "Heading 6 Colour",
                //   },
                //   bodyBg: {
                //     key: "bodyBg",
                //     variable: "--body-bg",
                //     label: "Background Color",
                //   },
                //   linkColor: {
                //     key: "linkColor",
                //     variable: "--link-color",
                //     label: "Link Color",
                //   },
                //   linkHoverColor: {
                //     key: "linkHoverColor",
                //     variable: "--link-hover-color",
                //     label: "Link Hover Color",
                //   },
                //   linkHoverDecoration: {
                //     key: "linkHoverDecoration",
                //     variable: "--link-hover-decoration",
                //     label: "Link Hover Decoration",
                //   },
                // },
            },
            cmOptions: {
                tabSize: 4,
                mode: "text/css",
                lineNumbers: true,
                line: true,
                // more CodeMirror options...
            },
            cmOptionsScript: {
                tabSize: 4,
                mode: "text/javascript",
                lineNumbers: true,
                line: true,
                // more CodeMirror options...
            },
            loaded: false,
            ckEditor: ClassicEditor,
            file: null,
            delfile: null,
            rerenderKey: 0,
            variableMap: {},
            
        }),
        async created() {
            this.fetchData();
            this.ckConfig = getCkEditorConfig(true);
            this.rerender();
           
        },
        methods: {
            rerender() {
                this.rerenderKey++;
            },
            fetchData() {
                get().then((result) => {
                    this.model = result.model;
                    delete result.model;
                    this.viewModel = result;
                    this.updateVariableMap();
                    this.loaded = true;
                    
                    if (this.model.logoUrl == null) {
                        this.delfile = null;
                    }
                    else if (this.model.logoUrl == '') {
                        this.delfile = null;
                    }
                    else if (this.model.logoUrl != '') {
                        this.delfile = this.model.logoUrl;
                    }
                });
            },
            reloadPreviewDebounced: debounce(function () {
                this.reloadPreview();
            }, 2000),
            reloadPreview() {
                localStorage.setItem(
                    `theme-preview`,
                    JSON.stringify({
                        variables: this.variableStyles,
                        css: this.model.themeCss,
                        headerHTML: this.model.headerHtml,
                        footerHTML: this.model.footerHtml,
                        logoUrl: this.model.logoUrl,
                        fonts: this.model.fonts,
                    })
                );
            },
            addFont(font) {
                this.model.fonts.push(font);
            },
            submitFile() {
                let formData = new FormData();
                formData.append("file", this.file);
                const $this = this;
                axios
                    .post("/api/projecttheme/uploadlogo", formData, {
                        headers: {
                            "Content-Type": "multipart/form-data",
                        },
                    })
                    .then(function (response) {
                        $this.model.logoUrl = response.data.logoUrl;
                        $this.file = null;
                        $this.delfile = response.data.logoUrl;
                    })
                    .catch(function (error) {
                        console.log(error);
                        $this.error = error.response.data;
                    });
            },
            deleteFile() {
                //let imgData = new Image();
                //imgData.append("file", this.model.logUrl);
                //alert(this.model.logoUrl);
                //debugger;
                //let imgData = new string();
                //imgData.append("imgurl", this.model.logUrl)
                //alert(imgData);
                const $this = this;
                axios
                    .post("/api/projecttheme/DeleteLogo", this.model, {
                        headers: {
                            'Content-Type': 'application/json;charset=UTF-8',
                            "Access-Control-Allow-Origin": "*",
                        }
                        })
                    .then(function (response) {
                        $this.model.logoUrl = null;
                        $this.delfile = null;
                        $this.file = null;
                    })
                    .catch(function (error) {
                        console.log(error);
                        $this.error = error.response.data;
                    });
            },
            handleFileUpload() {
                this.file = this.$refs.file.files[0];
                this.delfile = null;
            },
            save() {
                const $this = this;
                save(this.model)
                    .then((result) => {
                        $this.$root.$bvToast.toast(`Theme saved`, {
                            title: "Saved",
                            autoHideDelay: 3000,
                            variant: "success",
                        });
                        $this.fetchData();
                    })
                    .catch(function (error) {
                        $this.$root.$bvToast.toast(`There was an issue saving your theme.`, {
                            title: "Error",
                            autoHideDelay: 3000,
                            variant: "danger",
                        });
                    });
            },
            updateVariableMap() {
                const computedStyles = getComputedStyle(document.querySelector(".preview-variable"));
                const map = {};
                Object.keys(this.model.variables).map((e) => {
                    let value = this.model.variables[e];
                    if (!value) return;
                    const details = this.viewModel.variables[e];
                    if (!details) return;

                    let calculatedValue = value.trim();
                    if (value.startsWith("var(") && value.endsWith(")")) {
                        let vari = calculatedValue.slice(4);
                        vari = vari.substring(0, vari.length - 1);
                        console.log(vari);
                        calculatedValue = computedStyles.getPropertyValue(vari);
                    }
                    map[e] = {
                        model: details,
                        value: value,
                        calculated: calculatedValue,
                    };
                });

                this.variableMap = map;
            },
        },
        computed: {
            variableStyles() {
                const computedStyles = getComputedStyle(document.querySelector(".preview-variable"));
                return Object.keys(this.model.variables).map((e) => {
                    let value = this.model.variables[e];
                    if (!value) return;
                    const details = this.viewModel.variables[e];
                    if (!details) return;

                    if (details.type === "Font") {
                        value === `'${value}'`;
                    }

                    let calculatedValue = value.trim();
                    if (value.startsWith("var(") && value.endsWith(")")) {
                        let vari = calculatedValue.slice(4);
                        vari = vari.substring(0, vari.length - 1);
                        console.log(vari);
                        calculatedValue = computedStyles.getPropertyValue(vari);
                    }
                    return {
                        name: details.variable,
                        value: value,
                        calculated: calculatedValue,
                    };
                });
            },
            variableStyleString() {
                let styleString = "";
                for (let index = 0; index < this.variableStyles.length; index++) {
                    const variable = this.variableStyles[index];
                    styleString += `${variable.name}:${variable.value};`;
                }

                return styleString;
            },
        },
        watch: {
            model: {
                deep: true,
                handler: function (oldSate, newState) {
                    this.reloadPreviewDebounced();
                    this.$nextTick(() => {
                        this.updateVariableMap();
                    });
                },
            },
        },
    };
</script>