using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Soul_Talk.Models;
using Soul_Talk.Models.Services;

namespace Soul_Talk.ViewModels
{
    // ViewModel for hovedvinduet (MainWindow)
    // Styrer:
    //  - hvilke institutioner, kunder og indtægter der vises i TreeView
    //  - logikken for at tilføje en ny indtægt
    public class MainViewModel : ViewModelBase
    {
        // --------------------------------------------
        // "Model-data" i hukommelsen (vi bruger ingen database)
        // --------------------------------------------

        // Liste over alle institutioner (kommuner + private institutioner)
        private List<Institution> _institutioner = new List<Institution>();

        // Liste over alle kunder (både private og institutionskunder)
        private List<Kunde> _kunder = new List<Kunde>();

        // Liste over alle indtægter
        private List<Indtaegt> _indtaegter = new List<Indtaegt>();

        // Service der kan beregne timepris ud fra kunde + fysisk/online
        private Timepris _timeprisService = new Timepris();

        // --------------------------------------------
        // Data til TreeView i MainWindow
        // --------------------------------------------

        // Rodnoder i TreeView: 
        //  - "Offentlige institutioner"
        //  - "Private institutioner"
        //  - "Private kunder"
        public ObservableCollection<OverblikNode> RootNodes { get; set; }

        // Kommando til knappen "Tilføj indtægt"
        public ICommand TilfoejIndtaegtCommand { get; set; }

        // Constructor: kaldes når MainViewModel laves i MainWindow
        public MainViewModel()
        {
            RootNodes = new ObservableCollection<OverblikNode>();

            // Knyt knappen i MainWindow til metoden TilfoejIndtaegt()
            TilfoejIndtaegtCommand = new RelayCommand(TilfoejIndtaegt);

            // Læg nogle institutioner ind fra start (ingen kunder/indtægter endnu)
            AllerdeEksisterneData();

            // Byg TreeView-strukturen ud fra listerne
            BygTraeFraModel();
        }

        // --------------------------------------------
        // Initial data (seed) – institutioner der findes fra start
        // --------------------------------------------
        private void AllerdeEksisterneData()
        {
            // Odense Kommune (offentlig institution)
            Institution kommune1 = new Institution();
            kommune1.Id = 1;
            kommune1.Navn = "Odense Kommune";
            kommune1.Type = InstitutionType.Offentlig;

            // Nyborg Kommune (offentlig institution)
            Institution kommune2 = new Institution();
            kommune2.Id = 2;
            kommune2.Navn = "Nyborg Kommune";
            kommune2.Type = InstitutionType.Offentlig;

            // Skovbrynet (privat institution)
            Institution privatInst1 = new Institution();
            privatInst1.Id = 3;
            privatInst1.Navn = "Skovbrynet";
            privatInst1.Type = InstitutionType.Privat;

            _institutioner.Add(kommune1);
            _institutioner.Add(kommune2);
            _institutioner.Add(privatInst1);
        }

        // --------------------------------------------
        // Opretter et Indtaegt-objekt ud fra input
        // --------------------------------------------
        private Indtaegt OpretIndtaegt(Kunde kunde, DateTime dato, decimal timer, bool erFysisk, decimal kilometer)
        {
            // Find timepris (afhænger af kundetype + fysisk/online)
            decimal timepris = _timeprisService.HentTimepris(kunde, erFysisk);

            Indtaegt ind = new Indtaegt();
            // ind.Id findes ikke længere i Indtaegt-klassen og bruges ikke, så vi sætter ikke Id her
            ind.Kunde = kunde;
            ind.Dato = dato;
            ind.Timer = timer;
            ind.ErFysisk = erFysisk;
            ind.Kilometer = kilometer;
            ind.Timepris = timepris;
            ind.Beloeb = timepris * timer;

            return ind;
        }

        // --------------------------------------------
        // Hjælpemetoder som andre ViewModels (fx TilfoejIndtaegtViewModel) bruger
        // --------------------------------------------

        // Returnerer alle institutioner (bruges til ComboBox i "Tilføj indtægt"-vinduet)
        public List<Institution> HentAlleInstitutioner()
        {
            return _institutioner;
        }

        // Returnerer alle kunder (bruges til ComboBox i "Tilføj indtægt"-vinduet)
        public List<Kunde> HentAlleKunder()
        {
            return _kunder;
        }

