let SelectedText = window.color["Text"];



const daysTag = document.querySelector(".days"),
    currentDate = document.querySelector(".current-date"),
    prevNextIcon = document.querySelectorAll(".icons span");

let date = new Date(),
    currYear = date.getFullYear(),
    currMonth = date.getMonth();


const months = ["January", "February", "March", "April", "May", "June", "July",
    "August", "September", "October", "November", "December"];

let Val = ["2025-08-19", "2025-08-31"];

const pad = n => n.toString().padStart(2, '0');
const NvContainer = document.getElementById("Nouveau");
const Messages = document.getElementById("Messages");
const Notes = document.getElementById("Notes");
const Quizz =  document.getElementById("Quizz");

let MetaMess = ["General","Physique","SI"]
let DataMess = ["Lorem Ipsum Dolor sit amet","Lorem Ipsum Dolor sit amet","Lorem Ipsum Dolor sit amet"]
for (let i  = 0; i < MetaMess.length; i++) {
    FillMessages(MetaMess[i],DataMess[i],"Messages","");
}


// FillMessages("Anglais"," NDOYE Amala;13","Notes")
// FillMessages("Maths"," LAURENCON Beno\u00EFt;19","Notes")
// FillMessages("Physique"," ADROGUER PIERRE;10","Notes")
//
//
//
// let MetaQuizz = ["Si","Mathématiques","Physique"]
// let DataQuizz = ["Lorem Ipsum Dolor sit amet","Lorem Ipsum Dolor sit amet","Lorem Ipsum Dolor sit amet"]
// for (let i  = 0; i < MetaQuizz.length; i++) {
//     FillMessages(MetaQuizz[i],DataQuizz[i],"Quizz","");
// }



function FillMessages(Meta, Data, Position) {
    let Info = Data.split(';');
    let Mark = Info[1]
    Data = Info[0]
    console.log(Meta)
    console.log(Data)
    console.log(Position)
    let infos = document.createElement("div");
    infos.className = "Infos";

    let MarkInfo = Mark >= 15 ? "NotesG" : Mark < 11 ? "NotesB" : "NotesM";

    switch (Position) {
        case "Messages":
            infos.innerHTML = ` 
                <div class="round"></div>                   
                <label class="MetaM">${Meta}</label>
                <label class="DataM">${Data}</label>`
            Messages.appendChild(infos);
            break;
        case "Notes":
            infos.innerHTML = `                
                <label class="MetaA">${Meta}</label>
                <label class="DataA">${Data}</label>
                <label class="${MarkInfo}">${Mark}</label>`
            Notes.appendChild(infos);
            break;
        case "Quizz":
            infos.id = "QuizzI";
            infos.innerHTML = `                
                <label class="MetaA">${Meta}</label>
                <label class="DataA">${Data}</label>
                <button value="${Meta}-${Data}" onclick="get_data(this)"  class="OpenQuizz">Ouvrir</button>`
            Quizz.appendChild(infos);
            break;
    }
}

const renderCalendar = () => {
    let firstDayofMonth = new Date(currYear, currMonth, 1).getDay(),
        lastDateofMonth = new Date(currYear, currMonth + 1, 0).getDate(),
        lastDayofMonth = new Date(currYear, currMonth, lastDateofMonth).getDay(),
        lastDateofLastMonth = new Date(currYear, currMonth, 0).getDate();

    let liTag = "";
    let color = "";

    for (let i = firstDayofMonth; i > 0; i--) {
        liTag += `<li class="inactive">${lastDateofLastMonth - i + 1}</li>`;
    }

    for (let i = 1; i <= lastDateofMonth; i++) {
        const dateStr = `${currYear}-${pad(currMonth + 1)}-${pad(i)}`;
        let isActive = Val.includes(dateStr) ? "active" : "";
        const isActual = i === date.getDate() && currMonth === new Date().getMonth() && currYear === new Date().getFullYear() ? "actual": "";
        if (isActual == "actual")
        {
            isActive = "active"
        }

        liTag += `<li id="${isActual}" Style="color:${color};" class="${isActive}">${i}</li>`;
    }

    for (let i = lastDayofMonth; i < 6; i++) {
        liTag += `<li class="inactive">${i - lastDayofMonth + 1}</li>`;
    }

    // currentDate.innerText = `${months[currMonth]} ${currYear}`;
    daysTag.innerHTML = liTag;
}



