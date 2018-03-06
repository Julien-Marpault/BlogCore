function GetAntiForgeryToken() { return document.querySelector('input[name="__RequestVerificationToken"]').value; }

let closeString = '<svg class="svg-inline--fa fa-times-circle fa-w-16" aria-hidden="true" data-fa-processed="" data-prefix="fas" data-icon="times-circle" role="img" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512"><path fill="currentColor" d="M256 8C119 8 8 119 8 256s111 248 248 248 248-111 248-248S393 8 256 8zm121.6 313.1c4.7 4.7 4.7 12.3 0 17L338 377.6c-4.7 4.7-12.3 4.7-17 0L256 312l-65.1 65.6c-4.7 4.7-12.3 4.7-17 0L134.4 338c-4.7-4.7-4.7-12.3 0-17l65.6-65-65.6-65.1c-4.7-4.7-4.7-12.3 0-17l39.6-39.6c4.7-4.7 12.3-4.7 17 0l65 65.7 65.1-65.6c4.7-4.7 12.3-4.7 17 0l39.6 39.6c4.7 4.7 4.7 12.3 0 17L312 256l65.6 65.1z"></path></svg>';

let closeIcons = document.getElementsByClassName('close');
if (closeIcons !== undefined && closeIcons !== null && closeIcons.length > 0) {
    for (let i = 0; i < closeIcons.length; i++) {
        let icon = closeIcons[i];
        icon.addEventListener('click', function (e) { RemoveTag(e); });
    }
}

let tagSelector = document.getElementById('TagSelector');
if (tagSelector !== undefined && tagSelector !== null) {
    tagSelector.addEventListener('keyup', function (e) {
        AutoComplete(tagSelector.value);
        if (e.key === ";") {
            AddTag(tagSelector);
        }
    });

}

let autoCompleteTab = document.getElementById('AutoCompleteTab');
let tagsBlock = document.getElementById('tags');

let postLines = document.getElementsByClassName('Post');

let draggedElement;
let clonedElement;
let mock;

if (postLines != null && postLines.length > 0) {
    for (let i = 0; i < postLines.length; i++) {
        let line = postLines[i];
        line.addEventListener('drag', function (e) { }, false);
        line.addEventListener('dragenter', function (e) { DragOver(e); }, false);
        line.addEventListener('dragover', function (e) { DragOver(e); }, false);
        line.addEventListener('dragleave', function (e) { DragLeave(e); }, false);
        line.addEventListener('dragstart', function (e) { DragElement(e); }, false);
        //line.addEventListener('dragend', function (e) { DragEnd(e); }, false);
        line.addEventListener('drop', function (e) { Drop(e); }, false);
    }
}

function DragElement(e) {
    //e.preventDefault();
    draggedElement = e.currentTarget;
    clonedElement = draggedElement.cloneNode(true);
    clonedElement.addEventListener('dragenter', function (e) { DragOver(e); }, false);
    clonedElement.addEventListener('dragover', function (e) { DragOver(e); }, false);
    //clonedElement.addEventListener('dragend', function (e) { DragEnd(e); }, false);
    clonedElement.addEventListener('drop', function (e) { Drop(e); }, false);
}

let insert;
let pending;
function DragOver(e) {
    e.preventDefault();
    let element = e.currentTarget;
    let rect = element.getBoundingClientRect();
    if (element !== mock && element !== clonedElement) {
        if ((e.clientY - rect.top) > (element.clientHeight / 2)) {
            //console.log("Bas");

            element.parentElement.insertBefore(clonedElement, element.nextSibling);
            draggedElement.style.display = "none";
            insert = 1;
        }
        else {
            //console.log("Haut");
            pending = clonedElement.cloneNode(true);
            element.parentElement.insertBefore(clonedElement, element);
            draggedElement.style.display = "none";
            insert = -1;
        }
    }
}

