
using ACEClient.Content;
using ACEClient.Map;

using Module = Genetec.Sdk.Workspace.Modules.Module;

using ACEClient.Pages.Configuration;
using ACEClient.TileViews;
using Genetec.Sdk.Workspace.Services;

namespace ACEClient
{
    public sealed class Client : Module
    {
        // Custom MapObject for the custom entity
        private CustomCameraMapViewBuilder m_customMapViewBuilder;

        // Custom Config Page for the custom entity
        private CustomCameraConfigPage m_customCameraConfigPage;
        
        // Custom TileView to display in Monitoring
        private ButtonTileViewBuilder m_buttonTileView;

        // Custom Content to display in Monitoring
        private CustomCameraContentBuilder m_contentBuilder; 

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            if (Workspace == null)
                return;
           
            IConfigurationService configService = Workspace.Services.Get<IConfigurationService>();
            if (configService != null)
            {
                m_customCameraConfigPage = new CustomCameraConfigPage();
                m_customCameraConfigPage.Initialize(Workspace);

                configService.Register(m_customCameraConfigPage);
            }

            m_customMapViewBuilder = new CustomCameraMapViewBuilder();
            m_customMapViewBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_customMapViewBuilder);

            m_buttonTileView = new ButtonTileViewBuilder();
            m_buttonTileView.Initialize(Workspace);
            Workspace.Components.Register(m_buttonTileView);

            m_contentBuilder = new CustomCameraContentBuilder();
            m_contentBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_contentBuilder);
        }

        public override void Unload()
        {
            if (Workspace == null)
                return;

            IConfigurationService service = Workspace.Services.Get<IConfigurationService>();
            if (service != null)
            {
                service.Unregister(m_customCameraConfigPage);
            }

            if (m_customMapViewBuilder != null)
            {
                Workspace.Components.Unregister(m_customMapViewBuilder);
                m_customMapViewBuilder = null;
            }

            if (m_buttonTileView != null)
            {
                Workspace.Components.Unregister(m_buttonTileView);
                m_buttonTileView = null;
            }

            if (m_contentBuilder != null)
            {
                Workspace.Components.Unregister(m_contentBuilder);
                m_contentBuilder = null;
            }
        }
    }
}
