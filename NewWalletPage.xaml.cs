using System;
using System.IO;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using CFMS.Models;

namespace CFMS
{
    public partial class NewWalletPage : ContentPage
    {

        public NewWalletPage()
        {
            InitializeComponent();
            UsernameEntry.TextChanged += Entry_TextChanged;
            PasswordEntry.TextChanged += Entry_TextChanged;
            ConfirmPasswordEntry.TextChanged += Entry_TextChanged; // Добавляем обработчик события для поля подтверждения пароля
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isValid = !string.IsNullOrWhiteSpace(UsernameEntry.Text) && !string.IsNullOrWhiteSpace(PasswordEntry.Text) && !string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text); // Проверяем, чтобы оба поля и подтверждение пароля были заполнены

            if (isValid && (!IsEnglish(UsernameEntry.Text) || !IsEnglish(PasswordEntry.Text) || !IsEnglish(ConfirmPasswordEntry.Text))) // Проверяем, что введенные данные только на английском языке
            {
                ErrorLabel.Text = "Please enter username and password only in English.";
                isValid = false;
            }
            else if (PasswordEntry.Text != ConfirmPasswordEntry.Text) // Проверяем, совпадают ли введенные пароли
            {
                ErrorLabel.Text = "Passwords do not match.";
                isValid = false;
            }
            else
            {
                ErrorLabel.Text = "";
            }

            ContinueButton.IsEnabled = isValid;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string username = UsernameEntry.Text;
                string password = PasswordEntry.Text;

                if (!IsEnglish(username) || !IsEnglish(password))
                {
                    await DisplayAlert("Error", "Please enter username and password only in English.", "OK");
                    return;
                }

                User user = new User(username, password);
                user.SaveToJson();

                await Navigation.PushAsync(new SavingPhrase());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }

        }
        private bool IsEnglish(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z0-9]+$");
        }
    }
}
