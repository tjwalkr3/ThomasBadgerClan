using BadgerClan.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace BadgerClan.Maui.ViewModels;

public partial class ControlPageViewModel(IPlayerControlService playerControlService) : ObservableObject
{
    [ObservableProperty]
    private string _currentState = "Stopped";

    [RelayCommand]
    public async Task Attack()
    {
        await playerControlService.AttackAsync();
        CurrentState = "Attacking";
    }

    [RelayCommand]
    public async Task Defend()
    {
        await playerControlService.DefendAsync();
        CurrentState = "Defending";
    }

    [RelayCommand]
    public async Task Scatter()
    {
        await playerControlService.ScatterAsync();
        CurrentState = "Scattering";
    }

    [RelayCommand]
    public async Task Stop()
    {
        await playerControlService.StopAsync();
        CurrentState = "Stopped";
    }
}
