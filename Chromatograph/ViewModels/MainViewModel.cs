using Chromatograph.Commands;
using Chromatograph.Models;
using Chromatograph.Service;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Chromatograph.ViewModels;

public class MainViewModel : Notifier
{
    #region Fields

    private ObservableCollection<Polymer> _polymers;
    private Polymer? _loadedPolymer;
    private MeasuringDevice _device;
    private ChartViewModel _plot;
    private IDialogService _dialogService;
    private IDataSerive _dataService;

    #endregion

    #region Initialize

    public MainViewModel(IDataSerive dataService, IDialogService dialogService)
    {
        _dataService = dataService;
        _dialogService = dialogService;

        var result = _dataService.LoadPolymers();

        if (result.IsSuccess)
            _polymers = new ObservableCollection<Polymer>(result.Data);
        else
        {
            _dialogService.ShowMessage(result.Message);
            _polymers = new ObservableCollection<Polymer>();
        }

            _loadedPolymer = null;
        _device = new MeasuringDevice();
        _plot = new ChartViewModel();

        _device.PropertyChanged += DeviceOnPropertyChanged;
        _dataService = dataService;
        _dialogService = dialogService;
    }

    private void DeviceOnPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        switch(args.PropertyName)
        {
            case nameof(_device.CurrentPoint):
                if (LoadedPolymer != null)
                    Plot.AddPoint(LoadedPolymer.Data[_device.CurrentPoint]);
                break;
            case nameof(_device.IsRunning):
                ResetCommand.RaiseCanExecuteChanged();
                break;
        }
    }

    #endregion

    #region Properties

    public ObservableCollection<Polymer> Polymers
    {
        get { return _polymers; }
        set { SetValue(ref _polymers, value, nameof(Polymers)); }
    }

    public Polymer? LoadedPolymer
    {
        get { return _loadedPolymer; }
        set
        {
            SetValue(ref _loadedPolymer, value, nameof(LoadedPolymer));
        }
    }

    public ChartViewModel Plot
    {
        get { return _plot; }
        set { SetValue(ref _plot, value, nameof(Plot)); }
    }

    #endregion

    #region Commands

    private RelayCommand _loadedCommand;
    private RelayCommand _clearCommand;
    private RelayCommand _preparationCommand;
    private RelayCommand _resetCommand;
    private RelayCommand _startMeasurementCommand;
    private RelayCommand _printCommand;

    public RelayCommand LoadedCommand
    {
        get
        {
            return _loadedCommand ??
                (_loadedCommand = new RelayCommand(o =>
                {
                    var selected = o as Polymer;
                    LoadedPolymer = selected;
                }, 
                o => o is not null && LoadedPolymer is null));
        }
    }

    public RelayCommand ClearCommand
    {
        get
        {
            return _clearCommand ??
                (_clearCommand = new RelayCommand(o => LoadedPolymer = null, o => LoadedPolymer is not null && !_device.IsRunning));
        }
    }

    public RelayCommand PreparationCommand
    {
        get { return _preparationCommand ??
                (_preparationCommand = new RelayCommand(parameter =>
                {
                    var loadedPolymer = parameter as Polymer;

                    if (loadedPolymer == null || loadedPolymer.Data.Count > 0)
                        return;

                    var result = _dataService.LoadPolymerData(loadedPolymer.Name);

                    if (result.IsSuccess)
                        loadedPolymer.Data = new ObservableCollection<DataPoint>(result.Data);
                    else
                        _dialogService.ShowMessage(result.Message);

                    Plot.PreparationChart(loadedPolymer.Data);
                    OnPropertyChanged(nameof(Plot));
                },
                o => o is not null)); }
    }

    public RelayCommand StartMeasurementCommand
    {
        get { return _startMeasurementCommand ??
                (_startMeasurementCommand = new RelayCommand(OnStartMeasurement, o => LoadedPolymer?.Data.Count > 0)); }
    }

    private void OnStartMeasurement(object? parameter)
    {
        if (LoadedPolymer != null)
            _device.StartMeasurement(LoadedPolymer.Data.Count);
    }

    public RelayCommand ResetCommand
    {
        get { return _resetCommand ??
                (_resetCommand = new RelayCommand(o =>
                {
                    Plot.ResetChart();
                    OnPropertyChanged(nameof(Plot));
                    LoadedPolymer = null;
                },
                o =>
                {
                    return (o as Polymer)?.Data.Count > 0 && !_device.IsRunning;
                })); }
    }

    #endregion
}
