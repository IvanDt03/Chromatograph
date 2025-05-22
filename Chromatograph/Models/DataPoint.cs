namespace Chromatograph.Models;

public struct DataPoint
{
    public double Volume { get; set; }
    public double Signal { get; set; }

    public DataPoint(double volume, double signal)
    {
        Volume = volume;
        Signal = signal;
    }
}
