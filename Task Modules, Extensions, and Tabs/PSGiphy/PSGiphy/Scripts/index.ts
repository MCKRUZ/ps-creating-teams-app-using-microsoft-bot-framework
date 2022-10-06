import { VanillaJSGrid } from "./grid";

class GiphySearch {
	constructor() {
		let btn = document.getElementById("searchButton");
		btn.addEventListener("click", (e: Event) => this.search());
	}
	search() {
		let searchBox = (<HTMLInputElement>document.getElementById("search")).value;
		let gridTarget = document.getElementById("grid");
		new VanillaJSGrid(gridTarget, searchBox);
	}
}

// start the app
new GiphySearch();