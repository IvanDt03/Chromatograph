using Chromatograph.Commands;
using Chromatograph.Models;
using Chromatograph.Service;
using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

namespace Chromatograph.ViewModels;

public class MainViewModel : Notifier
{
    #region Fields

    private ObservableCollection<Polymer> _polymers;
    private Polymer? _loadedPolymer;
    private MeasuringDevice _device;
    //private IDialogService _dialogService;
    //private IDataSerive _dataService;

    #endregion

    public MainViewModel()
    {
        _polymers = new ObservableCollection<Polymer>() { new Polymer("ПВХ-1"), new Polymer("ПВХ-2"), new Polymer("ПВХ-3") };
        _loadedPolymer = null;
        _device = new MeasuringDevice();
    }

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

    #endregion

    #region Commands

    private RelayCommand _loadedCommand;
    private RelayCommand _clearCommand;
    private RelayCommand _preparationCommand;
    private RelayCommand _resetCommand;
    private RelayCommand _startMeasurement;
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
                o => o is not null));
        }
    }

    public RelayCommand ClearCommand
    {
        get
        {
            return _clearCommand ??
                (_clearCommand = new RelayCommand(o => LoadedPolymer = null, o => LoadedPolymer is not null));
        }
    }

    #endregion
}
