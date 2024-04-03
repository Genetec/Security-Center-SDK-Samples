using DoorTemplates.Annotations;
using Genetec.Sdk;
using Genetec.Sdk.Diagnostics;
using Genetec.Sdk.Diagnostics.Logging.Core;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.AccessPoints.DoorTemplates;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace DoorTemplates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Constants

        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Properties

        private string m_serverName;
        public string ServerName
        {
            get { return m_serverName; }
            set
            {
                m_serverName = value;
                OnPropertyChanged();
            }
        }

        private string m_userName;
        public string UserName
        {
            get { return m_userName; }
            set
            {
                m_userName = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> Logs { get; set; }
        public ObservableCollection<DisplayEntityModel> Doors { get; set; }
        public ObservableCollection<DisplayEntityModel> Units { get; set; }
        public ObservableCollection<DisplayEntityModel> InterfaceModules { get; set; }

        public ObservableCollection<DisplayEntityModel> InterfaceModuleDeviceDefinitions { get; set; }
        public ObservableCollection<DisplayEntityModel> DoorTemplates { get; set; }

        private DisplayEntityModel m_selectedDoor;
        public DisplayEntityModel SelectedDoor
        {
            get { return m_selectedDoor; }
            set
            {
                m_selectedDoor = value;
                OnPropertyChanged();
            }
        }

        private DisplayEntityModel m_selectedUnit;
        public DisplayEntityModel SelectedUnit
        {
            get { return m_selectedUnit; }
            set
            {
                m_selectedUnit = value;
                OnPropertyChanged();
            }
        }

        private DisplayEntityModel m_selectedInterfaceModule;
        public DisplayEntityModel SelectedInterfaceModule
        {
            get { return m_selectedInterfaceModule; }
            set
            {
                m_selectedInterfaceModule = value;
                OnPropertyChanged();
            }
        }


        public string CreateDoorTemplateName { get; set; }
        private DisplayEntityModel m_selectedInterfaceModuleDeviceDefinition;
        public DisplayEntityModel SelectedInterfaceModuleDeviceDefinition
        {
            get { return m_selectedInterfaceModuleDeviceDefinition; }
            set
            {
                m_selectedInterfaceModuleDeviceDefinition = value;
                OnPropertyChanged();
            }
        }

        private DisplayEntityModel m_selectedDoorTemplate;
        public DisplayEntityModel SelectedDoorTemplate
        {
            get { return m_selectedDoorTemplate; }
            set
            {
                m_selectedDoorTemplate = value;
                OnPropertyChanged();
            }
        }

        public TemplateItemSelectorType SelectedUnitType { get; set; }
        public string UnitSelectorParameter { get; set; }


        public TemplateItemSelectorType SelectedInterfaceType { get; set; }
        public string InterfaceModuleSelector { get; set; }
        #endregion

        #region Constructors

        static MainWindow()
        {
            ActivateLogging();
        }

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            // Register to important events
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;
            m_sdkEngine.EntitiesAdded += OnEngine_EntitiesAdded;

            ServerName = "::1";
            UserName = "admin";

            Logs = new ObservableCollection<string>();

            Doors = new ObservableCollection<DisplayEntityModel>();
            Units = new ObservableCollection<DisplayEntityModel>();
            InterfaceModules = new ObservableCollection<DisplayEntityModel>();

            InterfaceModuleDeviceDefinitions = new ObservableCollection<DisplayEntityModel>();
            DoorTemplates = new ObservableCollection<DisplayEntityModel>();            
        }
        
        private void OnEngine_EntitiesAdded(object sender, EntitiesAddedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (Guid entityGuid in e.Entities.Where(t =>
                        t.EntityType == EntityType.Door || t.EntityType == EntityType.DoorTemplate ||
                        t.EntityType == EntityType.InterfaceModuleDeviceDefinition || t.EntityType==EntityType.Unit || 
                        t.EntityType==EntityType.InterfaceModule)
                    .Select(g => g.EntityGuid))
                {
                    var entity = m_sdkEngine.GetEntity(entityGuid);
                    if (entity is Door && !Doors.Any(d => d.EntityGuid == entity.Guid))
                        Doors.Add(new DisplayEntityModel {EntityGuid = entity.Guid, EntityName = entity.Name, Icon = entity.GetIcon(true)});
                    if (entity is InterfaceModuleDeviceDefinition && !InterfaceModuleDeviceDefinitions.Any(d => d.EntityGuid == entity.Guid))
                        InterfaceModuleDeviceDefinitions.Add(new DisplayEntityModel {EntityGuid = entity.Guid, EntityName = entity.Name, Icon = entity.GetIcon(true)});
                    if (entity is DoorTemplate && !DoorTemplates.Any(d => d.EntityGuid == entity.Guid))
                        DoorTemplates.Add(new DisplayEntityModel {EntityGuid = entity.Guid, EntityName = entity.Name, Icon = entity.GetIcon(true)});
                    if (entity is Unit && !Units.Any(u => u.EntityGuid == entity.Guid))
                        Units.Add(new DisplayEntityModel { EntityGuid = entity.Guid, EntityName = entity.Name, Icon = entity.GetIcon(true)});
                    if (entity is InterfaceModule && !InterfaceModules.Any(u => u.EntityGuid == entity.Guid))
                        InterfaceModules.Add(new DisplayEntityModel { EntityGuid = entity.Guid, EntityName = entity.Name, Icon = entity.GetIcon(true) });
                }

                OnPropertyChanged("InterfaceModuleDeviceDefinitions");
                OnPropertyChanged("Doors");
                OnPropertyChanged("Units");
                OnPropertyChanged("InterfaceModules");
                OnPropertyChanged("DoorTemplates");
                OnPropertyChanged("Logs");
            }));
        }

        #endregion

        #region Event Handlers

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Console.WriteLine("Sdk has logged off");
            Logs.Add("Sdk has logged off");
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Console.WriteLine(e.UserName + " has logged on to " + e.ServerName);
            Logs.Add(e.UserName + " has logged on to " + e.ServerName);            

            FetchEntities();
        }

        void FetchEntities(int page = 1)
        {
            var ecq = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            ecq.EntityTypeFilter.Add(EntityType.Door);
            ecq.EntityTypeFilter.Add(EntityType.Unit);
            ecq.EntityTypeFilter.Add(EntityType.InterfaceModule);
            ecq.EntityTypeFilter.Add(EntityType.InterfaceModuleDeviceDefinition);
            ecq.EntityTypeFilter.Add(EntityType.DoorTemplate);
            ecq.PageSize = 1000;
            ecq.DownloadAllRelatedData = true;
            ecq.Page = page;
            Dispatcher.BeginInvoke(new Action(() => Logs.Add("Current page is: " + ecq.Page)));
            ecq.BeginQuery(null, ecq);
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            MessageBox.Show(e.FormattedErrorMessage);
            Logs.Add(e.FormattedErrorMessage);

            OnPropertyChanged("Logs");
        }

        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            Console.WriteLine("Server : " + e.ServerName + ". Status changed to " + e.Status);
            Logs.Add("Server : " + e.ServerName + ". Status changed to " + e.Status);
        }

        #endregion

        #region Public Methods

        public static void ActivateLogging()
        {
            // Logs can be found in the Logs subfolder
            DiagnosticServer logServer = DiagnosticServer.Instance;
            logServer.AddFileTracing(new[] { new LoggerTraces("DoorTemplates*", LogSeverity.Full) });
        }

        #endregion

        #region Connect Button
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            Logs.Add("Start logging on");

            string password = m_password.Password;
            m_sdkEngine.LoginManager.BeginLogOn(ServerName, UserName, password);
        }
        #endregion
        
        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
        
        private void CreateDoorTemplateButton_Click(object sender, RoutedEventArgs e)
        {
            // this is an example showing how to create a new door template
            // this example will simply map the two first readers to both door sides
            // and the first input and first output to the door sensor and door lock
            // it also shows how to set the Card And Pin option using the Card And Pin Descriptor

            lock (m_sdkEngine.TransactionManager.SyncUpdate)
            {
                try
                {
                    m_sdkEngine.TransactionManager.CreateTransaction();
                    DoorTemplate doorTemplate = m_sdkEngine.CreateEntity(CreateDoorTemplateName,EntityType.DoorTemplate) as DoorTemplate;
                    doorTemplate.IsHardwareMappingEnabled = true;
                    doorTemplate.InterfaceModuleDeviceDefinition = SelectedInterfaceModuleDeviceDefinition.EntityGuid;
                    doorTemplate.UpdateAccessPointSelectors(CreateAccessPointSelectors(SelectedInterfaceModuleDeviceDefinition.EntityGuid));
                    doorTemplate.IsHardwareSettingsEnabled = true;
                    doorTemplate.UpdateCardAndPinSettings( CreateCardAndPinSettingsDescriptors() );
                    
                    m_sdkEngine.TransactionManager.CommitTransaction();
                }
                catch (Exception exception)
                {
                    Logs.Add("Exception caught creating a door template. Error details: " + exception);
                    if(m_sdkEngine.TransactionManager.IsTransactionActive)
                        m_sdkEngine.TransactionManager.RollbackTransaction();
                }
            }
        }

        private List<CardAndPinSettingsDescriptor> CreateCardAndPinSettingsDescriptors()
        {
            // Card and pin descriptors 
            var result = new List<CardAndPinSettingsDescriptor>();
            result.Add( new CardAndPinSettingsDescriptor { Side = AccessPointSide.Alpha, CardAndPinCoverage = Schedule.AlwaysScheduleGuid, CardAndPinTimeout = 10000, ReaderMode = ReaderMode.CardAndPin } );
            result.Add( new CardAndPinSettingsDescriptor { Side = AccessPointSide.Omega, CardAndPinCoverage = Schedule.AlwaysScheduleGuid, CardAndPinTimeout = 10000, ReaderMode = ReaderMode.CardAndPin } );
            return result;
        }
        
        private List<AccessPointSelectorDescriptor> CreateAccessPointSelectors(Guid interfaceModuleDeviceDefinition)
        {
            var result = new List<AccessPointSelectorDescriptor>();
            InterfaceModuleDeviceDefinition interfaceModuleDefinition = m_sdkEngine.GetEntity<InterfaceModuleDeviceDefinition>(interfaceModuleDeviceDefinition);

            // map 2 first readers to both door side readers
            result.Add(new AccessPointSelectorDescriptor { Type = AccessPointType.CardReader, Side = AccessPointSide.Alpha, DeviceId = GetDeviceId(DeviceType.Reader, 1, interfaceModuleDefinition) });
            result.Add(new AccessPointSelectorDescriptor { Type = AccessPointType.CardReader, Side = AccessPointSide.Omega, DeviceId = GetDeviceId(DeviceType.Reader, 2, interfaceModuleDefinition) });

            // then we map the first input to door sensor and the first output to door lock
            result.Add(new AccessPointSelectorDescriptor { Type = AccessPointType.DoorSensor, DeviceId = GetDeviceId(DeviceType.Input, 1, interfaceModuleDefinition) });
            result.Add(new AccessPointSelectorDescriptor { Type = AccessPointType.DoorLock, DeviceId = GetDeviceId(DeviceType.Output, 1, interfaceModuleDefinition) });
            
            return result;
        }

        private string GetDeviceId(DeviceType deviceType, int index, InterfaceModuleDeviceDefinition interfaceModuleDefinition)
        {
            string deviceId = interfaceModuleDefinition.Devices.Where(x => x.DeviceType == deviceType).Skip(index - 1).Select(x => x.DeviceId).FirstOrDefault();
            return deviceId;
        }

        private void ApplyDoorTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DoorTemplate selectedDoorTemplate = m_sdkEngine.GetEntity<DoorTemplate>(SelectedDoorTemplate.EntityGuid);
                Guid selectedDoorGuid = SelectedDoor.EntityGuid;
                Guid selectedUnitGuid = SelectedUnit.EntityGuid;
                Guid selectedInterfaceModuleGuid = SelectedInterfaceModule.EntityGuid;

                selectedDoorTemplate.Apply(selectedDoorGuid, selectedUnitGuid, selectedInterfaceModuleGuid);
            }
            catch (Exception exception)
            {
                Logs.Add("Exception caught applying the door template: Error details: " + exception);
            }
        }
    }

}
