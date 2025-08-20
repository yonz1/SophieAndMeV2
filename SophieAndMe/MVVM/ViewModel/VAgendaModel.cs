using SophieAndMe.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace SophieAndMe.MVVM.ViewModel
{
    public class VAgendaModel : INotifyPropertyChanged
    {

        public int SpanNum = 1;
        public string OldMatiere = "";
        private readonly MainViewModel _mainViewModel;
        public ObservableCollection<HoursPanel> Hours { get; set; }
        public ObservableCollection<DaysItem> DaysItem { get; set; }
        public ObservableCollection<TimeTableItem> _TimeTableItems { get; set; }
        private List<string> Matiere = [];
        private List<string> Salle = [];
        private List<string> Enseignant = [];
        private List<string> Days = ["lundi", "mardi", "mercredi", "jeudi", "vendredi","samedi","dimanche"];
        public ICommand Back { get; }
        public ICommand Forward { get; }
        private string OldSem;
        private string ActualSem = "A";
        DayOfWeek[] days = { 
            DayOfWeek.Sunday, 
            DayOfWeek.Monday, 
            DayOfWeek.Tuesday, 
            DayOfWeek.Wednesday, 
            DayOfWeek.Thursday, 
            DayOfWeek.Friday, 
            DayOfWeek.Saturday };

        private List<string> Jours = ["Lun", "Mar", "Mer", "Jeu", "Ven", "Sam", "Dim"];
        private int z = 0;

        private string _monthText;

        public string MonthText
        {
            get => _monthText;
            set
            {
                _monthText = value;
                OnPropertyChanged();
                
            }
        }
        
        public VAgendaModel(MainViewModel mainVm)
        {
            _mainViewModel = mainVm;
            Back = new RelayCommand(o =>
            {
                _TimeTableItems.Clear();
                z = z - 7;
                var dates = GetDates(z);
                if (DbInteraction.IsHolliday(dates[0]))
                {
                    Console.WriteLine("Vacance");
                    FillPlanning("Vac");
                    FillInfos(dates,Jours);   
                }
                else
                {
                    FillPlanning(DiffSem());
                    FillInfos(dates,Jours);   
                    FillDs(DbInteraction.GetDS(dates[5]));
                }
                MonthText = GetMonthText(z);
            });
            Forward = new RelayCommand(o =>
            {
                _TimeTableItems.Clear();
                z = z + 7;
                var dates = GetDates(z);
                if (DbInteraction.IsHolliday(dates[0]))
                {
                    Console.WriteLine("Vacance");
                    FillPlanning("Vac");
                    FillInfos(dates,Jours);   
                    FillDs(DbInteraction.GetDS(dates[5]));
                }
                else
                {
                    FillPlanning(DiffSem());
                    FillInfos(dates,Jours); 
                    FillDs(DbInteraction.GetDS(dates[5]));
                }
                MonthText = GetMonthText(z);
                
            });
            var dates = GetDates(z);
            DateTime dateTime = DateTime.UtcNow.Date;
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

            
            
            
            DaysItem = new ObservableCollection<DaysItem>();
            _TimeTableItems= new ObservableCollection<TimeTableItem>();
            MonthText = $"{DateTime.Now.ToString("MMMMMMM")}  {DateTime.Now.Year.ToString()}";
            FillInfos(dates,Jours);
            if (DbInteraction.IsHolliday(dates[0]))
            {
                Console.WriteLine("Vacance");
                FillPlanning("Vac");
                FillInfos(dates,Jours);   
            }
            else
            {
                FillPlanning(ActualSem);
                FillInfos(dates,Jours);  
                FillDs(DbInteraction.GetDS(dates[5]));
            }
        }

        public void FillDs(string Mat)
        {
            if (Mat != "")
            {
                TimeTableItem item = new TimeTableItem();
                item.ColumnId = 5;
                item.RowId = 0;
                item.Salle = "";
                item.Enseignant = "";
                item.Matiere = Mat;
                item.RowNumSpan = 4;
                _TimeTableItems.Add(item);
            }

            else
            {
                for (int i = 0; i < 4; i++)
                {
                    TimeTableItem item = new TimeTableItem();
                    item.ColumnId = 5;
                    item.RowId = i;
                    item.Salle = "";
                    item.Enseignant = "";
                    item.Matiere = Mat;
                    item.RowNumSpan = 1;
                    _TimeTableItems.Add(item); 
                }
            }

        }
        
        public string GetMonthText(int z)
        {

            DateTime today = DateTime.Today;
            int currentDayOfWeek = (int) today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);
            if (z != 0)
            {
                DateTime nextMonday = monday.AddDays(z);
                return  $"{nextMonday.ToString("MMMMMMM")}  {nextMonday.Year}";
            }
            else
            {
                return $"{monday.ToString("MMMMMMM")}  {monday.Year}";;
            }
        }

        public List<DateTime> GetDates(int z)
        {
            DateTime today = DateTime.Today;
            int currentDayOfWeek = (int) today.DayOfWeek;
            DateTime sunday = today.AddDays(-currentDayOfWeek);
            DateTime monday = sunday.AddDays(1);
            if (currentDayOfWeek == 0)
            {
                monday = monday.AddDays(-7);
            }
            if (z != 0)
            {
                DateTime nextMonday = monday.AddDays(z);
                return Enumerable.Range(0, 7).Select(days => nextMonday.AddDays(days)).ToList();
            }
            else
            {
                return Enumerable.Range(0, 7).Select(days => monday.AddDays(days)).ToList();
            }
        }

        public void FillInfos(List<DateTime> dates, List<string> Jours)
        {
            for (int i = 0; i < dates.Count; i++)
            {
                DaysItem item = new DaysItem();
                item.ColumnId = i;
                item.Date = dates[i].ToString("dd"); 
                item.Days = Jours[i];
                DaysItem.Add(item);
            }
        }

        public void FillPlanning(string val)
        {
            if (val == "Vac")
            {
                for (int y = 0; y < Days.Count; y++)
                {
                    for (int i = 0; i < 11; i++)
                    {
                        TimeTableItem item = new TimeTableItem();
                        item.ColumnId = y;
                        item.RowId = i;
                        item.Salle = "";
                        item.Enseignant = "";
                        item.Matiere = "";
                        item.RowNumSpan = 1;
                        _TimeTableItems.Add(item);
                    }
                }
            }
            else
            {
                for (int y = 0; y < Days.Count; y++)
                {
                    (Matiere, Salle, Enseignant) = DbInteraction.GetPlanning(Days[y], val);
                    for (int i = 0; i < 11; i++)
                    {
                        TimeTableItem item = new TimeTableItem();
                        string s_val = i >= Salle.Count ? "" : Salle[i];
                        string m_val = i >= Matiere.Count ? "" : Matiere[i];
                        string e_val = i >= Enseignant.Count ? "" : Enseignant[i];
                        item.ColumnId = y;
                        item.RowId = i;
                        item.Salle = s_val;
                        item.Enseignant = e_val;
                        item.Matiere = m_val;
                        item.RowNumSpan = 1;
                        int Ret = i + 1;
                        OldMatiere = i >= Matiere.Count ? "" : Matiere[i];
                        if (Ret < Matiere.Count && OldMatiere != "")
                        {
                            while (Matiere[Ret] == OldMatiere)
                            {
                                Console.WriteLine(Ret);
                                item.RowNumSpan++;
                                i++;
                                Ret++;
                                if (Ret >= 10)
                                {
                                    break;
                                }
                            }
                        }
                        Console.WriteLine(OldMatiere);
                        _TimeTableItems.Add(item);
                    }
                }
            }
        }

        public string DiffSem()
        {
            switch (ActualSem)
            {
                case "A":
                    ActualSem = "B";
                    break;
                case "B":
                    ActualSem = "A";
                    break;
            }
            return ActualSem;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
