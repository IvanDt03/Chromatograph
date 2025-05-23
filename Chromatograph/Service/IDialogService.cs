using System;

namespace Chromatograph.Service;

public interface IDialogService
{
    void ShowMessage(string? message, string title = "Сообщение");

    Action? PrintChartRequested { get; set; }
}
