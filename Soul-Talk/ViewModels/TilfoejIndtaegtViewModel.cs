using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Soul_Talk.Models;

namespace Soul_Talk.ViewModels
{
    // ViewModel til vinduet "Tilføj indtægt"
    // Styrer data til felterne i vinduet og logikken, når brugeren klikker Gem/Annuller
    public class TilfoejIndtaegtViewModel : ViewModelBase
    {
        // Reference til MainViewModel, så vi kan tilføje nye kunder og indtægter
        private MainViewModel _main;

        // Methode der bliver kaldt for at lukke vinduet (kommer fra MainViewModel)
        private Action _close;

        // Liste med alle eksisterende kunder (til ComboBox)
        public ObservableCollection<Kunde> Kunder { get; set; }

        // Liste med alle institutioner (til ComboBox, når vi opretter ny kunde)
        public ObservableCollection<Institution> Institutioner { get; set; }

        // -------------------------------------------------------
        // Properties der er bundet til felter i XAML-vinduet
        // -------------------------------------------------------

        private Kunde _valgtKunde;
        // Den kunde der er valgt i "Eksisterende kunde"-ComboBoxen
        public Kunde ValgtKunde
        {
            get { return _valgtKunde; }
            set
            {
                _valgtKunde = value;
                OnPropertyChanged("ValgtKunde");
            }
        }

        private bool _opretNyKunde;
        // Checkbox: om vi vil oprette en ny kunde i stedet for at vælge en eksisterende
        public bool OpretNyKunde
        {
            get { return _opretNyKunde; }
            set
            {
                _opretNyKunde = value;
                OnPropertyChanged("OpretNyKunde");
            }
        }

        private string _nyKundeNavn = "";
        // Navn på den nye kunde, hvis OpretNyKunde = true
        public string NyKundeNavn
        {
            get { return _nyKundeNavn; }
            set
            {
                _nyKundeNavn = value;
                OnPropertyChanged("NyKundeNavn");
            }
        }

        private Institution _valgtInstitution;
        // Institution valgt til den nye kunde (valgfri – hvis tom, bliver det privat kunde)
        public Institution ValgtInstitution
        {
            get { return _valgtInstitution; }
            set
            {
                _valgtInstitution = value;
                OnPropertyChanged("ValgtInstitution");
            }
        }

        private DateTime _dato;
        // Dato for indtægten
        public DateTime Dato
        {
            get { return _dato; }
            set
            {
                _dato = value;
                OnPropertyChanged("Dato");
            }
        }

        private decimal _timer;
        // Antal timer der er arbejdet
        public decimal Timer
        {
            get { return _timer; }
            set
            {
                _timer = value;
                OnPropertyChanged("Timer");
            }
        }

        private bool _erFysisk;
        // Om mødet er fysisk (true) eller online (false)
        public bool ErFysisk
        {
            get { return _erFysisk; }
            set
            {
                _erFysisk = value;
                OnPropertyChanged("ErFysisk");
            }
        }

        private decimal _kilometer;
        // Kørte kilometer i forbindelse med denne opgave
        public decimal Kilometer
        {
            get { return _kilometer; }
            set
            {
                _kilometer = value;
                OnPropertyChanged("Kilometer");
            }
        }

        // Commands som knapperne i XAML binder til
        public ICommand GemCommand { get; set; }
        public ICommand AnnullerCommand { get; set; }

        // -------------------------------------------------------
        // Constructor
        // -------------------------------------------------------
        public TilfoejIndtaegtViewModel(MainViewModel main, Action close)
        {
            _main = main;
            _close = close;

            // Hent alle kunder og institutioner fra MainViewModel
            Kunder = new ObservableCollection<Kunde>(_main.HentAlleKunder());
            Institutioner = new ObservableCollection<Institution>(_main.HentAlleInstitutioner());

            // Standardværdier når vinduet åbnes
            OpretNyKunde = false;          // Udgangspunkt: brug eksisterende kunde
            Dato = DateTime.Today;         // Forslag: i dag
            ErFysisk = true;               // Forslag: fysisk møde som standard

            // Knapperne i vinduet kalder disse metoder
            GemCommand = new RelayCommand(Gem);
            AnnullerCommand = new RelayCommand(_close);
        }

        // -------------------------------------------------------
        // Logik når brugeren klikker "Gem"
        // -------------------------------------------------------
        private void Gem()
        {
            Kunde kundeSomSkalBruges;

            // 1) Find ud af hvilken kunde der skal bruges
            if (OpretNyKunde)
            {
                // Vi vil oprette en ny kunde
                if (string.IsNullOrWhiteSpace(NyKundeNavn))
                {
                    MessageBox.Show("Skriv et navn til den nye kunde.");
                    return;
                }

                // Opret ny kunde via MainViewModel
                // Hvis ValgtInstitution er null -> privat kunde
                kundeSomSkalBruges = _main.TilfoejNyKunde(NyKundeNavn, ValgtInstitution);
            }
            else
            {
                // Vi bruger en eksisterende kunde fra listen
                if (ValgtKunde == null)
                {
                    MessageBox.Show("Vælg en kunde.");
                    return;
                }

                kundeSomSkalBruges = ValgtKunde;
            }

            // 2) Tjek at timer giver mening
            if (Timer <= 0)
            {
                MessageBox.Show("Timer skal være større end 0.");
                return;
            }

            // 3) Hvis mødet er online, giver kilometer ikke mening
            if (!ErFysisk)
            {
                Kilometer = 0;
            }

            // 4) Giv alle oplysninger til MainViewModel,
            // som opretter Indtaegt-objektet og opdaterer træet
            _main.TilfoejIndtaegtFraDialog(kundeSomSkalBruges, Dato, Timer, ErFysisk, Kilometer);

            // 5) Luk vinduet
            _close();
        }
    }
}
