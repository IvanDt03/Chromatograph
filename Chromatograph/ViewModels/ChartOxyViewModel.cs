using DocumentFormat.OpenXml.Drawing;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Linq;

namespace Chromatograph.ViewModels;

public class ChartOxyViewModel : Notifier
{
    private PlotModel _model;
    private LineSeries _series;
    private LinearAxis _xAxis;
    private LinearAxis _yAxis;

    public ChartOxyViewModel()
    {
        _xAxis = new LinearAxis
        {
            Title = "Объем",
            Position = AxisPosition.Bottom,
            IsZoomEnabled = false,
            IsPanEnabled = true,
            MajorGridlineStyle = LineStyle.Solid,
            MinorGridlineStyle = LineStyle.Solid,
        };

        _yAxis = new LinearAxis
        {
            Title = "Сигнал",
            Position = AxisPosition.Left,
            IsZoomEnabled = false,
            IsPanEnabled = false,
        };

        _model = new PlotModel
        {
            Title = "",
        };

        _series = new LineSeries
        {
            StrokeThickness = 2,
            TrackerFormatString = "X: {2:0.000}\nY: {4:0.000}",
        };

        _model.Series.Add(_series);
        _model.Axes.Add(_xAxis);
        _model.Axes.Add(_yAxis);
    }

    public PlotModel Model
    {
        get { return _model; }
        set { SetValue(ref _model, value, nameof(Model)); }
    }

    public void PreparationChart(Chromatograph.Models.Polymer loaded)
    {
        _model.Title = loaded.Name;
        _xAxis.Minimum = loaded.Data.Min(p => p.Volume);
        _xAxis.Maximum = loaded.Data.Max(p => p.Volume);
        //_xAxis.AbsoluteMinimum = loaded.Data.Min(p => p.Volume);
        //_xAxis.AbsoluteMaximum = loaded.Data.Max(p => p.Volume);
        
        _yAxis.Minimum = loaded.Data.Min(p => p.Signal);
        _yAxis.Maximum = loaded.Data.Max(p => p.Signal + 0.5);

        _model.InvalidatePlot(true);
    }
    public void AddPoint(Chromatograph.Models.DataPoint point)
    {
        _series.Points.Add(new OxyPlot.DataPoint(point.Volume, point.Signal));
        //_xAxis.Minimum = Math.Max(_xAxis.AbsoluteMinimum, point.Volume - 18);
        //_xAxis.Maximum = Math.Min(_xAxis.AbsoluteMaximum, point.Volume + 8);
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