        // Opretter en ny kunde og lægger den i listen
        // "institution" kan være null -> så er det en privat kunde
        public Kunde TilfoejNyKunde(string navn, Institution institution)
        {
            Kunde kunde = new Kunde();
            kunde.Id = _kunder.Count + 1;
            kunde.Navn = navn;
            kunde.Institution = institution;

            _kunder.Add(kunde);
            return kunde;
        }

        // --------------------------------------------
        // Byg TreeView-strukturen (RootNodes) ud fra listerne
        // --------------------------------------------
        private void BygTraeFraModel()
        {
            RootNodes.Clear();

            // Tre top-noder i TreeView
            OverblikNode offentligeRoot = new OverblikNode("Offentlige institutioner");
            OverblikNode privateInstRoot = new OverblikNode("Private institutioner");
            OverblikNode privateKunderRoot = new OverblikNode("Private kunder");

            RootNodes.Add(offentligeRoot);
            RootNodes.Add(privateInstRoot);
            RootNodes.Add(privateKunderRoot);

            // Først: institutioner + kunder under dem
            foreach (Institution inst in _institutioner)
            {
                // Node for institutionen
                OverblikNode instNode = new OverblikNode(inst.Navn, inst);

                // Læg institutionen under den rigtige gruppe
                if (inst.Type == InstitutionType.Offentlig)
                {
                    offentligeRoot.Children.Add(instNode);
                }
                else
                {
                    privateInstRoot.Children.Add(instNode);
                }

                // Kunder under denne institution
                foreach (Kunde kunde in _kunder)
                {
                    if (kunde.Institution != null && kunde.Institution.Id == inst.Id)
                    {
                        OverblikNode kundeNode = new OverblikNode(kunde.Navn, kunde);
                        instNode.Children.Add(kundeNode);

                        // Indtægter under kunden
                        TilfoejIndtaegterTilKundeNode(kundeNode, kunde);
                    }
                }
            }

            // Derefter: private kunder (uden institution)
            foreach (Kunde kunde in _kunder)
            {
                if (kunde.Institution == null)
                {
                    OverblikNode kundeNode = new OverblikNode(kunde.Navn, kunde);
                    privateKunderRoot.Children.Add(kundeNode);

                    // Indtægter under den private kunde
                    TilfoejIndtaegterTilKundeNode(kundeNode, kunde);
                }
            }
        }

        // Tilføjer indtægtsnoder under en given kundenode
        private void TilfoejIndtaegterTilKundeNode(OverblikNode kundeNode, Kunde kunde)
        {
            foreach (Indtaegt ind in _indtaegter)
            {
                if (ind.Kunde != null && ind.Kunde.Id == kunde.Id)
                {
                    // Teksten, der vises for indtægten i TreeView
                    string tekst = ind.Dato.ToShortDateString() + " | " +
                                   ind.Timer + " timer | " +
                                   ind.Beloeb + " kr";

                    // Tilføj km hvis der er kørt
                    if (ind.ErFysisk && ind.Kilometer > 0)
                    {
                        tekst = tekst + " | " + ind.Kilometer + " km";
                    }

                    OverblikNode indNode = new OverblikNode(tekst, ind);
                    kundeNode.Children.Add(indNode);
                }
            }
        }

        // --------------------------------------------
        // Command-metode: åbner "Tilføj indtægt"-dialogen
        // --------------------------------------------
        private void TilfoejIndtaegt()
        {
            TilfoejIndtaegtWindow window = new TilfoejIndtaegtWindow();

            // Opret ViewModel til dialogen og giv:
            //  - this (MainViewModel) så dialogen kan kalde tilbage
            //  - en metode til at lukke vinduet
            TilfoejIndtaegtViewModel vm = new TilfoejIndtaegtViewModel(this, () => window.Close());
            window.DataContext = vm;

            window.ShowDialog();
        }

        // Bliver kaldt fra TilfoejIndtaegtViewModel, når brugeren klikker "Gem"
        public void TilfoejIndtaegtFraDialog(Kunde kunde, DateTime dato, decimal timer, bool erFysisk, decimal kilometer)
        {
            // Opret ny indtægt og læg den i listen
            Indtaegt ind = OpretIndtaegt(kunde, dato, timer, erFysisk, kilometer);
            _indtaegter.Add(ind);

            // Byg træet igen, så den nye indtægt vises i UI
            BygTraeFraModel();
        }
    }
}
