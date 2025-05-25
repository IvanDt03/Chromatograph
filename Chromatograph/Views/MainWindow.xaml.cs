using Chromatograph.Service;
using Chromatograph.ViewModels;
using OxyPlot;
using OxyPlot.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chromatograph.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var dialodService = new DialogService();
        dialodService.PrintChartRequested += PrintChart;

        DataContext = new MainViewModel(new ExcelDataService("Content/Data.xlsx"), dialodService);
    }

    private void PrintChart()
    {

        var printDialog = new PrintDialog();
        printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
        if (printDialog.ShowDialog() != true) return;

        double paddingFactor = 1.1;
        int exportWidth = (int)(printDialog.PrintableAreaWidth * 96 / 100 * paddingFactor);
        int exportHeight = (int)(printDialog.PrintableAreaHeight * 96 / 100 * paddingFactor);

        var exporter = new PngExporter
        {
            Width = exportWidth,
            Height = exportHeight,
        };

        var originalPadding = chart.Model.Padding;
        chart.Model.Padding = new OxyThickness(30);

        var bitmap = exporter.ExportToBitmap(chart.Model);

        chart.Model.Padding = originalPadding;

        var image = new System.Windows.Controls.Image
        {
            Source = bitmap,
            Stretch = Stretch.Uniform,
            Width = printDialog.PrintableAreaWidth * 0.95,
            Height = printDialog.PrintableAreaHeight * 0.95
        };

        var printCanvas = new Canvas
        {
            Width = printDialog.PrintableAreaWidth,
            Height = printDialog.PrintableAreaHeight,
            Background = Brushes.White
        };

        printCanvas.Children.Add(image);
        Canvas.SetLeft(image, (printDialog.PrintableAreaWidth - image.Width) / 2);
        Canvas.SetTop(image, (printDialog.PrintableAreaHeight - image.Height) / 2);

        printDialog.PrintVisual(printCanvas, "Печать грфика");

        // Тут печать LiveCharts2
        /*
        var printDialog = new PrintDialog();

        printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;

        if (printDialog.ShowDialog() != true)
            return;

        double printableWidth = printDialog.PrintableAreaWidth;
        double printableHeight = printDialog.PrintableAreaHeight;

        var bitmap = new RenderTargetBitmap(
            (int)chart.ActualWidth,
            (int)chart.ActualHeight,
            96, 96,  // Можно 300 DPI, если нужно высокое качество
            PixelFormats.Pbgra32);
        bitmap.Render(chart);

        var image = new Image
        {
            Source = bitmap,
            Stretch = Stretch.Fill,
            Width = printableWidth,
            Height = printableHeight
        };

        var printCanvas = new Canvas
        {
            Width = printableWidth,
            Height = printableHeight,
            Background = Brushes.White
        };
        printCanvas.Children.Add(image);
        Canvas.SetLeft(image, (printDialog.PrintableAreaWidth - image.Width) / 2);
        Canvas.SetTop(image, (printDialog.PrintableAreaHeight - image.Height) / 2);

        printDialog.PrintVisual(printCanvas, "График LiveCharts2 (полная страница)");
        */

        // тут тоже печать LiveChart2, yj c незначительным изменнеием исходного UI элемента
        /*
        var pd = new PrintDialog();
        if (pd.ShowDialog() == true)
        {
            var originalScale = chart.LayoutTransform;
            var scale = pd.PrintableAreaWidth / chart.ActualWidth;
            chart.LayoutTransform = new ScaleTransform(scale, scale);

            pd.PrintVisual(chart, "Печать графика");
            chart.LayoutTransform = originalScale;
        */
    }
}