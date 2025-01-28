using BadgerClan.Maui.ViewModels;

namespace BadgerClan.Maui.Views;

public partial class ControlPage : ContentPage
{
	public ControlPage(ControlPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}