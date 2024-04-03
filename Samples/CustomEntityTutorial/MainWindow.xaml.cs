// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Diagnostics;
using Genetec.Sdk.Diagnostics.Logging.Core;
using Genetec.Sdk.Entities;
using System;
using System.Security;
using System.Windows;

namespace CustomEntityTutorial
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Private Fields

        private readonly Logger m_logger;

        private readonly Engine m_sdkEngine = new Engine();

        #endregion Private Fields

        #region Public Properties

        public CustomEntityTypeDescriptor CETD { get; private set; }

        // Never store Entity types in  memory. Use the Engine to GetEntity() via Guids
        public Guid CustomEntityGuid { get; private set; }

        public SystemConfiguration SystemConfig { get; private set; }

        public Engine UserEngine { get; private set; }

        public Guid UserEntityGuid { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        static MainWindow()
        {
            ActivateLogging();
        }

        public MainWindow()
        {
            InitializeComponent();
            m_logger = Logger.CreateInstanceLogger(this);
            // Register to important events
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;

            // Logon to Sdk engine
            const string server = "";
            const string username = "admin";
            const string password = "";
            m_sdkEngine.LoginManager.LogOn(server, username, password);
        }

        #endregion Public Constructors

        #region Public Methods

        public static void ActivateLogging()
        {
            // In this template, the logger are activated via code.
            // An alternate way is to activate them in the LogTargets.gconfig file found in the Security Center installation folder
            // Logs can be found in the Logs subfolder of that directory.

            var logServer = DiagnosticServer.Instance;
            logServer.AddFileTracing(new[] { new LoggerTraces("Genetec.Sdk.Workspace*", LogSeverity.Full) });
            logServer.AddFileTracing(new[] { new LoggerTraces("WPFApplication2*", LogSeverity.Full) });
        }

        #endregion Public Methods

        #region Private Methods

        private void AddADD_Click(object sender, RoutedEventArgs e)
        {
            // Lets set make it so that you need an add alarms privilege to add (which our user has!)
            CETD.AddPrivilege = SdkPrivilege.AddAlarms;

            // Don't forget to update the custom entity type descriptor!
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void AddDELETE_Click(object sender, RoutedEventArgs e)
        {
            // Lets set make it so that you don't need a specific privilege to delete
            CETD.DeletePrivilege = SdkPrivilege.None;

            // Don't forget to update the custom entity type descriptor!
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void AddMODIFY_Click(object sender, RoutedEventArgs e)
        {
            // Lets make it so that you need a modify doors privilege to modify (which our user has!)
            CETD.ModifyPrivilege = SdkPrivilege.ModifyDoors;

            // Don't forget to update the custom entity type descriptor!
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void AddVIEW_Click(object sender, RoutedEventArgs e)
        {
            // Lets set make it so that you need a change tile pattern privilege to view (which our user has!)
            CETD.ViewPrivilege = SdkPrivilege.ChangeTilePattern;

            // Don't forget to update the custom entity type descriptor!
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void CETD_Click(object sender, RoutedEventArgs e)
        {
            // Create a custom entity type descriptor as admin.
            CETD = new CustomEntityTypeDescriptor(Guid.NewGuid(), DateTime.Now.ToShortTimeString(),
                CustomEntityTypeCapabilities.IsVisible, new Version(1, 1))
            {
                Capabilities = CustomEntityTypeCapabilities.CreateDelete | CustomEntityTypeCapabilities.IsVisible
            };

            // Lets add the ability to create and delete the custom entity from config tool.
            SystemConfig = m_sdkEngine.GetEntity<SystemConfiguration>(SystemConfiguration.SystemConfigurationGuid);

            // Whenever you change the custom entity type descriptor, you must explicitly update it. It does not get update automatically.
            SystemConfig.AddOrUpdateCustomEntityType(CETD);

            // You must specify which SDK privilege is needed for the custom entity to be able to add, modify, update or delete.
            // If no privilege is specified, the user must be admin.
            CETD.AddPrivilege = SdkPrivilege.AddAlarms;
            CETD.ModifyPrivilege = SdkPrivilege.ModifyDoors;
            CETD.ViewPrivilege = SdkPrivilege.ChangeTilePattern;
            CETD.DeletePrivilege = SdkPrivilege.None;

            // Whenever you change the custom entity type descriptor, you must explicitly update it. It does not get update automatically.
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void CREATE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check if CustomEntity already exists
                var customEntity = m_sdkEngine.GetEntity(CustomEntityGuid) as CustomEntity;
                if (customEntity == null)
                {
                    customEntity = UserEngine.CreateCustomEntity("TemporaryEntityName", CETD.Id);
                    if (customEntity == null || m_sdkEngine.GetEntity<CustomEntity>(customEntity.Guid) == null)
                        throw new ArgumentNullException("Couldn't add a custom entity.");

                    CustomEntityGuid = customEntity.Guid;
                    customEntity.Name = "CustomEntity-" + CustomEntityGuid;

                    Console.WriteLine(customEntity.Name);
                    m_logger.TraceDebug(customEntity.Name);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(customEntity.Name + @" already exists", @"Create Entity");
                }
            }
            catch (Exception ex)
            {
                if (ex is SecurityException || ex is SdkException)
                {
                    System.Windows.Forms.MessageBox.Show(@"You do not have the permission to create a custom entity", @"Create Entity");
                }
            }
        }

        private void DELETE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Use the SDK Engine to get the CustomEntity because we are not testing ViewPrivilege in this method
                var customEntity = m_sdkEngine.GetEntity(CustomEntityGuid) as CustomEntity;
                if (customEntity != null)
                {
                    // Delete using User Engine. An error gets thrown if the user does not have sufficient privileges
                    UserEngine.DeleteEntity(customEntity);
                }
                else
                    System.Windows.Forms.MessageBox.Show(@"Entity has already been deleted.", @"Delete Entity");
            }
            catch (SdkException ex)
            {
                System.Windows.Forms.MessageBox.Show(@"Error code: " + ex.ErrorCode, @"Delete Entity");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, @"Delete Entity");
            }
        }

        private void MODIFY_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lets try to change the running state of the custom entity.
                var capabilities = CETD.Capabilities;
                CETD.Capabilities = CustomEntityTypeCapabilities.HasRunningState | capabilities;

                // Don't forget to update the custom entity type descriptor!
                SystemConfig.AddOrUpdateCustomEntityType(CETD);

                // Change the running state
                var customEntity = UserEngine.GetEntity(CustomEntityGuid) as CustomEntity;
                if (customEntity == null)
                    throw new NullReferenceException("The custom entity does not exist");

                customEntity.RunningState = State.NotRunning;

                if (customEntity.RunningState != State.NotRunning)
                    throw new SdkException(SdkError.CannotSetProperty, "Running state was not changed");

                customEntity.RunningState = State.Running;
            }
            catch (SdkException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message + @" - You do not have the permission to modify the custom entity.", @"Modify Entity");
            }
            catch (NullReferenceException ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    ex.Message, @"Modify Entity");
            }
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Console.WriteLine(@"Sdk has logged off");
            m_logger.TraceDebug("Sdk has logged off");

            m_sdkEngine.DeleteEntity(CustomEntityGuid);
            m_sdkEngine.DeleteEntity(UserEntityGuid);
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Console.WriteLine(e.UserName + @" has logged on to " + e.ServerName);
            m_logger.TraceDebug(e.UserName + " has logged on to " + e.ServerName);
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            MessageBox.Show(e.FormattedErrorMessage);
            m_logger.TraceDebug(e.FormattedErrorMessage);
        }

        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            Console.WriteLine(@"Server : " + e.ServerName + @". Status changed to " + e.Status);
            m_logger.TraceDebug("Server : " + e.ServerName + ". Status changed to " + e.Status);
        }
        private void RemoveADD_Click(object sender, RoutedEventArgs e)
        {
            // Lets set make it so that only an admin can add
            CETD.AddPrivilege = SdkPrivilege.AdminOnly;

            // Don't forget to update the custom entity type descriptor!
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void RemoveDELETE_Click(object sender, RoutedEventArgs e)
        {
            // Lets set make it so that only an admin can delete
            CETD.DeletePrivilege = SdkPrivilege.AdminOnly;

            // Don't forget to update the custom entity type descriptor!
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void RemoveMODIFY_Click(object sender, RoutedEventArgs e)
        {
            // Lets set make it so that only an admin can modify
            CETD.ModifyPrivilege = SdkPrivilege.AdminOnly;

            // Don't forget to update the custom entity type descriptor!
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void RemoveVIEW_Click(object sender, RoutedEventArgs e)
        {
            // Lets set make it so that only an admin can view
            CETD.ViewPrivilege = SdkPrivilege.AdminOnly;

            // Don't forget to update the custom entity type descriptor!
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        private void USER_Click(object sender, RoutedEventArgs e)
        {
            // Create a user that should be able to add, modify and view.
            UserEntityGuid = (m_sdkEngine.CreateEntity("TemporaryUserName", EntityType.User) as User).Guid;

            var userEntity = m_sdkEngine.GetEntity(UserEntityGuid) as User;
            userEntity.Name = "UserEntity-" + UserEntityGuid;

            if (UserEntityGuid == null)
                throw new ArgumentNullException("Unable to create user.");

            // Grant the user access to logon through the SDK.
            userEntity.SetPrivilegeState(SdkPrivilege.SdkLogon, PrivilegeAccess.Granted, PrivilegeChangeBehavior.Default);

            // Give the user the required privileges to add, delete, modify and update custom entities.
            userEntity.SetPrivilegeState(SdkPrivilege.AddAlarms, PrivilegeAccess.Granted, PrivilegeChangeBehavior.Default);
            userEntity.SetPrivilegeState(SdkPrivilege.ModifyDoors, PrivilegeAccess.Granted, PrivilegeChangeBehavior.Default);
            userEntity.SetPrivilegeState(SdkPrivilege.ChangeTilePattern, PrivilegeAccess.Granted, PrivilegeChangeBehavior.Default);

            // Add the user to the default partition.
            var defaultPartition = m_sdkEngine.GetEntity<Partition>(Partition.DefaultPartitionGuid);
            defaultPartition.AddUserAccess(UserEntityGuid);

            // Create user engine
            UserEngine = new Engine();

            // Register to important events
            UserEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            UserEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            UserEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            UserEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;

            // Logon as a non-admin user
            var server = string.Empty;
            var username = userEntity.Name;
            var password = string.Empty;
            UserEngine.LoginManager.LogOn(server, username, password);
        }

        private void VIEW_Click(object sender, RoutedEventArgs e)
        {
            // If we are able to get the custom entity, then we are able to view it.
            try
            {
                var getCustomEntity = UserEngine.GetEntity(CustomEntityGuid) as CustomEntity;
                if (getCustomEntity != null)
                {
                    System.Windows.Forms.MessageBox.Show(@"Custom Entity Guid: " + getCustomEntity.Guid +
                                                         Environment.NewLine +
                                                         @"Type: " + getCustomEntity.TypeDescriptor.Name,
                                                         @"View Entity");
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(@"No CustomEntity exists or you do not have permission", @"View Entity");
                }
            }
            catch (System.Security.SecurityException)
            {
                System.Windows.Forms.MessageBox.Show(@"You do not have the permission to get the custom entity", @"View Entity");
            }
        }

        #endregion Private Methods

    }

}