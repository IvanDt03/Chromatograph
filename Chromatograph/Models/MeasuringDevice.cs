using Chromatograph.ViewModels;
using System;
using System.Windows.Threading;

namespace Chromatograph.Models;

public class MeasuringDevice : Notifier
{
    private DispatcherTimer _timer;
    private int _currentPoint;
    private int _amountOfData;

    public MeasuringDevice()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(300);
        _timer.Tick += OnStartMeasurement;
    }

    private void OnStartMeasurement(object? sender, EventArgs args)
    {
        if (!IsRunning)
            return;

        if (CurrentPoint + 1 >= _amountOfData)
        {
            _timer.Stop();
            OnMeasurementCompleted();
            OnPropertyChanged(nameof(IsRunning));
            return;
        }

        ++CurrentPoint;
    }

    public void StartMeasurement(int amountOfdata)
    {
        _amountOfData = amountOfdata;
        CurrentPoint = 0;
        _timer.Start();
    }

    public bool IsRunning
    {
        get { return _timer.IsEnabled; }
    }

    public int CurrentPoint
    {
        get { return _currentPoint; }
        set { SetValue(ref _currentPoint, value, nameof(CurrentPoint)); }
    }

    public event EventHandler? MeasurementCompleted;
    private void OnMeasurementCompleted()
    {
        if (MeasurementCompleted != null)
            MeasurementCompleted.Invoke(this, new MeasurementCompletedEventArgs());
    }
}
