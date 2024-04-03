using Genetec.Sdk;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Queries;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace Listener
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        /// <summary>
        /// Information displayed in the UI
        /// </summary>
        public ObservableCollection<string> DisplayInformation { get; } = new ObservableCollection<string>();

        /// <summary>
        /// Represent the SDK class used to control Security Center
        /// </summary>
        private readonly Engine m_sdkEngine;

        public MainWindow()
        {
            // UI related stuff
            InitializeComponent();
            Title = "Listener";
            DataContext = this;

            // Create the engine and it's hooks for login
            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;

            // Also listen for events
            m_sdkEngine.EventReceived += OnEngineEventReceived;
        }

        private void OnEngineEventReceived(object sender, EventReceivedEventArgs e)
        {
            var entity = m_sdkEngine.GetEntity(e.SourceGuid);
            if (entity != null)
            {
                DisplayInformation.Add($"{e.Timestamp} {e.EventType} on {entity.Name}");
            }
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            SetUIWorkInProgress(false);
            ExecuteOnUIThread(() => DisplayInformation.Clear());
            FetchEntity();
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
            => SetUIWorkInProgress(false);

        private void LogButton_Unchecked(object sender, RoutedEventArgs e)
            => m_sdkEngine.LoginManager.LogOff();

        private async void LogButton_Checked(object sender, RoutedEventArgs e)
        {
            SetUIWorkInProgress(true);
            await m_sdkEngine.LoginManager.LogOnAsync(string.Empty, "admin", string.Empty);
        }

        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            var result = MessageBox.Show(
                "The identity of the Directory server cannot be verified. \n" +
                "The certificate is not from a trusted certifying authority. \n" +
                "Do you trust this server?", 
                "Secure Communication",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );

            e.AcceptDirectory = result == MessageBoxResult.Yes;
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            ExecuteOnUIThread(() => LogButton.IsChecked = false);
            SetUIWorkInProgress(false);

            MessageBox.Show(
                e.FormattedErrorMessage,
                "Unable to connect",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        /// <summary>
        /// Fetches the entities from Security Center
        /// </summary>
        /// <param name="page">The current desired page from the page query data.</param>
        private void FetchEntity(int page = 1)
        {
            ExecuteOnUIThread(() => DisplayInformation.Clear());
            SetUIWorkInProgress(true);

            var allEntityTypes = Enum.GetValues(typeof(EntityType))
                .Cast<EntityType>()
                .Where(e => e != EntityType.None)
                .Distinct()
                .ToArray();

            var entityConfigurationQuery = (EntityConfigurationQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);
            entityConfigurationQuery.Page = page;                              //The 1 based page index that will be received in the callback
            entityConfigurationQuery.PageSize = 1000;                          //It's usually the size of page that SecurityCenter likes.
            entityConfigurationQuery.EntityTypeFilter.AddRange(allEntityTypes);//The type of entities for which we want to have data.
            entityConfigurationQuery.DownloadAllRelatedData = false;           //Be careful with this it downloads a lot of data, depending on the entities.

            entityConfigurationQuery.BeginQuery(OnEntityQueryResultsReceived, entityConfigurationQuery);
        }

        private void OnEntityQueryResultsReceived(IAsyncResult ar)
        {
            var query = ar.AsyncState as EntityConfigurationQuery;
            var results = query.EndQuery(ar);
            var entities = results.Data.Rows
                                  .Cast<DataRow>()
                                  .Select(row => (Guid)row[0])                 // First row of the query is the guid
                                  .Select(guid => m_sdkEngine.GetEntity(guid)) // Get the entities into the engine cache, now they are synchronized with the server
                                  .Where(entity => entity != null)             // Filter out the potential nulls
                                  .ToArray();

            // Update the display on the UI thread
            ExecuteOnUIThread(() =>
            {
                foreach (var e in entities)
                {
                    DisplayInformation.Add($"Name : {e.Name}, Type : {e.EntityType}");
                }
            });

            // If there is more to query re-fetch the entities with an higher page number
            if (results.Data.Rows.Count >= 1000)
                FetchEntity(query.Page + 1);
            else
            {
                // Otherwise we are done and unlock the UI
                SetUIWorkInProgress(false);
            }
        }

        #region UI Related Methods

        /// <summary>
        /// Set the UI elements to indicate that something is in progress.
        /// This sets the Progressbar and lock or unlock the login/logout button.
        /// </summary>
        /// <param name="connectionInProgress">Indicates whether the UI is waiting for progress.</param>
        private void SetUIWorkInProgress(bool connectionInProgress)
            => ExecuteOnUIThread(() =>
            {
                LogButton.IsEnabled = !connectionInProgress;
                ConnectionProgressBar.IsIndeterminate = connectionInProgress;
            });

        /// <summary>
        /// Forces the execution of an <see cref="Action"/> to be executed on the UI thread.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to perform on the UI thread.</param>
        private void ExecuteOnUIThread(Action action)
            => Application.Current.Dispatcher.Invoke(action);

        #endregion IU
    }
}
