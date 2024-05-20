using Crypto;

namespace CFMS;
public partial class RestoreWalletPage : ContentPage
{
    public RestoreWalletPage()
    {
        InitializeComponent();
    }

    private void ImportButton_Clicked(object sender, EventArgs e)
    {
        string mnemonicPhrase = $"{Word1Entry.Text.Trim()} {Word2Entry.Text.Trim()} {Word3Entry.Text.Trim()} {Word4Entry.Text.Trim()} {Word5Entry.Text.Trim()} {Word6Entry.Text.Trim()} {Word7Entry.Text.Trim()} {Word8Entry.Text.Trim()} {Word9Entry.Text.Trim()} {Word10Entry.Text.Trim()} {Word11Entry.Text.Trim()} {Word12Entry.Text.Trim()}";

        if (string.IsNullOrWhiteSpace(mnemonicPhrase))
        {
            DisplayAlert("Error", "Please enter all words", "OK");
            return;
        }

        try
        {
            btc.GenerateBitcoinAddressFromSeed(mnemonicPhrase);

            var createUserWalletPage = new CreateUserWalletPage(mnemonicPhrase);
            Application.Current.MainPage = new NavigationPage(createUserWalletPage);
            createUserWalletPage.Navigation.PopToRootAsync();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", $"Failed to import wallet: {ex.Message}", "OK");
        }
    }
}
