using Chromatograph.Models;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chromatograph.ViewModels;

public class ChartViewModel : Notifier
{
    private ObservableCollection<ISeries> _series;
    private ObservableCollection<ICartesianAxis> _xAxis;
    private ObservableCollection<ICartesianAxis> _yAxis;
    private ObservableCollection<DataPoint> _data;

    public ChartViewModel()
    {
        _data = new ObservableCollection<DataPoint>();

        _series = new ObservableCollection<ISeries>
        {
            new LineSeries<DataPoint>
            {
                Values = _data,
                Mapping = (p, i) => new Coordinate(p.Volume, p.Signal)
            }
        };

        _xAxis = new ObservableCollection<ICartesianAxis>
        {
            new Axis
            {
                Name = "Объем"
            }
        };

        _yAxis = new ObservableCollection<ICartesianAxis>
        {
            new Axis
            {
                Name = "Сигнал"
            }
        };
    }

    public ObservableCollection<ISeries> Series
    {
        get { return _series; }
        set { SetValue(ref _series, value, nameof(Series)); }
    }

    public ObservableCollection<ICartesianAxis> XAxis
    {
        get { return _xAxis; }
        set { SetValue(ref _xAxis, value, nameof(XAxis)); }
    }

    public ObservableCollection<ICartesianAxis> YAxis
    {
        get { return _yAxis; }
        set { SetValue(ref _yAxis, value, nameof(YAxis)); }
    }

    public void AddPoint(DataPoint point)
    {
        _data.Add(point);

        OnPropertyChanged(nameof(Series));
    }

    public void UpdateChart()
    {
        _data.Clear();
        XAxis[0].MinLimit = _data.Min(p => p.Volume);
        XAxis[0].MaxLimit = _data.Max(p => p.Volume);
        YAxis[0].MinLimit = _data.Min(p => p.Signal);
        YAxis[0].MaxLimit = _data.Max(p => p.Signal);

        OnPropertyChanged(nameof(Series));
        OnPropertyChanged(nameof(XAxis));
        OnPropertyChanged(nameof(YAxis));
    }
}
