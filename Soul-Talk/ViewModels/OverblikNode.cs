using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Soul_Talk.ViewModels
{
    public class OverblikNode : ViewModelBase
    {
        public string Title { get; }

        public ObservableCollection<OverblikNode> Children { get; } = new();

        public object? Data { get; }

        public OverblikNode(string title, object? data = null)
        {
            Title = title;
            Data = data;
        }
    }
}
