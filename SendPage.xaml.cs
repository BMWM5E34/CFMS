using Crypto;
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
                NetworkPicker.SelectedIndex == -1 ||
                string.IsNullOrWhiteSpace(FeeEntry.Text))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            string address = AddressEntry.Text;
            double amount = double.Parse(AmountEntry.Text, CultureInfo.InvariantCulture);
            string network = NetworkPicker.SelectedItem.ToString();
            double fee = double.Parse(FeeEntry.Text, CultureInfo.InvariantCulture);

            string message = $"Address: {address}\nAmount: {amount}\nNetwork: {network}\nFee: {fee}";
            bool result = await DisplayAlert("Confirm sending", message, "Cancel", "OK");

            if (!result)
            {
                // User confirmed sending
                // Perform sending operation here
                DisplayAlert("Sending", "Bitcoin sent successfully!", "OK");
            }
            else
            {
                // User canceled sending
                DisplayAlert("Cancelled", "Sending operation cancelled.", "OK");
            }
        }


        private void OnEntryChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(AddressEntry.Text) &&
                double.TryParse(AmountEntry.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _) &&
                NetworkPicker.SelectedIndex != -1 &&
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
