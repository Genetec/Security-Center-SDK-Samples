using System;
using System.Linq;
using ACECommon;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.CustomEvents;
using Genetec.Sdk.Events;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.TileView;
using Genetec.Sdk.Workspace.Pages.Contents;
namespace ACEClient.TileViews
{
    public sealed class ButtonTileViewModel : TileView
    {
        private readonly ButtonTileViewView m_View;     // View for this Model
        private Workspace m_workspace;
        private TilePluginContext m_context;            // Context information about this TileView

        public override System.Windows.UIElement View
        {
            get { return m_View; }
        }

        public ButtonTileViewModel (Workspace workspace)
        {
            m_workspace = workspace;
            m_View = new ButtonTileViewView(this);  // Pass the Model (this) to the View
            Placement = Placement.Extended;         // Takes the full tile
        }

        public override void Update(TilePluginContext context)
        {
            // Update information about the current context
            m_context = context;

            // Update the UI
            m_View.Update(context);
        }

        public void FireCustomEvent()
        {
            if (m_context == null)
                return;

            SystemConfiguration systemConfig = m_workspace.Sdk.GetEntity(SystemConfiguration.SystemConfigurationGuid) as SystemConfiguration;

            // Get the entity that this tile represents by using the TilePluginContext 
            Guid tileOwnerGuid = ((EntityContentGroup)m_context.TileState.Content).EntityId;
            Entity tileOwnerEntity = m_workspace.Sdk.GetEntity(tileOwnerGuid);

            /* The event will be one of the custom events from the "Custom Camera" custom entity. Use the 
             * event Name (shared between server and client via a "Common" DLL) to find the assigned ID of the custom event */
            var customEventToTrigger =
                systemConfig.CustomEventService.CustomEvents.FirstOrDefault(ce =>
                    ce.Name.Equals(CustomCamera.CustomEventNames.CustomCameraAlert)); // Look for the custom event definition with the desired name "CustomCameraAlert"

            if (customEventToTrigger == null)
            {
                // The desired Custom Event has not yet been registered with SystemConfiguration
                System.Windows.MessageBox.Show("The CustomEvent with Name " + CustomCamera.CustomEventNames.CustomCameraAlert
                                               + " has not yet been registered in System Configurations", "Raise CustomEvent");
                return;
            }

            // Create a new event with the custom entity as the source, 
            // then assign the ID of the event definition to the new event that we're about to raise
            var newEvent = m_workspace.Sdk.ActionManager.BuildEvent(EventType.CustomEvent, tileOwnerEntity.Guid) as CustomEventInstance;
            newEvent.Id = new CustomEventId(customEventToTrigger.Id);

            // Raise the event
            m_workspace.Sdk.ActionManager.RaiseEvent(newEvent);

            // A serparate test for regular Events
            var motionEvent =
                m_workspace.Sdk.ActionManager.BuildEvent(EventType.CameraMotionOn, tileOwnerEntity.Guid);
            m_workspace.Sdk.ActionManager.RaiseEvent(motionEvent);
        }
    }
}