prevNextIcon.forEach(icon => {
    icon.addEventListener("click", () => {
        currMonth = icon.id === "prev" ? currMonth - 1 : currMonth + 1;
        if (currMonth < 0 || currMonth > 11) {
            date = new Date(currYear, currMonth, new Date().getDate());
            currYear = date.getFullYear();
            currMonth = date.getMonth();
        } else {
            date = new Date();
        }
        renderCalendar();
    });
});


var options = {
    chart: {
        type: 'bar',
        height: window.innerWidth * 0.23,
        width: window.innerWidth * 0.45,
        toolbar: { show: false }
    },
    tooltip: {
        enabled: false
    },
    grid: {
        yaxis: {
            lines: {
                show: false,// supprime les lignes de niveau
                height: 0.2,
                colors: '#636363',
            }
        }
    },
    plotOptions: {
        bar: {
            borderRadius: 14,
            columnWidth: '80%',
            distributed: false
        }
    },
    dataLabels: {
        enabled: false,
    },
    xaxis: {
        axisBorder: {
            show: false // enlève la ligne horizontale en bas
        },
        axisTicks: {
            show: false // enlève les petites "barrettes" sous chaque label
        },
        categories: [
            "Lun", "Mar", "Mer", "Jeu", "Ven", "Sam", "Dim"
        ],
        labels: {
            style: {
                colors: SelectedText,
                fontSize: '12px',
                fontFamily: 'Inherit',
                fontWeight: '400',
            }
        }
    },

    fill: {
        type:'gradient',
        gradient: {
            shade: 'light',
            type:'vertical',
            stops: [0,100],
            opacity:1,
            opacityFrom: 1,
            inverseColors: false,
            gradientToColors: ["#8b5cf6"]
        }
    },
    yaxis:{
        labels: {
            style:{
                fontSize: '12px',
                fontFamily: 'Inherit',
                fontWeight: '400',
                colors: SelectedText
            }
        }
    },

    series: [{
        name: "Temps passé",
        data: [30, 40, 20, 50, 60, 10, 0]
    }],
    colors: ['#38bdf8'],
};
var chart = new ApexCharts(document.querySelector(".chart"), options);
chart.render();
// renderCalendar();

document.getElementById("Maths").style.width = "20%";
window.chrome.webview.addEventListener('message', event => {
    dico = event.data;
    console.log(dico);
    //if (dico.Maths)
    //{
       //document.getElementById("Maths").style.width = dico.Mathématiques + "%";
       //document.getElementById("Physique").style.width = dico.Physique + "%";
        //document.getElementById("SI").style.width = dico.SI + "%";
        //document.getElementById("Anglais").style.width = dico.Anglais + "%";
        //document.getElementById("Français").style.width = dico.Français + "%";
        //document.getElementById("Erreurs").style.width = dico.Erreurs + "%";
    //}
    if(dico.lundi  !== "")
    {
        chart.updateSeries([{
            name: "Temps passé",
            data: [parseInt(dico.lundi), parseInt(dico.mardi), parseInt(dico.mercredi), parseInt(dico.jeudi), parseInt(dico.vendredi), parseInt(dico.samedi), parseInt(dico.dimanche)]
        }]);
    }
    else if (dico.DateColle !== "")
    {
        Val.push(dico.DateColle)
    }
    else
    {
        renderCalendar();
        console.log("Called")
        FillMessages(dico.Meta,dico.Data,dico.Position)
    }
});

function  get_data(button)
{
    const card = button.closest(".OpenQuizz")
    const question = card.value;
    const action = "edit";
    const data = {action,question };
    console.log(question);
    window.chrome.webview.postMessage(data);
}

window.addEventListener('resize', () => {
    chart.updateOptions({
        chart: { width: window.innerWidth * 0.45 , height : window.innerWidth * 0.23 },
    });
});