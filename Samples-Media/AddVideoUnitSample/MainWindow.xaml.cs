using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Security;
using System.Windows;
using AddVideoUnitSample.Annotations;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Video;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Workflows.UnitManager;

namespace AddVideoUnitSample
{

    public partial class MainWindow : INotifyPropertyChanged
    {
        public string Server { get; set; }
        public string UserName { get; set; }

        public string UnitIpAddress { get; set; }
        public string UnitPort { get; set; }
        public string DiscoveryPort { get; set; }
        public string UnitUserName { get; set; }

        public ObservableCollection<string> Logs { get; set; }

        public List<string> Manufacturers { get; set; }
        public List<string> ProductTypes { get; set; }
        public ObservableCollection<EntityModel> AvailableArchivers { get; set; }

        public ObservableCollection<EntityModel> AvailableCameras { get; set; }

        private Engine m_sdkEngine;
        private string m_selectedManufacturer;
        private string m_selectedProductType;
        private EntityModel m_selectedArchiverModel;
        private EntityModel m_selectedCameraModel;

        public string SelectedManufacturer
        {
            get { return m_selectedManufacturer; }
            set
            {
                if (value == m_selectedManufacturer) return;
                m_selectedManufacturer = value;
                OnPropertyChanged(nameof(SelectedManufacturer));

                OnManufacturerSelected();
            }
        }

        public string SelectedProductType
        {
            get { return m_selectedProductType; }
            set
            {
                if (value == m_selectedProductType) return;
                m_selectedProductType = value;
                OnPropertyChanged(nameof(SelectedProductType));
            }
        }

        public EntityModel SelectedArchiverModel
        {
            get { return m_selectedArchiverModel; }
            set
            {
                if (value == m_selectedArchiverModel) return;
                m_selectedArchiverModel = value;
                OnPropertyChanged(nameof(SelectedArchiverModel));
            }
        }

        public EntityModel SelectedCameraModel
        {
            get { return m_selectedCameraModel; }
            set
            {
                if (value == m_selectedCameraModel) return;
                m_selectedCameraModel = value;
                OnPropertyChanged(nameof(SelectedCameraModel));
            }
        }

        private void OnManufacturerSelected()
        {
            var productInfos = m_sdkEngine.VideoUnitManager.FindProductsByManufacturer(m_selectedManufacturer);
            ProductTypes.Clear();

            ProductTypes = productInfos.Select(x => x.ProductType).ToList();
            OnPropertyChanged(nameof(ProductTypes));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Logs=new ObservableCollection<string>();
            Manufacturers = new List<string>();
            ProductTypes = new List<string>();
            AvailableArchivers = new ObservableCollection<EntityModel>();
            AvailableCameras = new ObservableCollection<EntityModel>();

            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += SdkEngine_LoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += SdkEngine_LoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += SdkEngine_LogonFailed;
            m_sdkEngine.EntitiesAdded += M_sdkEngine_EntitiesAdded;
            m_sdkEngine.EntitiesRemoved += M_sdkEngine_EntitiesRemoved;
        }

        private void M_sdkEngine_EntitiesRemoved(object sender, EntitiesRemovedEventArgs e)
        {
            if (e.Entities.Any(o => o.EntityType == EntityType.Camera))
                FetchAvailableCameras();
        }

        private void M_sdkEngine_EntitiesAdded(object sender, EntitiesAddedEventArgs e)
        {
            if (e.Entities.Any(o => o.EntityType == EntityType.Camera))
                FetchAvailableCameras();
        }

        private void VideoUnitManager_EnrollmentStatusChanged(object sender, Genetec.Sdk.EventsArgs.UnitEnrolledEventArgs e)
        {
            DisplayLog("Enrollement status changed: " + e.EnrollmentResult);
        }

        private void SdkEngine_LogonFailed(object sender, LogonFailedEventArgs e)
        {
            DisplayLog("SDK Logon Failed. Reason: " + e.FailureCode);
        }

        private void SdkEngine_LoggedOff(object sender, LoggedOffEventArgs e)
        {
            DisplayLog("SDK Logged off");
            m_sdkEngine.VideoUnitManager.EnrollmentStatusChanged -= VideoUnitManager_EnrollmentStatusChanged;
        }

        private void SdkEngine_LoggedOn(object sender, LoggedOnEventArgs e)
        {
            DisplayLog("SDK Logged on");
            m_sdkEngine.VideoUnitManager.EnrollmentStatusChanged += VideoUnitManager_EnrollmentStatusChanged;
            GetManufacturers();
            FetchAvailableArchivers();
            FetchAvailableCameras();
        }

        private void GetManufacturers()
        {
            Manufacturers.Clear();
            Manufacturers = m_sdkEngine.VideoUnitManager.Manufacturers.ToList();
            OnPropertyChanged(nameof(Manufacturers));
        }

        private void FetchAvailableArchivers()
        {
            AvailableArchivers.Clear();

            var archiversQuery = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            archiversQuery.EntityTypeFilter.Add(EntityType.Role,(byte) RoleType.Archiver);
            archiversQuery.BeginQuery(OnArchiverQueryReceived, archiversQuery);
        }

