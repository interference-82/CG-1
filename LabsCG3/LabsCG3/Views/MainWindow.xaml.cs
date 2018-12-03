using System.Windows;
using LabsCG3.ViewModels;

namespace LabsCG3.Views
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new PlotDrawingViewModel();
        }
    }
}
