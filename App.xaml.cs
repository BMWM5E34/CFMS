using CFMS.Models;

namespace CFMS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var user = User.GetUsername();
            if (user!=null)
            {
                MainPage = new VerificationPage();
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }
    }
}
