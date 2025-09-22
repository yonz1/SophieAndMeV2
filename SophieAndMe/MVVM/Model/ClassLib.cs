using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using SophieAndMe.Core;
using Color = System.Drawing.Color;

namespace SophieAndMe.MVVM.Model;


public class ProfileUserInfo : INotifyPropertyChanged
{
    public int RowId { get; set; }
    public string UserInfo { get; set; }
    public string UserData {get; set; }
    
    public ICommand EditValue { get; set; }
    public ICommand CancelValue { get; set; }
    public ICommand ConfirmValue { get; set; }

    private bool _isview;
    public bool  IsView
    {
        get => _isview;
        set { _isview = value;
            OnPropertyChanged();
        }
    }
    private bool _isviewedit;

    public bool IsViewEdit
    {
        get => _isviewedit;
        set
        {
            _isviewedit = value;
            OnPropertyChanged();
        }
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}


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
    private readonly IDataService _dataService;   
    public  TimeTableItem()
    {
        _dataService = App.DataService;
    }

    public string UrlProg { get; set; }
    public bool IsButtonActive { get; set; } = false;

    public bool IsColle { get; set; } = false;
    public string Matiere { get; set; }
    public string Salle {get; set;}
    public string Enseignant { get; set; }
    public string Infos => Enseignant == "" ? $"{Salle}" : $"{Salle} - {Enseignant}";
    public Brush  MatColor => new SolidColorBrush(MatToColor(Matiere,IsColle));
    public int ColumnId  { get; set; }
    public int RowId  { get; set; }
    public int RowNumSpan { get; set; }
    public System.Windows.Media.Color MatToColor(string Mat,bool IsColle)
    {
        System.Windows.Media.Color val = _dataService.IsDark
            ? System.Windows.Media.Color.FromRgb(22, 23, 23)
            : System.Windows.Media.Color.FromRgb(248, 250, 252);
        if (IsColle)
        {
            return System.Windows.Media.Color.FromRgb(255, 56, 60);
        }
        switch (Mat)
        {
            case "Maths":
                val = System.Windows.Media.Color.FromRgb(30, 110, 244);
                break;
            case "TD maths" or "TD Maths":
                val = System.Windows.Media.Color.FromRgb(0, 136, 255);
                break;
            case "Physique" or "SPC":
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
            case "Francais" or "TD Francais" or "Français":
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
            // case "Colles":
            //     val = System.Windows.Media.Color.FromRgb(255, 56, 60);
            //     break;
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