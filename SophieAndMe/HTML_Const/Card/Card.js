const divmain = document.getElementById("main");
i = 0;
y = 0;
let ArrayMain = [];
let batch = [];
let selectedCard = null;
let observerTrigger = document.createElement("div");
observerTrigger.id = "scroll-trigger";
console.log("Charger")
let UsedQuestion = [];
let OldId = "";
window.MathJax = {
    tex: {
        inlineMath: [['$', '$'], ['\\(', '\\)']],
        displayMath: [['$$', '$$'], ['\\[', '\\]']]
    }, 
    options: {
        skipHtmlTags: ['script', 'noscript', 'style', 'textarea', 'pre', 'code'],
        renderActions: {
            addMenu: [] 
        }
    },
};



function TestArrayMain()
{
    console.log("Test Demandée")
    let saved = localStorage.getItem("Dicos2");
    if(saved)
    {
        ArrayMain = JSON.parse(saved);
        console.log("Données restaurée");
    }
    if (ArrayMain.length === 0)
    {
        const question = "";
        const action = "Demande";
        const data = { action, question };
        window.chrome.webview.postMessage(data);
    }
}


window.chrome.webview.addEventListener('message', event => {
    dico = event.data;
    console.log(i);
    if (dico.Id !== OldId)
    {
        OldId = dico.Id;
        clearcard();
    }
    if (dico.Action === "Test")
    {
            TestArrayMain();
            console.log("test appelée");
    }
    else
    {
        if (dico.Action === "Import")
        {
            localStorage.setItem("Dicos2", JSON.stringify(ArrayMain));
        }
        ArrayMain.push(dico);
        if (i < 10){
            CreateCardMain(
                dico.level,
                dico.course,
                dico.question,
                dico.repnse,
                dico.urlQuestion,
                dico.urlRep,
                dico.difficulty,
                dico.len,
                dico.Action
            );
            i++;}
        

    }

});

function Clickdiv()
{
    console.writeline("Card Clicked")
}

const oberserver = new IntersectionObserver(entries => {
    if (entries[0].isIntersecting)
    {
        let y = 0;
        console.log("scrollTriger");
        console.log(i);
        console.log(ArrayMain[0].len);
        while (y < 10 && i < ArrayMain.length) {
            console.log("scrollTriger2");
            CreateCardMain(
                ArrayMain[i].level,
                ArrayMain[i].course,
                ArrayMain[i].question,
                ArrayMain[i].repnse,
                ArrayMain[i].urlQuestion,
                ArrayMain[i].urlRep,
                ArrayMain[i].difficulty,
                ArrayMain[i].len,
                ArrayMain[i].Action
            );
            i++;
            y++;
        }

}},{threshold:0.1});

function clearcard()
{
    console.log("Clear card")
    divmain.innerHTML = "";
    ArrayMain = [];
    i = 0;
    divmain.appendChild(observerTrigger);
    oberserver.observe(observerTrigger);
}
// ################################################################ Fonction des différente cartes

