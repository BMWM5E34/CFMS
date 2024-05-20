using CFMS.Models;
using Crypto;
using Microsoft.Maui.Controls;
using System;
using System.Globalization;

namespace CFMS
{
    public partial class SendPage : ContentPage
    {
        public SendPage()
        {
            InitializeComponent();
            SendBitcoinButton.IsEnabled = false;
        }

        private async void OnSendBTCClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AddressEntry.Text) ||
                string.IsNullOrWhiteSpace(AmountEntry.Text) ||
                string.IsNullOrWhiteSpace(FeeEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            string address = AddressEntry.Text;
            double amount;
            if (!double.TryParse(AmountEntry.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out amount))
            {
                await DisplayAlert("Error", "Invalid amount format.", "OK");
                return;
            }

            double fee;
            if (!double.TryParse(FeeEntry.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out fee))
            {
                await DisplayAlert("Error", "Invalid fee format.", "OK");
                return;
            }

            string message = $"Address: {address}\nAmount: {amount}\nFee: {fee}";
            bool result = await DisplayAlert("Confirm sending", message, "OK", "Cancel");

            if (result)
            {
                // User confirmed sending
                try
                {
                    string amount = Convert.ToDecimal(amount);
                    await btc.SendTransaction(btc.network, address, AmountToSend);
                    await DisplayAlert("Success", "Bitcoin sent successfully!", "OK");
                    await Navigation.PushAsync(new WalletPage());
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to send Bitcoin: {ex.Message}", "OK");
                }
            }
            else
            {
                // User canceled sending
                await DisplayAlert("Cancelled", "Sending operation cancelled.", "OK");
            }
        }

        private void OnEntryChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AddressEntry.Text) &&
                double.TryParse(AmountEntry.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _) &&
                double.TryParse(FeeEntry.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
            {
                SendBitcoinButton.IsEnabled = true;
            }
            else
            {
                SendBitcoinButton.IsEnabled = false;
            }
        }
    }
}
