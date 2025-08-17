using SophieAndMe.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SophieAndMe.MVVM.ViewModel
{
    public class VAgendaModel
    {
        public ObservableCollection<HoursPanel> Hours { get; set; }
        public ObservableCollection<DaysItem> DaysItem { get; set; }
        public ObservableCollection<TimeTableItem> _TimeTableItems { get; set; }

        public VAgendaModel() 
        { 
            Hours = new ObservableCollection<HoursPanel>
            {
                new HoursPanel {RowID = 1+0,  TopText= "8h"  },
                new HoursPanel {RowID = 1+1,  TopText= "9h"  },
                new HoursPanel {RowID = 1+2,  TopText= "10h" },
                new HoursPanel {RowID = 1+3,  TopText= "11h" },
                new HoursPanel {RowID = 1+4,  TopText= "12h" },
                new HoursPanel {RowID = 1+5,  TopText= "13h" },
                new HoursPanel {RowID = 1+6,  TopText= "14h" },
                new HoursPanel {RowID = 1+7,  TopText= "15h" },
                new HoursPanel {RowID = 1+8,  TopText= "16h" },
                new HoursPanel {RowID = 1+9,  TopText= "17h" },
                new HoursPanel {RowID = 1+10, TopText= "18h" },
                new HoursPanel {RowID = 1+11, TopText= "19h" },
                new HoursPanel {RowID = 1+11, TopText= "19h" },
            };

            DaysItem = new ObservableCollection<DaysItem>
            {
                new DaysItem {ColumnId = 0, Days="Lun", Date = "01"},
                new DaysItem {ColumnId = 1, Days="Mar", Date = "02"},
                new DaysItem {ColumnId = 2, Days="Mer", Date = "03"},
                new DaysItem {ColumnId = 3, Days="Jeu", Date = "04"},
                new DaysItem {ColumnId = 4, Days="Ven", Date = "05"},
                new DaysItem {ColumnId = 5, Days="Sam", Date = "06"},
                new DaysItem {ColumnId = 6, Days="Dim", Date = "07"}
            };

            List<string> Matiere = ["Mathématiques","Physique","SI","Anglais","","Français","Sport","Colles"];
            List<string> Salle = ["B101","B102","A102","A12","","B111","S14","Amphi S1"];
            List<string> Enseignant = ["Solnon","Fuxa","Blasheck","Estorges","","Allais","","Regnaud"];
            _TimeTableItems= new ObservableCollection<TimeTableItem>();
            for (int  i = 0;  i < Matiere.Count;  i++)
            {
                TimeTableItem item = new TimeTableItem();
                item.ColumnId = 0;
                item.RowId = i;
                item.Salle = Salle[i];
                item.Enseignant = Enseignant[i];
                item.Matiere = Matiere[i];
                _TimeTableItems.Add(item);
                Console.WriteLine(item.MatColor);
                Console.WriteLine(item.Infos);
            }
            Console.WriteLine(_TimeTableItems.Count);

            // TimeTableItem = new ObservableCollection<TimeTableItem>
            // {
            //     new TimeTableItem { ColumnId = 0, RowId = 0, Matiere = "Maths",Salle  = "B101", Enseignant = "Solnon"},
            // };
        }
    }
}
