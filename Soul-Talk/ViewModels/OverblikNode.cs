using System.Collections.ObjectModel;

namespace Soul_Talk.ViewModels
{
    public class OverblikNode : ViewModelBase
    {
        public string Title { get; set; }

        public ObservableCollection<OverblikNode> Children { get; set; }

        public object Data { get; set; }

        public OverblikNode(string title, object data = null)
        {
            Title = title;
            Data = data;
            Children = new ObservableCollection<OverblikNode>();
        }
    }
}
