using CFMS.Models;



namespace CFMS;
public partial class VerificationPage : ContentPage
{
	public VerificationPage()
	{
		InitializeComponent();
	}

    private void OnLoginClicked(object sender, EventArgs e)
    {
        string enteredPassword = PasswordEntry.Text;

        if (enteredPassword == User.GetUserPassword())
        {
            var walletPage = new WalletPage();

            Application.Current.MainPage = new NavigationPage(walletPage);

            walletPage.Navigation.PopToRootAsync();
        }
        else
        {
            ErrorMessage.Text = "Incorrect password. Please try again.";
        }
    }
    private void OnRestoreClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new RestoreWalletPage();
    }
    private void OnQuitClicked(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
}