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

        User user = User.LoadFromJson();

        if (enteredPassword == user.Password)
        {
            Application.Current.MainPage = new WalletPage();
        }
        else
        {
            ErrorMessage.Text = "Incorrect password. Please try again.";
        }
    }
    private void OnRestoreClicked(object sender, EventArgs e)
    {

    }
    private void OnQuitClicked(object sender, EventArgs e)
    {

    }
}