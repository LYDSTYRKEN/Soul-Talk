using System.ComponentModel;

namespace Soul_Talk.ViewModels
{
    // Fælles base-klasse til ViewModels
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Kaldes manuelt fra hver property
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
