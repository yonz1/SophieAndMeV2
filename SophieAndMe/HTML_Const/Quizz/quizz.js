const question = ["Développement limité de \\(e^x\\)"];
const reponse = [" \\( \\large e^x  = 1 + x + \\frac{x^2}{2!}  + \\frac{x^3}{3!} +  \\cdots + \\frac{x^n}{n!} + o(x^n) \\) "];
const quest_img = "";
const rep_img = "";
let para = "";
let val= "";




// updatequizz("re", question[0],reponse[0],"","");
// const textarea_rep = document.getElementById('input_rep')
// textarea_rep.value = "x";
// updatequizz("questin", question[0],reponse[0],"","");


function updatequizz(action,question,reponse,quest_img,rep_img)
{

    console.log("Updating quizz...");
    let img1 = "";
    let img2 = "";

    question = question.replace(/\n/g, "<br>");
    reponse = reponse.replace(/\n/g, "<br>");
    if (quest_img.includes("data:") || rep_img.includes("data:")) {
        quest_img = quest_img.replace("\\/", "/");
        rep_img = rep_img.replace("\\/", "/");
    }
    else
    {
        quest_img = quest_img.replaceAll("//", "/")
        rep_img = rep_img.replaceAll("//", "/");
    }

    const main = document.getElementById("main");
    console.log(main.innerHTML)
    // if (main.innerHTML.includes("input_rep"))
    // {
    //     // alert("in")
    //     const textarea_rep = document.getElementById('input_rep')
    //     val = textarea_rep.value;
    // }

    if (quest_img !== "")
    {
        img1 = `<img src= ${quest_img} >`;
    }
    else if (rep_img !== "")
    {
         img2 = `<img src= ${rep_img} >`;
    }


    if (action == "question") {
        main.innerHTML = "";
        main.innerHTML = `<div class="container">  ${img1} <p class="question">${question}</p>  </div>`;
        // main.innerHTML += `        <div class="input-group">
        //     <textarea required="" type="text" name="text" autocomplete="off" class="input_nq" id="input_rep"></textarea>
        //     <label class="user-label">Votre réponse ...</label>
        // </div>`;
    }
    else {
        main.innerHTML = "";
        main.innerHTML = `<div class="container_q"> ${img1}  <p class="question">${question}</p>  </div>`;
        main.innerHTML += `<div class="container_r"> ${img2} <p class="reponse">${reponse}</p> </div>`;
        // alert("Value: ",val)
        // console.log("Value: ",val)
    }
    console.log(main.innerHTML)


        if (window.MathJax) {
            MathJax.typeset();
            return "val";
    }
};

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
