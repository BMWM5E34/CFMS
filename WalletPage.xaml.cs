using System;
using CFMS.Models;

namespace CFMS
{
    public partial class WalletPage : ContentPage
    {
        public WalletPage()
        {
            InitializeComponent();

            string username = User.GetUsername();

            UsernameLabel.Text = $"Hello, {username}!";
        }
    }
}
