function getItems() {
    fetch("ConvexHull")
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error("Unable to get items.", error));
}

const svgNameSpace = "http://www.w3.org/2000/svg";

function createCircle(x, y, color) {
    const circle = document.createElementNS(svgNameSpace, "circle");
    circle.setAttributeNS(null, "cx", x);
    circle.setAttributeNS(null, "cy", y);
    circle.setAttributeNS(null, "r", "0.005");
    circle.setAttributeNS(null, "fill", color);
    return circle;
}

function createLine(x1, y1, x2, y2, style) {
    const line = document.createElementNS(svgNameSpace, "line");
    line.setAttributeNS(null, "x1", x1);
    line.setAttributeNS(null, "y1", y1);
    line.setAttributeNS(null, "x2", x2);
    line.setAttributeNS(null, "y2", y2);
    line.setAttributeNS(null, "style", style);
    return line;
}

function createStyle(color) {
    return `stroke:${color};stroke-width:0.005`;
}

function drawPoint(p, color, canvas) {
    canvas.appendChild(createCircle(p.x, p.y, color));
}

function drawLine(l, color, canvas) {
    canvas.appendChild(createLine(l.start.x, l.start.y, l.end.x, l.end.y, createStyle(color)));
}

var _data;

function clear(prnt) {
    const children = prnt.children;
    for (let i = 0; i < children.length;) {
        const el = children[i];
        if (el.tagName !== "defs") {
            el.remove();
        } else (i++);
    }
}

function distance(p1, p2) {
    const a = p2.x - p1.x;
    const b = p2.y - p1.y;
    return Math.sqrt(a * a + b * b);
}

function equalsPoints(p1, p2) {
    return distance(p1, p2) < 0.000001;
}

function contain(list, tp) {

    for (let i = 0; i < list.length; i++) {
        if (equalsPoints(list[i], tp)) return true;
    }
    return false;
}

function _displayItems(data) {
    _data = data;
    if (data.selectPoints == null) data.selectPoints = [];
    const canvas1 = document.getElementById("canvas1");
    clear(canvas1);

    for (let i = 0; i < data.points.length; i++) {
        let color = "blue";
        const p = data.points[i];
        if (contain(data.selectPoints, p)) color = "grey";
        drawPoint(p, color, canvas1);
    }

    if (data.convexHull != null) data.convexHull.forEach(l => drawLine(l, "green", canvas1));
    if (data.unselectLines != null) data.unselectLines.forEach(l => drawLine(l, "grey", canvas1));
    if (data.currentLine != null) drawLine(data.currentLine, "red", canvas1);
    //canvas1.appendChild(createCircle(0.5, 0.5, 'blue'));
}

async function addItem() {

    var response1 = await fetch("ConvexHull",
        {
            method: "POST",
            headers: {
                'Accept': "application/json",
                'Content-Type': "application/json"
            },
            body: JSON.stringify(_data)
        });

    if (response1.ok) { // если HTTP-статус в диапазоне 200-299
        // получаем тело ответа (см. про этот метод ниже)
        const json = await response1.json();
        _displayItems(json);
    } else {
        alert(`Ошибка HTTP: ${response1.status}`);
    }
}