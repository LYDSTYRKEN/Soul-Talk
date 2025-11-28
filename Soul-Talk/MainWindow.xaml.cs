using Soul_Talk.ViewModels;
using System.Windows;

namespace Soul_Talk
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}