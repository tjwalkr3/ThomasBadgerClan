using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using BadgerClan.Maui.Services;
namespace BadgerClan.Maui.ViewModels;

public partial class StartPageViewModel(IPlayerControlService playerControlService) : ObservableObject
{
    [ObservableProperty]
    private string _baseUrl = string.Empty;

    partial void OnBaseUrlChanged(string? oldValue, string newValue)
    {
        StartControllingClientCommand.NotifyCanExecuteChanged();
    }

    public bool AllValid()
    {
        Uri? uriResult;
        if (Uri.TryCreate(BaseUrl, UriKind.Absolute, out uriResult) && uriResult != null &&
           (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [RelayCommand(CanExecute=nameof(AllValid))]
    public async Task StartControllingClient()
    {
        playerControlService.SetBaseUrl(BaseUrl);
        await Shell.Current.GoToAsync("///Control");
    }
}
