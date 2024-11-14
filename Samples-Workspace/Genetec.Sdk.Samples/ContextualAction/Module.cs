using Genetec.Sdk.Workspace.Services;

namespace ContextualActionSample
{
    public class Module : Genetec.Sdk.Workspace.Modules.Module
    {
        private IContextualActionsService m_contextualActionsService;
        private PopupMessageContextualAction m_popupMessageContextualAction;

        public override void Load()
        {
            m_contextualActionsService = Workspace.Services.Get<IContextualActionsService>();
            if (m_contextualActionsService != null)
            {
                m_popupMessageContextualAction = new PopupMessageContextualAction();
                m_popupMessageContextualAction.Initialize(Workspace);
                m_contextualActionsService.Register(m_popupMessageContextualAction);
            }
        }

        public override void Unload()
        {
            m_contextualActionsService?.Unregister(m_popupMessageContextualAction);
        }
    }
}
