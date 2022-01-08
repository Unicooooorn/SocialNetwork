using SocialNetwork.Desktop.ViewModel;
using System.Windows;
using SocialNetwork.Desktop.Services;

namespace SocialNetwork.Desktop.View
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();

            var accountService = new AccountService();

            DataContext = new RegistrationWindowViewModel(accountService);
        }
    }
}
