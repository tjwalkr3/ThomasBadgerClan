using BadgerClan.Maui.ViewModels;
using BadgerClan.Maui.Services;

namespace BadgerClan.Test;

public class MauiTest
{
    [Theory]
    [InlineData("", false)]
    [InlineData("https://www.google.com", true)]
    [InlineData("https://", false)]
    public void TestUrlValidation(string baseUrl, bool expected)
    {
        var playerControlService = new PlayerControlService();
        var startPageViewModel = new StartPageViewModel(playerControlService);
        startPageViewModel.BaseUrl = baseUrl;
        startPageViewModel.Name = "Test Client";

        Assert.Equal(expected, startPageViewModel.NewClientValid());
    }
}
