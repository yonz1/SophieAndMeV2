console.log("Landing")
const daysTag = document.querySelector(".days"),
    currentDate = document.querySelector(".current-date"),
    prevNextIcon = document.querySelectorAll(".icons span");

let date = new Date(),
    currYear = date.getFullYear(),
    currMonth = date.getMonth();

const months = ["January", "February", "March", "April", "May", "June", "July",
    "August", "September", "October", "November", "December"];

const Val = ["2025-07-19", "2025-07-31"];

const pad = n => n.toString().padStart(2, '0');

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
        const isActual = i === date.getDate() && currMonth === new Date().getMonth() && currYear === new Date().getFullYear() ? "actual" : "";
        if (isActual == "actual") {
            isActive = "active"
        }

        liTag += `<li id="${isActual}" class="${isActive}">${i}</li>`;
    }

    for (let i = lastDayofMonth; i < 6; i++) {
        liTag += `<li class="inactive">${i - lastDayofMonth + 1}</li>`;
    }

    currentDate.innerText = `${months[currMonth]} ${currYear}`;
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
        height: 250,
        toolbar: { show: false }
    },
    tooltip: {
        enabled: false
    },
    grid: {
        yaxis: {
            lines: {
                show: false // supprime les lignes de niveau
            }
        }
    },
    plotOptions: {
        bar: {
            borderRadius: 8,
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
                fontSize: '12px'
            }
        }
    },

    fill: {
        opacity: 1
    },
    yaxis: {
        labels: {
            style: {
                colors: '#fff'
            }
        }
    },

    series: [{
        name: "Temps passé",
        data: [30, 40, 20, 50, 60, 10, 0]
    }],
    colors: ['#7B68EE', '#7B68EE', '#7B68EE', '#7B68EE', '#7B68EE', '#7B68EE', '#7B68EE'],
};

var chart = new ApexCharts(document.querySelector(".chart"), options);
chart.render();