using CredentialFormatsSample.Annotations;
using CredentialFormatsSample.Models;
using GalaSoft.MvvmLight.Command;
using Genetec.Sdk;
using Genetec.Sdk.AccessControl.Credentials.CardCredentials;
using Genetec.Sdk.AccessControl.Helpers;
using Genetec.Sdk.Credentials;
using Genetec.Sdk.Diagnostics;
using Genetec.Sdk.Diagnostics.Logging.Core;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CredentialFormatsSample
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<FormatTypesModel> CredentialFormats { get; set; }
        public ObservableCollection<CredentialModel> Credentials { get; set; }
        public ObservableCollection<string> SearchCredentialResults { get; set; }

        private FormatTypesModel m_selectedFormatType;
        public FormatTypesModel SelectedFormatType
        {
            get { return m_selectedFormatType; }
            set
            {
                m_selectedFormatType = value;
                OnPropertyChanged();
            }
        }

        private CredentialModel m_selectedCredentialModel;
        public CredentialModel SelectedCredentialModel
        {
            get
            {
                return m_selectedCredentialModel;
            }
            set
            {
                m_selectedCredentialModel = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand m_createCredentialFromFieldsCommand;
        public ICommand CreateCredentialFromFieldsCommand
        {
            get
            {
                if(m_createCredentialFromFieldsCommand==null)
                    m_createCredentialFromFieldsCommand=new RelayCommand(OnCreateCredentialFromFields);
                return m_createCredentialFromFieldsCommand;
            }
        }

        private void OnCreateCredentialFromFields()
        {
            if (SelectedFormatType == null) return;

            try
            {
                m_sdkEngine.TransactionManager.ExecuteTransaction(() =>
                {
                    try
                    {
                        var credentialBuilder = m_sdkEngine.EntityManager.GetCredentialBuilder();
                        credentialBuilder.SetName("Credential From Test");
                        CredentialFormat format = null;
                        switch (SelectedFormatType.Name)
                        {
                            case "Standard 26 bits":
                                format = new WiegandStandardCredentialFormat(
                                    int.Parse(SelectedFormatType.Fields[0].FieldValue),
                                    int.Parse(SelectedFormatType.Fields[1].FieldValue));
                                break;
                            case "HID H10306 34 Bits":
                                format = new WiegandH10306CredentialFormat(
                                    int.Parse(SelectedFormatType.Fields[0].FieldValue),
                                    int.Parse(SelectedFormatType.Fields[1].FieldValue));
                                break;
                            case "HID H10302 37 Bits":
                                format = new WiegandH10302CredentialFormat(
                                    int.Parse(SelectedFormatType.Fields[0].FieldValue));
                                break;
                            case "HID H10304 37 Bits":
                                format = new WiegandH10304CredentialFormat(
                                    int.Parse(SelectedFormatType.Fields[0].FieldValue),
                                    int.Parse(SelectedFormatType.Fields[1].FieldValue));
                                break;
                            case "HID Corporate 1000 35 Bits":
                                format = new WiegandCorporate1000CredentialFormat(
                                    int.Parse(SelectedFormatType.Fields[0].FieldValue),
                                    int.Parse(SelectedFormatType.Fields[1].FieldValue));
                                break;
                            case "HID Corporate 1000 48 Bits":
                                format = new Wiegand48BitCorporate1000CredentialFormat(
                                    int.Parse(SelectedFormatType.Fields[0].FieldValue),
                                    int.Parse(SelectedFormatType.Fields[1].FieldValue));
                                break;
                            case "CSN":
                                format = new WiegandCsn32CredentialFormat(
                                    long.Parse(SelectedFormatType.Fields[0].FieldValue,
                                        NumberStyles.AllowHexSpecifier));
                                break;
                            case "FASC-N 75 bits":
                                // We create a dictionary and fill it.
                                var fascN75dictionary = new Dictionary<string, string>
                                {
                                    { FascN75BitCardCredentialFormat.AGENCY_CODE_FIELD_NAME, SelectedFormatType.Fields[0].FieldValue },
                                    { FascN75BitCardCredentialFormat.SYSTEM_CODE_FIELD_NAME, SelectedFormatType.Fields[1].FieldValue },
                                    { FascN75BitCardCredentialFormat.CREDENTIAL_NUMBER_FIELD_NAME, SelectedFormatType.Fields[2].FieldValue },
                                    { FascN75BitCardCredentialFormat.EXP_DATE_FIELD_NAME, SelectedFormatType.Fields[3].FieldValue }
                                };
                                format = new FascN75BitCardCredentialFormat(fascN75dictionary);
                                break;
                            case "FASC-N 200 bits":
                                // We create a dictionary and fill it.
                                var fascN200Dictionary = new Dictionary<string, string>
                                {
                                    { FascN200BitCardCredentialFormat.AGENCY_CODE_FIELD_NAME, SelectedFormatType.Fields[0].FieldValue },
                                    { FascN200BitCardCredentialFormat.SYSTEM_CODE_FIELD_NAME, SelectedFormatType.Fields[1].FieldValue },
                                    { FascN200BitCardCredentialFormat.CREDENTIAL_NUMBER_FIELD_NAME, SelectedFormatType.Fields[2].FieldValue },
                                    { FascN200BitCardCredentialFormat.CS_FIELD_NAME, SelectedFormatType.Fields[3].FieldValue },
                                    { FascN200BitCardCredentialFormat.ICI_FIELD_NAME, SelectedFormatType.Fields[4].FieldValue },
                                    { FascN200BitCardCredentialFormat.PI_FIELD_NAME, SelectedFormatType.Fields[5].FieldValue },
                                    { FascN200BitCardCredentialFormat.OC_FIELD_NAME, SelectedFormatType.Fields[6].FieldValue },
                                    { FascN200BitCardCredentialFormat.OI_FIELD_NAME, SelectedFormatType.Fields[7].FieldValue },
                                    { FascN200BitCardCredentialFormat.POA_FIELD_NAME, SelectedFormatType.Fields[8].FieldValue },
                                    { FascN200BitCardCredentialFormat.LRC_FIELD_NAME, SelectedFormatType.Fields[9].FieldValue },
                                };
                                format = new FascN200BitCardCredentialFormat(fascN200Dictionary);
                                break;
                            case "Raw data":
                                int bitlength = -1;
                                int.TryParse(SelectedFormatType.Fields[1].FieldValue, out bitlength);
                                if (bitlength <= 0)
                                    throw new Exception("Bit Length must be an integer > 0");
                                format = new RawCardCredentialFormat(SelectedFormatType.Fields[0].FieldValue, bitlength);
                                break;
                            default:
                                format = new CustomCredentialFormat(SelectedFormatType.Id,
                                    SelectedFormatType.Fields.ToDictionary(key => key.FieldName,
                                        value => value.FieldValue));
                                break;
                        }
                        credentialBuilder.SetFormat(format);
                        credentialBuilder.Build();
                    }
                    catch (Exception e)
                    { 
                        MessageBox.Show(e.Message);
                    }
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private RelayCommand m_searchCredentialCommand;
        public ICommand SearchCredentialCommand
        {
            get
            {
                if (m_searchCredentialCommand == null)
                    m_searchCredentialCommand = new RelayCommand(OnSearchCredential);
                return m_searchCredentialCommand;
            }
        }

        private void OnSearchCredential()
        {
            if(SelectedCredentialModel != null)
            {
                var credentialConfigurationQuery = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.CredentialConfiguration) as CredentialConfigurationQuery;
                credentialConfigurationQuery.UniqueIds.Add(SelectedCredentialModel.UniqueId);
                credentialConfigurationQuery.BeginQuery(OnSearchCredentiaReceived, credentialConfigurationQuery);
            }
        }

        private void OnSearchCredentiaReceived(IAsyncResult ar)
        {
            var query = ar.AsyncState as CredentialConfigurationQuery;
            var results = query.EndQuery(ar);
            var sb = new StringBuilder();
            sb.AppendLine("Search credential received with " + results.Data.Rows.Count + " results: ");
            foreach (DataRow dataRow in results.Data.Rows)
            {
                Guid credentialGuid = (Guid) dataRow[0];
                Credential cred = m_sdkEngine.GetEntity(credentialGuid) as Credential;

                sb.AppendLine("Credential " + cred.Name + " with format " + cred.Format.GetType().Name);
            }

            Dispatcher.BeginInvoke(new Action(() => SearchCredentialResults.Insert(0, sb.ToString())));
        }

        private RelayCommand m_refreshCredentialsCommand;

        public ICommand RefreshCredentialCommand
        {
            get
            {
                if(m_refreshCredentialsCommand == null)
                    m_refreshCredentialsCommand=new RelayCommand(OnRefreshCredentials);
                return m_refreshCredentialsCommand;
            }
        }

        private void OnRefreshCredentials()
        {
            GetListOfCredentialFormats();
            PrefetchCredentials();
        }

        private RelayCommand m_copyCredentialCommand;
        public ICommand CopyCredentialCommand
        {
            get
            {
                if (m_copyCredentialCommand == null)
                    m_copyCredentialCommand = new RelayCommand(OnCopyCredential);
                return m_copyCredentialCommand;
            }
        }

        private void OnCopyCredential()
        {
            if (SelectedCredentialModel == null) return;
            try
            {
                var model = new CredentialFormatModel
                {
                    FormatId = SelectedCredentialModel.FormatId,
                    EncodedData = SelectedCredentialModel.EncodedData,
                    BitLength = SelectedCredentialModel.BitLength,
                    Type = SelectedCredentialModel.CredentialType
                };

                var format = CredentialFormatHelper.CreateCredentialFormatFromModel(model);
                m_sdkEngine.TransactionManager.ExecuteTransaction(() =>
                {
                    try
                    {
                        var builder = m_sdkEngine.EntityManager.GetCredentialBuilder();
                        builder.SetName("Copied").SetFormat(format);
                        builder.Build();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                });
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #region Constants

        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Fields

        private readonly Logger m_logger;

        #endregion

        #region Constructors

        static MainWindow()
        {
            ActivateLogging();
        }

        public MainWindow()
        {
            DataContext = this;
            CredentialFormats = new ObservableCollection<FormatTypesModel>();
            Credentials = new ObservableCollection<CredentialModel>();
            SearchCredentialResults = new ObservableCollection<string>();

            InitializeComponent();
            m_logger = Logger.CreateInstanceLogger(this);
            // Register to important events
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;

            // Logon to Sdk engine
            string server = "";
            string username = "admin";
            string password = "";
            m_sdkEngine.LoginManager.LogOn(server, username, password);
        }

        #endregion

        #region Event Handlers

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Console.WriteLine("Sdk has logged off");
            m_logger.TraceDebug("Sdk has logged off");
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Console.WriteLine(e.UserName + " has logged on to " + e.ServerName);
            m_logger.TraceDebug(e.UserName + " has logged on to " + e.ServerName);

            GetListOfCredentialFormats();
            PrefetchCredentials();
        }

        private void PrefetchCredentials(int page = 1)
        {
            if (page == 1)
                Dispatcher.BeginInvoke(new Action(() => Credentials.Clear()));
            
            var entityConfigurationQuery = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            entityConfigurationQuery.Page = page;
            entityConfigurationQuery.PageSize = 1000;
            entityConfigurationQuery.EntityTypeFilter.Add(EntityType.Credential);
            entityConfigurationQuery.DownloadAllRelatedData = true;
            entityConfigurationQuery.BeginQuery(OnCredentialPrefetchReceived, entityConfigurationQuery);
        }

        private void OnCredentialPrefetchReceived(IAsyncResult ar)
        {
            var query = ar.AsyncState as EntityConfigurationQuery;
            var results = query.EndQuery(ar);
            foreach (DataRow dataRow in results.Data.Rows)
            {
                var credential = m_sdkEngine.GetEntity((Guid)dataRow[0]) as Credential;
                if(credential == null || credential.Format == null) continue;

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (!Credentials.Any(x => x.Guid == credential.Guid))
                    {
                        var model = CredentialFormatHelper.ExtractModelFromCredentialFormat(credential.Format);
                        Credentials.Add(new CredentialModel
                        {
                            Guid = credential.Guid,
                            EntityName = credential.Name,
                            FormatId = model.FormatId,
                            BitLength = model.BitLength,
                            CredentialType = model.Type,
                            EncodedData = model.EncodedData,
                            UniqueId = credential.Format.UniqueId,
                            Icon = credential.GetIcon(true)
                        });
                    }
                }));
            }

            if (results.Data.Rows.Count >= 1000)
                PrefetchCredentials(query.Page + 1);
        }

        private void GetListOfCredentialFormats()
        {
            Dispatcher.BeginInvoke(new Action(() => CredentialFormats.Clear()));

            var systemConfig = m_sdkEngine.GetEntity<SystemConfiguration>(SdkGuids.SystemConfiguration);

            if (systemConfig == null) return;
            foreach (var credentialFormat in systemConfig.CredentialFormats)
            {
                var model = new FormatTypesModel
                {
                    Id = credentialFormat.FormatId,
                    Name = credentialFormat.Name
                };

                var customCredentialFormat = credentialFormat as CustomCredentialFormat;
                if(customCredentialFormat != null)
                {
                    foreach (var field in customCredentialFormat.UnfixedFields)
                    {
                        model.Fields.Add(new FieldModel(field, string.Empty));
                    }

                    Dispatcher.BeginInvoke(new Action(() => CredentialFormats.Add(model)));
                }
            }
            AppendRawCredentialFormat();
        }

        private void AppendRawCredentialFormat()
        {
            var rawFormatModel = new FormatTypesModel
            {
                Id = Guid.Empty,
                Name = "Raw data"
            };
            rawFormatModel.Fields.Add(new FieldModel("Raw Data",""));
            rawFormatModel.Fields.Add(new FieldModel("Bit Length",""));

            Dispatcher.BeginInvoke(new Action(() => CredentialFormats.Add(rawFormatModel)));
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            MessageBox.Show(e.FormattedErrorMessage);
            m_logger.TraceDebug(e.FormattedErrorMessage);
        }

        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            Console.WriteLine("Server : " + e.ServerName + ". Status changed to " + e.Status);
            m_logger.TraceDebug("Server : " + e.ServerName + ". Status changed to " + e.Status);
        }

        #endregion

        #region Public Methods

        public static void ActivateLogging()
        {
            // Logs can be found in the Logs subfolder
            var logServer = DiagnosticServer.Instance;
            logServer.AddFileTracing(new[] { new LoggerTraces("CredentialFormatsSample*", LogSeverity.Full) });
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}
