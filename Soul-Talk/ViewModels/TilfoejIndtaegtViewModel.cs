using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Soul_Talk.Models;

namespace Soul_Talk.ViewModels
{
    public class TilfoejIndtaegtViewModel : ViewModelBase
    {
        private MainViewModel _main;
        private Action _close;

        public ObservableCollection<Kunde> Kunder { get; set; }
        public ObservableCollection<Institution> Institutioner { get; set; }

        private Kunde _valgtKunde;
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
        public decimal Kilometer
        {
            get { return _kilometer; }
            set
            {
                _kilometer = value;
                OnPropertyChanged("Kilometer");
            }
        }

        public ICommand GemCommand { get; set; }
        public ICommand AnnullerCommand { get; set; }

        public TilfoejIndtaegtViewModel(MainViewModel main, Action close)
        {
            _main = main;
            _close = close;

            Kunder = new ObservableCollection<Kunde>(_main.HentAlleKunder());
            Institutioner = new ObservableCollection<Institution>(_main.HentAlleInstitutioner());

            OpretNyKunde = false;
            Dato = DateTime.Today;
            ErFysisk = true;

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
