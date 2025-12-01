using System;
using System.Windows.Input;

namespace Soul_Talk.ViewModels
{
    // Simpel implementation af ICommand.
    // Bruges til at binde knapper i XAML til metoder i ViewModels.
    public class RelayCommand : ICommand
    {
        // Metoden der skal køres, når kommandoen udføres
        private Action _execute;

        // Constructor: her giver vi den metode videre, som skal køres
        public RelayCommand(Action execute)
        {
            _execute = execute;
        }

        // Fortæller WPF om kommandoen "må" køre.
        // Vi siger bare altid ja (true) for at holde det simpelt.
        public bool CanExecute(object parameter)
        {
            return true;
        }

        // Kaldes når kommandoen udføres (f.eks. når brugeren klikker på en knap)
        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute();
            }
        }

        // Bruges normalt til at fortælle WPF "CanExecute er ændret".
        // Her gør vi ingenting for at holde det simpelt.
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }
    }
}
