using System.Windows;
using SocialNetwork.Desktop.ViewModel;

namespace SocialNetwork.Desktop.View
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

            DataContext = new LoginViewModel();
        }
    }
}
