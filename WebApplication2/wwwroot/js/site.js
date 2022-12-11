const svgNameSpace = "http://www.w3.org/2000/svg";

//Создаём круг
function createCircle(x, y, color) {
    const circle = document.createElementNS(svgNameSpace, "circle");
    circle.setAttributeNS(null, "cx", x);
    circle.setAttributeNS(null, "cy", y);
    circle.setAttributeNS(null, "r", "0.005");
    circle.setAttributeNS(null, "fill", color);
    return circle;
}

//Создаём отрезок
function createLine(x1, y1, x2, y2, style) {
    const line = document.createElementNS(svgNameSpace, "line");
    line.setAttributeNS(null, "x1", x1);
    line.setAttributeNS(null, "y1", y1);
    line.setAttributeNS(null, "x2", x2);
    line.setAttributeNS(null, "y2", y2);
    line.setAttributeNS(null, "style", style);
    return line;
}

function createLine(id, x1, y1, x2, y2, style) {
    const line = document.createElementNS(svgNameSpace, "line");
    line.setAttributeNS(null, "id", `line${id}`);
    line.setAttributeNS(null, "x1", x1);
    line.setAttributeNS(null, "y1", y1);
    line.setAttributeNS(null, "x2", x2);
    line.setAttributeNS(null, "y2", y2);
    line.setAttributeNS(null, "style", style);
    return line;
}

//Создаём стиль
function createStyle(color) {
    return `stroke:${color};stroke-width:0.005`;
}

//Создаём путь
function createPath(points, stroke) {
    const path = document.createElementNS(svgNameSpace, "path");
    var pStr = "M";
    points.forEach(p => pStr += `${p.x} ${p.y}`);
    path.setAttributeNS(null, "d", pStr);
    path.setAttributeNS(null, "stroke", stroke);
    path.setAttributeNS(null, "stroke-width", 0.005);
    path.setAttributeNS(null, "fill", "transparent");
    return path;
}

//Рисуем путь
function drawPath(points, stroke, canvas) {
    if (points.length > 1) canvas.appendChild(createPath(points, stroke));
}

//Рисуем точку кругом
function drawPoint(p, color, canvas) {
    canvas.appendChild(createCircle(p.x, p.y, color));
}

//Рисуем отрезок
function drawLine(l, color, canvas) {
    canvas.appendChild(createLine(l.start.x, l.start.y, l.end.x, l.end.y, createStyle(color)));
}

function drawLine(id, l, color, canvas) {
    canvas.appendChild(createLine(id, l.start.x, l.start.y, l.end.x, l.end.y, createStyle(color)));
}

//Очищаем рисунок
function clear(prnt) {
    const children = prnt.children;
    for (let i = 0; i < children.length;) {
        const el = children[i];
        if (el.tagName !== "defs") {
            el.remove();
        } else (i++);
    }
}

//Растояние между точками
function distance(p1, p2) {
    const a = p2.x - p1.x;
    const b = p2.y - p1.y;
    return Math.sqrt(a * a + b * b);
}

//Сравнение точек
function equalsPoints(p1, p2) {
    return distance(p1, p2) < 0.000001;
}

//Наличие точки в списке
function contain(list, tp) {

    for (let i = 0; i < list.length; i++) {
        if (equalsPoints(list[i], tp)) return true;
    }
    return false;
}