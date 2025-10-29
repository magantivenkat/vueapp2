const path = require("path");
const CKEditorWebpackPlugin = require("@ckeditor/ckeditor5-dev-webpack-plugin");
const { styles } = require("@ckeditor/ckeditor5-dev-utils");

module.exports = {
  filenameHashing: false,
  publicPath: "/",
  //publicPath: 'https://localhost:8081/',
  parallel: false,
  configureWebpack: {
    output: {
      hotUpdateMainFilename: "__hmr/[hash].hot-update.json",
      hotUpdateChunkFilename: "__hmr/[id].[hash].hot-update.js",
    },
    plugins: [
      // CKEditor needs its own plugin to be built using webpack, only run for the admin build
      ...(process.env.USE_CKEDITOR === 'true' ? [new CKEditorWebpackPlugin({
        // See https://ckeditor.com/docs/ckeditor5/latest/features/ui-language.html
        language: "en",
        // Append translations to the file matching the `app` name.
        translationsOutputFile: /app/,
      })] : []),
    ],
  },
  devServer: {
    //writeToDisk: true,
    host: "localhost",
    public: "https://localhost:5021",
    // https: {
    //     key: fs.readFileSync('C:/Users/hwestb/Downloads/localhost+2-key.pem'),
    //     cert: fs.readFileSync('C:/Users/hwestb/Downloads/localhost+2.pem'),
    // },
    //transportMode: 'ws',
    //sockHost: 'localhost',
    //sockPort: 5021,
    port: 8081,
    publicPath: "/",
    //hot: true
  },
  // devServer: {
  //     proxy: "https://localhost:8081",
  //     https: {
  //         key: fs.readFileSync('C:/Users/hwestb/Downloads/localhost+2-key.pem'),
  //         cert: fs.readFileSync('C:/Users/hwestb/Downloads/localhost+2.pem'),
  //     },
  // }
  transpileDependencies: [/ckeditor5-[^/\\]+[/\\]src[/\\].+\.js$/],

  // Vue CLI would normally use its own loader to load .svg and .css files, however:
  //	1. The icons used by CKEditor must be loaded using raw-loader,
  //	2. The CSS used by CKEditor must be transpiled using PostCSS to load properly.
  chainWebpack: (config) => {
    // (1.) To handle editor icons, get the default rule for *.svg files first:
    const svgRule = config.module.rule("svg");

    // Then you can either:
    //
    // * clear all loaders for existing 'svg' rule:
    //
    //		svgRule.uses.clear();
    //
    // * or exclude ckeditor directory from node_modules:
    svgRule.exclude.add(path.join(__dirname, "node_modules", "@ckeditor"));

    // Add an entry for *.svg files belonging to CKEditor. You can either:
    //
    // * modify the existing 'svg' rule:
    //
    //		svgRule.use( 'raw-loader' ).loader( 'raw-loader' );
    //
    // * or add a new one:
    config.module
      .rule("cke-svg")
      .test(/ckeditor5-[^/\\]+[/\\]theme[/\\]icons[/\\][^/\\]+\.svg$/)
      .use("raw-loader")
      .loader("raw-loader");

    // (2.) Transpile the .css files imported by the editor using PostCSS.
    // Make sure only the CSS belonging to ckeditor5-* packages is processed this way.
    config.module
      .rule("cke-css")
      .test(/ckeditor5-[^/\\]+[/\\].+\.css$/)
      .use("postcss-loader")
      .loader("postcss-loader")
      .tap(() => {
        return styles.getPostCssConfig({
          themeImporter: {
            themePath: require.resolve("@ckeditor/ckeditor5-theme-lark"),
          },
          minify: true,
        });
      });
  },
};
