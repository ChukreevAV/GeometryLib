// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function selectPoint(pid) {
    const tr = this.document.getElementById(`tr${pid}`);
    const cir = this.document.getElementById(`cir${pid}`);

    const at = cir.attributes["fill"];
    const val = at.nodeValue;
    const color = "red";

    if (val === color) {
        at.nodeValue = "blue";
        tr.style.backgroundColor = "";
    }
    else {

        at.nodeValue = color;
        tr.style.backgroundColor = color;
    }
}