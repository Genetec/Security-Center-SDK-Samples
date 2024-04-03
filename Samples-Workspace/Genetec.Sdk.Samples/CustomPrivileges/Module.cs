using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;

namespace CustomPrivileges
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {
        private RegisterCustomPrivileges m_registerCustomPrivileges;

        public override void Load()
        {
            m_registerCustomPrivileges = new RegisterCustomPrivileges();
            m_registerCustomPrivileges.Initialize(Workspace);
        }

        public override void Unload()
        {
            if(m_registerCustomPrivileges != null)
            {
                m_registerCustomPrivileges.UnregisterAll();
                m_registerCustomPrivileges.Dispose();
                m_registerCustomPrivileges = null;
            }
        }
    }
}