        private void OnArchiverQueryReceived(IAsyncResult ar)
        {
            var archiversQuery = ar.AsyncState as EntityConfigurationQuery;
            var results = archiversQuery.EndQuery(ar);

            foreach (DataRow dataRow in results.Data.Rows)
            {
                Guid archiverGuid = (Guid) dataRow[0];
                var archiverEntity = m_sdkEngine.GetEntity<Role>(archiverGuid);
                if (archiverEntity == null) continue;
                AvailableArchivers.Add(new EntityModel
                {
                    EntityGuid = archiverGuid,
                    EntityName = archiverEntity.Name,
                    EntityIcon = archiverEntity.GetIcon(true)
                });
            }
        }

        private void FetchAvailableCameras()
        {
            var camerasQuery = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            camerasQuery.EntityTypeFilter.Add(EntityType.Camera);
            camerasQuery.BeginQuery(OnCameraQueryReceived, camerasQuery);
        }

        private void OnCameraQueryReceived(IAsyncResult ar)
        {
            lock (AvailableCameras)
            {
                Dispatcher.Invoke(AvailableCameras.Clear);

                var cameraQuery = ar.AsyncState as EntityConfigurationQuery;
                var results = cameraQuery.EndQuery(ar);

                foreach (DataRow dataRow in results.Data.Rows)
                {
                    Guid cameraguid = (Guid)dataRow[0];
                    if (AvailableCameras.Any(o => o.EntityGuid == cameraguid))
                        continue;

                    var cameraEntity = m_sdkEngine.GetEntity<Camera>(cameraguid);
                    if (cameraEntity == null || cameraEntity.Unit == Guid.Empty)
                        continue;

                    Dispatcher.Invoke(new Action(() =>
                    {
                        AvailableCameras.Add(new EntityModel
                        {
                            EntityGuid = cameraguid,
                            EntityName = cameraEntity.Name,
                            EntityIcon = cameraEntity.GetIcon(true)
                        });
                    }));
                }
            }
        }

        private async void EnrollVideoUnitButton_OnClick(object sender, RoutedEventArgs e)
        {
            IPAddress unitAddress;
            if (!IPAddress.TryParse(UnitIpAddress, out unitAddress))
            {
                DisplayLog("Unable to parse Ip Address");
                return;
            }

            int port;
            if (!int.TryParse(UnitPort, out port) || port <= 0)
            {
                DisplayLog("Unable to parse port");
                return;
            }

            if (m_selectedArchiverModel == null)
            {
                DisplayLog("No archiver selected");
                return;
            }

            var videoUnitProductInfo =
                m_sdkEngine.VideoUnitManager
                    .FindProductsByManufacturer(m_selectedManufacturer)
                    .FirstOrDefault(x => x.ProductType == m_selectedProductType);

            if (videoUnitProductInfo.ProductCapability.SpecialFeatures.Any())
            {
                DisplayLog("This specific product may contain special feature input fields to be enrolled. " +
                    Environment.NewLine +
                    "This basic sample only supports the basic field inputs." +
                    Environment.NewLine +
                    $"Product Special Features: {string.Join(", ", videoUnitProductInfo.ProductCapability.SpecialFeatures)}");
                return;
            }

            var ip = new IPEndPoint(unitAddress, port);
            string password = m_unitPasswordBox.Password;
            var addVideoUnitInfos = new AddVideoUnitInfo(videoUnitProductInfo, ip, false)
            {
                UserName = UnitUserName,
                Password = CreateSecureString(password)
            };

            int discoPort = -1;
            if (int.TryParse(DiscoveryPort, out discoPort) && discoPort >= 0)
            {
                addVideoUnitInfos.DiscoveryPort = discoPort;
            }

            AddUnitResponse response =  await m_sdkEngine.VideoUnitManager.AddVideoUnit(addVideoUnitInfos, m_selectedArchiverModel.EntityGuid);
            if(response != null)
                DisplayLog("Response" +
                    Environment.NewLine +
                    $"Error: {response.Error} " +
                    Environment.NewLine +
                    $"Missing Information: {response.MissingInformation}");
            else
                DisplayLog("No response.");
        }

        public void DisplayLog(string logContent)
        {
            Dispatcher.BeginInvoke(new Action(() => Logs.Add(DateTime.Now.ToShortTimeString() + " - " + logContent)));
        }
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            string password = m_passwordBox.Password;
            m_sdkEngine.LoginManager.BeginLogOn(Server, UserName, password);
        }

        private SecureString CreateSecureString(string str)
        {
            var sec = new SecureString();

            if (!string.IsNullOrEmpty(str))
            {
                str.ToCharArray().ToList().ForEach(sec.AppendChar);
            }

            return sec;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DeleteVideoUnitButton_OnClick(object sender, RoutedEventArgs e)
        {
            var camera = m_sdkEngine.GetEntity<Camera>(m_selectedCameraModel.EntityGuid);
            if (camera == null)
                return;

            if (camera.Unit == Guid.Empty)
            {
                DisplayLog("Cannot delete camera with empty unit Guid");
                return;
            }

            m_sdkEngine.VideoUnitManager.DeleteVideoUnit(camera.Unit, (bool)DeleteArchives.IsChecked);
        }
    }
}