function CreateCardMain(level,course,quest, rep,Qimg, Rimg,difficulty, len,Action) {
    let card = document.createElement("div");
    let val = "\"" + quest + "\"";
    card.className = "card";
    card.id = quest;
    console.log(i);
    console.log(len);
    card.addEventListener("click", () => {
        if (selectedCard && selectedCard !== card)
        {
            selectedCard.style.backgroundColor = "#242424";
            selectedCard.style.boxShadow = "none";
            selectedCard.classList.remove("active");
        }
        card.style.backgroundColor = selectedCard && selectedCard === card ? "#242424" : "transparent";
        card.style.boxShadow = selectedCard && selectedCard === card ? "none" : "6px 6px 10px 0px lightblue";
        if (selectedCard && selectedCard === card)
        {
            selectedCard.classList.remove("active");
        }
        else {
            card.classList.add("active");
        }
        selectedCard = card;
    })
    switch (Action)
    {
        case "Resp":
            card.innerHTML = `
    <div class="container">
        <div class="DivInline">
            ${Qimg ? `<img src="${Qimg}" loading="lazy">` : ""}
            <p>${quest.replace(/\n/g, "<br>")}</p>
        </div>
        <hr>
        <div class="DivInline">
            ${Rimg ? `<img src="${Rimg}" loading="lazy">` : ""}
            <p>${rep.replace(/\n/g, "<br>")}</p>
        </div>
    </div>`;
            break;
            
        case "Import":
            card.innerHTML = `
    <div class="container">
        <div class="DivInline">
            ${Qimg ? `<img src="${Qimg}" loading="lazy">` : ""}
            <p>${quest.replace(/\n/g, "<br>")}</p>
        </div>
        <hr>
        <div class="DivInline">
            ${Rimg ? `<img src="${Rimg}" loading="lazy">` : ""}
            <p>${rep.replace(/\n/g, "<br>")}</p>
        </div>
    </div>`;
            break;
        
        
        case "Marked":
            card.innerHTML = `
    <div class="container">
        <div class="DivInline">
            ${Qimg? `<img src="${Qimg}" loading="lazy">` : ""}
            <p>${quest.replace(/\n/g, "<br>")}</p>
        </div>
        <hr>
        <div class="DivInline">
            ${Rimg ? `<img src="${Rimg}" loading="lazy">` : ""}
            <p>${rep.replace(/\n/g, "<br>")}</p>
        </div>
        <div class="card-button">
                <button value=${val} onclick="get_val(this)" class="Delete">Delete</button>
        </div>
    </div>`;
            break;
        
        
        case "Created":
            card.innerHTML = `
    <div class="container">
        <div class="DivInline">
            ${Qimg ? `<img src="${Qimg}" loading="lazy">` : ""}
            <p>${quest.replace(/\n/g, "<br>")}</p>
        </div>
        <hr>
        <div class="DivInline">
            ${Rimg? `<img src="${Rimg}" loading="lazy">` : ""}
            <p>${rep.replace(/\n/g, "<br>")}</p>
        </div>
        <div class="card-button">
                <button class="ReplaceC" value=${val} onclick="get_data(this)" >Replace</button>
                <button class="DeleteC" value=${val} onclick="get_val(this)" >Delete</button>
                </div>
    </div>`;
            break;
            
    }
    batch.push(card);
    if (batch.length >= 10 || i === parseInt(len)-1) {
        CreatCardLogicGlobal();
    }
}
// ################################################################ Fonction de rendu

function ClickedCard(id)
{
    console.log("recçus")
    console.log(id);
    Cliked =  document.getElementById(id);
    Cliked.style.borderColor = "white"
    Cliked.style.backgroundColor = "transparent";
    
}

function CreatCardLogicGlobal()
{
    console.log("Created")
    renderCardsSmoothly(batch);
    //MathJax.typesetPromise([divmain]);
    MathJax.typesetPromise([divmain]).catch(err => console.log("MathJax error:", err));
    batch = [];
}

function renderCardsSmoothly(cards) {
    let i = 0;
    function step() {
        if (i < cards.length) {
            divmain.insertBefore(cards[i], document.getElementById("scroll-trigger"));
            i++;
            requestAnimationFrame(step); // continue d'ajouter la carte suivante
        } else {
            if (window.MathJax) {
                requestAnimationFrame(() => {
                    MathJax.typesetPromise([divmain]);
                });
            }
        }
    }

    requestAnimationFrame(step); // démarre le rendu
}



// ######################################################################## Fonction pour action des bouttons



function get_val(button) {

    const card = button.closest(".card");

    if (card) {
        card.classList.add("fade-out");
        setTimeout(() => {
            card.remove();
        }, 200);
    }
    const question = button.value;
    const action = "Delete";
    const data = { action, question };
    console.log(data);
    window.chrome.webview.postMessage(data);
}


function get_data(button){
    const card = button.closest(".card")
    const question = button.value;
    const action = "edit";
    const data = {action,question };
    console.log(question);
    window.chrome.webview.postMessage(data);
}

// document.addEventListener("click", () => {
//     if (selectedCard)
//     {
//         selectedCard.style.backgroundColor = "#242424";
//         selectedCard = null
//     }
// })