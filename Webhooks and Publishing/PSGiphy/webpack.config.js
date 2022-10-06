const path = require('path');

module.exports = {
	entry: {
		index: './Scripts/index',
		grid: './Scripts/grid'
	},
	mode: 'production',
	optimization: {
		minimize: true,
		splitChunks: {
			chunks: 'all',
			minSize: 0,
			name: 'shared'
		}
	},
	module: {
		rules: [
			{
				test: /\.tsx?$/,
				use: 'ts-loader',
				exclude: /node_modules/
			}
		]
	},
	resolve: {
		extensions: ['.tsx', '.ts', '.js']
	},
	output: {
		filename: '[name].js',
		path: path.resolve(__dirname, 'wwwroot/js'),
		library: 'sample',
		libraryTarget: 'umd'
	}
};