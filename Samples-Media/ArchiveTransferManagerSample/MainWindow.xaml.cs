using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using ArchiveTransferManagerSample.Controls.TransferGroupControl.ViewModels;
using ArchiveTransferManagerSample.Services;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Application = System.Windows.Application;

namespace ArchiveTransferManagerSample
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly LoginService m_loginService;
        private readonly QueryService m_queryService;
        private readonly Engine m_sdkEngine;

        public MainWindow()
        {
            InitializeComponent();
            Title = "ArchiveTransferManagerSample";


            DataContext = this;

            m_sdkEngine = new Engine();
            m_sdkEngine.EntitiesInvalidated += OnEntitiesInvalidated;

            m_queryService = new QueryService(m_sdkEngine);
            m_queryService.OnQueryCompleted += OnQueryCompleted;

            m_loginService = new LoginService(m_sdkEngine.LoginManager);
            m_loginService.OnSuccessfullyLoggedIn += OnEngineLoggedOn;
            m_loginService.Logon("admin", string.Empty);

            ManualTransferGroup = new ManualTransferGroupViewModel();
            TransferGroupsList = new TransferGroupsListViewModel();
            CameraRestore = new CameraRestoreViewModel();
            DuplicateArchive = new DuplicateArchiveViewModel();

            ManualTransferGroup.SetConnectedEngine(m_sdkEngine);
            ManualTransferGroup.SetOwnerWindow(this);

            TransferGroupsList.SetConnectedEngine(m_sdkEngine);

            CameraRestore.SetConnectedEngine(m_sdkEngine);
            CameraRestore.SetOwnerWindow(this);

            DuplicateArchive.SetConnectedEngine(m_sdkEngine);
            DuplicateArchive.SetOwnerWindow(this);
        }

        public ManualTransferGroupViewModel ManualTransferGroup { get; set; }
        public TransferGroupsListViewModel TransferGroupsList { get; set; }
        public CameraRestoreViewModel CameraRestore { get; set; }
        public DuplicateArchiveViewModel DuplicateArchive { get; set; }

        public ObservableCollection<string> DisplayInformation { get; } = new ObservableCollection<string>();

        private void OnQueryCompleted(object sender, EventArgs e)
        {
            var archiver = m_sdkEngine.GetEntities<ArchiverRole>(EntityType.Role).First();
            var camera = m_sdkEngine.GetEntities<Camera>(EntityType.Camera).First();

            // This is how you do it, for each of the different manager you only needs to create a builder and configure it.
            // Like this.
            ////var transferGroup = m_sdkEngine.ArchiveTransferManager.CreateBackupBuilder()
            ////    .SetSources(archiver.Guid)
            ////    .SetRecurringDaily(TimeAndDate.Now.AddMinutes(2))
            ////    .SetEverythingSinceLastTransfer()
            ////    .SetName("BACKUP")
            ////    .Build();

            ////m_sdkEngine.ArchiveTransferManager.StartTransfer(transferGroup.Guid);
        }

        public static void ExecuteOnUIThread(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(action);
        }

        private void OnEntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_queryService.AddEntitiesToCache(new[] {EntityType.Role, EntityType.Camera, EntityType.Agent});
        }
    }
}