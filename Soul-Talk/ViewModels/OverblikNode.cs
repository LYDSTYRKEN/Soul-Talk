using System.Collections.ObjectModel;

namespace Soul_Talk.ViewModels
{
    // Repræsenterer én node i TreeView'et i MainWindow.
    // Vi bruger samme type til:
    //  - grupper (fx "Offentlige institutioner")
    //  - institutioner (fx "Odense Kommune")
    //  - kunder (fx "Kunde 1")
    //  - indtægter (fx "01-12-2024 - 2 t - 900 kr")
    public class OverblikNode : ViewModelBase
    {
        // Teksten der vises i TreeView'et (headeren på noden)
        public string Title { get; set; }

        // Underliggende noder (børn i træet)
        // Eksempel:
        //  - "Offentlige institutioner" har institutioner som børn
        //  - En institution har kunder som børn
        //  - En kunde har indtægter som børn
        public ObservableCollection<OverblikNode> Children { get; set; }

        // Reference til den "rigtige" data bag noden.
        // Kan fx være:
        //  - Institution
        //  - Kunde
        //  - Indtaegt
        //  - eller null for rene grupper
        public object Data { get; set; }

        // Constructor: kræver en titel, og Data er valgfri
        public OverblikNode(string title, object data = null)
        {
            Title = title;
            Data = data;
            Children = new ObservableCollection<OverblikNode>();
        }
    }
}
