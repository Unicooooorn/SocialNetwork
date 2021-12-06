using SocialNetwork.Api.Dto.Account;
using SocialNetwork.Desktop.Model;
using SocialNetwork.Desktop.Services;
using SocialNetwork.Desktop.View;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SocialNetwork.Desktop.ViewModel
{
    public class RegistrationWindowViewModel : Notifier
    {
        public RegistrationWindowViewModel()
        {
            RegistrationCommand = new Command(_ => Registration());
            CancelCommand = new Command(_ => Cancel());
        }

        public RegistrationAccountRequestDto RegistrationAccountRequest { get; set; } = new();
        
        public ICommand RegistrationCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private async Task Registration()
        {
            await RegistrationService.Registration(RegistrationAccountRequest);
        }

        private static void Cancel()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Hide();
            var logWindow = new LoginWindow();
            logWindow.Show();
        }
    }
}
