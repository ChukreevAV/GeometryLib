const uri = "LineIntersections";

function start() {
    window.fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error("Unable to get items.", error));
}

var _data;

function _displayItems(data) {
    _data = data;
    write1();

    const canvas1 = document.getElementById("canvas1");
    clear(canvas1);

    if (data.lines != null) data.lines.forEach(l => drawLine(l.id, l, "green", canvas1));
}

function pointToStr(p) {
    return `${p.x.toFixed(4)};${p.y.toFixed(4)}`;
}

function createLineToP(line) {
    const p = document.createElement("div");
    //p.innerText = `test1`;
    p.setAttribute("id", `div${line.id}`);
    p.setAttribute("class", `div-1`);
    p.setAttribute("onclick", `selectPoint1(${line.id})`);
    p.innerText = `start: ${pointToStr(line.start)}; end ${pointToStr(line.end)}`;
    return p;
}

function selectPoint1(pid) {
    const line = this.document.getElementById(`line${pid}`);
    const div1 = this.document.getElementById(`div${pid}`);

    if (line != null) {
        const at = line.attributes["style"];
        const val = at.nodeValue;
        const color = "stroke:red;stroke-width:0.005";

        if (val === color) {
            at.nodeValue = "stroke:green;stroke-width:0.005";
            div1.setAttribute("class", `div-1`);
        }
        else {
            at.nodeValue = color;
            div1.setAttribute("class", `div-2`);
        }
    }

}

function createDetails(ev) {
    const details = document.createElement("details");
    const summary = document.createElement("summary");
    summary.innerText = `${pointToStr(ev.point)}`;
    //details.innerText = `test1\ntest2\ntest3\n`;
    details.appendChild(summary);

    if (ev.lines != null) ev.lines.forEach(l => details.appendChild(createLineToP((l))));
    //details.appendChild(`test1\ntest2\ntest3\n`);
    return details;
}

function write1() {
    const canvas1 = document.getElementById("column1");
    //var test1 = "";
    //if (_data.sweepEvents != null) _data.sweepEvents.forEach(ev => test1 += `${pointToStr(ev.point)}\n`);
    //canvas1.innerText = test1;

    if (_data.sweepEvents != null) _data.sweepEvents.forEach(ev => {
        canvas1.appendChild(createDetails(ev));
    });
}