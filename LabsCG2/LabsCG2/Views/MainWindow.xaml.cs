using System.Windows;
using LabsCG2.ViewModels;

namespace LabsCG2.Views
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