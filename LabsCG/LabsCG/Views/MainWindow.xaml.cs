namespace LabsCG.Views
{
    using System.Windows;
    using LabsCG.ViewModels;

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