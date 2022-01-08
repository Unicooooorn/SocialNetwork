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
    public class LoginViewModel : Notifier
    {
        private readonly IAccountService _accountService;

        public LoginViewModel(IAccountService accountService)
        {
            _accountService = accountService;
            LoginCommand = new Command(_ => LoginCommandAsync());
            ExitCommand = new Command(_ => Exit());
            RegistrationCommand = new Command(_ => Registration());
        }

        public LoginModelDto LoginModel { get; set; } = new();
        

        public ICommand LoginCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand RegistrationCommand { get; set; }


        private async Task LoginCommandAsync()
        {
            await _accountService.LoginServiceAsync(LoginModel);
        }

        private static void Exit()
        {
            Application.Current.Shutdown();
        }

        private static void Registration()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window.Hide();
            var regWindow = new RegistrationWindow();
            regWindow.Show();
        }
    }
}
