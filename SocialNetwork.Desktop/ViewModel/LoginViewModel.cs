using SocialNetwork.Api.Dto.Account;
using SocialNetwork.Desktop.Model;
using SocialNetwork.Desktop.Services;
using SocialNetwork.Desktop.View;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SocialNetwork.Desktop.ViewModel
{
    public class LoginViewModel : Notifier
    {
        public LoginViewModel()
        {
            LoginCommand = new Command(_ => LoginCommandAsync());
            ExitCommand = new Command(_ => Exit());
            RegistrationCommand = new Command(_ => Registration());
        }
        public LoginModelDto LoginModel { get; set; } = new LoginModelDto();

        public LoginService LoginService { get; set; } = new LoginService();



        public ICommand LoginCommand { get; set; }
        public ICommand ExitCommand { get; set; }
        public ICommand RegistrationCommand { get; set; }


        private async Task LoginCommandAsync()
        {
            await LoginService.LoginServiceAsync(LoginModel);
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private void Registration()
        {
            var regWindow = new RegistrationWindow();
            regWindow.Show();
        }
    }
}
