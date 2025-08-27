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
    '_': { value: '_{}', pos: 2 },
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






