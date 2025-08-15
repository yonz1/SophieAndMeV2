let nom = [];
imgQuestion = "";
imgRep = "";

const QuestionBox = document.getElementById('QuestionCheck');
const ReponseBox =  document.getElementById('ReponseCheck');
const bannerQuestion = document.getElementById('ques_img');
const bannerResponse = document.getElementById('rep_img');


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
        clear(); 
    } else {
        btnAction.id = "btnReplace";
        btnAction.textContent = "Replace";
        btnAction.addEventListener("click", Replace);
    }

    buttonTag.appendChild(btnClear);
    buttonTag.appendChild(btnAction);

}


function fill_edit(matier,name,question,ImgQuestion,rep,ImgRep) {
    Matier.value = matier;
    Name.value = name;
    inputText.value = question;
    input_rep.value = rep;
    imgQuestion = ImgQuestion;
    imgRep = ImgRep;
    bannerResponse.src = imgRep;
    bannerQuestion.src = ImgQuestion;
    NullImg(bannerQuestion);
    NullImg(bannerResponse);
    button_fill("");
    
    const textarea = document.getElementById('inputText')
    const output = document.getElementById('OutputText')
    output.innerHTML = textarea.value;
    MathJax.typesetPromise([output]);

    const textarea_rep = document.getElementById('input_rep')
    const Output_rep = document.getElementById('Output_rep')
    Output_rep.innerHTML = textarea_rep.value;
    MathJax.typesetPromise([Output_rep]);
}

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
function Replace(){
    const action = "Replace";
    const matier = document.getElementById("Matier").value;
    const name = document.getElementById("Name").value;
    const question = document.getElementById("inputText").value;
    const rep = document.getElementById("input_rep").value;


    const data = { action, matier, name, question, imgQuestion, rep, imgRep };
    console.log(data);
    clear();
    window.chrome.webview.postMessage(data);
    button_fill("Add");
}

function clear()
{
    document.querySelectorAll('input,textarea').forEach(el => el.value = "");
    const textarea = document.getElementById('inputText')
    const output = document.getElementById('OutputText')
    const output_rep = document.getElementById('Output_rep')
    output_rep.innerHTML = textarea.value;
    output.innerHTML = textarea.value;
    QuestionBox.checked = true;
    ReponseBox.checked = true;
    imgRep = "";
    imgQuestion = "";
    bannerResponse.src = "";
    bannerQuestion.src = "";
}

document.getElementById('fileInpu_rept').addEventListener('change', function (event) {
    const file = event.target.files[0];
    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();

        reader.onload = function (e) {
            imgRep = e.target.result;
            bannerResponse.src = e.target.result;
            bannerResponse.width = "0px"
            bannerResponse.height = "0px"
        };

        reader.readAsDataURL(file);
    } else {
        alert("Veuillez sélectionner une image.");
    }
});




document.getElementById('fileInput').addEventListener('change', function (event) {
    const file = event.target.files[0];
    

    if (file && file.type.startsWith('image/')) {
        const reader = new FileReader();

        reader.onload = function (e) {
            imgQuestion = e.target.result;
            bannerQuestion.src = e.target.result;
            bannerQuestion.width = "0px"
            bannerQuestion.height = "0px"
        };

        reader.readAsDataURL(file);
    } else {
        alert("Veuillez sélectionner une image.");
    }
});



QuestionBox.addEventListener("change", () =>
    QuestionBox.checked ? UnShowImages(QuestionBox) : ShowImage(QuestionBox)
);

ReponseBox.addEventListener("change", () =>
    ReponseBox.checked ? UnShowImages(ReponseBox) : ShowImage(ReponseBox)
);
function ShowImage(Box)
{
   switch (Box){
       case ReponseBox:
            Show(bannerResponse);
           break;
       case QuestionBox:
           Show(bannerQuestion);
           // bannerQuestion.style.width = "200px";
           // bannerQuestion.style.height = "200px";
           break;
   }
}


function Show(Banner)
{
    Banner.style.width =  "100%";             /* S'adapte au conteneur */
    Banner.style.maxWidth =  "500px";        /* Taille maximale (tu choisis selon ton design) */
    Banner.style.height =  "auto";            /* Garde les proportions */
    Banner.style.imageRendering =  "auto";   /* ou 'crisp-edges' si image pixel art */
    Banner.style.display =  "block";
    Banner.style.margin =  "0 auto";          /* Centre l'image */
    Banner.style.objectFit =  "contain";     /* Assure que l’image ne soit jamais déformée */
    Banner.marginTop = "300px"
    Banner.marginBottom = "30px"
}

function UnShowImages(Box)
{
    switch (Box){
        case ReponseBox:
            NullImg(bannerResponse)
            break;
        case QuestionBox:
            NullImg(bannerQuestion);
            break;
    }
}

function NullImg (box)
{
    box.style.width = "0px";
    box.style.height = "0px";
}