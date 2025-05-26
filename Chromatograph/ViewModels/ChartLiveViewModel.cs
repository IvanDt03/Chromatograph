using Chromatograph.Models;
using LiveChartsCore;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Linq;

namespace Chromatograph.ViewModels;

public class ChartLiveViewModel : Notifier
{
    private ObservableCollection<ISeries> _series;
    private ObservableCollection<ICartesianAxis> _xAxis;
    private ObservableCollection<ICartesianAxis> _yAxis;
    private ObservableCollection<DataPoint> _data;
    private LabelVisual _title;

    public ChartLiveViewModel()
    {
        _data = new ObservableCollection<DataPoint>();
        
        _series = new ObservableCollection<ISeries>
        {
            new LineSeries<DataPoint>
            {
                Values = _data,
                Mapping = (p, i) => new Coordinate(p.Volume, p.Signal),
                Fill = null,
                Stroke = new SolidColorPaint(SKColors.Green) { StrokeThickness = 4 },
                GeometryFill = null,
                GeometryStroke = null,
            }
        };

        _xAxis = new ObservableCollection<ICartesianAxis>
        {
            new Axis
            {
                Name = "Объем",
               
            }
        };

        _yAxis = new ObservableCollection<ICartesianAxis>
        {
            new Axis
            {
                Name = "Сигнал",
            }
        };

        _title = new LabelVisual() { Text = "" };
    }

    public ObservableCollection<ISeries> Series
    {
        get { return _series; }
        private set { SetValue(ref _series, value, nameof(Series)); }
    }

    public ObservableCollection<ICartesianAxis> XAxis
    {
        get { return _xAxis; }
        private set { SetValue(ref _xAxis, value, nameof(XAxis)); }
    }

    public ObservableCollection<ICartesianAxis> YAxis
    {
        get { return _yAxis; }
        private set { SetValue(ref _yAxis, value, nameof(YAxis)); }
    }

    public LabelVisual Title
    {
        get { return _title; }
        set { SetValue(ref _title, value, nameof(Title)); }
    }

    public void AddPoint(DataPoint point)
    {
        _data.Add(point);
        OnPropertyChanged(nameof(Series));
    }

    public void ResetChart()
    {
        _data.Clear();
        Title.Text = "";
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(Series));
    }

    public void PreparationChart(Polymer loaded)
    {
        Title.Text = loaded.Name;

        XAxis[0].MinLimit = loaded.Data.Min(p => p.Volume);
        XAxis[0].MaxLimit = loaded.Data.Max(p => p.Volume);

        YAxis[0].MinLimit = loaded.Data.Min(p => p.Signal);
        YAxis[0].MaxLimit = loaded.Data.Max(p => p.Signal);

        OnPropertyChanged(nameof(XAxis));
        OnPropertyChanged(nameof(YAxis));
        OnPropertyChanged(nameof(Title));
    }

    public bool IsEmpty() => _data.Count == 0;
}
