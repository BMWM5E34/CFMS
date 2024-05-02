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
        private void RefreshBalance_clicked(object sender, EventArgs e)
        {
            DisplayBalance();
        }
        private async void DisplayBalance()
        {
            try
            {
                string bitcoinAddress = Other.GetAddress("bitcoin");

                if (bitcoinAddress != null)
                {
                    decimal balance = Bitcoin.GetBitcoinBalance(bitcoinAddress);
                    //decimal balance = 1;

                    if (Balance_Label != null)
                    {
                        Balance_Label.Text = balance.ToString("0.########") + " BTC";
                    }
                    else
                    {
                        await DisplayAlert("Error", "Balance_Label is null", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Bitcoin address is null", "OK");
                }
            }
            catch (Exception ex)
            {
                Balance_Label.Text = $"Error, {ex.Message}";
            }
        }
        private void Settings_clicked(object sender, EventArgs e)
        {

        }
        private void OnSendButton_clicked(object sender, EventArgs e)
        {
            Bitcoin.SendBitcoin();
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
