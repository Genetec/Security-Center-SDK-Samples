using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Queries;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace UserPrivileges
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private ReadOnlyCollection<PrivilegeInformation> m_privileges;

        private Engine m_sdkEngine;

        #endregion

        #region Nested Classes and Structures

        /// <summary>
        /// This class is used to encapsulate a User or a Group so that we can Add them to a tree
        /// And so that we can preserve their Entity and Name information
        /// </summary>
        public class MyTreeViewItem : TreeViewItem
        {
            #region Properties

            public string CustomName { get; set; }

            public Entity Item { get; set; }

            #endregion

            #region Constructors

            public MyTreeViewItem(Entity entity, string name)
                            : base()
            {
                Item = entity;
                Header = name;
            }

            #endregion
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            m_logout.IsEnabled = false;
            m_sdkEngine = new Engine();

            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggingOff += OnEngineLoggingOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;

            m_users.SelectedItemChanged += OnSelectedItemChanged;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
        }

        #endregion

        #region Event Handlers

        // Logon button click.
        private void OnButtonLogonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                m_sdkEngine.LoginManager.BeginLogOn(m_serverName.Text, m_usernameInput.Text, m_passwordInput.Password);
            }
            catch (SdkException ex)
            {
                PostMessage("Unable to logon: " + ex.Message);
            }
        }

        // Logout button click.
        private void OnButtonLogoutClick(object sender, RoutedEventArgs e)
        {
            m_userPrivileges.Children.Clear();
            m_users.Items.Clear();
            m_messages.Items.Clear();

            m_sdkEngine.LoginManager.LogOff();
        }

        // Buttons to refresh user group of the tree.
        private void OnButtonRefreshUserGroupClick(object sender, RoutedEventArgs e)
        {
            MyTreeViewItem selectedItem = (MyTreeViewItem)m_users.SelectedItem;
            Entity entity = selectedItem.Item;
            m_userPrivileges.Children.Clear();
            if (entity != null)
            {
                UpdatePrivileges(entity);
            }
            else
            {
                m_users.Items.Clear();
            }
            PostMessage("Refreshed Successfully");
        }

        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("The identity of the Directory server cannot be verified. \n" +
                                                      "The certificate is not from a trusted certifying authority. \n" +
                                                      "Do you trust this server?", "Secure Communication", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                e.AcceptDirectory = true;
            }
        }

        // Logs off.
        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            PostMessage("Logged Off");
            m_login.IsEnabled = true;
            m_logout.IsEnabled = false;
        }

        // Logins the success.
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            RefreshUserAndGroups();
            PostMessage("Logon Success");

            m_login.IsEnabled = false;
            m_logout.IsEnabled = true;
        }

        // Logging off.
        private void OnEngineLoggingOff(object sender, LoggingOffEventArgs e)
        {
            PostMessage("Logging Off");
        }

        // Logon failure.
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            PostMessage(e.FormattedErrorMessage);
        }

        // Logon status change.
        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            PostMessage(string.Format("Logon Status Changed : {0}", e.Status));
        }

        /// <summary>
        /// Called when [selected item changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="object"/> instance containing the event data.</param>
        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MyTreeViewItem selectedItem = (MyTreeViewItem)e.NewValue;
            if (selectedItem != null)
            {
                m_userPrivileges.Children.Clear();
                Entity entity = selectedItem.Item as Entity;
                if (entity != null)
                {
                    UpdatePrivileges(entity);
                    m_refresh.IsEnabled = true;
                }
                else
                {
                    m_refresh.IsEnabled = false;
                    RefreshUserAndGroups();
                }
            }
        }

        void OnUserControlPrivilegeChangeRequested(object sender, PrivilegeChangeRequestEventArgs e)
        {
            //we retrieve the selection user or usergroup
            MyTreeViewItem selectedItem = (MyTreeViewItem)m_users.SelectedItem;
            User user = selectedItem.Item as User;
            UserGroup usergroup = selectedItem.Item as UserGroup;

            try
            {
                if (user != null)
                {
                    //set the user's privilege to the new one
                    user.SetPrivilegeState(e.PrivilegeId, e.Access, PrivilegeChangeBehavior.ApplyToChildrenPrivileges);
                }
                else if (usergroup != null)
                {
                    //set the usergroup's privilege
                    usergroup.SetPrivilegeState(e.PrivilegeId, e.Access, PrivilegeChangeBehavior.ApplyToChildrenPrivileges);
                }
                PostMessage("Saved Successfully");
            }
            catch (SdkException ex)
            {
                e.Cancel = true;
                PostMessage(ex.Message);
            }
            catch (ArgumentException ex)
            {
                e.Cancel = true;
                PostMessage(ex.Message);
            }

            ReadOnlyCollection<PrivilegeInformation> privs = null;
            if (user != null)
            {
                privs = user.Privileges;
            }
            if (usergroup != null)
            {
                privs = usergroup.Privileges;
            }
            if (privs != null)
            {
                foreach (var child in m_userPrivileges.Children)
                {
                    var userControlPrivilege = child as UserControlPrivilege;
                    if (userControlPrivilege == null)
                    {
                        continue;
                    }
                    PrivilegeInformation privilegeInformation = privs.First(x => x.PrivilegeGuid.Equals(userControlPrivilege.ControlGuid));
                    userControlPrivilege.ForceSetPrivilegeAccess(privilegeInformation.State);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Posts the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void PostMessage(string message)
        {
            m_messages.Items.Add(message);
            object lastItem = m_messages.Items[m_messages.Items.Count - 1];
            m_messages.ScrollIntoView(lastItem);
        }

        /// <summary>
        /// Refreshes the user and groups in the treeview.
        /// </summary>
        public void RefreshUserAndGroups()
        {
            m_users.Items.Clear();
            EntityConfigurationQuery query =
                m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            QueryCompletedEventArgs result = null;
            if (query != null)
            {
                query.EntityTypeFilter.Add(EntityType.UserGroup);
                query.EntityTypeFilter.Add(EntityType.User);
                result = query.Query();
            }
            if (result != null && result.Success)
            {
                //we create a root item for All the groups
                MyTreeViewItem root = new MyTreeViewItem(null, "All the Groups");
                m_users.Items.Add(root);
                root.ExpandSubtree();
                foreach (DataRow dr in result.Data.Rows)
                {
                    //we take each group, one by one
                    UserGroup group = m_sdkEngine.GetEntity((Guid)dr[0]) as UserGroup;
                    User user = m_sdkEngine.GetEntity((Guid)dr[0]) as User;

                    if (group != null)
                    {
                        //A wrapper of the entity
                        MyTreeViewItem parent = new MyTreeViewItem(group, group.Name);

                        root.Items.Add(parent);

                        //retrieve the users from the group
                        var groupusers = group.Children;
                        foreach (var guid in groupusers)
                        {
                            User myuser = m_sdkEngine.GetEntity(guid) as User;
                            if (myuser != null)
                            {
                                MyTreeViewItem treeItem = new MyTreeViewItem(myuser, myuser.Name);
                                parent.Items.Add(treeItem);
                            }
                        }
                    }

                    if (user != null && user.UserGroups.Count == 0)
                    {
                        MyTreeViewItem treeItem = new MyTreeViewItem(user, user.Name);
                        root.Items.Add(treeItem);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the privileges.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void UpdatePrivileges(Entity entity)
        {
            User user = entity as User;
            UserGroup usergroup = entity as UserGroup;

            if (usergroup != null)
            {
                m_privileges = usergroup.Privileges;
                foreach (PrivilegeInformation privilegeInformation in m_privileges)
                {
                    PrivilegeAccess pa = privilegeInformation.State;
                    AddPrivilege(pa, privilegeInformation.PrivilegeGuid, usergroup.Guid);
                }
            }
            if (user != null)
            {
                m_privileges = user.Privileges;
                foreach (PrivilegeInformation privilegeInformation in m_privileges)
                {
                    PrivilegeAccess pa = privilegeInformation.State;
                    AddPrivilege(pa, privilegeInformation.PrivilegeGuid, user.Guid);
                }
            }
        }

        #endregion

        #region Private Methods

        private void AddPrivilege(PrivilegeAccess privilege, Guid guid, Guid entityGuid)
        {
            string description = string.Empty;
            //to use GetPrivilegeDefinitions
            //copy/paste Genetec.Platform.Resources.Core.dll in the output folder
            foreach (PrivilegeDefinitionInformation pdi in m_sdkEngine.SecurityManager.GetPrivilegeDefinitions())
            {
                if (pdi.Id == guid)
                {
                    description = pdi.Description;
                }
            }

            var userControlPrivilege = new UserControlPrivilege(privilege, description, guid);
            userControlPrivilege.PrivilegeChangeRequested += OnUserControlPrivilegeChangeRequested;
            m_userPrivileges.Children.Add(userControlPrivilege);
        }

        // Do some cleanup before closing the windows.
        // De-register from events before disposing of the engine
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //de-register from events before logging off
            m_sdkEngine.LoginManager.LoggedOn -= OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff -= OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggingOff -= OnEngineLoggingOff;
            m_sdkEngine.LoginManager.LogonFailed -= OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged -= OnEngineLogonStatusChanged;

            //dispose of the engine
            m_sdkEngine.Dispose();
            m_sdkEngine = null;
        }

        #endregion
    }

    #endregion
}

