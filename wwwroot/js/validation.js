let forms = document.getElementsByTagName('FORM');

if (forms !== undefined && forms !== null && forms.length > 0) {
    for (let i = 0; i < forms.length; i++) {
        let form = forms[i];
        form.addEventListener('submit', function (ev) {
            ev.preventDefault();
            ValidForm(form);
        }, false);
    }
}

//});

function ValidForm(form) {
    let ValidForm;
    let inputs = form.querySelectorAll('input,textarea,select');
    for (let i = 0; i < inputs.length; i++) {
        let input = inputs[i];
        let tagName = input.tagName;
        if (input.getAttribute('data-val') === 'true') {
            if (tagName === 'INPUT' || tagName === "TEXTAREA") {
                input.addEventListener('keyup', function (e) { ValidInput(input, tagName); });
            }
            else if (tagName === "SELECT") {
                input.addEventListener('change', function (e) { ValidInput(input, tagName); });
            }
            if (!ValidInput(input, tagName)) {
                ValidForm = false;
            }
        }
    }
    if (ValidForm !== false) {
        form.submit();
    }
}

function ValidInput(input, tagName) {
    let required = input.getAttribute('data-val-required');
    if (required !== undefined && required !== null) {
        if (tagName === "INPUT" || tagName === "TEXTAREA") {
            if (input.value.length === 0) {
                input.classList.add('validation-error');
                return false;
            }
            else {
                input.classList.remove('validation-error');
                return true;
            }
        }
        else if (tagName === "SELECT") {
            if (input.value === "") {
                input.classList.add('validation-error');
                return false;
            }
            else {
                input.classList.remove('validation-error');
                return true;
            }
        }
        else {
            input.classList.remove('validation-error');
        }
    }
}