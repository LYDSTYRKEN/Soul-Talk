using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Soul_Talk.Models;
using Soul_Talk.Models.Forretningslogik;
using Soul_Talk.Models.Repositories;
using System.IO;

namespace Soul_Talk.ViewModels
{
    // ViewModel for hovedvinduet (MainWindow)
    // Styrer:
    //  - hvilke institutioner, kunder og indtægter der vises i TreeView
    //  - logikken for at tilføje en ny indtægt
    public class MainViewModel : ViewModelBase
    {
        // --------------------------------------------
        // Repositories (lager i hukommelsen)
        // --------------------------------------------

        private InstitutionRepository _instRepo = new InstitutionRepository();
        private KundeRepository _kundeRepo = new KundeRepository();
        private IndtaegtRepository _indtaegtRepo = new IndtaegtRepository();

        // Klasse der kan beregne timepris
        private Timepris _timepris = new Timepris();

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

            LaesAltFraFiler();

            // Byg TreeView-strukturen ud fra repositories
            BygTraeFraModel();
        }

        // --------------------------------------------
        // Initial data (seed) – institutioner der findes fra start
        // --------------------------------------------
        private void AllerdeEksisterneData()
        {
            // Haderslev Kommune (offentlig institution)
            Institution kommune1 = new Institution();
            kommune1.Id = 1;
            kommune1.Navn = "Haderslev Kommune";
            kommune1.Type = InstitutionType.Offentlig;

            // Christiansfeld Kommune (offentlig institution)
            Institution kommune2 = new Institution();
            kommune2.Id = 2;
            kommune2.Navn = "Christiansfeld Kommune";
            kommune2.Type = InstitutionType.Offentlig;

            // Horsens Kommune (offentlig institution)
            Institution kommune3 = new Institution();
            kommune3.Id = 3;
            kommune3.Navn = "Horsens Kommune";
            kommune3.Type = InstitutionType.Offentlig;

            // Odense Kommune (offentlig institution)
            Institution kommune4 = new Institution();
            kommune4.Id = 4;
            kommune4.Navn = "Odense Kommune";
            kommune4.Type = InstitutionType.Offentlig;

            // Skovbrynet (privat institution)
            Institution privatInst1 = new Institution();
            privatInst1.Id = 5;
            privatInst1.Navn = "Skovbrynet";
            privatInst1.Type = InstitutionType.Privat;

            // Fuglereden (privat institution)
            Institution privatInst2 = new Institution();
            privatInst2.Id = 6;
            privatInst2.Navn = "Fuglereden";
            privatInst2.Type = InstitutionType.Privat;

            // Hønegården (privat institution)
            Institution privatInst3 = new Institution();
            privatInst3.Id = 7;
            privatInst3.Navn = "Hønegården";
            privatInst3.Type = InstitutionType.Privat;

            _instRepo.Tilfoej(kommune1);
            _instRepo.Tilfoej(kommune2);
            _instRepo.Tilfoej(kommune3);
            _instRepo.Tilfoej(kommune4);
            _instRepo.Tilfoej(privatInst1);
            _instRepo.Tilfoej(privatInst2);
            _instRepo.Tilfoej(privatInst3);
        }

        // --------------------------------------------
        // Opretter et Indtaegt-objekt ud fra input
        // --------------------------------------------
        private Indtaegt OpretIndtaegt(Kunde kunde, DateTime dato, decimal timer, bool erFysisk, decimal kilometer)
        {
            // Find timepris (afhænger af kundetype + fysisk/online)
            decimal timepris = _timepris.HentTimepris(kunde, erFysisk);

            Indtaegt ind = new Indtaegt();
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
        // Hjælpemetoder som andre ViewModels bruger
        // --------------------------------------------

        // Returnerer alle institutioner (bruges til ComboBox i "Tilføj indtægt"-vinduet)
        public List<Institution> HentAlleInstitutioner()
        {
            return _instRepo.HentAlle();
        }

        // Returnerer alle kunder (bruges til ComboBox i "Tilføj indtægt"-vinduet)
        public List<Kunde> HentAlleKunder()
        {
            return _kundeRepo.HentAlle();
        }

        // Opretter en ny kunde og lægger den i repository
        // "institution" kan være null -> så er det en privat kunde
        public Kunde TilfoejNyKunde(string navn, Institution institution)
        {
            Kunde kunde = new Kunde();
            kunde.Id = _kundeRepo.HentAlle().Count + 1;
            kunde.Navn = navn;
            kunde.Institution = institution;

            _kundeRepo.Tilfoej(kunde);
            return kunde;
        }

        // --------------------------------------------
        // Byg TreeView-strukturen (RootNodes) ud fra repositories
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

            List<Institution> institutioner = _instRepo.HentAlle();
            List<Kunde> kunder = _kundeRepo.HentAlle();

            // Først: institutioner + kunder under dem
            foreach (Institution inst in institutioner)
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
                foreach (Kunde kunde in kunder)
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
            foreach (Kunde kunde in kunder)
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
            List<Indtaegt> alleIndtaegter = _indtaegtRepo.HentAlle();

            foreach (Indtaegt ind in alleIndtaegter)
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
            // Opret ny indtægt og læg den i repository
            Indtaegt ind = OpretIndtaegt(kunde, dato, timer, erFysisk, kilometer);
            _indtaegtRepo.Tilfoej(ind);

            // Byg træet igen, så den nye indtægt vises i UI
            BygTraeFraModel();

            GemAltTilFiler(); // Gem data til filer efter tilføjelse af indtægt
        }
        
