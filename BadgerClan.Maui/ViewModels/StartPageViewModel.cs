using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BadgerClan.Maui.Services;
using System.Collections.ObjectModel;
namespace BadgerClan.Maui.ViewModels;

public partial class StartPageViewModel(IPlayerControlService playerControlService) : ObservableObject
{
    [ObservableProperty]
    private string _baseUrl = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private bool _grpcEnabled = false;

    public ObservableCollection<string> ClientList { get; } = [];

    public bool NewClientValid()
    {
        Uri? uriResult;
        if (Uri.TryCreate(BaseUrl, UriKind.Absolute, out uriResult) && uriResult != null &&
           (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps) && 
           !string.IsNullOrWhiteSpace(Name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    partial void OnBaseUrlChanged(string? oldValue, string newValue)
    {
        AddNewClientCommand.NotifyCanExecuteChanged();
    }

    partial void OnNameChanged(string? oldValue, string newValue)
    {
        AddNewClientCommand.NotifyCanExecuteChanged();
    }

    partial void OnGrpcEnabledChanged(bool oldValue, bool newValue)
    {
        AddNewClientCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(NewClientValid))]
    public void AddNewClient()
    {
        ClientList.Add(Name);
        playerControlService.AddClient(Name, BaseUrl, GrpcEnabled);
        StartControllingCommand.NotifyCanExecuteChanged();
    }

    public bool HasClients()
    {
        return playerControlService.Clients.Count > 0;
    }

    [RelayCommand(CanExecute = nameof(HasClients))]
    public void StartControlling()
    {
        Shell.Current.GoToAsync("///Control");
    }
}
