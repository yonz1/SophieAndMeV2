let nom = [];



function autocomplete(inp, arr) {
    /*the autocomplete function takes two arguments,
    the text field element and an array of possible autocompleted values:*/
    var currentFocus;
    /*execute a function when someone writes in the text field:*/
    inp.addEventListener("input", function(e) {
        var a, b, i, val = this.value;
        /*close any already open lists of autocompleted values*/
        closeAllLists();
        if (!val) { return false;}
        currentFocus = -1;
        /*create a DIV element that will contain the items (values):*/
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        /*append the DIV element as a child of the autocomplete container:*/
        this.parentNode.appendChild(a);
        /*for each item in the array...*/
        for (i = 0; i < arr.length; i++) {
            /*check if the item starts with the same letters as the text field value:*/
            if (arr[i].substr(0, val.length).toUpperCase() == val.toUpperCase()) {
                /*create a DIV element for each matching element:*/
                b = document.createElement("DIV");
                /*make the matching letters bold:*/
                b.innerHTML = "<strong>" + arr[i].substr(0, val.length) + "</strong>";
                b.innerHTML += arr[i].substr(val.length);
                /*insert a input field that will hold the current array item's value:*/
                b.innerHTML += "<input type='hidden' value=" + JSON.stringify(arr[i]) + ">";
                console.log(JSON.parse(JSON.stringify(arr[i])));
                /*execute a function when someone clicks on the item value (DIV element):*/
                b.addEventListener("click", function(e) {
                    /*insert the value for the autocomplete text field:*/
                    // item = JSON.parse(JSON.stringify(this.getElementsByTagName("input")[0].value));
                    // console.log(item)
                    inp.value = this.getElementsByTagName("input")[0].value;
                    /*close the list of autocompleted values,
                    (or any other open lists of autocompleted values:*/
                    closeAllLists();
                });
                a.appendChild(b);
            }
        }
    });
    /*execute a function presses a key on the keyboard:*/
    inp.addEventListener("keydown", function(e) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (e.keyCode == 40) {
            /*If the arrow DOWN key is pressed,
            increase the currentFocus variable:*/
            currentFocus++;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 38) { //up
            /*If the arrow UP key is pressed,
            decrease the currentFocus variable:*/
            currentFocus--;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (e.keyCode == 13) {
            /*If the ENTER key is pressed, prevent the form from being submitted,*/
            e.preventDefault();
            if (currentFocus > -1) {
                /*and simulate a click on the "active" item:*/
                if (x) x[currentFocus].click();
            }
        }
    });
    function addActive(x) {
        /*a function to classify an item as "active":*/
        if (!x) return false;
        /*start by removing the "active" class on all items:*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        /*add class "autocomplete-active":*/
        x[currentFocus].classList.add("autocomplete-active");
    }
    function removeActive(x) {
        /*a function to remove the "active" class from all autocomplete items:*/
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }
    function closeAllLists(elmnt) {
        /*close all autocomplete lists in the document,
        except the one passed as an argument:*/
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
    }
    /*execute a function when someone clicks in the document:*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}

// function ReceiveDataCsharp(data){
//   nom = data

// }
var matiére = ["Physique", "Mathématiques", "Français", "Anglais", "Erreurs", "SI"]
nom = ["Géometrie dans l\'espace", "Application Linéaire", "Calcul", "fonction","Equation différentielle"]


function Suggestion(arr,val)
{
    console.log("Suggestions faite");
    autocomplete(document.getElementById("Matier"), matiére);
    autocomplete(document.getElementById("Name"), arr);
    button_fill(val);
}


function button_fill(val)
{
    const buttonTag = document.getElementById("button_div");

    buttonTag.innerHTML = ''; 

    const btnClear = document.createElement("button");
    btnClear.textContent = "Clear";
    btnClear.className = "animated-button";
    btnClear.id = "btnclear";
    btnClear.addEventListener("click", clear);

    const btnAction = document.createElement("button");
    btnAction.className = "animated-button";
    if (val === "Add") {
        btnAction.id = "btnSave";
        btnAction.textContent = "Add";
        btnAction.addEventListener("click", save);
    } else {
        btnAction.id = "btnReplace";
        btnAction.textContent = "Replace";
        btnAction.addEventListener("click", Replace);
    }

    buttonTag.appendChild(btnClear);
    buttonTag.appendChild(btnAction);
}


function fill_edit(matier,name,question,rep) {
    Matier.value = matier;
    Name.value = name;
    inputText.value = question;
    input_rep.value = rep;
    button_fill("");
    
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
}


function Replace(){
    const action = "Replace";
    const matier = document.getElementById("Matier").value;
    const name = document.getElementById("Name").value;
    const question = document.getElementById("inputText").value;
    const rep = document.getElementById("input_rep").value;


    const data = { action, matier, name, question, img_question, rep, img_rep };
    console.log(data);
    clear();
    window.chrome.webview.postMessage(data);
    button_fill("Add");
}
