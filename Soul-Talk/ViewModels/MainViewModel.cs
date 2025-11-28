using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Soul_Talk.Models;
using Soul_Talk.Models.Services;

namespace Soul_Talk.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Data i hukommelsen
        private List<Institution> _institutioner = new List<Institution>();
        private List<Kunde> _kunder = new List<Kunde>();
        private List<Indtaegt> _indtaegter = new List<Indtaegt>();

        private TimeprisService _timeprisService = new TimeprisService();

        public ObservableCollection<OverblikNode> RootNodes { get; set; }

        public ICommand TilfoejIndtaegtCommand { get; set; }

        public MainViewModel()
        {
            RootNodes = new ObservableCollection<OverblikNode>();

            TilfoejIndtaegtCommand = new RelayCommand(TilfoejIndtaegt);

            // Fyld nogle institutioner ind fra start
            AllerdeEksisterneData();

            // Byg træet ud fra listerne
            BygTraeFraModel();
        }

        private void AllerdeEksisterneData()
        {
            // Institutioner
            Institution kommune1 = new Institution();
            kommune1.Id = 1;
            kommune1.Navn = "Odense Kommune";
            kommune1.Type = InstitutionType.Offentlig;

            Institution kommune2 = new Institution();
            kommune2.Id = 2;
            kommune2.Navn = "Nyborg Kommune";
            kommune2.Type = InstitutionType.Offentlig;

            Institution privatInst1 = new Institution();
            privatInst1.Id = 3;
            privatInst1.Navn = "Skovbrynet";
            privatInst1.Type = InstitutionType.Privat;

            _institutioner.Add(kommune1);
            _institutioner.Add(kommune2);
            _institutioner.Add(privatInst1);
        }

        private Indtaegt OpretIndtaegt(Kunde kunde, DateTime dato, decimal timer, bool erFysisk, decimal kilometer)
        {
            decimal timepris = _timeprisService.HentTimepris(kunde, erFysisk);

            Indtaegt ind = new Indtaegt();
            ind.Id = _indtaegter.Count + 1;
            ind.Kunde = kunde;
            ind.Dato = dato;
            ind.Timer = timer;
            ind.ErFysisk = erFysisk;
            ind.Kilometer = kilometer;
            ind.Timepris = timepris;
            ind.Beloeb = timepris * timer;

            return ind;
        }

        public List<Institution> HentAlleInstitutioner()
        {
            return _institutioner;
        }

        public List<Kunde> HentAlleKunder()
        {
            return _kunder;
        }

        public Kunde TilfoejNyKunde(string navn, Institution institution)
        {
            Kunde kunde = new Kunde();
            kunde.Id = _kunder.Count + 1;
            kunde.Navn = navn;
            kunde.Institution = institution; // kan også være null for privat kunde

            _kunder.Add(kunde);
            return kunde;
        }

        private void BygTraeFraModel()
        {
            RootNodes.Clear();

            OverblikNode offentligeRoot = new OverblikNode("Offentlige institutioner");
            OverblikNode privateInstRoot = new OverblikNode("Private institutioner");
            OverblikNode privateKunderRoot = new OverblikNode("Private kunder");

            RootNodes.Add(offentligeRoot);
            RootNodes.Add(privateInstRoot);
            RootNodes.Add(privateKunderRoot);

            // Institutioner + deres kunder + indtægter
            foreach (Institution inst in _institutioner)
            {
                OverblikNode instNode = new OverblikNode(inst.Navn, inst);

                if (inst.Type == InstitutionType.Offentlig)
                {
                    offentligeRoot.Children.Add(instNode);
                }
                else
                {
                    privateInstRoot.Children.Add(instNode);
                }

                // kunder under denne institution
                foreach (Kunde kunde in _kunder)
                {
                    if (kunde.Institution != null && kunde.Institution.Id == inst.Id)
                    {
                        OverblikNode kundeNode = new OverblikNode(kunde.Navn, kunde);
                        instNode.Children.Add(kundeNode);

                        TilfoejIndtaegterTilKundeNode(kundeNode, kunde);
                    }
                }
            }

            // Private kunder (uden institution)
            foreach (Kunde kunde in _kunder)
            {
                if (kunde.Institution == null)
                {
                    OverblikNode kundeNode = new OverblikNode(kunde.Navn, kunde);
                    privateKunderRoot.Children.Add(kundeNode);

                    TilfoejIndtaegterTilKundeNode(kundeNode, kunde);
                }
            }
        }

        private void TilfoejIndtaegterTilKundeNode(OverblikNode kundeNode, Kunde kunde)
        {
            foreach (Indtaegt ind in _indtaegter)
            {
                if (ind.Kunde != null && ind.Kunde.Id == kunde.Id)
                {
                    string tekst = ind.Dato.ToShortDateString() + " | " +
                                   ind.Timer + " timer | " +
                                   ind.Beloeb + " kr";

                    if (ind.ErFysisk && ind.Kilometer > 0)
                    {
                        tekst = tekst + " | " + ind.Kilometer + " km";
                    }

                    OverblikNode indNode = new OverblikNode(tekst, ind);
                    kundeNode.Children.Add(indNode);
                }
            }
        }

        private void TilfoejIndtaegt()
        {
            TilfoejIndtaegtWindow window = new TilfoejIndtaegtWindow();

            TilfoejIndtaegtViewModel vm = new TilfoejIndtaegtViewModel(this, () => window.Close());
            window.DataContext = vm;

            window.ShowDialog();
        }

        public void TilfoejIndtaegtFraDialog(Kunde kunde, DateTime dato, decimal timer, bool erFysisk, decimal kilometer)
        {
            Indtaegt ind = OpretIndtaegt(kunde, dato, timer, erFysisk, kilometer);
            _indtaegter.Add(ind);
            BygTraeFraModel();
        }
    }
}
