using System.Windows;
using SocialNetwork.Desktop.Services;
using SocialNetwork.Desktop.ViewModel;

namespace SocialNetwork.Desktop.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            var accountService = new AccountService();
            DataContext = new LoginViewModel(accountService);
        }
    }
}
