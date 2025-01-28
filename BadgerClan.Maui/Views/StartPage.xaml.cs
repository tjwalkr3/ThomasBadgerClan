using BadgerClan.Maui.ViewModels;

namespace BadgerClan.Maui.Views;

public partial class StartPage : ContentPage
{
	public StartPage(StartPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}