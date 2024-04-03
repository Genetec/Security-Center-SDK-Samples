// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using WebSDKStudio.MultipartRequests;
using WebSDKStudio.Requests;

namespace WebSDKStudio
{

    /// <summary>
    /// Interaction logic for WebSdkStudioWindow.xaml
    /// </summary>
    public partial class WebSdkStudioWindow
    {

        #region Public Fields

        public static readonly DependencyProperty AddRequiredEntitiesEnableProperty =
            DependencyProperty.Register("AddRequiredEntitiesEnable", typeof(bool), typeof(WebSdkStudioWindow), new PropertyMetadata(true));

        public static readonly DependencyProperty ApplicationIdProperty =
            DependencyProperty.Register("ApplicationId", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata("KxsD11z743Hf5Gq9mv3+5ekxzemlCiUXkTFY5ba1NOGcLCmGstt2n0zYE9NsNimv"));

        public static readonly DependencyProperty AssignCardholderToCardholderGroupProperty =
            DependencyProperty.Register("AssignCardholderToCardholderGroup", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty BackupButtonToCreateRequiredEntitiesProperty =
            DependencyProperty.Register("BackupButtonToCreateRequiredEntities", typeof(Visibility), typeof(WebSdkStudioWindow), new PropertyMetadata(Visibility.Hidden));

        /// <summary>
        /// Base URI of the Web SDK role.  This value is configurable
        /// in the Web SDK role configuration pages in the Config Tool.
        /// </summary>
        public static readonly DependencyProperty BaseUriProperty =
            DependencyProperty.Register("BaseUri", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata("WebSdk"));

        public static readonly DependencyProperty BuildAndRaiseCustomEventProperty =
            DependencyProperty.Register("BuildAndRaiseCustomEvent", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty BuildAndRaiseEventProperty =
            DependencyProperty.Register("BuildAndRaiseEvent", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty CustomFieldFilterProperty =
            DependencyProperty.Register("CustomFieldFilter", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty MotionQueryProperty =
            DependencyProperty.Register("MotionQuery", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty GetPublicTasksProperty =
            DependencyProperty.Register("GetPublicTasks", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty GetPrivateTasksProperty =
            DependencyProperty.Register("GetPrivateTasks", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ExecutePublicTaskProperty =
            DependencyProperty.Register("ExecutePublicTask", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ExecutePrivateTaskProperty =
            DependencyProperty.Register("ExecutePrivateTask", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ExecuteTaskProperty =
            DependencyProperty.Register("ExecuteTask", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty GetValueOfTheCustomFieldProperty =
            DependencyProperty.Register("GetValueOfTheCustomField", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HttpRequestSelectedItemProperty =
            DependencyProperty.Register("HttpRequestSelectedItem", typeof(ComboBoxItem), typeof(WebSdkStudioWindow), new PropertyMetadata(default(ComboBoxItem)));

        public static readonly DependencyProperty ImagePathProperty =
            DependencyProperty.Register("ImagePath", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty IsHttpsProperty =
            DependencyProperty.Register("IsHttps", typeof(bool), typeof(WebSdkStudioWindow), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty ModifyValueOfTheCustomFieldProperty =
            DependencyProperty.Register("ModifyValueOfTheCustomField", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        /// <summary>
        /// Dependency property used for the Multi-part request to set the cardholder picture. It contains the Cardholder Guid.
        /// If the required entities are accepted, the text-box will contains the created cardholder Guid. Otherwise, it will be empty.
        /// </summary>
        public static readonly DependencyProperty MultipartRequestCardholderGuidProperty =
            DependencyProperty.Register("MultipartRequestCardholderGuid", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty OpenDoorProperty =
            DependencyProperty.Register("OpenDoor", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty OverlayForDemoEntitiesProperty =
            DependencyProperty.Register("OverlayForDemoEntities", typeof(Visibility), typeof(WebSdkStudioWindow), new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        /// <summary>
        /// HTTP port of the Web SDK role.  This value is configurable
        /// in the Web SDK role configuration pages in the Config Tool.
        /// </summary>
        public static readonly DependencyProperty PortProperty =
            DependencyProperty.Register("Port", typeof(int), typeof(WebSdkStudioWindow), new PropertyMetadata(4590));

        public static readonly DependencyProperty RaiseAnEventProperty =
            DependencyProperty.Register("RaiseAnEvent", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty RequestsSentSelectedItemProperty =
            DependencyProperty.Register("RequestsSentSelectedItem", typeof(Request), typeof(WebSdkStudioWindow), new PropertyMetadata(default(Request)));

        /// <summary>
        /// Address of the server running the Web SDK role.  This value can
        /// be a hostname or an IP address.
        /// Ex.:    myHostname
        ///         myHostname.genetec.com
        ///         10.1.1.25
        /// </summary>
        public static readonly DependencyProperty ServerProperty =
            DependencyProperty.Register("Server", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata("localhost"));

        public static readonly DependencyProperty SubscribedEventsProperty =
            DependencyProperty.Register("SubscribedEvents", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SubscribeToAllEventProperty =
            DependencyProperty.Register("SubscribeToAllEvent", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SubscribeToEventProperty =
            DependencyProperty.Register("SubscribeToEvent", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TimeoutProperty =
            DependencyProperty.Register("Timeout", typeof(int), typeof(WebSdkStudioWindow), new PropertyMetadata(10000));
        public static readonly DependencyProperty UnAssignCardholderToCardholderGroupProperty =
            DependencyProperty.Register("UnAssignCardholderToCardholderGroup", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty UnSubscribeFromEventProperty =
            DependencyProperty.Register("UnSubscribeFromEvent", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty UnSubscribeToAllEventProperty =
            DependencyProperty.Register("UnSubscribeToAllEvent", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));
        /// <summary>
        /// The Url of the request to send which is shown on the UI
        /// </summary>
        public static readonly DependencyProperty UrlProperty =
            DependencyProperty.Register("Url", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty CustomEventPayloadProperty =
            DependencyProperty.Register("CustomEventPayload", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty UsernameProperty =
            DependencyProperty.Register("Username", typeof(string), typeof(WebSdkStudioWindow), new PropertyMetadata("Admin"));

        #endregion Public Fields

        #region Private Fields

        private const string CLOSING_MESSAGE_BOX_TEXT = "Do you want to delete the default Entities which have been created? If you used a query to create an entity, it will not be removed. Only the ones created by the sample automatically will be.";

        private const string CLOSING_MESSAGE_BOX_TITLE = "Delete the default query entities created?";

        private const int NUMBER_OF_PARTS_BEFORE_QUERY = 4;

        private const int NUMBER_OF_PARTS_DEFAULT = 5;

        private const string PNG_FILTER = "png Images|*.png";

        /// <summary>
        /// The guid of the Admin in the Directory
        /// </summary>
        private static readonly string AdminGuid = "00000000-0000-0000-0000-000000000003";

        private static readonly int HighestPort = 65535;

        private static readonly Regex Numeric = new Regex(@"^[0-9]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// The Guid of the SstemConfiguration in the Directory
        /// </summary>
        private static readonly string SystemConfigurationGuid = "00000000-0000-0000-0000-000000000007";

        /// <summary>
        /// Value for guid which are still waiting for the <see cref="AddRequiredEntities"/>
        /// to finish before puting the guids in the queries
        /// </summary>
        private static readonly string WaitingForGuid = "{Guid}";

        private bool m_askForCleanup;

        private string m_baseUriForDeletionOfEntitiesGenerated = string.Empty;

        private Window m_eventsMonitor;

        private bool m_isHttpsForDeletionOfEntitiesGenerated = (bool)IsHttpsProperty.DefaultMetadata.DefaultValue;

        private int m_portForDeletionOfEntitiesGenerated;

        /// <summary>
        /// The query to send with the request.
        /// </summary>
        private string m_query = string.Empty;

        private string m_serverForDeletionOfEntitiesGenerated = string.Empty;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Button tooltip
        /// It's actually used.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static string ButtonQueryToolTip => "You can RIGHT CLICK on a Query Button to send it instantly. LEFT CLICK will add the query to the Url textbox";

        /// <summary>
        /// List View toolTip
        /// It's actually used.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public static string RequestsSentToolTip => "You can DOUBLE CLICK on an item from the list to place it in the Url textbox";

        // A Query
        public string AddCustomEvent { get; private set; }

        public bool AddRequiredEntitiesEnable
        {
            get => (bool)GetValue(AddRequiredEntitiesEnableProperty);
            set => SetValue(AddRequiredEntitiesEnableProperty, value);
        }

        public Guid AlarmGuidSample { get; set; }

        public string ApplicationId
        {
            get => (string)GetValue(ApplicationIdProperty);
            set
            {
                Request.ApplicationId = value;
                SetValue(ApplicationIdProperty, value);
            }
        }

        public string AssignCardholderToCardholderGroup
        {
            get => (string)GetValue(AssignCardholderToCardholderGroupProperty);
            set => SetValue(AssignCardholderToCardholderGroupProperty, value);
        }

        // A Query
        public string AssignCredentialToCardholder { get; private set; }

        public Visibility BackupButtonToCreateRequiredEntities
        {
            get => (Visibility)GetValue(BackupButtonToCreateRequiredEntitiesProperty);
            set => SetValue(BackupButtonToCreateRequiredEntitiesProperty, value);
        }

        public string BaseUri
        {
            get => (string)GetValue(BaseUriProperty);
            set => SetValue(BaseUriProperty, value);
        }

        public string BuildAndRaiseCustomEvent
        {
            get => (string)GetValue(BuildAndRaiseCustomEventProperty);
            set => SetValue(BuildAndRaiseCustomEventProperty, value);
        }

        public string BuildAndRaiseEvent
        {
            get => (string)GetValue(BuildAndRaiseEventProperty);
            set => SetValue(BuildAndRaiseEventProperty, value);
        }

        public Guid CardholderGuidSample { get; private set; }

        // A Query
        public string CreateAccessRule { get; private set; }

        // A Query
        public string CreateCardholder { get; private set; }

        // A Query
        public string CreateCardholderGroup { get; private set; }

        // A Query
        public string CreateCredential { get; private set; }

        public string CreateCustomEntity { get; set; }

        // A Query
        public string CreateCustomField { get; set; }

        // A Query
        public string CreateEntity { get; private set; }

        // A Query
        public string CreateVisitor { get; private set; }
        
        // A Query
        public string CreationPartition { get; private set; }

        public string CustomFieldFilter
        {
            get => (string)GetValue(CustomFieldFilterProperty);
            set => SetValue(CustomFieldFilterProperty, value);
        }

        public string MotionQuery
        {
            get => (string)GetValue(MotionQueryProperty);
            set => SetValue(MotionQueryProperty, value);
        }

        public string GetPublicTasks
        {
            get => (string)GetValue(GetPublicTasksProperty);
            set => SetValue(GetPublicTasksProperty, value);
        }
        
        public string GetPrivateTasks
        {
            get => (string)GetValue(GetPrivateTasksProperty);
            set => SetValue(GetPrivateTasksProperty, value);
        }

        public string ExecutePublicTask
        {
            get => (string)GetValue(ExecutePublicTaskProperty);
            set => SetValue(ExecutePublicTaskProperty, value);
        }
        
        public string ExecutePrivateTask
        {
            get => (string)GetValue(ExecutePrivateTaskProperty);
            set => SetValue(ExecutePrivateTaskProperty, value);
        }
        
        public string ExecuteTask
        {
            get => (string)GetValue(ExecuteTaskProperty);
            set => SetValue(ExecuteTaskProperty, value);
        }

        public Guid CustomFieldGuidSample { get; set; }
        public string DeleteCustomEntity { get; set; }
        public string DeleteCustomEntityTypeDescriptor { get; set; }
        // A Query
        public string DeleteCustomField { get; set; }

        // A Query
        public string DeleteEntity { get; private set; }

        public string DisableAlarmMonitoring { get; set; }

        public string DisplayInTile { get; set; }

        public Guid DoorGuidSample { get; private set; }

        public string EnableAlarmMonitoring { get; set; }
        // A Query
        public string EntityExistsByGuid { get; private set; }

        public string GetAllCustomEntityTypeDescriptor { get; set; }
        public string GetCustomEntityTypeDescriptor { get; set; }
        // A Query
        public string GetEntityBasic { get; private set; }

        // A Query
        public string GetEntityByLogicalId { get; private set; }

        public string GetEntityFull { get; set; }

        public string GetTile { get; set; }

        public string GetTiles { get; set; }
        // A Query
        public string GetValueOfTheCustomField
        {
            get => (string)GetValue(GetValueOfTheCustomFieldProperty);
            set => SetValue(GetValueOfTheCustomFieldProperty, value);
        }

        // Query: Security tab
        public string GetWebTokenEncoded { get; set; }

        public string GetWebTokenDecoded { get; set; }

        public ObservableCollection<ComboBoxItem> HttpRequestItems { get; }

        public ComboBoxItem HttpRequestSelectedItem
        {
            get => (ComboBoxItem)GetValue(HttpRequestSelectedItemProperty);
            set => SetValue(HttpRequestSelectedItemProperty, value);
        }

        public string ImagePath
        {
            get => (string)GetValue(ImagePathProperty);
            set => SetValue(ImagePathProperty, value);
        }

        public bool IsHttps
        {
            get => (bool)GetValue(IsHttpsProperty);
            set => SetValue(IsHttpsProperty, value);
        }

        public string ModifyCustomEntity { get; set; }

        // A Query
        public string ModifyEntity { get; private set; }

        // A Query
        public string ModifyValueOfTheCustomField
        {
            get => (string)GetValue(ModifyValueOfTheCustomFieldProperty);
            set => SetValue(ModifyValueOfTheCustomFieldProperty, value);
        }

        public string MultipartRequestCardholderGuid
        {
            get => (string)GetValue(MultipartRequestCardholderGuidProperty);
            set => SetValue(MultipartRequestCardholderGuidProperty, value);
        }

        public string MultiQueryRequest { get; private set; }

        // A Query
        public string OpenDoor
        {
            get => (string)GetValue(OpenDoorProperty);
            set => SetValue(OpenDoorProperty, value);
        }

        public Visibility OverlayForDemoEntities
        {
            get => (Visibility)GetValue(OverlayForDemoEntitiesProperty);
            set => SetValue(OverlayForDemoEntitiesProperty, value);
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set
            {
                Request.Password = value;
                SetValue(PasswordProperty, value);
            }
        }

        public int Port
        {
            get => (int)GetValue(PortProperty);
            set => SetValue(PortProperty, value);
        }

        public string RaiseAnEvent
        {
            get => (string)GetValue(RaiseAnEventProperty);
            set => SetValue(RaiseAnEventProperty, value);
        }

        // A Query
        public string RaiseCustomEvent { get; set; }

        // A Query
        public string RemoveCustomEvent { get; set; }

        public ObservableCollection<Request> RequestsSent { get; }

        public Request RequestsSentSelectedItem
        {
            get => (Request)GetValue(RequestsSentSelectedItemProperty);
            set => SetValue(RequestsSentSelectedItemProperty, value);
        }

        public Queue<Request> RequestWaitingForResponse { get; }

        // A Query
        public string SendEmail { get; private set; }

        // A Query
        public string SendMessage { get; private set; }

        public string Server
        {
            get => (string)GetValue(ServerProperty);
            set => SetValue(ServerProperty, value);
        }

        public string SubscribedEvents
        {
            get => (string)GetValue(SubscribedEventsProperty);
            set => SetValue(SubscribedEventsProperty, value);
        }

        public string SubscribeToAllEvent
        {
            get => (string)GetValue(SubscribeToAllEventProperty);
            set => SetValue(SubscribeToAllEventProperty, value);
        }

        // A Query
        public string SubscribeToEvent
        {
            get => (string)GetValue(SubscribeToEventProperty);
            set => SetValue(SubscribeToEventProperty, value);
        }

        public int Timeout
        {
            get => (int)GetValue(TimeoutProperty);
            set => SetValue(TimeoutProperty, value);
        }
        // A Query
        public string UnAssignCardholderToCardholderGroup
        {
            get => (string)GetValue(UnAssignCardholderToCardholderGroupProperty);
            set => SetValue(UnAssignCardholderToCardholderGroupProperty, value);
        }

        // A Query
        public string UnAssignCredentialToCardholder { get; private set; }
        
        public string GetAllLicenseItemUsage { get; private set; }
        
        public string GetCameraLicenseItemUsage { get; private set; }

        public string UnSubscribeFromEvent
        {
            get => (string)GetValue(UnSubscribeFromEventProperty);
            set => SetValue(UnSubscribeFromEventProperty, value);
        }

        public string UnSubscribeToAllEvent
        {
            get => (string)GetValue(UnSubscribeToAllEventProperty);
            set => SetValue(UnSubscribeToAllEventProperty, value);
        }

        public string Url
        {
            get => (string)GetValue(UrlProperty);
            set => SetValue(UrlProperty, value);
        }

        public string CustomEventPayload
        {
            get => (string)GetValue(CustomEventPayloadProperty);
            set => SetValue(CustomEventPayloadProperty, value);
        }
        
        public string Username
        {
            get => (string)GetValue(UsernameProperty);
            set
            {
                Request.Username = value;
                SetValue(UsernameProperty, value);
            }
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Constructor for the class <see cref="WebSdkStudioWindow"/>
        /// </summary>
        public WebSdkStudioWindow()
        {
            InitializeComponent();
            DataContext = this;

            HttpRequestItems = new ObservableCollection<ComboBoxItem>
            {
                new ComboBoxItem {Content = "GET", Tag = "GET"},
                new ComboBoxItem {Content = "POST", Tag = "POST"},
                new ComboBoxItem {Content = "PUT", Tag = "PUT"},
                new ComboBoxItem {Content = "DELETE", Tag = "DELETE"}
            };

            HttpRequestSelectedItem = HttpRequestItems.First();
            RequestsSent = new ObservableCollection<Request>();
            RequestWaitingForResponse = new Queue<Request>();

            PopulateQueryButtons();
            UrlUpdate(false);
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnMultipartResponse(Request request)
        {
            Dispatcher.Invoke(() =>
            {
                RequestsSent.Add(request);
                RequestsSentSelectedItem = RequestsSent.Last();
                m_requestsSent.ScrollIntoView(RequestsSentSelectedItem);
            });
        }

        #endregion Public Methods

        #region Private Methods

        private Task<Guid> AddEntityAsync(string method, string url)
            => AddEntityAsync(method, url, xmlData => new Guid(XDocument.Parse(xmlData).Root?.Element("Guid")?.Value ?? throw new InvalidOperationException()));

        private async Task<Guid> AddEntityAsync(string method, string url, Func<string, Guid> xmlDataToGuidFunc)
        {
            var request = CreateHttpWebRequest(new Request(method, url));
            request.AllowReadStreamBuffering = false;

            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception("An error as occurred.");
                    }

                    using (var reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        var xmlData = reader.ReadToEnd().Trim();
                        return xmlDataToGuidFunc(xmlData);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                    return Guid.Empty;
                }
            });
        }

        /// <summary>
        /// We make sure to have entities and their guids for this sample,
        /// so we create entities which will be used in the queries from <see cref="PopulateQueryButtons"/>.
        /// </summary>
        private async void AddRequiredEntities()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            m_portForDeletionOfEntitiesGenerated = Port;
            m_serverForDeletionOfEntitiesGenerated = Server;
            m_baseUriForDeletionOfEntitiesGenerated = BaseUri;
            m_isHttpsForDeletionOfEntitiesGenerated = IsHttps;
            UrlUpdate(false);

            //Create a Custom Field on Cardholder
            var webRequestCreateCustomField = CreateHttpWebRequest(new Request("POST", Url + "customField/Cardholder/CustomFieldWebSdkFilterSample/Text/CustomFieldDefaultValue"));
            webRequestCreateCustomField.AllowReadStreamBuffering = false;
            await GetHttpResponse(webRequestCreateCustomField);

            //Create a Door
            DoorGuidSample = await AddEntityAsync("POST", Url + "entity?q=entity=NewEntity(Door),Name=DoorWebSdkSample,Guid");

            //Create a Cardholder
            CardholderGuidSample = await AddEntityAsync("POST", Url + "entity?q=entity=NewEntity(Cardholder),Name=CardholderWebSdkSample,Guid");

            //Create an alarm
            AlarmGuidSample = await AddEntityAsync("POST", Url + "entity?q=entity=NewEntity(Alarm),Name=AlarmSample,ReactivationThreshold=00:00:00,Recipients@(" + AdminGuid + ",00:00:01),Guid");

            //Get the created Custom Field Guid
            CustomFieldGuidSample = await AddEntityAsync("GET", Url + "entity?q=entity=" + SystemConfigurationGuid + ",customfields", xmlData => new Guid(XDocument.Parse(xmlData).Root.Element("CustomFields").Element("item").Element("CustomField").Element("Guid").Value));

            if (DoorGuidSample == Guid.Empty
                || CardholderGuidSample == Guid.Empty
                || AlarmGuidSample == Guid.Empty
                || CustomFieldGuidSample == Guid.Empty)
            {
                MessageBox.Show("An error happened while creating the entities. You can fix the problem, retry or select the option No to skip this step. Look in the console to see the traces", "Error creating entities", MessageBoxButton.OK, MessageBoxImage.Error);
                AddRequiredEntitiesEnable = true;
                m_askForCleanup = false;
                Mouse.OverrideCursor = null;
                return;
            }

            //CustomField
            CustomFieldFilter = CustomFieldFilter.Replace(WaitingForGuid, CustomFieldGuidSample.ToString());

            //Door
            OpenDoor = OpenDoor.Replace(WaitingForGuid, DoorGuidSample.ToString());

            //Alarm
            SubscribeToEvent = SubscribeToEvent.Replace(WaitingForGuid, AlarmGuidSample.ToString());
            RaiseAnEvent = RaiseAnEvent.Replace(WaitingForGuid, AlarmGuidSample.ToString());
            UnSubscribeFromEvent = UnSubscribeFromEvent.Replace(WaitingForGuid, AlarmGuidSample.ToString());

            //Cardholder
            AssignCardholderToCardholderGroup = AssignCardholderToCardholderGroup.Replace(WaitingForGuid, CardholderGuidSample.ToString());
            UnAssignCardholderToCardholderGroup = UnAssignCardholderToCardholderGroup.Replace(WaitingForGuid, CardholderGuidSample.ToString());
            ModifyValueOfTheCustomField = ModifyValueOfTheCustomField.Replace(WaitingForGuid, CardholderGuidSample.ToString());
            GetValueOfTheCustomField = GetValueOfTheCustomField.Replace(WaitingForGuid, CardholderGuidSample.ToString());
            MultipartRequestCardholderGuid = CardholderGuidSample.ToString();

            //Hide the overlay as everything went well.
            OverlayForDemoEntities = Visibility.Hidden;
            m_askForCleanup = true;
            Mouse.OverrideCursor = null;
        }

        private StringBuilder BuildRequestServerPart()
        {
            //If we need to use SSL connection, we need to change the localhost to the hostname, because the certificate is set
            //to it, not to localhost.
            var server = new StringBuilder();
            if (IsHttps) // SSL
            {
                server.Append("https://");

                //The Certificate uses the hostname.
                if (Server.ToLower() == "localhost" || Dns.GetHostEntry("").HostName.Split('.')[0].ToLower().Equals(Server.ToLower()))
                {
                    server.Append(Dns.GetHostEntry("").HostName);
                }
                else
                {
                    server.Append(Server);
                }
            }
            else // Not SSL
            {
                server.Append("http://").Append(Server);
            }

            //Uri for the web sdk
            return server.Append(":").Append(Port).Append("/").Append(BaseUri).Append("/");
        }

        private void CopyToClipboardClicked(object sender, RoutedEventArgs e)
                            => Clipboard.SetText(RequestsSentSelectedItem.Response);

        /// <summary>
        /// Method to create the HttpWebRequest
        /// </summary>
        /// <param name="request">The <see cref="Request"/> to be sent.</param>
        /// <returns></returns>
        private HttpWebRequest CreateHttpWebRequest(Request request)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(request.Url);
            webRequest.Method = request.HttpMethod;
            webRequest.Credentials = request.WebRequestCredentials;
            webRequest.KeepAlive = Request.KEEP_ALIVE;
            webRequest.Accept = Request.Accept;
            webRequest.ContentLength = Request.CONTENT_LENGTH;
            webRequest.Timeout = Timeout;

            return webRequest;
        }

        private Task<string> GetHttpResponse(WebRequest webRequest)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    // Get the response
                    var webResponse = (HttpWebResponse)webRequest.GetResponse();
                    using (var sr = new StreamReader(webResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                    {
                        var response = sr.ReadToEnd().Trim();
                        try
                        {
                            response = XDocument.Parse(response).ToString();
                        }
                        catch
                        {
                            try
                            {
                                dynamic parsedJson = JsonConvert.DeserializeObject(response);
                                response = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                            }
                            catch
                            {
                                // Ignored
                            }
                        }

                        return response;
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            });
        }

        /// <summary>
        /// When the user click on a button with the right mouse button, it will
        /// trigger an instant send of the query under the button to the Web Sdk.
        /// It will not show in the Url textbox for the user to modify and it will
        /// show as an item from the sent list.
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e">The Event</param>
        private void OnAnyButtonMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var tagValue = ((string)((Button)sender).Tag).Split('|');

            //Sending the request
            SendHttpRequest(new Request(tagValue[0], BuildRequestServerPart() + tagValue[1]));
        }

        /// <summary>
        /// Event called whenever a query button is clicked.
        /// </summary>
        /// <param name="sender">The button containing the query</param>
        /// <param name="e">The click event</param>
        private void OnAnyButtonQueryClick(object sender, EventArgs e)
        {
            var tagValue = ((string)((Button)sender).Tag).Split('|');
            HttpRequestSelectedItem = HttpRequestItems.First(item => (string)item.Tag == tagValue[0]);
            m_query = tagValue[1];
            UrlUpdate(true);
        }

        /// <summary>
        /// Method to add the required entities before using the queries.
        /// </summary>
        /// <param name="sender">The button Yes</param>
        /// <param name="e">The event</param>
        private void OnButtonAddRequiredEntitiesClick(object sender, RoutedEventArgs e)
        {
            AddRequiredEntitiesEnable = false;
            AddRequiredEntities();
        }

        /// <summary>
        /// Open the file browser to select a picture to send.
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e">The click event</param>
        private void OnButtonBrowseClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = PNG_FILTER
            };

            if ((bool)fileDialog.ShowDialog(this))
            {
                ImagePath = fileDialog.FileName;
            }
        }

        /// <summary>
        /// Method called if the user doesn't want to add the required entities.
        /// </summary>
        /// <param name="sender">The button No</param>
        /// <param name="e">The click event</param>
        private void OnButtonDontAddRequiredEntitiesClick(object sender, RoutedEventArgs e)
        {
            OverlayForDemoEntities = Visibility.Hidden;
            BackupButtonToCreateRequiredEntities = Visibility.Visible;
        }

        /// <summary>
        /// The event called when the user click on the send request button.
        /// </summary>
        /// <param name="sender">The send button</param>
        /// <param name="e">The event</param>
        private void OnButtonSendClick(object sender, RoutedEventArgs e)
            => SendHttpRequest(new Request(HttpRequestSelectedItem.Tag as string, Url));

        /// <summary>
        /// Event for the button send picture. Will create a multipart request.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonSendPictureClick(object sender, RoutedEventArgs e)
        {
            if (!Guid.TryParse(MultipartRequestCardholderGuid, out var cardholderGuid))
            {
                MessageBox.Show("Given Guid is not valid as a guid.");
            }
            else
            {
                //Check if file exists
                if (File.Exists(ImagePath))
                {
                    var mpRequest = new CardholderImageMulitpartRequest(ImagePath, BuildRequestServerPart().ToString(), cardholderGuid, this);
                    mpRequest.UploadData();
                }
                else
                {
                    MessageBox.Show("Wrong file path");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Url.Contains("RaiseCustomEvent"))
            {
                MessageBox.Show("URL field should contain a RaiseCustomEvent type of command");
            }
            else
            {
                Dictionary<string, string> post = new Dictionary<string, string>();
                post.Add("payload", m_customEvenPayload.Text);

                var mpRequest = new BodyDictionnaryDataMultipartRequest(BuildRequestServerPart().ToString(), this);
                mpRequest.UploadData(Url, post);
            }

            
        }

        /// <summary>
        /// Show the overlay to let the user choose if he wants to add the required entities.
        /// </summary>
        /// <param name="sender">The button.</param>
        /// <param name="e">The event.</param>
        private void OnButtonShowOverlayClick(object sender, RoutedEventArgs e)
        {
            OverlayForDemoEntities = Visibility.Visible;
            BackupButtonToCreateRequiredEntities = Visibility.Hidden;
        }

        /// <summary>
        /// Method to start the event monitoring window.
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e">The click event</param>
        private void OnButtonStartMonitoringEventsClick(object sender, RoutedEventArgs e)
        {
            if (m_eventsMonitor == null)
            {
                m_eventsMonitor = new EventsMonitor(BuildRequestServerPart().ToString());
                m_eventsMonitor.Closed += OnEventMonitorClosed;
                m_eventsMonitor.Show();
            }
        }

        /// <summary>
        /// When the Checkbox for Https is checked or unchecked, we rewrite the Url.
        /// </summary>
        /// <param name="sender">The checkbox</param>
        /// <param name="e">The event</param>
        private void OnCheckBoxHttpsCheckedChange(object sender, EventArgs e)
            => UrlUpdate(false);

        private void OnComboBoxContentTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                Request.Accept = ((ComboBoxItem)e.AddedItems[0]).Tag.ToString();
        }

        /// <summary>
        /// Prevent the creation of multiple Event monitor window.
        /// </summary>
        /// <param name="sender">Event Monitor</param>
        /// <param name="e">EventArgs</param>
        private void OnEventMonitorClosed(object sender, EventArgs e)
            => m_eventsMonitor = null;

        /// <summary>
        /// Replace the current Url with the one double clicked.
        /// </summary>
        /// <param name="sender">ListItem</param>
        /// <param name="e">The event</param>
        private void OnListViewRequestsSentMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var obj = (DependencyObject)e.OriginalSource;

            while (obj != null && !Equals(obj, m_requestsSent))
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    // Set the HttpMethod.
                    HttpRequestSelectedItem = HttpRequestItems.FirstOrDefault(httpMethod => httpMethod.Content as string == RequestsSentSelectedItem.HttpMethod.ToString());
                    // Set the Request Url.
                    Url = RequestsSentSelectedItem.Url;
                    m_url.Select(m_url.Text.Length, 0);
                    m_url.Focus();
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        /// <summary>
        /// Event raised when the user change the password. Sets the Request class static password.
        /// </summary>
        /// <param name="sender">The password passwordBox.</param>
        /// <param name="e">The event.</param>
        private void OnPasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            Request.Password = m_Password.Password;
        }

        private void OnTextBoxApplicationIdTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplicationId = m_applicationId.Text;
        }

        /// <summary>
        /// When the baseuri textbox change, making sure to update the url.
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">TextChangedEventArgs</param>
        private void OnTextBoxBaseUriTextChanged(object sender, TextChangedEventArgs e)
        {
            BaseUri = ((TextBox)sender).Text;
            UrlUpdate(false);
        }

        /// <summary>
        /// Make sure the port is valid
        /// </summary>
        /// <param name="sender">The Textbox</param>
        /// <param name="e">The event</param>
        private void OnTextBoxPortPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Numeric.IsMatch(e.Text))
            {
                var numericPort = Convert.ToInt32(int.Parse(Port + e.Text));
                e.Handled = numericPort > HighestPort;
            }
            else
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Event called when the text changed in the Port textbox. The event updates the url.
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">Text changed</param>
        private void OnTextBoxPortTextChanged(object sender, TextChangedEventArgs e)
        {
            var value = ((TextBox)sender).Text;
            if (string.IsNullOrEmpty(value))
            {
                ((TextBox)sender).Text = "0";
            }
            else
            {
                Port = Convert.ToInt32(value);
                UrlUpdate(false);
            }
        }

        /// <summary>
        /// Event called when the text changed in the server textbox. The event updates the url.
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e">Text changed</param>
        private void OnTextBoxServerTextChanged(object sender, TextChangedEventArgs e)
        {
            Server = ((TextBox)sender).Text;
            UrlUpdate(false);
        }

        /// <summary>
        /// Event called when the user enters manually the query or modify an existing one. Will update the
        /// m_query variable.
        /// </summary>
        /// <param name="sender">The textbox Url</param>
        /// <param name="e">On key up event</param>
        private void OnTextBoxUrlKeyUp(object sender, KeyEventArgs e)
        {
            var splitedUrl = m_url.Text.Split('/');

            // Check to see if the slipped url gives 5 or more parts.
            if (splitedUrl.Length < NUMBER_OF_PARTS_DEFAULT)
                return;

            var count = 0;
            var query = new StringBuilder();
            foreach (var value in splitedUrl)
            {
                // Do nothing while it is the part of server.
                if (++count > NUMBER_OF_PARTS_BEFORE_QUERY)
                {
                    query.Append(value).Append("/");
                }
            }
            m_query = query.Remove(query.Length - 1, 1).ToString();

            if (e.Key == Key.Enter)
            {
                Url = m_url.Text;
                SendHttpRequest(new Request(HttpRequestSelectedItem.Tag as string, Url));
            }
        }

        private void OnTextBoxUsernameTextChanged(object sender, TextChangedEventArgs e)
        {
            Username = m_username.Text;
        }

        /// <summary>
        /// We make sure to clean the entities added from <see cref="AddRequiredEntities"/> when the sample is closed.
        /// </summary>
        private void OnWindowWebSdkStudioWindowClosing(object sender, CancelEventArgs e)
        {
            //Check if there is a need to clean
            if (m_askForCleanup)
            {
                var result =
                    MessageBox.Show(
                        CLOSING_MESSAGE_BOX_TEXT,
                        CLOSING_MESSAGE_BOX_TITLE,
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                switch (result)
                {
                    case MessageBoxResult.Yes:

                        //Reset properties to what we had to create them
                        Port = m_portForDeletionOfEntitiesGenerated;
                        Server = m_serverForDeletionOfEntitiesGenerated;
                        BaseUri = m_baseUriForDeletionOfEntitiesGenerated;
                        IsHttps = m_isHttpsForDeletionOfEntitiesGenerated;
                        m_query = string.Empty;

                        UrlUpdate(false);

                        //Remove the Door
                        var webRequest = CreateHttpWebRequest(new Request("DELETE", Url + "entity/" + DoorGuidSample));
                        webRequest.BeginGetResponse(asyncresult => { }, webRequest);

                        //Remove the Cardholder
                        webRequest = CreateHttpWebRequest(new Request("DELETE", Url + "entity/" + CardholderGuidSample));
                        webRequest.BeginGetResponse(asyncresult => { }, webRequest);

                        //Remove the CustomField
                        webRequest = CreateHttpWebRequest(new Request("DELETE", Url + "customField/Cardholder/CustomFieldWebSdkFilterSample"));
                        webRequest.BeginGetResponse(asyncresult => { }, webRequest);

                        try
                        {
                            //Acknowledge alarm part 1
                            webRequest = CreateHttpWebRequest(new Request("GET", Url + "report/AlarmActivity?q=Alarms@( " + AlarmGuidSample + "),States@Active"));
                            var response = webRequest.GetResponse();
                            var instanceIds = ParseInstanceIds(response);

                            var removeComa = false;
                            var sb = new StringBuilder(Url + "alarm?q=");
                            foreach (var instanceId in instanceIds)
                            {
                                removeComa = true;
                                sb.Append("AcknowledgeAlarm(")
                                    .Append(instanceId)
                                    .Append(",")
                                    .Append(AlarmGuidSample)
                                    .Append(", Ack),");
                            }
                            if (removeComa)
                            {
                                //Removes the last coma
                                sb.Length--;
                            }
                            //Acknowledge alarm part 2
                            webRequest = CreateHttpWebRequest(new Request("GET", sb.ToString()));
                            webRequest.GetResponse();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Could not Acknowledge the alarm.");
                        }

                        //Remove the Event
                        webRequest = CreateHttpWebRequest(new Request("DELETE", Url + "entity/" + AlarmGuidSample));
                        webRequest.BeginGetResponse(asyncresult => { }, webRequest);
                        break;

                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }

            m_eventsMonitor?.Close();
        }
        /// <summary>
        /// Parse the instanceIds out of the query for the alarms active.
        /// </summary>
        /// <param name="response">The WebResponse</param>
        /// <returns>Returns a list of int (Instance ids)</returns>
        private List<int> ParseInstanceIds(WebResponse response)
        {
            var instanceIds = new List<int>();
            using (var sr = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                instanceIds.AddRange(XDocument.Parse(sr.ReadToEnd())
                                        .Root?.Element("QueryResult")?.Elements("Row")
                                        .Select(xElement => int.Parse(xElement.Element("Cell")?.Value
                                        ?? throw new InvalidOperationException()))
                                        ?? throw new InvalidOperationException());
            }
            return instanceIds;
        }

        /// <summary>
        /// Here you will find all the Queries used in this Sample.
        /// All the queries are linked to the tag element on the Button.
        ///
        /// You might need to change the LogicalIds numbers or the Guids to fit your environnement.
        ///
        /// ** LogicalID(Type, Dd) and guids are the same. You can replace each other and it will still work. **
        /// So entity/exists/00000000-0000-0000-0000-000000000003 = entity/exists/LogicalId(Type, Id)
        /// <see cref="AdminGuid"/> for the Admin Guid.
        ///
        /// <see cref="OnAnyButtonQueryClick"/> for more information on what we do
        /// when we receive the event click on a query.
        /// </summary>
        private void PopulateQueryButtons()
        {
            // Entities Section
            // To view more Queries about entities, please refer to the documentation.
            // -> Web SDK -> Reference -> Entities
            EntityExistsByGuid = "GET|entity/exists/" + AdminGuid;
            GetEntityByLogicalId = "GET|entity/LogicalId(Door, 1)";
            GetEntityBasic = "GET|entity/basic/" + AdminGuid;
            GetEntityFull = "GET|entity/" + AdminGuid;
            MultiQueryRequest = "POST|entity?q=entity=NewEntity(Door),Name=MultiQueryRequestDoor1,entity=NewEntity(Door),Name=MultiQueryRequestDoor2";
            CreateEntity = "POST|entity?q=entity=NewEntity(Door),Name=MyNewDoor,LogicalId=1,Guid";
            DeleteEntity = "DELETE|entity/LogicalId(Door, 1)";
            ModifyEntity = "POST|entity?q=entity=LogicalId(door, 1),Name=MyNewDoorUpdated";
            CreateAccessRule = "POST|entity?q=entity=NewEntity(AccessRule,Permanent),Name=MyNewRule,Guid";
            // Cardholder Management Section
            CreateCardholder = "POST|entity?q=entity=NewEntity(Cardholder),LogicalId=2,Name=MyCardholder,FirstName=MyFirstName,LastName=MyLastName,EmailAddress=MyEmail@Email.com";
            CreateCredential = "POST|entity?q=entity=NewEntity(Credential),LogicalId=3,Name=MyPlateCredential,Format=LicensePlateCredentialFormat(WEBSDK),ActivationMode=RelativeDeactivation(6.00:00:00)";
            CreateVisitor = "POST|entity?q=entity=NewEntity(Visitor),Departure=" + DateTime.Now.AddDays(5).ToString("O");
            CreateCardholderGroup = "POST|entity?q=entity=NewEntity(CardholderGroup),Name=MyCardHolderGroup,LogicalId=4";
            // The add, remove and clear from a collection property are documented in Web sdk documentation section of entities.
            AssignCredentialToCardholder = "POST|entity?q=entity=LogicalId(Cardholder, 2),Credentials@LogicalId(Credential, 3)";
            UnAssignCredentialToCardholder = "POST|entity?q=entity=LogicalId(Cardholder, 2),Credentials-LogicalId(Credential, 3)";
            // Members@{Guid of the Cardholder}
            AssignCardholderToCardholderGroup = "POST|entity?q=entity=LogicalId(CardholderGroup, 4),AddCardholderIntoGroup(00000000-0000-0000-0000-000000000000)";
            UnAssignCardholderToCardholderGroup = "POST|entity?q=entity=LogicalId(CardholderGroup, 4),RemoveCardholderFromGroup(00000000-0000-0000-0000-000000000000)";

            // Action Manager Section
            var doorGuid = DoorGuidSample == Guid.Empty ? WaitingForGuid : DoorGuidSample.ToString();
            // This will send a message to a Config tool or security desk where the admin is connected. Note that we escaped the coma in the message using "\\".
            SendMessage = "POST|action?q=SendMessage(" + AdminGuid + ", Hi\\, this is a message)";
            // Will send an email to the email address specified in the user admin.
            SendEmail = "POST|action?q=SendEmail(" + AdminGuid + ", Hi\\, this is an email)";
            OpenDoor = "POST|action?q=Open(" + doorGuid + ")";
            GetTile = "POST|action?q=GetTile(65,3)";
            GetTiles = "POST|action?q=GetTile(65,1),GetTile(65,2),GetTile(65,3),GetTile(65,4)";
            DisplayInTile = "POST|action?q=DisplayInTile(65,1,<TileContentGroup><Contents><VideoContent Camera=\"00000001-0000-babe-0000-222222222222\" VideoMode=\"Live\" IsPaused=\"true\"/></Contents></TileContentGroup>)";

            // Events Section
            var alarmGuid = AlarmGuidSample == Guid.Empty ? WaitingForGuid : AlarmGuidSample.ToString();
            AddCustomEvent = "POST|entity?q=entity=" + SystemConfigurationGuid + ",CustomEvents.Add(5000,test,Camera)";
            RaiseCustomEvent = "POST|action?q=RaiseCustomEvent(CustomEventId(5000))";
            RemoveCustomEvent = "POST|entity?q=entity=" + SystemConfigurationGuid + ",CustomEvents-5000";
            // We use a door for this sample, you will have to change the Guid for it to work on your side.
            SubscribeToEvent = "GET|events/subscribe?q=event(" + alarmGuid + ", AlarmTriggered)";
            UnSubscribeFromEvent = "GET|events/unsubscribe?q=event(" + alarmGuid + ", AlarmTriggered)";
            SubscribeToAllEvent = "GET|events/subscribe?q=event(Alarm, AlarmTriggered)";
            UnSubscribeToAllEvent = "GET|events/unsubscribe?q=event(Alarm, AlarmTriggered)";
            SubscribedEvents = "GET|events/subscribed";
            RaiseAnEvent = "GET|alarm?q=TriggerAlarm(" + alarmGuid + ")";
            BuildAndRaiseEvent = "POST|events/RaiseEvent/VideoAnalyticsObjectConditionChange/{Guid}";
            BuildAndRaiseCustomEvent = "POST|events/RaiseCustomEvent/5000/00000000-0000-0000-0000-000000000003/This%20is%20the%20message";

            // Alarm Monitoring Section
            EnableAlarmMonitoring = "POST|events/alarmMonitoring/on";
            DisableAlarmMonitoring = "POST|events/alarmMonitoring/off";

            // Custom fields section
            var cardholderGuid = CardholderGuidSample == Guid.Empty ? WaitingForGuid : CardholderGuidSample.ToString();
            var customFieldGuid = CustomFieldGuidSample == Guid.Empty ? WaitingForGuid : CustomFieldGuidSample.ToString();
            CreateCustomField = "POST|customField/Cardholder/MyCustomField/Text/MyCustomFieldValue";
            DeleteCustomField = "DELETE|customField/Cardholder/MyCustomField";
            ModifyValueOfTheCustomField = "PUT|customField/" + cardholderGuid + "/MyCustomField/MyCustomFieldValueUpdated";
            GetValueOfTheCustomField = "GET|customField/" + cardholderGuid + "/MyCustomField";

            // Custom Entity section
            CreateCustomEntity = "POST|entity?q=entity=NewEntity(CustomEntity,{CustomEntityDescriptorGuid}),Name=NewCustomEntity,Guid";
            GetCustomEntityTypeDescriptor = "GET|customEntityType/{CustomEntityDescriptorGuid}";
            GetAllCustomEntityTypeDescriptor = "GET|customEntityType/all";
            DeleteCustomEntityTypeDescriptor = "DELETE|customEntityType/{CustomEntityDescriptorGuid}";

            // Reports
            CustomFieldFilter = "GET|report/EntityConfiguration?q=EntityTypes@Cardholder,CustomFields@CustomField(" + customFieldGuid + ",CustomFieldDefaultValue)";
            MotionQuery = "GET|report/VideoMotion?q=Cameras@{CameraGuid},MotionThreshold=1,ConsecutivesFrames=2,TimeRange.SetTimeRange(2020-01-06T10:00:00,2020-01-06T10:30:00),Mask=MotionQuery%2BMotionMask(2,2,{true,true,true,true})";
            GetPublicTasks = "GET|tasks/public";
            GetPrivateTasks = "GET|tasks/private";
            ExecutePublicTask = "POST|tasks/public/execute/{taskType}/{taskName}";
            ExecutePrivateTask = "POST|tasks/private/execute/{taskType}/{taskName}";
            ExecuteTask = "POST|tasks/execute/{taskGuid}";
            
            // Security
            GetWebTokenEncoded = "GET|GetWebTokenEncoded";

            // Security
            GetWebTokenDecoded = "GET|GetWebTokenDecoded";

            // License usage
            GetAllLicenseItemUsage = "GET|licenseItemUsage";
            GetCameraLicenseItemUsage = "GET|licenseItemUsage/Camera";
            
            // General
            CreationPartition = "POST|creationpartition/{PartitionGuid}";
        }

        /// <summary>
        /// This method is to send the <see cref="HttpWebRequest"/> to the Web Sdk.
        /// </summary>
        /// <param name="userRequest">The <see cref="Request"/> created from the event <see cref="OnButtonSendClick"/></param>
        private async void SendHttpRequest(Request userRequest)
        {
            //Create web request
            var webRequest = CreateHttpWebRequest(userRequest);

            userRequest.Response = await GetHttpResponse(webRequest);

            RequestsSent.Add(userRequest);
            RequestsSentSelectedItem = RequestsSent.Last();
            m_requestsSent.ScrollIntoView(RequestsSentSelectedItem);
        }

        /// <summary>
        /// Method to update the Url when new information is given
        /// </summary>
        private void UrlUpdate(bool focusOnUrlTextbox)
        {
            Url = BuildRequestServerPart() + m_query;
            if (!focusOnUrlTextbox)
                return;
            m_url.Select(m_url.Text.Length, 0);
            m_url.Focus();
        }

        #endregion Private Methods

        
    }

}
