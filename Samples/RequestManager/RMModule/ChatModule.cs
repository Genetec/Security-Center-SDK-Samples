// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Tasks;
using RMModule.Components;
using RMModule.Notifications;
using RMModule.Options;
using RMModule.Pages;
using RMModule.Services;
using System;
using System.Collections.Generic;

namespace RMModule
{

    /// <summary>
    /// Main entry point for the module. Creates the workspace module components and registers/unregisters them.
    /// </summary>
    public sealed class ChatModule : Module
    {

        #region Public Fields

        public static readonly Guid CustomCategoryId = new Guid(TaskCategories.Maintenance);

        #endregion Public Fields

        #region Private Fields

        private readonly List<Task> m_tasks = new List<Task>();

        private ChatNotifContentBuilder m_chatNotificationContentBuilder;

        private ChatOptionsExtension m_chatOptionsExtension;
        private ChatService m_chatService;
        private ChatTimelineProviderBuilder m_chatTimelineProviderBuilder;
        private ChatTray m_chatTray;

        #endregion Private Fields

        #region Public Constructors

        static ChatModule()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            if (Workspace.ApplicationType != ApplicationType.None)
            {
                RegisterTaskExtensions();
            }
            m_chatService = new ChatService();
            m_chatService.Initialize(Workspace);
            Workspace.Services.Register(m_chatService);

            m_chatTray = new ChatTray();
            m_chatTray.Initialize(Workspace);

           Workspace.DefaultMonitor?.Notifications?.Add(m_chatTray);
           
            RegisterOptionsExtension();
            RegisterComponents();
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (Workspace != null)
            {
                if (m_chatTray != null)
                {
                    Workspace.DefaultMonitor.Notifications.Remove(m_chatTray);
                    m_chatTray.Dispose();
                    m_chatTray = null;
                }

                if (m_chatService != null)
                {
                    Workspace.Services.Unregister(m_chatService);
                    m_chatService = null;
                }

                UnregisterComponents();
                UnregisterTaskExtensions();
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RegisterComponents()
        {
            m_chatTimelineProviderBuilder = new ChatTimelineProviderBuilder();
            m_chatTimelineProviderBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_chatTimelineProviderBuilder);

            m_chatNotificationContentBuilder = new ChatNotifContentBuilder();
            m_chatNotificationContentBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_chatNotificationContentBuilder);
        }

        private void RegisterOptionsExtension()
        {
            m_chatOptionsExtension = new ChatOptionsExtension();
            m_chatOptionsExtension.Initialize(Workspace);
            Workspace.Options.Register(m_chatOptionsExtension);
        }

        private void RegisterTaskExtensions()
        {
            Task extension = new CreatePageTask<ChatPage>(true);
            extension.Initialize(Workspace);
            m_tasks.Add(extension);

            // Register them to the workspace
            foreach (var task in m_tasks)
            {
                Workspace.Tasks.Register(task);
            }
        }

        private void UnregisterComponents()
        {
            if (m_chatTimelineProviderBuilder != null)
            {
                Workspace.Components.Unregister(m_chatTimelineProviderBuilder);
                m_chatTimelineProviderBuilder = null;
            }

            if (m_chatNotificationContentBuilder != null)
            {
                Workspace.Components.Unregister(m_chatNotificationContentBuilder);
                m_chatNotificationContentBuilder = null;
            }
        }

        private void UnregisterTaskExtensions()
        {
            // Register them to the workspace
            foreach (var task in m_tasks)
            {
                Workspace.Tasks.Unregister(task);
            }

            m_tasks.Clear();
        }

        #endregion Private Methods

    }

}