using Chromatograph.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chromatograph.Models;

public class Polymer : Notifier
{
    private string _name;
    private ObservableCollection<DataPoint> _data;

    public Polymer(string name)
    {
        _name = name;
        _data = new ObservableCollection<DataPoint>();
    }

    public string Name
    {
        get { return _name; }
        set { SetValue(ref _name, value, nameof(Name)); }
    }

    public ObservableCollection<DataPoint> Data
    {
        get { return _data; }
        set { SetValue(ref _data, value, nameof(Data)); }
    }
}
