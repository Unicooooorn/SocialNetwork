using SocialNetwork.Desktop.ViewModel;
using System.Windows;

namespace SocialNetwork.Desktop.View
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
            DataContext = new RegistrationWindowViewModel();
        }
    }
}
