"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var grid_1 = require("./grid");
var GiphySearch = /** @class */ (function () {
    function GiphySearch() {
        var _this = this;
        var btn = document.getElementById("searchButton");
        btn.addEventListener("click", function (e) { return _this.search(); });
    }
    GiphySearch.prototype.search = function () {
        var searchBox = document.getElementById("search").value;
        var gridTarget = document.getElementById("grid");
        new grid_1.VanillaJSGrid(gridTarget, searchBox);
    };
    return GiphySearch;
}());
// start the app
new GiphySearch();
//# sourceMappingURL=index.js.map