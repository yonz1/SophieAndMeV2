using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace SophieAndMe.MVVM.Model;

public class HoursPanel
{
    public int RowID { get; set; }
    public string TopText { get; set; }
    public string BottomText { get; set; }
}

public class DaysItem
{
    public int ColumnId { get; set; }
    public string Days { get; set; }
    public string Date { get; set; }


}


public class TimeTableItem
{
    public string Matiere { get; set; }
    public string Salle {get; set;}
    public string Enseignant { get; set; }
    public string Infos => Enseignant == "" ? $"{Salle}" : $"{Salle} - {Enseignant}";
    public Brush  MatColor => new SolidColorBrush(MatToColor(Matiere));
    public int ColumnId  { get; set; }
    public int RowId  { get; set; }
    public static System.Windows.Media.Color MatToColor(string Mat)
    {
        System.Windows.Media.Color val = System.Windows.Media.Color.FromRgb(22, 23, 23);
        switch (Mat)
        {
            case "Maths":
                val = System.Windows.Media.Color.FromRgb(30, 110, 244);
                break;
            case "TD maths" or "TD Maths":
                val = System.Windows.Media.Color.FromRgb(0, 136, 255);
                break;
            case "Physique":
                val = System.Windows.Media.Color.FromRgb(52, 199, 89);
                break;
            case "SI":
                val = System.Windows.Media.Color.FromRgb(255, 141, 40);
                break;
            case "TP SI":
                val = System.Windows.Media.Color.FromRgb(251, 192, 45);
                break;
            case "TD Si":
                val =  System.Windows.Media.Color.FromRgb(255, 193, 7);    
                break;
            case "Anglais":
                val = System.Windows.Media.Color.FromRgb(179, 136, 255);     //A revoir Trop proche de Maths
                break;
            case "Francais" or "TD Francais":
                val = System.Windows.Media.Color.FromRgb(59, 221, 236);
                break;
            case "Info":
                val = System.Windows.Media.Color.FromRgb(26, 35, 126);
                break;
            case "TD info" or "TD Info":
                val = System.Windows.Media.Color.FromRgb(40, 53, 147);
                break;
            case "Sport":
                val = System.Windows.Media.Color.FromRgb(219, 166, 121);
                break;
            case "Colles":
                val = System.Windows.Media.Color.FromRgb(255, 56, 60);
                break;
            case "TIPE":
                val = System.Windows.Media.Color.FromRgb(93, 64, 55);
                break;
        }
        return val;
    }
    
}

public class SubjectItem : INotifyPropertyChanged
{
    public string Name { get; set; }
    public object Navigation { get; set; }
    public string Value { get; set; }
    public ICommand SelectCommand { get; set; }
    public object IconVal { get; set; }



    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set { _isSelected = value; OnPropertyChanged(); }
    }

    private bool _isToggled;
    public bool IsToggled
    {
        get => _isToggled;
        set
        {
            if (_isToggled != value)
            {
                _isToggled = value;
                OnPropertyChanged(nameof(IsToggled));
            }
        }
    }


    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}



public class CheckBoxItem : INotifyPropertyChanged
{
    public string? Name { get; set; }
    private bool _isChecked;
    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            if (_isChecked != value)
            {
                _isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}