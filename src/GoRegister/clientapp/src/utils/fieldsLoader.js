let fieldComponents = {};

let fields = require.context("../components/fields", false, /^\.\/([\w-_]+)\.vue$/);
console.log(fields.keys())
for (let index = 0; index < fields.keys().length; index++) {
    const key = fields.keys()[index];
	let compName = key.replace(/^\.\//, "").replace(/\.vue/, "");
	fieldComponents[compName] = fields(key).default;
}

module.exports = fieldComponents;