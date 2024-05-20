using System;
using CFMS.Models;
using Crypto;
using CFMS.Pages;

namespace CFMS
{
    public partial class WalletPage : ContentPage
    {
        public WalletPage()
        {
            string MnemonicPhrase = User.GetUserMnemonic();
            btc.GenerateBitcoinAddressFromSeed(MnemonicPhrase);
            User.GetUserAddress("bitcoin");

            InitializeComponent();
            DisplayProfileName();
            DisplayBalance();
            DisplayBtcToUsd();
            DisplayMainUSDBalance();
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

        private async void DisplayBtcToUsd()
        {
            try
            {
                decimal rate = btc.GetRateBtc();
                //decimal rate = 64000;
                string str_btc_balance = Balance_Label.Text;

                str_btc_balance = str_btc_balance.Replace(" BTC", "");

                if (decimal.TryParse(str_btc_balance, out decimal btc_balance))
                {
                    decimal balance = rate * btc_balance;

                    ConvertToUsd_Label.Text = $"$ {balance.ToString("0.00")}";
                }
                else
                {
                    ConvertToUsd_Label.Text = "Invalid BTC balance format";
                }
            }
            catch (Exception ex)
            {
                ConvertToUsd_Label.Text = $"Error: {ex.Message}";
            }
        }
        private void DisplayMainUSDBalance()
        {
            MainUSDBalance.Text = ConvertToUsd_Label.Text;
        }
        private void OnRefreshClicked(object sender, EventArgs e)
        {
            DisplayBalance();
            DisplayBtcToUsd();
            DisplayMainUSDBalance();
        }
        private async void DisplayBalance()
        {
            try
            {
                string bitcoinAddress = User.GetUserAddress("bitcoin");

                if (bitcoinAddress != null)
                {
                    decimal balance = User.GetBalance(bitcoinAddress);
                    //double balance = 0.00011765;

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
        private async void Settings_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }
        private void refresh_clicked(object sender, EventArgs e)
        {
            DisplayBalance();
            DisplayBtcToUsd();
            DisplayMainUSDBalance();
        }
        private async void OnSendButton_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SendPage());
        }

        private void BalanceLabel(object sender, EventArgs e)
        {

        }
        private void OnReceiveButton_clicked(object sender, EventArgs e)
        {
            try
            {
                string MnemonicPhrase = User.GetUserMnemonic();
                btc.GenerateBitcoinAddressFromSeed(MnemonicPhrase);

                string BTCaddress = User.GetUserAddress("bitcoin");
                DisplayAlert("Bitcoin address", BTCaddress, "Copy");

                Clipboard.SetTextAsync(BTCaddress);
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
