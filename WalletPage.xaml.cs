using System;
using CFMS.Models;
using CFMS.Crypto;
using Crypto;
namespace CFMS
{
    public partial class WalletPage : ContentPage
    {
        public WalletPage()
        {
            InitializeComponent();
            DisplayProfileName();
            DisplayBalance();
        }

        private void DisplayProfileName()
        {
            string Username = User.GetUsername();
            ProfileNameLabel.Text = "Profile: " + Username;
        }

        private async void DisplayBalance()
        {
            try
            {
                string bitcoinAddress = Other.GetAddress("bitcoin");
                decimal balance = await QNinja.GetBitcoinBalanceAsync(bitcoinAddress);

                // Display the balance
                Balance_Label.Text = "Bitcoin Balance: " + balance.ToString("0.########");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
        private void Settings_clicked(object sender, EventArgs e)
        {

        }
        private void OnSendButton_clicked(object sender, EventArgs e)
        {

        }
        private void BalanceLabel(object sender, EventArgs e)
        {

        }
        private void OnReceiveButton_clicked(object sender, EventArgs e)
        {
            try
            {
                string MnemonicPhrase = User.GetUserMnemonic();
                string BTCaddress = Other.GetAddress("bitcoin");
                DisplayAlert("Bitcoin address", BTCaddress, "Copy");
                Bitcoin.GenerateBitcoinAddressFromSeed(MnemonicPhrase);

                Clipboard.SetTextAsync(BTCaddress);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
