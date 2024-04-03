using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Tasks;
using ModuleBlank.Tasks;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace ModuleBlank
{
    #region Classes

    public sealed class MyModule : Module
    {
        #region Fields

        private CreatePageTask<MyTask> m_extension;

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the module and registers its extensions in the application's workspace
        /// </summary>
        public override void Load()
        {
            // The purpose of the extension is to represent the task's descriptor. It validate privileges related to the task,
            // define what needs to be executed when the user creates the task, etc.
            m_extension = new CreatePageTask<MyTask>();
            m_extension.Initialize(Workspace);

            Workspace.Tasks.Register(m_extension);
        }

        /// <summary>
        /// Unloads the module and unregisters its extensions from the application's workspace
        /// </summary>
        public override void Unload()
        {
            Workspace.Tasks.Unregister(m_extension);
        }

        #endregion
    }

    #endregion
}