function DragLeave(e) {
    //e.preventDefault();
}
function DragEnd(e) {
    //e.preventDefault();
    console.log('DragEnd');
    draggedElement.style.display = "flex";
    clonedElement.remove();
}

function Drop(e) {
    e.preventDefault();
    e.stopPropagation();
    console.log('drop');

    let _insert;
    if (insert === 1) {
        insert = "after";
    }
    else {
        insert = "before";
    }
    console.log(insert);
    let id = clonedElement.getAttribute("id");
    let target;
    if (insert === "before") {
        target = e.currentTarget.nextSibling;
    } else {
        target = e.currentTarget.previousSibling;
    }
    if (target !== undefined && target !== null) {
        let nextId = target.id;

        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function (event) {
            // XMLHttpRequest.DONE === 4
            if (this.readyState === XMLHttpRequest.DONE) {
                if (this.status === 200) {
                    console.log('clonedElement.parentElement ' + clonedElement.parentElement);
                    clonedElement.parentElement.insertBefore(draggedElement, clonedElement);
                    draggedElement.style.display = "flex";
                    clonedElement.remove();
                } else {
                    clonedElement.remove();
                    draggedElement.style.display = "flex";
                }
            }
        };
        xhr.open("post", "/post/order");
        //xhr.setRequestHeader('Content-Type', "application/x-www-form-urlencoded");
        //xhr.setRequestHeader('Content-Type', 'application/form-data');

        let data = new FormData();
        data.append('__RequestVerificationToken', GetAntiForgeryToken());
        data.append('id', id);
        data.append('insert', insert);
        data.append('target', nextId);
        //let data = { __RequestVerificationToken: GetAntiForgeryToken(), id: id, insert: insert, target: nextId };
        console.log(data);
        xhr.send(data);
    }
}

function AutoComplete(text) {

    let xhr = new XMLHttpRequest();

    xhr.onreadystatechange = function (event) {

        if (this.readyState === XMLHttpRequest.DONE) {
            if (this.status === 200) {
                autoCompleteTab.innerHTML = null;
                let tags = JSON.parse(this.responseText);
                if (tags.length > 0) {
                    let selectList = document.createElement('select');
                    for (let i = 0; i < tags.length; i++) {
                        let option = document.createElement('div');
                        option.innerText = tags[i];
                        autoCompleteTab.appendChild(option);
                        option.addEventListener('click', function (e) { SelectTag(option.innerText); });
                    }
                    autoCompleteTab.style.display = 'block';
                }
            } else {
                console.log("Status de la réponse: %d (%s)", this.status, this.statusText);
            }
        }
    };
    xhr.open('GET', '/completetags/' + text);
    xhr.send();
}

function SelectTag(option) {
    tagSelector.value = null;
    let tagInput = document.createElement('input');
    tagInput.setAttribute('type', 'hidden');
    tagInput.setAttribute('name', 'Tags[' + tagCount + ']');
    tagInput.value = option;
    autoCompleteTab.parentElement.appendChild(tagInput);

    autoCompleteTab.style.display = "none";
    autoCompleteTab.innerHTML = null;

    let span = document.createElement('span');
    span.innerText = option;


    let closeSpan = document.createElement('span');
    closeSpan.classList.add('close');
    closeSpan.innerHTML = closeString;
    closeSpan.setAttribute('data-tag', option);
    span.appendChild(closeSpan);
    closeSpan.addEventListener('click', function (e) { RemoveTag(e); });

    autoCompleteTab.style.display = "none";
    autoCompleteTab.parentElement.insertBefore(span, tagSelector);
    tagCount++;
}

function AddTag(tagSelector) {
    let tag = tagSelector.value.replace(";", "");
    SelectTag(tag);
}

function RemoveTag(e) {
    let tag = e.currentTarget.parentElement;
    let tagName = e.currentTarget.getAttribute('data-tag');
    let input = document.querySelectorAll("input[value='" + tagName + "']")[0];
    tag.remove();
    input.remove();
}
