using Chromatograph.Service;
using Chromatograph.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Printing;

namespace Chromatograph.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var dialodService = new DialogService();
        dialodService.PrintChartRequested += PrintChart;

        DataContext = new MainViewModel(new ExcelDataService("Content/Data.xlsx"),dialodService);
    }

    private void PrintChart()
    {
        // 1. Настраиваем PrintDialog
        var printDialog = new PrintDialog();

        // 2. Можно задать альбомную ориентацию, если график широкий
        // printDialog.PrintTicket.PageOrientation = PageOrientation.Landscape;

        if (printDialog.ShowDialog() != true)
            return;

        // 3. Получаем размеры печатной области
        double printableWidth = printDialog.PrintableAreaWidth * 0.95;
        double printableHeight = printDialog.PrintableAreaHeight * 0.95;

        // 4. Рендерим график в Bitmap (можно увеличить DPI для качества)
        var bitmap = new RenderTargetBitmap(
            (int)chart.ActualWidth,
            (int)chart.ActualHeight,
            96, 96,  // Можно 300 DPI, если нужно высокое качество
            PixelFormats.Pbgra32);
        bitmap.Render(chart);

        // 5. Создаем Image и растягиваем на всю область (без сохранения пропорций!)
        var image = new Image
        {
            Source = bitmap,
            Stretch = Stretch.Fill,  // Важно: Fill (не Uniform)
            Width = printableWidth,
            Height = printableHeight
        };

        // 6. Размещаем Image на Canvas
        var printCanvas = new Canvas
        {
            Width = printableWidth,
            Height = printableHeight,
            Background = Brushes.White
        };
        printCanvas.Children.Add(image);

        // 7. Печатаем
        printDialog.PrintVisual(printCanvas, "График LiveCharts2 (полная страница)");
    }
}