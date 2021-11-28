using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SocialNetwork.Desktop.ViewModel
{
    public class Notifier : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
