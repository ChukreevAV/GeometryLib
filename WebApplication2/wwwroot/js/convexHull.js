const uri = 'ConvexHull';

function start() {
    window.fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error("Unable to get items.", error));
}

async function addItem() {

    var response1 = await window.fetch(uri,
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

var _data;

function _displayItems(data) {
    _data = data;

    const canvas1 = document.getElementById("canvas1");
    clear(canvas1);

    //drawPath(data.points, "red", canvas1);

    for (let i = 0; i < data.points.length; i++) {
        let color = "blue";
        const p = data.points[i];
        //if (contain(data.selectPoints, p)) color = "grey";
        drawPoint(p, color, canvas1);
    }

    if (_data.upConvexHull != null) drawPath(_data.upConvexHull, "red", canvas1);
    if (_data.downConvexHull != null) drawPath(_data.downConvexHull, "blue", canvas1);
}

function drawPath(points, stroke, canvas) {
    if (points.length > 1) canvas.appendChild(createPath(points, stroke));
}

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