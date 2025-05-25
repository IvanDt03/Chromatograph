using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Linq;

namespace Chromatograph.ViewModels;

public class ChartOxyViewModel : Notifier
{
    private PlotModel _model;
    private LineSeries _series;

    public ChartOxyViewModel()
    {
        var xAxis = new LinearAxis
        {
            Title = "Объем",
            Position = AxisPosition.Bottom,
        };

        var yAxis = new LinearAxis
        {
            Title = "Сигнал",
            Position = AxisPosition.Left,
        };

        _model = new PlotModel
        {
            Title = "",
        };

        _series = new LineSeries
        {
            StrokeThickness = 2,
        };

        _model.Series.Add(_series);
        _model.Axes.Add(xAxis);
        _model.Axes.Add(yAxis);
    }

    public PlotModel Model
    {
        get { return _model; }
        set { SetValue(ref _model, value, nameof(Model)); }
    }

    public void PreparationChart(Chromatograph.Models.Polymer loaded)
    {
        _model.Title = loaded.Name;
        _series.XAxis.Minimum = loaded.Data.Min(p => p.Volume);
        _series.XAxis.Maximum = loaded.Data.Max(p => p.Volume);
        _series.YAxis.Minimum = loaded.Data.Min(p => p.Signal - 0.05);
        _series.YAxis.Maximum = loaded.Data.Max(p => p.Signal + 0.3);

        _model.InvalidatePlot(true);
    }
    public void AddPoint(Chromatograph.Models.DataPoint point)
    {
        _series.Points.Add(new OxyPlot.DataPoint(point.Volume, point.Signal));
        _model.InvalidatePlot(true);
    }

    public void ResetChart()
    {
        _series.Points.Clear();
        _model.Title = "";
        _model.InvalidatePlot(true);
    }

    public bool IsEmpty()
    {
        return _series.Points.Count == 0;
    }
}
