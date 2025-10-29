let fieldEditorComponents = {};

let fields = require.context("./editors/", false, /^\.\/([\w-_]+)\.vue$/);
console.log(fields.keys())
for (let index = 0; index < fields.keys().length; index++) {
  const key = fields.keys()[index];
  let compName = key.replace(/^\.\//, "").replace(/\.vue/, "");
  fieldEditorComponents[compName] = fields(key).default;
}



export { fieldEditorComponents }