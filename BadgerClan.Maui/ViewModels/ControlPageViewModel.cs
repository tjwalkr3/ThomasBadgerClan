using BadgerClan.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BadgerClan.Maui.Models;
using System.Collections.ObjectModel;
namespace BadgerClan.Maui.ViewModels;

public partial class ControlPageViewModel(IPlayerControlService playerControlService) : ObservableObject
{
    [ObservableProperty]
    private string _currentState = "Scattering";

    [ObservableProperty]
    private string _selectedClient = "No Client Selected";

    public ObservableCollection<string> ClientList { get; } = new(playerControlService.Clients.Select(c => c.Name).ToList());

    partial void OnSelectedClientChanged(string? oldValue, string newValue)
    {
        playerControlService.SetCurrentClient(newValue);
        AttackCommand.NotifyCanExecuteChanged();
        DefendCommand.NotifyCanExecuteChanged();
        ScatterCommand.NotifyCanExecuteChanged();
        StopCommand.NotifyCanExecuteChanged();
    }

    public bool ClientSet()
    {
        return playerControlService.CurrentClient != null;
    }

    [RelayCommand(CanExecute = nameof(ClientSet))]
    public async Task Attack()
    {
        await playerControlService.AttackAsync();
        CurrentState = "Attacking";
    }

    [RelayCommand(CanExecute = nameof(ClientSet))]
    public async Task Defend()
    {
        await playerControlService.DefendAsync();
        CurrentState = "Defending";
    }

    [RelayCommand(CanExecute = nameof(ClientSet))]
    public async Task Scatter()
    {
        await playerControlService.ScatterAsync();
        CurrentState = "Scattering";
    }

    [RelayCommand(CanExecute = nameof(ClientSet))]
    public async Task Stop()
    {
        await playerControlService.StopAsync();
        CurrentState = "Stopped";
    }
}
