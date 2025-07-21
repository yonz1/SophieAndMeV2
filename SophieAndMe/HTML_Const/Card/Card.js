function CreateCardMarked(questarr, reparr, Qimg, Rimg) {
    const divmain = document.getElementById("main")
    divmain.innerHTML = "";


    questarr.forEach((question, index) => {

        let img1 = "";
        let img2 = ""
        const answer = reparr[index];
        const quest_img = Qimg[index].replace("\\/", "/");
        const rep_img = Rimg[index].replace("\\/", "/");
        console.log(question)
        console.log(answer)
        const val = "\"" + question + "\"";

        if (quest_img !== "") {
            img1 = `<img src= ${quest_img} >`;
        }
        if (rep_img !== "") {
            img2 = `<img src= ${rep_img} >`;
        }

        console.log(img1)
        console.log(img2)


        let card = document.createElement("div");
        card.className = "card";

        card.innerHTML = `
    <button class="bin-button" value=${val} onclick="get_val(this)">
      <svg class="bin-top" viewBox="0 0 39 7" fill="none" xmlns="http://www.w3.org/2000/svg">
        <line y1="5" x2="39" y2="5" stroke="white" stroke-width="4"></line>
        <line x1="12" y1="1.5" x2="26.0357" y2="1.5" stroke="white" stroke-width="3"></line>
      </svg>
      <svg class="bin-bottom" viewBox="0 0 33 39" fill="none" xmlns="http://www.w3.org/2000/svg">
        <mask id="path-1-inside-1_8_19" fill="white">
          <path d="M0 0H33V35C33 37.2091 31.2091 39 29 39H4C1.79086 39 0 37.2091 0 35V0Z"></path>
        </mask>
        <path d="M0 0H33H0ZM37 35C37 39.4183 33.4183 43 29 43H4C-0.418278 43 -4 39.4183 -4 35H4H29H37ZM4 43C-0.418278 43 -4 39.4183 -4 35V0H4V35V43ZM37 0V35C37 39.4183 33.4183 43 29 43V35V0H37Z" fill="white" mask="url(#path-1-inside-1_8_19)"></path>
        <path d="M12 6L12 29" stroke="white" stroke-width="4"></path>
        <path d="M21 6V29" stroke="white" stroke-width="4"></path>
      </svg>
    </button>
    <div class="container">
        ${img1}
      <p>${question}</p>
      <hr>
      ${img2}
      <p>${answer}</p>
    </div>
  `;

        divmain.appendChild(card)
    });
    if (window.MathJax) {
        MathJax.typeset();
    }
}


function CreateCardResp(questarr, reparr, Qimg, Rimg){
    const divmain = document.getElementById("main")
    divmain.innerHTML = "";


    questarr.forEach((question, index) => {

        let img1 = "";
        let img2 = ""
        const answer = reparr[index];
        const quest_img = Qimg[index].replace("\\/", "/");
        const rep_img = Rimg[index].replace("\\/", "/");
        console.log(question)
        console.log(answer)
        const val = "\"" + question + "\"";

        if (quest_img !== "") {
            img1 = `<img src= ${quest_img} >`;
        }
        if (rep_img !== "") {
            img2 = `<img src= ${rep_img} >`;
        }

        console.log(img1)
        console.log(img2)


        let card = document.createElement("div");
        card.className = "card";

        card.innerHTML = `
    <div class="container">
        ${img1}
      <p>${question}</p>
      <hr>
      ${img2}
      <p>${answer}</p>
    </div>
  `;

        divmain.appendChild(card)
    });
    if (window.MathJax) {
        MathJax.typeset();
    }
}


function get_val(button) {

    const card = button.closest(".card");

    if (card) {
        card.classList.add("fade-out"); // Lance l'animation

        // Attend la fin de l'animation avant de supprimer le DOM
        setTimeout(() => {
            card.remove();
        }, 200); // 400ms = même durée que dans le CSS
    }
    const id = button.value;
    const action = "Delete";
    const data = { action, id };
    console.log(data);
    window.chrome.webview.postMessage(data);
}
