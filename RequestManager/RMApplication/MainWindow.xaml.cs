// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Workflows;
using RMSerialization;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Application = Genetec.Sdk.Entities.Application;

namespace RMApplication
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region Public Fields

        public static readonly DependencyProperty ConnectContentProperty = DependencyProperty.Register(
                    "ConnectContent", typeof(string), typeof(MainWindow), new PropertyMetadata("Connect"));

        public static readonly DependencyProperty IsSdkEngineConnectedProperty = DependencyProperty.Register(
                    "IsSdkEngineConnected", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty MessageToSendProperty =
                                                    DependencyProperty.Register("MessageToSend", typeof(string), typeof(MainWindow), new PropertyMetadata(""));

        public static readonly DependencyProperty ReceivedTextProperty =
                                                    DependencyProperty.Register("ReceivedText", typeof(string), typeof(MainWindow), new PropertyMetadata(""));

        public static readonly DependencyProperty RemoteEndPointSelectedIndexProperty = DependencyProperty.Register(
                    "RemoteEndPointSelectedIndex", typeof(int), typeof(MainWindow), new PropertyMetadata(default(int)));

        #endregion Public Fields

        #region Private Fields

        /// <summary>
        /// The Engine.
        /// </summary>
        private readonly Engine m_sdkEngine = new Engine();

        private IAsyncResult m_loggingOnResult;

        #endregion Private Fields

        #region Public Properties

        public string ConnectContent
        {
            get => (string)GetValue(ConnectContentProperty);
            set => SetValue(ConnectContentProperty, value);
        }

        public bool IsSdkEngineConnected
        {
            get => (bool)GetValue(IsSdkEngineConnectedProperty);
            set => SetValue(IsSdkEngineConnectedProperty, value);
        }
        public string MessageToSend
        {
            get => (string)GetValue(MessageToSendProperty);
            set => SetValue(MessageToSendProperty, value);
        }

        public string ReceivedText
        {
            get => (string)GetValue(ReceivedTextProperty);
            set => SetValue(ReceivedTextProperty, value);
        }

        public ObservableCollection<ComboBoxItem> RemoteEndPointItems { get; private set; }

        public int RemoteEndPointSelectedIndex
        {
            get => (int)GetValue(RemoteEndPointSelectedIndexProperty);
            set => SetValue(RemoteEndPointSelectedIndexProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Default constructor for the Class MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            RemoteEndPointItems = new ObservableCollection<ComboBoxItem>();
            DataContext = this;
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// Handle the request of ChatMessage.
        /// </summary>
        /// <param name="req">The request.</param>
        /// <param name="ret">The response of completing the request.</param>
        private void HandleRequest(ChatMessage req, RequestCompletion<bool> ret)
        {
            ret.SetResponse(true);
            Dispatcher?.BeginInvoke((Action)(() => ShowMessageUi(req)));
        }

        /// <summary>
        /// When the user clicks on the button connect, this event is raised.
        /// </summary>
        /// <param name="sender">The button.</param>
        /// <param name="e">The event.</param>
        /// <remarks>
        /// This method act as a Toggle for the connection. If it is not connected, it will connect it. Otherwise it is disconnecting it.
        /// </remarks>
        private void OnButtonConnectClick(object sender, RoutedEventArgs e)
        {
            if (m_loggingOnResult != null)
            {
                ConnectContent = "Connect";
                m_sdkEngine.LoginManager.EndLogOn(m_loggingOnResult);
                m_loggingOnResult = null;
                return;
            }

            // The connection toggle is here.
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                ConnectContent = "Connecting... Click to cancel";
                m_loggingOnResult = (checkboxWindowsCredentials.IsChecked == true)
                    ? m_sdkEngine.LoginManager.BeginLogOnUsingWindowsCredential(directory.Text)
                    : m_sdkEngine.LoginManager.BeginLogOn(directory.Text, userName.Text, password.Password);
            }
            else
            {
                m_sdkEngine.LoginManager.LogOff();
            }
        }

        /// <summary>
        /// Event for the button Send. It sends the message to the Engine.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The event.</param>
        private void OnButtonSendClick(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        /// <summary>
        /// Event raised when the Engine is logged off.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            m_loggingOnResult = null;
            ConnectContent = "Connect";
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;
            Action<ChatMessage, RequestCompletion<bool>> handleRequest = HandleRequest;
            m_sdkEngine.RequestManager.RemoveRequestHandler(handleRequest);
            RemoteEndPointItems.Clear();
            ReceivedText += "Sdk Engine logged off\n";
        }

        /// <summary>
        /// Event raised when the Engine is logged on.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_loggingOnResult = null;
            ConnectContent = "Disconnect";
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;

            ReceivedText += "Sdk Engine logged on\n";
            Action<ChatMessage, RequestCompletion<bool>> handleRequest = HandleRequest;
            m_sdkEngine.RequestManager.AddRequestHandler(handleRequest);
            PopulateApplicationsList();
        }

        /// <summary>
        /// Event raised when the Engine logon failed.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            m_loggingOnResult = null;
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;
            ConnectContent = "Connect";
            MessageBox.Show(e.FormattedErrorMessage);
        }

        /// <summary>
        /// Checks to see if the user inputs Enter. If he does, send the message.
        /// </summary>
        /// <param name="sender">The TextBox.</param>
        /// <param name="e">The Event.</param>
        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }
        /// <summary>
        /// Method to populate the Application List.
        /// </summary>
        private void PopulateApplicationsList()
        {
            RemoteEndPointItems.Clear();
            if (!(m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) is EntityConfigurationQuery query))
                return;

            query.EntityTypeFilter.Add(EntityType.Application);

            var results = query.Query();
            foreach (DataRow row in results.Data.Rows)
            {
                var item = new ComboBoxItem();
                var app = (Application)m_sdkEngine.GetEntity((Guid)row[0]);
                if (app.ApplicationType == ApplicationType.SecurityDesk || app.ApplicationType == ApplicationType.ConfigTool)
                {
                    item.Content = app.Name;
                    item.Tag = app.Guid;
                    RemoteEndPointItems.Add(item);
                }
            }
            RemoteEndPointSelectedIndex = 0;
        }

        /// <summary>
        /// Prints a message on the UI.
        /// </summary>
        /// <param name="author">The author of the message.</param>
        /// <param name="message">The message</param>
        private void PrintMessage(string author, string message)
        {
            ReceivedText += author + " : " + message + "\n";
            richtextboxMessages.ScrollToEnd();
        }

        /// <summary>
        /// Send a ChatMessage request to the engine.
        /// </summary>
        private void SendMessage()
        {
            var msg = new ChatMessage { Content = MessageToSend, TimeStamp = DateTime.UtcNow, AppGuid = m_sdkEngine.ClientGuid };
            try
            {
                // In this sample, the return value is a boolean confirming that the message was received.
                // In fact, if the message is not received, the call throws an exception.
                if (m_sdkEngine.RequestManager.SendRequest<ChatMessage, bool>((Guid)(((ComboBoxItem)comboboxEndPoint.SelectedItem).Tag), msg))
                {
                    PrintMessage("Me", msg.ToString());
                    MessageToSend = "";
                }
            }
            catch (Exception)
            {
                PrintMessage("System", new ChatMessage { Content = "Invalid end point", TimeStamp = DateTime.UtcNow, AppGuid = Guid.Empty }.ToString());
            }
        }

        /// <summary>
        /// Show a message receive on the Ui.
        /// </summary>
        /// <param name="msg">The message received.</param>
        private void ShowMessageUi(ChatMessage msg)
        {
            PrintMessage("Them", msg.ToString());
        }

        #endregion Private Methods

    }

}