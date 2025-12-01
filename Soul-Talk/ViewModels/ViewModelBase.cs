using System.ComponentModel;

namespace Soul_Talk.ViewModels
{
    // Fælles base-klasse til alle ViewModels i projektet.
    // Den sørger for, at WPF får besked, når en property ændrer sig,
    // så bindings (fx TextBox, TextBlock osv.) bliver opdateret automatisk.
    public class ViewModelBase : INotifyPropertyChanged
    {
        // Event som WPF lytter på. Når vi "raiser" den,
        // ved WPF at en property har ændret værdi.
        public event PropertyChangedEventHandler PropertyChanged;

        // Denne metode kaldes fra set-delen på en property
        // fx:
        //   _navn = value;
        //   OnPropertyChanged("Navn");
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                // Fortæl WPF: "propertyName" er ændret
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
