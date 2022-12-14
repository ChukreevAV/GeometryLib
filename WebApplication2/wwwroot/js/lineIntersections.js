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
    write2();

    const canvas1 = document.getElementById("canvas1");
    clear(canvas1);

    if (data.lines != null) data.lines.forEach(l => drawLine(l.id, l, "green", canvas1));
    if (data.result != null) data.result.forEach(r => drawPoint(r.point, "blue", canvas1));
    if (_data.sweepEvents != null) {
        var firstEvent = _data.sweepEvents[0];
        drawPoint(firstEvent.point, "red", canvas1);
    }

    const canvas2 = document.getElementById("lines");

    if (_data.lines != null && canvas2.childElementCount === 0) {
        var str1 = "";
        _data.lines.forEach(l => str1 += `${lineToStr(l)}\n`);
        canvas2.innerText = str1;
    }
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

function pointToStr(p) {
    return `${p.x.toFixed(4)};${p.y.toFixed(4)}`;
}

function lineToStr(line) {
    return `id: ${line.id} start: ${pointToStr(line.start)}; end ${pointToStr(line.end)}`;
}

function createLineToP(line) {
    const p = document.createElement("div");
    //p.innerText = `test1`;
    p.setAttribute("id", `div${line.id}`);
    p.setAttribute("class", `div-1`);
    p.setAttribute("onclick", `selectPoint1(${line.id})`);
    p.innerText = lineToStr(line);
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

function selectPoint2(pid) {
    const line = this.document.getElementById(`line${pid}`);
    const div1 = this.document.getElementById(`li${pid}`);

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

function createTreeNode(node) {
    const li = document.createElement("li");
    const span = document.createElement("span");
    span.setAttribute("class", `caret`);
    //span.setAttribute("onclick", `ExpanseNode()`);
    span.addEventListener("click", function () {
        this.parentElement.querySelector(".nested").classList.toggle("active");
        this.classList.toggle("caret-down");
    });

    var spanText = "null";
    if (node.line != null) spanText = lineToStr(node.line);
    span.innerText = spanText;
    li.appendChild(span);

    const ul = document.createElement("ul");
    ul.setAttribute("class", `nested`);
    li.appendChild(ul);

    const leftLi = document.createElement("li");
    if (node.leftLine != null) {
        leftLi.innerText = `leftLine : ${lineToStr(node.leftLine)}`;
        leftLi.setAttribute("id", `li${node.leftLine.id}`);
        leftLi.setAttribute("onclick", `selectPoint2(${node.leftLine.id})`);
    }
    else leftLi.innerText = `leftLine : null`;
    ul.appendChild(leftLi);

    const rightLi = document.createElement("li");
    if (node.rightLine != null) {
        rightLi.innerText = `righLine : ${lineToStr(node.rightLine)}`;
        rightLi.setAttribute("id", `li${node.rightLine.id}`);
        rightLi.setAttribute("onclick", `selectPoint2(${node.rightLine.id})`);
    }
    else rightLi.innerText = `righLine : null`;
    ul.appendChild(rightLi);

    if (node.leftNode != null) ul.appendChild(createTreeNode(node.leftNode));
    else {
        const leftNode = document.createElement("li");
        leftNode.innerText = "LeftNode : null";
        ul.appendChild(leftNode);
    }

    if (node.rightNode != null) ul.appendChild(createTreeNode(node.rightNode));
    else {
        const rightNode = document.createElement("li");
        rightNode.innerText = "RightNode : null";
        ul.appendChild(rightNode);
    }

    return li;
}

function write1() {
    const canvas1 = document.getElementById("column1");
    clear(canvas1);
    //var test1 = "";
    //if (_data.sweepEvents != null) _data.sweepEvents.forEach(ev => test1 += `${pointToStr(ev.point)}\n`);
    //canvas1.innerText = test1;

    if (_data.sweepEvents != null) _data.sweepEvents.forEach(ev => {
        canvas1.appendChild(createDetails(ev));
    });
}

function write2() {
    const canvas1 = document.getElementById("column2");
    clear(canvas1);

    if (_data.tree != null) canvas1.appendChild(createTreeNode(_data.tree));
}

function ExpanseNode() {
    this.parentElement.querySelector(".nested").classList.toggle("active");
    this.classList.toggle("caret-down");
}