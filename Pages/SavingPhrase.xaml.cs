using CFMS.Models;

namespace CFMS
{
    public partial class SavingPhrase : ContentPage
    {
        public SavingPhrase()
        {
            InitializeComponent();
            DisplayMnemonicPhrase();
        }

        private void DisplayMnemonicPhrase()
        {
            string mnemonic = User.GetUserMnemonic();
            MnemonicLabel.Text = mnemonic;
        }

        private void ContinueButton_Clicked(object sender, EventArgs e)
        {
            var walletPage = new WalletPage();

            Application.Current.MainPage = new NavigationPage(walletPage);

            walletPage.Navigation.PopToRootAsync();
        }
    }
}
