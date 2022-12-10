const uri = 'SlowConvexHull';

function getItems() {
    window.fetch(uri)
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error("Unable to get items.", error));
}

async function addItem() {

    const response1 = await window.fetch(uri,
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
    write1();
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
}

function write1() {
    const canvas1 = document.getElementById("column1");
    var test1 = "";
    test1 += `index1 : ${_data.index1}\n`;
    test1 += `index2 : ${_data.index2}\n`;
    test1 += `sign : ${_data.sign}\n`;
    canvas1.innerText = test1;
}