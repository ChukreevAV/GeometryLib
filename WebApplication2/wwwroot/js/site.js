function getItems() {
    fetch('ConvexHull')
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function createCircle(x, y, color) {
    const svgns = "http://www.w3.org/2000/svg";
    const circle = document.createElementNS(svgns, 'circle');
    circle.setAttributeNS(null, 'cx', x);
    circle.setAttributeNS(null, 'cy', y);
    circle.setAttributeNS(null, 'r', '0.005');
    circle.setAttributeNS(null, 'fill', color);
    return circle;
}

function createStyle(color) {
    return `stroke:${color};stroke-width:0.005`;
}

function createLine(x1, y1, x2, y2, style) {
    const svgns = "http://www.w3.org/2000/svg";
    const line = document.createElementNS(svgns, 'line');
    line.setAttributeNS(null, 'x1', x1);
    line.setAttributeNS(null, 'y1', y1);
    line.setAttributeNS(null, 'x2', x2);
    line.setAttributeNS(null, 'y2', y2);
    line.setAttributeNS(null, 'style', style);
    return line;
}

function drawPoint(p, canvas) {
    canvas.appendChild(createCircle(p.x, p.y, 'blue'));
}

function drawLine(l, color, canvas) {
    canvas.appendChild(createLine(l.start.x, l.start.y, l.end.x, l.end.y, createStyle(color)));
}

var _data;

function clear(prnt) {
    let children = prnt.children;
    for (let i = 0; i < children.length;) {
        let el = children[i];
        if (el.tagName !== 'defs') {
            el.remove();
        } else (i++);
    }
}

function _displayItems(data) {
    _data = data;
    const canvas1 = document.getElementById('canvas1');
    clear(canvas1);
    data.points.forEach(p => drawPoint(p, canvas1));
    if (data.convexHull != null) data.convexHull.forEach(l => drawLine(l, 'green', canvas1));
    if (data.unselectLines != null) data.unselectLines.forEach(l => drawLine(l, 'grey', canvas1));
    if (data.currentLine != null) drawLine(data.currentLine, 'red', canvas1);
    //canvas1.appendChild(createCircle(0.5, 0.5, 'blue'));
}

async function addItem() {
    var j1 = JSON.stringify(_data);

    let response = await window.fetch('ConvexHull',
        {
            method: 'POST',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: j1
        });

    if (response.ok) { // если HTTP-статус в диапазоне 200-299
        // получаем тело ответа (см. про этот метод ниже)
        const json = await response.json();
        _displayItems(json);
    } else {
        alert("Ошибка HTTP: " + response.status);
    }
}