        // Gemmer alle data til tre tekstfiler i programmets mappe
        public void GemAltTilFiler()
        {
            _instRepo.GemTilFil("institutioner.txt");
            _kundeRepo.GemTilFil("kunder.txt");
            _indtaegtRepo.GemTilFil("indtaegter.txt");
        }

        private void LaesAltFraFiler()
        {
            // 1) Institutioner
            if (File.Exists("institutioner.txt"))
            {
                string[] linjer = File.ReadAllLines("institutioner.txt");

                foreach (string linje in linjer)
                {
                    string[] dele = linje.Split(';');   // dele[0] = Id, dele[1] = Navn, dele[2] = Type

                    Institution inst = new Institution();
                    inst.Id = int.Parse(dele[0]);
                    inst.Navn = dele[1];
                    inst.Type = (InstitutionType)Enum.Parse(typeof(InstitutionType), dele[2]);

                    _instRepo.Tilfoej(inst);
                }
            }

            // 2) Kunder
            if (File.Exists("kunder.txt"))
            {
                string[] linjer = File.ReadAllLines("kunder.txt");
                List<Institution> institutioner = _instRepo.HentAlle();

                foreach (string linje in linjer)
                {
                    string[] dele = linje.Split(';');   // dele[0] = Id, dele[1] = Navn, dele[2] = InstitutionId

                    Kunde k = new Kunde();
                    k.Id = int.Parse(dele[0]);
                    k.Navn = dele[1];
                    int instId = int.Parse(dele[2]);

                    if (instId != 0)
                    {
                        // find institution med samme Id
                        Institution fundetInst = null;
                        foreach (Institution inst in institutioner)
                        {
                            if (inst.Id == instId)
                            {
                                fundetInst = inst;
                                break;
                            }
                        }
                        k.Institution = fundetInst;
                    }

                    _kundeRepo.Tilfoej(k);
                }
            }

            // 3) Indtægter
            if (File.Exists("indtaegter.txt"))
            {
                string[] linjer = File.ReadAllLines("indtaegter.txt");
                List<Kunde> kunder = _kundeRepo.HentAlle();

                foreach (string linje in linjer)
                {
                    string[] dele = linje.Split(';');   // 0=KundeId, 1=Dato, 2=Timer, 3=ErFysisk, 4=Km, 5=Timepris, 6=Beloeb

                    int kundeId = int.Parse(dele[0]);

                    // find kunde med samme Id
                    Kunde fundetKunde = null;
                    foreach (Kunde k in kunder)
                    {
                        if (k.Id == kundeId)
                        {
                            fundetKunde = k;
                            break;
                        }
                    }

                    if (fundetKunde == null)
                        continue;

                    Indtaegt ind = new Indtaegt();
                    ind.Kunde = fundetKunde;
                    ind.Dato = DateTime.Parse(dele[1]);
                    ind.Timer = decimal.Parse(dele[2]);
                    ind.ErFysisk = bool.Parse(dele[3]);
                    ind.Kilometer = decimal.Parse(dele[4]);
                    ind.Timepris = decimal.Parse(dele[5]);
                    ind.Beloeb = decimal.Parse(dele[6]);

                    _indtaegtRepo.Tilfoej(ind);
                }
            }
        }


    }
}
