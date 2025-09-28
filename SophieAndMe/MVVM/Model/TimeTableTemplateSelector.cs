using System.Windows;
using System.Windows.Controls;

namespace SophieAndMe.MVVM.Model;

public class TimeTableTemplateSelector : DataTemplateSelector
{
    public DataTemplate ButtonTemplate { get; set; }
    public DataTemplate TextTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is TimeTableItem data)
        {
            return data.IsButtonActive ? ButtonTemplate : TextTemplate;
        }
        return base.SelectTemplate(item, container);
    }
}