using Soul_Talk.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Soul_Talk.ViewModels
{
    public class TilfoejIndtaegtViewModel : ViewModelBase
    {
        private readonly MainViewModel _main;
        private readonly Action _close;

        public ObservableCollection<Kunde> Kunder { get; }

        private Kunde? _valgtKunde;
        public Kunde? ValgtKunde
        {
            get => _valgtKunde;
            set
            {
                _valgtKunde = value;
                OnPropertyChanged(nameof(ValgtKunde));
            }
        }

        public ObservableCollection<Institution> Institutioner { get; }

        private bool _opretNyKunde;
        public bool OpretNyKunde
        {
            get => _opretNyKunde;
            set
            {
                _opretNyKunde = value;
                OnPropertyChanged(nameof(OpretNyKunde));
            }
        }

        private string _nyKundeNavn = string.Empty;
        public string NyKundeNavn
        {
            get => _nyKundeNavn;
            set
            {
                _nyKundeNavn = value;
                OnPropertyChanged(nameof(NyKundeNavn));
            }
        }

        private Institution? _valgtInstitution;
        public Institution? ValgtInstitution
        {
            get => _valgtInstitution;
            set
            {
                _valgtInstitution = value;
                OnPropertyChanged(nameof(ValgtInstitution));
            }
        }

        private DateTime _dato;
        public DateTime Dato
        {
            get => _dato;
            set
            {
                _dato = value;
                OnPropertyChanged(nameof(Dato));
            }
        }

        private decimal _timer;
        public decimal Timer
        {
            get => _timer;
            set
            {
                _timer = value;
                OnPropertyChanged(nameof(Timer));
            }
        }

        private bool _erFysisk;
        public bool ErFysisk
        {
            get => _erFysisk;
            set
            {
                _erFysisk = value;
                OnPropertyChanged(nameof(ErFysisk));
            }
        }

        private decimal _kilometer;
        public decimal Kilometer
        {
            get => _kilometer;
            set
            {
                _kilometer = value;
                OnPropertyChanged(nameof(Kilometer));
            }
        }


        public ICommand GemCommand { get; }
        public ICommand AnnullerCommand { get; }

        public TilfoejIndtaegtViewModel(MainViewModel main, Action close)
        {
            _main = main;
            _close = close;

            Kunder = new ObservableCollection<Kunde>(_main.HentAlleKunder());

            Institutioner = new ObservableCollection<Institution>(_main.HentAlleInstitutioner());

            OpretNyKunde = false;      // standard: brug eksisterende kunde
            Dato = DateTime.Today;
            ErFysisk = true;

            Dato = DateTime.Today;
            ErFysisk = true;   // standard: fysisk møde

            GemCommand = new RelayCommand(Gem);
            AnnullerCommand = new RelayCommand(_close);
        }

        private void Gem()
        {
            Kunde kundeSomSkalBruges;

            if (OpretNyKunde)
            {
                if (string.IsNullOrWhiteSpace(NyKundeNavn))
                {
                    MessageBox.Show("Skriv et navn til den nye kunde.");
                    return;
                }

                // Opret ny kunde (privat hvis ValgtInstitution == null)
                kundeSomSkalBruges = _main.TilfoejNyKunde(NyKundeNavn, ValgtInstitution);
            }
            else
            {
                if (ValgtKunde == null)
                {
                    MessageBox.Show("Vælg en kunde.");
                    return;
                }

                kundeSomSkalBruges = ValgtKunde;
            }

            if (Timer <= 0)
            {
                MessageBox.Show("Timer skal være større end 0.");
                return;
            }

            if (!ErFysisk)
            {
                // Online: km giver ikke mening
                Kilometer = 0;
            }

            _main.TilfoejIndtaegtFraDialog(kundeSomSkalBruges, Dato, Timer, ErFysisk, Kilometer);

            _close();
        }

    }
}