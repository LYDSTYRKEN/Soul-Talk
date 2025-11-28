using Soul_Talk.Models;
using Soul_Talk.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Soul_Talk.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<OverblikNode> RootNodes { get; } = new();

        public ICommand TilfoejIndtaegtCommand { get; }

        public MainViewModel()
        {
            TilfoejIndtaegtCommand = new RelayCommand(_ => TilfoejIndtaegt());

            BygDemoTrae();
        }

        private void BygDemoTrae()
        {
            RootNodes.Clear();

            // Topnoder
            var offentligeRoot = new OverblikNode("Offentlige institutioner");
            var privateInstRoot = new OverblikNode("Private institutioner");
            var privateKunderRoot = new OverblikNode("Private kunder");

            RootNodes.Add(offentligeRoot);
            RootNodes.Add(privateInstRoot);
            RootNodes.Add(privateKunderRoot);

            // Offentlige institutioner
            var kommune1 = new OverblikNode("Kommune Navn 1");
            kommune1.Children.Add(new OverblikNode("Offentlig tilknyttet klient 1"));
            kommune1.Children.Add(new OverblikNode("Offentlig tilknyttet klient 2"));

            var kommune2 = new OverblikNode("Kommune Navn 2");
            kommune2.Children.Add(new OverblikNode("Offentlig tilknyttet klient 1"));

            offentligeRoot.Children.Add(kommune1);
            offentligeRoot.Children.Add(kommune2);

            // Private institutioner
            var privatInst1 = new OverblikNode("Privat institutions navn 1");
            privatInst1.Children.Add(new OverblikNode("Privat tilknyttet klient 1"));
            privatInst1.Children.Add(new OverblikNode("Privat tilknyttet klient 2"));

            var privatInst2 = new OverblikNode("Privat institutions navn 2");
            privatInst2.Children.Add(new OverblikNode("Privat tilknyttet klient 3"));

            privateInstRoot.Children.Add(privatInst1);
            privateInstRoot.Children.Add(privatInst2);

            // Private kunder
            privateKunderRoot.Children.Add(new OverblikNode("Kunde 1"));
            privateKunderRoot.Children.Add(new OverblikNode("Kunde 2"));
            privateKunderRoot.Children.Add(new OverblikNode("Kunde 3"));
        }

        private void TilfoejIndtaegt()
        {
            // Her kommer senere:
            //  - åbne "Tilføj indtægt"-vindue
            //  - opdatere træet bagefter.
        }
    }
}