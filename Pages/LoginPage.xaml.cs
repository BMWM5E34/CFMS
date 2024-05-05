using System;
using CFMS;

namespace CFMS;
public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnCreateWalletClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NewWalletPage());
    }
    private async void OnImportWalletClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ImportWalletPage());
    }
}