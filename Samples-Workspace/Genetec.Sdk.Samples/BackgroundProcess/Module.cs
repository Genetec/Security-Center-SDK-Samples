// Copyright (C) 2022 by Genetec, Inc. All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.

namespace BackgroundProcess
{
    using Tasks;

    public sealed class BackgroundProcessModule : Genetec.Sdk.Workspace.Modules.Module
    {
        private BackgroundProcessPageTask m_task = new BackgroundProcessPageTask();

        public override void Load()
        {
            m_task.Initialize(Workspace);
            Workspace.Tasks.Register(m_task);
        }

        public override void Unload()
        {
            Workspace.Tasks.Unregister(m_task);
        }
    }
}
