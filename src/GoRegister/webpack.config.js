
var path = require('path')
var webpack = require('webpack')
//var ExtractTextPlugin = require('extract-text-webpack-plugin')
const bundleOutputDir = './wwwroot/dist'
var production = process.env.NODE_ENV === 'production';


function getEntryPoints() {
    var entry = {
        admin: './Apps/frontend/main.js'
    };
    return entry;
}

module.exports = {
    entry: getEntryPoints(),
    output: {
        path: path.join(__dirname, bundleOutputDir),
        publicPath: '/dist/',
        filename: production ? '[name].min.js' : '[name].js',
        library: '[name]'
    },
    module: {
        rules: [
            {
                test: /\.vue$/,
                loader: 'vue-loader',
                options: {
                    loaders: {
                        // other vue-loader options go here
                    }
                }
            },
            {
                test: /\.js$/,
                loader: 'babel-loader',
                exclude: /node_modules/
            }
        ]
    },
    resolve: {
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        },
        extensions: ['*', '.js', '.vue', '.json']
    },
    devServer: {
        historyApiFallback: true,
        noInfo: true,
        overlay: true
    },
    performance: {
        hints: false
    },
    devtool: '#eval-source-map'
}

if (process.env.NODE_ENV === 'production') {
    module.exports.devtool = '#source-map'
    // http://vue-loader.vuejs.org/en/workflow/production.html
    module.exports.plugins = (module.exports.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        new webpack.optimize.UglifyJsPlugin({
            sourceMap: true,
            compress: {
                warnings: false
            }
        }),
        new webpack.LoaderOptionsPlugin({
            minimize: true
        })
    ])
}


//var path = require('path');
//var webpack = require('webpack')
//const MiniCssExtractPlugin = require("mini-css-extract-plugin");
//const autoprefixer = require('autoprefixer')
//var HtmlWebpackPlugin = require('html-webpack-plugin');
//const bundleOutputDir = './wwwroot/dist';
//var production = process.env.NODE_ENV === 'production';

//const CSSModuleLoader = {
//    loader: 'css-loader',
//    options: {
//        modules: {
//            localIdentName: '[name]_[local]_[hash:base64:5]',
//        },        
//        importLoaders: 2,
//        localsConvention: 'camelCase',
//        sourceMap: false, // turned off as causes delay
//    }
//}

//const CSSLoader = {
//    loader: 'css-loader',
//    options: {
//        modules: {
//            mode: "global"
//        },
//        importLoaders: 2,
//        localsConvention: 'camelCase',
//        sourceMap: false, // turned off as causes delay
//    }
//}

//const PostCSSLoader = {
//    loader: 'postcss-loader',
//    options: {
//        ident: 'postcss',
//        sourceMap: false, // turned off as causes delay
//        plugins: () => [
//            autoprefixer()
//        ]
//    }
//}

//const styleLoader = process.env.NODE_ENV !== 'production' ? 'style-loader' : MiniCssExtractPlugin.loader;

//function getEntryPoints() {
//    var entry = {
//        regeditor: './Apps/reg-editor/index.js'
//    };
//    return entry;
//}

//module.exports = {
//    context: path.resolve(__dirname),
//    entry: getEntryPoints(),
//    output: {
//        path: path.join(__dirname, bundleOutputDir),
//        publicPath: '/dist/',
//        filename: production ? '[name].min.js' : '[name].js',
//        library: '[name]'
//    },
//    module: {
//        rules: [
//            {
//                test: /\.(js)$/,
//                exclude: /node_modules/,
//                use: [{
//                    loader: 'babel-loader',
//                    options: {
//                        presets: [
//                            ['@babel/preset-env', {
//                                'targets': {
//                                    'node': 'current'
//                                },
//                                'modules': false
//                            }],
//                            '@babel/react', {
//                                'plugins': ['@babel/plugin-proposal-class-properties', 'react-hot-loader/babel']
//                            }
//                        ]

//                    }
//                }]
//            },
//            {
//                test: /\.(sa|sc|c)ss$/,
//                exclude: /\.module\.(sa|sc|c)ss$/,
//                use: [styleLoader, CSSLoader, PostCSSLoader, "sass-loader"]
//            },
//            {
//                test: /\.module\.(sa|sc|c)ss$/,
//                use: [styleLoader, CSSModuleLoader, PostCSSLoader, "sass-loader"]
//            },
//            //{ test: /\.css$/, use: ['style-loader', 'css-loader'] }
//        ]
//    },
//    plugins: [
//        new MiniCssExtractPlugin()
//    ],
//    devServer: {
//        historyApiFallback: true,
//        //noInfo: true,
//        overlay: true
//    },
//    performance: {
//        hints: false
//    },
//    devtool: '#eval-source-map',
//    mode: 'development'
//}

//if (production) {
//    module.exports.devtool = '#source-map'
//    module.exports.mode = 'production'
//    //module.exports.plugins = (module.exports.plugins || []).concat([
//    //    new webpack.DefinePlugin({
//    //        'process.env': {
//    //            NODE_ENV: '"production"'
//    //        }
//    //    }),
//    //    new webpack.optimize.minimize({
//    //        sourceMap: true,
//    //        compress: {
//    //            warnings: false
//    //        }
//    //    }),
//    //    new webpack.LoaderOptionsPlugin({
//    //        minimize: true
//    //    })
//    //])
//}
