using System.Windows;
using System.Windows.Controls;
using Genetec.Sdk.Workspace.Components;


namespace ACEClient.TileViews
{
    /// <summary>
    /// Interaction logic for ButtonTileViewView.xaml
    /// </summary>
    public partial class ButtonTileViewView : UserControl
    {
        // Model for this View
        private ButtonTileViewModel m_model;

        public ButtonTileViewView(ButtonTileViewModel model)
        {
            InitializeComponent();
            m_model = model;
        }

        public void Update(TilePluginContext context)
        {
            // Perform updates to the UI here
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            m_model.FireCustomEvent();
        }
    }
}
