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