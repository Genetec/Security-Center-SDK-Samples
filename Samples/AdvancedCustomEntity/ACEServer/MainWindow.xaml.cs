using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Media.Imaging;
using Genetec.Sdk;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Entities;
using ACECommon;
using Genetec.Sdk.Entities.CustomEvents;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 20:
//  622 – Muhammad and Abu Bakr arrived in Medina
//  1187 – Saladin begins the Siege of Jerusalem.
//  1961 – Greek general Konstantinos Dovas becomes Prime Minister of Greece.
// ==========================================================================
namespace ACEServer
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow :IDisposable
    {
        #region Constants

        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Properties

        public CustomEntityTypeDescriptor CETD { get; private set; }

        public SystemConfiguration SystemConfig { get; private set; }

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            // Register to important events
            m_sdkEngine.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.EntitiesAdded += OnEngine_EntitiesAdded;

            Closing += MainWindow_Closing;

            // Display CustomEntity data to the user
            m_nameTextBox.Text = CustomCamera.Name;
            m_guidTextBox.Text = CustomCamera.TypeGuid.ToString();
        }
      

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_sdkEngine.LogOff();
        }

        #endregion

        #region Event Handlers
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            SystemConfig = m_sdkEngine.GetEntity<SystemConfiguration>(SystemConfiguration.SystemConfigurationGuid);

            // Find all the entities this server is responsible for, set their RunningState to Running
            // This simulates the idea that the ACEServer is servicing the Custom Cameras whenever the server is on.
            SetCustomCameraRunningStates(State.Running);
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            System.Windows.MessageBox.Show(e.FormattedErrorMessage);
        }

        /// <summary>
        /// Things to do whenever a new entity is added by the Engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEngine_EntitiesAdded(object sender, EntitiesAddedEventArgs e)
        {
            // Set RunningState = Running on all custom entities that this server is responsible for. (i.e. all the Custom Camera entities)

            // Filter new entities to retrieve only the Custom Camera entities
            IEnumerable<EntityUpdateInfo> allNewCustomCams = e.Entities.Where(
                newEntityInfo =>
                newEntityInfo.EntityType == EntityType.CustomEntity &&  // Filter by CustomEntity, ensures that the following cast will work                                                   
                ((CustomEntity)m_sdkEngine.GetEntity(newEntityInfo.EntityGuid)).CustomEntityType == CustomCamera.TypeGuid);    // Filter by Custom Camera custom entity

            foreach (EntityUpdateInfo customCamInfo in allNewCustomCams)
            {
                // Set the RunningState
                m_sdkEngine.GetEntity(customCamInfo.EntityGuid).RunningState = State.Running;
            }
        }

        /// <summary>
        /// Remove the CETD from the server. Will not work if there still exists any CustomEntity that was made using the CETD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRemoveCetdClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!m_sdkEngine.IsConnected)
                return;

            // Delete the CustomEntityTypeDescriptor
            try
            {
                SystemConfig.RemoveCustomEntityType(CustomCamera.TypeGuid);
            }
            catch (SdkException ex)
            {
                System.Windows.MessageBox.Show(ex.ErrorCode +" - " +ex.Message,"Remove CETD");
            }
        }

        private void OnCreateCetdClick(object sender, System.Windows.RoutedEventArgs e)
        {
            CreateCustomEntityTypeDescriptor();
        }
       

        private void OnLoginClick(object sender, System.Windows.RoutedEventArgs e)
        {
            LoginAsAdmin();
        }

        private void OnRemoveEntitiesClick(object sender, System.Windows.RoutedEventArgs e)
        {
            RemoveAllCustomEntities(CustomCamera.TypeGuid);
        }

        #endregion

        /// <summary>
        /// Creates the "CustomCamera" CustomEntityTypeDescriptor and adds it to the server. 
        /// </summary>
        private void CreateCustomEntityTypeDescriptor()
        {
            // Check if SDK Engine is connected
            if (!m_sdkEngine.IsConnected)
                return;

            // Create the custom entity type descriptor. If it already exists the following code will modify the existing
            CETD = new CustomEntityTypeDescriptor(CustomCamera.TypeGuid, CustomCamera.Name, CustomCamera.Capabilities, new Version(1, 1));
            
            // Use a camera's icon for this custom entity (optional)
            CETD.SmallIcon = new BitmapImage(new Uri("pack://application:,,,/ACEServer;component/Resources/CameraIcon.png"));
            CETD.LargeIcon = new BitmapImage(new Uri("pack://application:,,,/ACEServer;component/Resources/CameraIcon.png"));

            // You must specify which SDK privilege is needed by the user for the custom entity to be able to add, modify, update or delete.
            // If no privilege is specified, the user must be admin.
            CETD.AddPrivilege = new SdkPrivilege(CustomCamera.AddPrivilege);
            CETD.ModifyPrivilege = new SdkPrivilege(CustomCamera.ModifyPrivilege);
            CETD.ViewPrivilege = new SdkPrivilege(CustomCamera.ViewPrivilege);
            CETD.DeletePrivilege = new SdkPrivilege(CustomCamera.RemovePrivilege);

            // Set the list of entities that can appear under this custom type
            CETD.HierarchicalChildTypes = new List<EntityType> { EntityType.Camera };            

            
            ICustomEventBuilder builder = SystemConfig.CustomEventService.CreateCustomEventBuilder();

            // Read the custom event names of CustomCamera, then create and register the CustomEvents   
            List<int> customEventsToSupport = new List<int>();
            CustomCamera.CustomEventNames eventNames = new CustomCamera.CustomEventNames();                                          
            foreach (var field in typeof(CustomCamera.CustomEventNames).GetFields(BindingFlags.Public|BindingFlags.Static))
            {
                string newEventName = (string) field.GetValue(eventNames);

                // Check to make sure the event doesn't already exist
                bool eventExists = SystemConfig.CustomEventService.CustomEvents.Any(existingEvent => existingEvent.Name == newEventName);
                if(eventExists)
                    continue;

                // Register the new custom event on the server, add it to the list of events that the CETD must support
                builder.SetName(newEventName);
                builder.SetEntityType(EntityType.CustomEntity);
                CustomEvent newCustomEvent = builder.Build();
                SystemConfig.CustomEventService.Add(newCustomEvent);
                customEventsToSupport.Add(newCustomEvent.Id);
            }

            // Register supported events
            CETD.SupportedCustomEvents = customEventsToSupport;
            CETD.SupportedEvents = new List<EventType> { EventType.CameraMotionOn, EventType.CameraMotionOff };

            // Whenever you create/change the custom entity type descriptor, you must explicitly update it. It does not update automatically.
            SystemConfig.AddOrUpdateCustomEntityType(CETD);
        }

        /// <summary>
        /// Log in using default administrator credentials
        /// </summary>
        private void LoginAsAdmin()
        {
            if (m_sdkEngine.IsConnected)
            {
                System.Windows.MessageBox.Show("Already logged in as " + m_sdkEngine.LoggedUser.Name, "Login");
                return;
            }
            // Logon to Sdk engine
            var server = "";
            var username = "admin";
            var password = "";
            m_sdkEngine.LogOn(server, username, password);
        }


        /// <summary>
        /// Query the server (Directory) for all entities of a specified type. This is different from Engine.GetEntities() because
        /// the Engine only gets entities stored on the local cache.
        /// </summary>
        /// <param name="searchType">The entity type to query for</param>
        /// <param name="customEntityType">Custom Entity Type Descriptor ID if looking for a Custom Entity. leave empty otherwise.</param>
        /// <returns>List of guids of entities of the specified custom type</returns>
        private List<Guid> FetchEntities(EntityType searchType, Guid? customEntityType)
        {
            List<Guid> entityGuids = new List<Guid>();

            // Set up the query
            var query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            query.EntityTypeFilter.Add(searchType);
            if (customEntityType.HasValue)
                query.CustomEntityTypes.Add(customEntityType.Value);

            // Start query
            QueryCompletedEventArgs result = query.Query();

            if (result.Success)
            {
                Console.WriteLine("Found {0} entities\r\n", result.Data.Rows.Count);
                foreach (DataRow dr in result.Data.Rows)
                {
                    // Guids are stored in the first index of the row
                    Console.WriteLine("\t{0}\r\n", dr[0]);
                    entityGuids.Add(Guid.Parse(dr[0].ToString()));
                }
            }
            else
            {
                Console.WriteLine ("The query has failed");
            }

            return entityGuids;
        }

        /// <summary>
        /// Removes all custom entities of a specified type
        /// </summary>
        /// <param name="customEntityType">The GUID of your CustomEntityTypeDescriptor</param>
        private void RemoveAllCustomEntities(Guid customEntityType)
        {
            if (!m_sdkEngine.IsConnected)
                return;

            // Get all Custom Entities
            List<Guid> customEntityGuids = FetchEntities(EntityType.CustomEntity, customEntityType);

            // Only remove Custom Camera entities
            foreach (Guid g in customEntityGuids)
            {
                CustomEntity customEntity = m_sdkEngine.GetEntity(g) as CustomEntity;
                if (customEntity.CustomEntityType.Equals(CustomCamera.TypeGuid))    //just a last check before deleting to make sure it's indeed the correct type. Better safe than sorry.
                    m_sdkEngine.DeleteEntity(g);
            }
        }

        /// <summary>
        /// Set the RunningState of all Custom Camera entities.
        /// When the ACEServer is on, they should be set to State.Running.
        /// This is to simulate a Server servicing the entities at the Client.
        /// </summary>
        /// <param name="runningState"></param>
        private void SetCustomCameraRunningStates(State runningState)
        {
            List<Guid> customEntityGuids = FetchEntities(EntityType.CustomEntity, CustomCamera.TypeGuid);                          // Get all Custom Entities
            IEnumerable<Guid> customCameraGuids = customEntityGuids.Where(
                g => ((CustomEntity)m_sdkEngine.GetEntity(g)).CustomEntityType == CustomCamera.TypeGuid);   // Filter by Custom Camera entities

            // Batching modifications into a transaction
            m_sdkEngine.TransactionManager.ExecuteTransaction(() =>
            {
                foreach (Guid id in customCameraGuids)
                    m_sdkEngine.GetEntity(id).RunningState = runningState; // Set the RunningState
            });
        }

        /// <summary>
        /// Properly dispose references
        /// </summary>
        public void Dispose()
        {
            if (m_sdkEngine != null)
            {
                m_sdkEngine.LoggedOn -= OnEngineLoggedOn;
                m_sdkEngine.LogonFailed -= OnEngineLogonFailed;
                m_sdkEngine.EntitiesAdded -= OnEngine_EntitiesAdded;

                m_sdkEngine.Dispose();
            }

            Closing -= MainWindow_Closing;
        }
    }

    #endregion
}
