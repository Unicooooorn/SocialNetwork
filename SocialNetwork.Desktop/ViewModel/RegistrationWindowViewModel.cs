using System.Threading.Tasks;
using System.Windows.Input;
using SocialNetwork.Api.Dto.Account;
using SocialNetwork.Desktop.Model;
using SocialNetwork.Desktop.Services;

namespace SocialNetwork.Desktop.ViewModel
{
    public class RegistrationWindowViewModel : Notifier
    {
        public RegistrationWindowViewModel()
        {
            RegistrationCommand = new Command(_ => Registration());
        }

        public RegistrationAccountRequest RegistrationAccountRequest { get; set; } = new RegistrationAccountRequest();
        
        private RegistrationService RegistrationService { get; set; } = new RegistrationService();

        public ICommand RegistrationCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private async Task Registration()
        {
            await RegistrationService.Registration(RegistrationAccountRequest);
        }
    }
}
