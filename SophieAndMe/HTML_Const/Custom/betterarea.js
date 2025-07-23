imgQuestion = "";
imgRep = "";
button_fill("Add");

const wordReplacements = {
    "somme": "\\sum_{k}^{n}",
    "produit": "\\prod_{k}^{n}",
    "square": "\\sqrt{a}",
    "intab": "\\int_{a}^{b}",
    "limite": "\\lim_{x \\to a}",
    "infini": "\\infty",
    "implique": "\\implies",
    "equivalent": "\\iff",
    "binome": "\\binom{n}{k}",
    "deriv": "\\dv{f}{x}",
    "inclue": "\\subset",
    "mat3": "\\begin{pmatrix}\r\n &  &  \\\\\r\n &  &  \\\\\r\n &  & \r\n\\end{pmatrix}",
    "mat2": "\\begin{pmatrix}\r\n &  \\\\\r\n & \r\n\\end{pmatrix}"
};

const keymap = {
    '<': { value: '<>', pos: 1 },
    '(': { value: '()', pos: 1 },
    '{': { value: '{}', pos: 1 },
    '[': { value: '[]', pos: 1 },
    '\'': { value: '\'\'', pos: 1 },
    '"': { value: '""', pos: 1 },
    '“': { value: '“”', pos: 1 },
    '`': { value: '``', pos: 1 },
    '‘': { value: '‘’', pos: 1 },
    '«': { value: '«»', pos: 1 },
    '「': { value: '「」', pos: 1 },
    '*': { value: '**', pos: 1 },
    '_': { value: '__', pos: 1 },
    '>': { value: '> ', pos: 2 },
    '~': { value: '~~', pos: 1 },
    '/': { value: '\\frac{}{}', pos: 6 },
    '$': { value: '$$', pos: 1 }
};



function setupInputBehavior(editing) {
    editing.addEventListener('keydown', event => {
        if (keymap[event.key]) {
            event.preventDefault();
            const pos = editing.selectionStart;
            editing.value = editing.value.slice(0, pos) +
                keymap[event.key].value +
                editing.value.slice(editing.selectionEnd);
            editing.selectionStart = editing.selectionEnd = pos + keymap[event.key].pos;
        }
    });

    editing.addEventListener('input', () => {
        const pos = editing.selectionStart;
        let text = editing.value;

        for (const [target, replacement] of Object.entries(wordReplacements)) {
            const regex = new RegExp(`\\b${target}\\b`, 'gi');
            text = text.replace(regex, replacement);
        }

        editing.value = text;
        editing.selectionStart = editing.selectionEnd = pos;
    });
}

// Initialise le comportement après que le DOM est chargé
window.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('textarea').forEach(setupInputBehavior);
});

window.MathJax = {
    tex: {
        inlineMath: [['$', '$'], ['\\(', '\\)']],
        displayMath: [['$$', '$$'], ['\\[', '\\]']]
    },
    options: {
        skipHtmlTags: ['script', 'noscript', 'style', 'textarea', 'pre', 'code'],
        renderActions: {
            addMenu: [] // désactive le menu contextuel MathJax
        }
    }
};
const textarea = document.getElementById('inputText')
const output = document.getElementById('OutputText')

textarea.addEventListener('input', () => {
    output.innerHTML = textarea.value;
    MathJax.typesetPromise([output]);
});

const textarea_rep = document.getElementById('input_rep')
const Output_rep = document.getElementById('Output_rep')

textarea_rep.addEventListener('input', () => {
    Output_rep.innerHTML = textarea_rep.value;
    MathJax.typesetPromise([Output_rep]);
});


document.getElementById('searchImage_quest').addEventListener('click', function()
{
    document.getElementById('fileInput').click();
});

document.getElementById('searchImage_rep').addEventListener('click', function()
{
    document.getElementById('fileInpu_rept').click();
});


function save(){
    const action = "save";
    const matier = document.getElementById("Matier").value;
    const name = document.getElementById("Name").value;
    const question = document.getElementById("inputText").value;
    const rep = document.getElementById("input_rep").value;



    const data = { action, matier, name, question, imgQuestion, rep, imgRep };
    console.log(data);
    clear();
    window.chrome.webview.postMessage(data);
}


function clear()
{
    document.querySelectorAll('input,textarea').forEach(el => el.value = "");
    const textarea = document.getElementById('inputText')
    const output = document.getElementById('OutputText')
    Output_rep.innerHTML = textarea_rep.value;
    output.innerHTML = textarea.value;
}



document.getElementById('fileInpu_rept').addEventListener('change', function (event) {
    const file = event.target.files[0];
    const banner = document.getElementById('rep_img');

    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();

        reader.onload = function (e) {
            imgRep = e.target.result;
            // banner.src = e.target.result;
            // banner.style.maxWidth = "10cm"; 
            // banner.style.height = "auto";   
            // banner.style.maxHeight = "10cm"; 
        };

        reader.readAsDataURL(file);
    } else {
        alert("Veuillez sélectionner une image.");
    }
});




document.getElementById('fileInput').addEventListener('change', function (event) {
    const file = event.target.files[0];
    const banner = document.getElementById('ques_img');

    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();

        reader.onload = function (e) {
            imgQuestion = e.target.result;
            // banner.src = e.target.result;
            // banner.style.maxWidth = "10cm"; 
            // banner.style.height = "auto";   
            // banner.style.maxHeight = "10cm"; 
        };

        reader.readAsDataURL(file);
    } else {
        alert("Veuillez sélectionner une image.");
    }
});
// console.log(document.getElementById("input").files[0]);



