// ==========================================================================
// Copyright (C) 2021 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================


using CardholderFieldsExtractor.Extractors;

namespace CardholderFieldsExtractor
{
    
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {
        #region Private Fields

        private GenetecCardholderFieldsExtractor m_cardholderFieldsExtractor;

        #endregion Private Fields

        #region Public Methods
        
        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_cardholderFieldsExtractor = new GenetecCardholderFieldsExtractor();
            m_cardholderFieldsExtractor.Initialize(Workspace);
            Workspace.Components.Register(m_cardholderFieldsExtractor);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_cardholderFieldsExtractor == null) return;
            
            Workspace.Components.Unregister(m_cardholderFieldsExtractor);
            m_cardholderFieldsExtractor = null;
        }
        
        #endregion Public Methods
    }
}