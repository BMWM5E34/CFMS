using System;
using CFMS.Models;

namespace CFMS
{
    public partial class WalletPage : ContentPage
    {
        public WalletPage()
        {
            InitializeComponent();
            DisplayMnemonicPhrase();
        }

        private void DisplayMnemonicPhrase()
        {
            string Username = User.GetUsername();
            ProfileNameLabel.Text = "Profile: " + Username;
        }
        private void Settings_clicked(object sender, EventArgs e)
        {

        }
        private void OnSendButton_clicked(object sender, EventArgs e)
        {

        }
        private void OnReceiveButton_clicked(object sender, EventArgs e)
        {

        }
    }
}
