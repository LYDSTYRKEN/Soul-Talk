using Soul_Talk.Models;
using Soul_Talk.Models.Repositories;
using Soul_Talk.Models.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Soul_Talk.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // Model-data i hukommelsen
        private readonly List<Institution> _institutioner = new();
        private readonly List<Kunde> _kunder = new();
        private readonly List<Indtaegt> _indtaegter = new();

        private readonly ITimeprisService _timeprisService = new TimeprisService();

        // TreeView data
        public ObservableCollection<OverblikNode> RootNodes { get; } = new();

        public ICommand TilfoejIndtaegtCommand { get; }

        public MainViewModel()
        {
            TilfoejIndtaegtCommand = new RelayCommand(TilfoejIndtaegt);

            SeedDemoData();      // opretter nogle få institutioner/kunder/indtægter
            BygTraeFraModel();   // bygger OverblikNode-træet ud fra model-data
        }

        // ----------------------------------------------------
        // 1) Demo-data i hukommelsen (MEGET simpelt)
        // ----------------------------------------------------
        private void SeedDemoData()
        {
            // Institutioner
            var kommune1 = new Institution { Id = 1, Navn = "Kommune Navn 1", Type = InstitutionType.Offentlig };
            var kommune2 = new Institution { Id = 2, Navn = "Kommune Navn 2", Type = InstitutionType.Offentlig };

            var privatInst1 = new Institution { Id = 3, Navn = "Privat institutions navn 1", Type = InstitutionType.Privat };
            var privatInst2 = new Institution { Id = 4, Navn = "Privat institutions navn 2", Type = InstitutionType.Privat };

            _institutioner.AddRange(new[] { kommune1, kommune2, privatInst1, privatInst2 });

            // Kunder tilknyttet institutioner
            var offKunde1 = new Kunde { Id = 1, Navn = "Offentlig tilknyttet klient 1", Institution = kommune1 };
            var offKunde2 = new Kunde { Id = 2, Navn = "Offentlig tilknyttet klient 2", Institution = kommune1 };
            var offKunde3 = new Kunde { Id = 3, Navn = "Offentlig tilknyttet klient 1", Institution = kommune2 };

            var privKunde1 = new Kunde { Id = 4, Navn = "Privat tilknyttet klient 1", Institution = privatInst1 };
            var privKunde2 = new Kunde { Id = 5, Navn = "Privat tilknyttet klient 2", Institution = privatInst1 };
            var privKunde3 = new Kunde { Id = 6, Navn = "Privat tilknyttet klient 3", Institution = privatInst2 };

            // Private kunder (ingen institution)
            var privatDirekte1 = new Kunde { Id = 7, Navn = "Kunde 1", Institution = null };
            var privatDirekte2 = new Kunde { Id = 8, Navn = "Kunde 2", Institution = null };
            var privatDirekte3 = new Kunde { Id = 9, Navn = "Kunde 3", Institution = null };

            _kunder.AddRange(new[]
            {
                offKunde1, offKunde2, offKunde3,
                privKunde1, privKunde2, privKunde3,
                privatDirekte1, privatDirekte2, privatDirekte3
            });

            // Indtægter (meget simple – bare for at have noget at vise)
            _indtaegter.Add(OpretIndtaegt(offKunde1, DateTime.Today, 2m, true, 10m));
            _indtaegter.Add(OpretIndtaegt(offKunde2, DateTime.Today.AddDays(-1), 1.5m, false, 0m));
            _indtaegter.Add(OpretIndtaegt(privKunde1, DateTime.Today, 3m, true, 25m));
            _indtaegter.Add(OpretIndtaegt(privatDirekte1, DateTime.Today, 1m, true, 5m));
        }

        private Indtaegt OpretIndtaegt(Kunde kunde, DateTime dato, decimal timer, bool erFysisk, decimal kilometer)
        {
            var timepris = _timeprisService.HentTimepris(kunde, erFysisk);

            var ind = new Indtaegt
            {
                Id = _indtaegter.Count + 1,
                Kunde = kunde,
                Dato = dato,
                Timer = timer,
                ErFysisk = erFysisk,
                Kilometer = kilometer,
                Timepris = timepris,
                Beloeb = (timepris * timer)
            };

            return ind;
        }

        public IReadOnlyCollection<Institution> HentAlleInstitutioner()
        {
            return _institutioner.AsReadOnly();
        }

        internal Kunde TilfoejNyKunde(string navn, Institution? institution)
        {
            var kunde = new Kunde
            {
                Id = _kunder.Count + 1,
                Navn = navn,
                Institution = institution
            };

            _kunder.Add(kunde);
            return kunde;
        }

        // ----------------------------------------------------
        // 2) Byg OverblikNode-træet ud fra model-listerne
        // ----------------------------------------------------
        private void BygTraeFraModel()
        {
            RootNodes.Clear();

            var offentligeRoot = new OverblikNode("Offentlige institutioner");
            var privateInstRoot = new OverblikNode("Private institutioner");
            var privateKunderRoot = new OverblikNode("Private kunder");

            RootNodes.Add(offentligeRoot);
            RootNodes.Add(privateInstRoot);
            RootNodes.Add(privateKunderRoot);

            // Institutioner + deres kunder + indtægter
            foreach (var inst in _institutioner)
            {
                var instNode = new OverblikNode(inst.Navn, inst);

                if (inst.Type == InstitutionType.Offentlig)
                    offentligeRoot.Children.Add(instNode);
                else
                    privateInstRoot.Children.Add(instNode);

                foreach (var kunde in _kunder.Where(k => k.Institution?.Id == inst.Id))
                {
                    var kundeNode = new OverblikNode(kunde.Navn, kunde);
                    instNode.Children.Add(kundeNode);

                    TilføjIndtaegterTilKundeNode(kundeNode, kunde);
                }
            }

            // Private kunder (uden institution)
            foreach (var kunde in _kunder.Where(k => k.Institution == null))
            {
                var kundeNode = new OverblikNode(kunde.Navn, kunde);
                privateKunderRoot.Children.Add(kundeNode);

                TilføjIndtaegterTilKundeNode(kundeNode, kunde);
            }
        }

        private void TilføjIndtaegterTilKundeNode(OverblikNode kundeNode, Kunde kunde)
        {
            foreach (var ind in _indtaegter.Where(i => i.Kunde.Id == kunde.Id))
            {
                string tekst = $"{ind.Dato:d} - {ind.Timer} t - {ind.Beloeb} kr";

                if (ind.ErFysisk && ind.Kilometer > 0)
                    tekst += $" - {ind.Kilometer} km";

                var indNode = new OverblikNode(tekst, ind);
                kundeNode.Children.Add(indNode);
            }
        }

        // ----------------------------------------------------
        // 3) Command (gør ikke noget avanceret endnu)
        // ----------------------------------------------------
        private void TilfoejIndtaegt()
        {
            var window = new TilfoejIndtaegtWindow();

            var vm = new TilfoejIndtaegtViewModel(this, () => window.Close());
            window.DataContext = vm;

            window.ShowDialog();
        }
        public IReadOnlyCollection<Kunde> HentAlleKunder()
        {
            return _kunder.AsReadOnly();
        }

        internal void TilfoejIndtaegtFraDialog(Kunde kunde, DateTime dato, decimal timer, bool erFysisk, decimal kilometer)
        {
            var ind = OpretIndtaegt(kunde, dato, timer, erFysisk, kilometer);
            _indtaegter.Add(ind);
            BygTraeFraModel();
        }
    }
}
