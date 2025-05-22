using Chromatograph.ViewModels;
using System;
using System.Windows.Threading;

namespace Chromatograph.Models;

public class MeasuringDevice : Notifier
{
    private DispatcherTimer _timer;

    public MeasuringDevice()
    {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromMicroseconds(80);
    }
}
