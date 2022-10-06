import { throttle } from "throttle-debounce";
import { GiphyFetch } from "@giphy/js-fetch-api";
import { renderGrid } from "@giphy/js-components";

const getWidth = () => window.innerWidth;

const gf = new GiphyFetch("sXpGFDGZs0Dv1mmNFvYaGUvYwKX0PWIh");

var searchValue = "test";
// paginator
const fetchGifs = (offset: number) => gf.search(searchValue, { offset, limit: 10 });

export class VanillaJSGrid {
	mountNode: HTMLElement;
	el: HTMLElement;
	constructor(mountNode: HTMLElement, serachQuery: string) {
		searchValue = serachQuery;
		this.mountNode = mountNode;
		const resizeRender = throttle(500, () => this.render());
		window.addEventListener("resize", resizeRender, false);
		this.render();
	}
	render = () => {
		const width = getWidth();
		this.el = renderGrid(
			{
				width,
				fetchGifs,
				columns: width < 500 ? 2 : 3,
				gutter: 6,
				user: {}
			},
			this.mountNode
		) as unknown as HTMLElement;
	};
	remove() {
		this.el.parentNode.removeChild(this.el);
	}
}