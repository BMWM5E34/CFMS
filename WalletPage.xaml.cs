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
                decimal rate = Bitcoin.GetRateBtc();
                //decimal rate = 64000;
                string str_btc_balance = Balance_Label.Text;

                // Удаляем слово "BTC" из строки баланса
                str_btc_balance = str_btc_balance.Replace(" BTC", "");

                if (decimal.TryParse(str_btc_balance, out decimal btc_balance))
                {
                    decimal balance = rate * btc_balance;

                    ConvertToUsd_Label.Text = $"$ {balance}";
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

        private async void DisplayBalance()
        {
            try
            {
                string bitcoinAddress = Other.GetAddress("bitcoin");

                if (bitcoinAddress != null)
                {
                    decimal balance = Bitcoin.GetBitcoinBalance(bitcoinAddress);
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
        private void Settings_clicked(object sender, EventArgs e)
        {

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
