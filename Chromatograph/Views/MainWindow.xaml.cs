using Chromatograph.Service;
using Chromatograph.ViewModels;
using OxyPlot;
using OxyPlot.Wpf;
using System;
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
    }

    // Очень грубая издержка, так делать нелья :), 
    // но для простаты реализации прибегаем к магии и немного нарушаем прицип паттена MVVM,
    // меня свойство ViewMdelи во View
    private void StoryBoard_Completed(object? sender, EventArgs e)
    {
        var vm = this.DataContext as MainViewModel;

        if (vm is not null)
            vm.ProgressModel = 100;

        prepareBtn.IsChecked = false;
    }
}