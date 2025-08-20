const daysTag = document.querySelector(".days"),
    currentDate = document.querySelector(".current-date"),
    prevNextIcon = document.querySelectorAll(".icons span");

let date = new Date(),
    currYear = date.getFullYear(),
    currMonth = date.getMonth();


const months = ["January", "February", "March", "April", "May", "June", "July",
    "August", "September", "October", "November", "December"];

const Val = ["2025-08-19", "2025-08-31"]; 

const pad = n => n.toString().padStart(2, '0');
const NvContainer = document.getElementById("Nouveau");
const Messages = document.getElementById("Messages"); 
const Notes = document.getElementById("Notes");
const Quizz =  document.getElementById("Quizz");
 
MetaMess = ["General","Physique","SI"]
DataMess = ["Lorem Ipsum Dolor sit amet","Lorem Ipsum Dolor sit amet","Lorem Ipsum Dolor sit amet"]
for (i  = 0; i < MetaMess.length; i++) {
    FillMessages(MetaMess[i],DataMess[i],"Messages");
}

// FillMessages("Anglais"," NDOYE Amala; Rg:6/14; Moy:13,61; ET:3,31;14","Notes")
// FillMessages("Maths"," LAURENCON Beno\u00EFt; Rg:1/31; Moy:12,90; ET:2,76;19","Notes")
// FillMessages("Physique"," ADROGUER PIERRE; Rg:1/15; Moy:10,53; ET:4,03;16","Notes")
//
//
//
// MetaQuizz = ["Si","Mathématiques","Physique"]
// DataQuizz = ["Lorem Ipsum Dolor sit amet","Lorem Ipsum Dolor sit amet","Lorem Ipsum Dolor sit amet"]
// for (i  = 0; i < MetaQuizz.length; i++) {
//     FillMessages(MetaQuizz[i],DataQuizz[i],"Quizz");
// }

window.chrome.webview.addEventListener('message', event => {
    dico = event.data;
    FillMessages(dico.Meta,dico.Data,dico.Position)
});

function FillMessages(Meta,Data,Position)
{
    console.log(Meta)
    console.log(Data)
    console.log(Position)
    let infos = document.createElement("div");
    infos.className = "Infos";
    infos.innerHTML = `                
                <label class="Meta">${Meta}</label>
                <label class="Data">${Data}</label>`
    switch (Position){
        case "Messages":
            Messages.appendChild(infos);
            break;
        case "Notes":
            Notes.appendChild(infos);
            break;
        case "Quizz":
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

        liTag += `<li id="${isActual}" class="${isActive}">${i}</li>`;
    }

    for (let i = lastDayofMonth; i < 6; i++) {
        liTag += `<li class="inactive">${i - lastDayofMonth + 1}</li>`;
    }

    // currentDate.innerText = `${months[currMonth]} ${currYear}`;
    daysTag.innerHTML = liTag;
}

renderCalendar();

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
        height: window.innerHeight * 0.35,
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
          "Lundi", "Mardi", "Mercredi", "Jeudi", "Vendredi", "Samedi", "Dimanche"
        ],
        labels: {
          style: {
            colors: '#fff',
            fontSize: '13px'
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
              gradientToColors: ["#242424"]
          }
},
      yaxis:{
        labels: {
            style:{
                colors: '#fff'
            }
        }
      },

      series: [{
        name: "Temps passé",
        data: [30, 40, 20, 50, 60, 10, 0]
      }],
      colors: ['#9B59B6'],
    };

    var chart = new ApexCharts(document.querySelector(".chart"), options);
    chart.render();


    window.addEventListener('resize', () => {
  chart.updateOptions({
    chart: { height: window.innerHeight * 0.35 }
  });
});
