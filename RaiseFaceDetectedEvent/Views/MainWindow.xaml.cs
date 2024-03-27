using System.Windows;
using RaiseFaceDetectedEvent.ViewModels;

namespace RaiseFaceDetectedEvent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CustomEventMessageViewModel m_viewModel;

        public MainWindow()
        {
            InitializeComponent();
            m_viewModel = (CustomEventMessageViewModel)base.DataContext;
        }
    }
}
