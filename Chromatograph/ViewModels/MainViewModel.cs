﻿using Chromatograph.Commands;
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
        
        _device = new MeasuringDevice();
        _plot = new ChartViewModel();

        InitializePolymers();

        _device.PropertyChanged += DeviceOnPropertyChanged;
        _dataService = dataService;
        _dialogService = dialogService;
    }
    private void InitializePolymers()
    {
        var result = _dataService.LoadPolymers();

        if (result.IsSuccess)
        {
            _polymers = new ObservableCollection<Polymer>(result.Data);
        }
        else
        {
            _dialogService.ShowMessage(result.Message);
            _polymers = new ObservableCollection<Polymer>();
        }
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
                o => o is not null && !_device.IsRunning && Plot.IsEmpty() && LoadedPolymer is null));
        }
    }
    public RelayCommand PreparationCommand
    {
        get { return _preparationCommand ??
                (_preparationCommand = new RelayCommand(parameter =>
                {
                    var loadedPolymer = parameter as Polymer;

                    if (loadedPolymer == null)
                        return;
                    OnPreparationPolymer(loadedPolymer);
                },
                o => o is not null && !_device.IsRunning && Plot.IsEmpty() && (o as Polymer)?.Data.Count == 0)); }
    }

    private void OnPreparationPolymer(Polymer loadedPolymer)
    {
        var result = _dataService.LoadPolymerData(loadedPolymer.Name);

        if (result.IsSuccess)
        {
            loadedPolymer.Data = new ObservableCollection<DataPoint>(result.Data);

            Plot.PreparationChart(loadedPolymer);
            OnPropertyChanged(nameof(Plot));
        }
        else
            _dialogService.ShowMessage(result.Message);
    }

    public RelayCommand StartMeasurementCommand
    {
        get
        {
            return _startMeasurementCommand ??
                (_startMeasurementCommand = new RelayCommand(OnStartMeasurement, CanExecuteStartMeasurement));
        }
    }

    private void OnStartMeasurement(object? parameter)
    {
        var loaded = parameter as Polymer;

        if (loaded != null)
            _device.StartMeasurement(loaded.Data.Count);
    }

    private bool CanExecuteStartMeasurement(object? parameter)
    {
        var loaded = parameter as Polymer;

        if (loaded == null)
            return false;

        return Plot.IsEmpty() && !_device.IsRunning && loaded.Data.Count > 0;
    }

    public RelayCommand ResetCommand
    {
        get 
        {
            return _resetCommand ??
                (_resetCommand = new RelayCommand(o =>
                {
                    var loaded = o as Polymer;

                    if (loaded != null)
                    {
                        loaded.Data.Clear();
                        LoadedPolymer = null;

                        Plot.ResetChart();
                        OnPropertyChanged(nameof(Plot));
                    }
                },
                o => o is not null && !_device.IsRunning));
        }
    }

    public RelayCommand PrintCommand
    {
        get 
        { 
            return _printCommand ??
                (_printCommand = new RelayCommand(o =>
                {
                    _dialogService.PrintChartRequested?.Invoke();
                },
                o => !_device.IsRunning)); 
        }
    }

    #endregion
}
