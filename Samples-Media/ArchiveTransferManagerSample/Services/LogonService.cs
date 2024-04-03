using Genetec.Sdk;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Workflows.LoginManagers;
using System;
using System.Windows;

namespace ArchiveTransferManagerSample.Services
{
    /// <summary>
    /// Simple class the remove the login related stuff from the MainWindow
    /// </summary>
    public class LoginService
    {
        private readonly LoginManager m_loginManager;

        public event EventHandler<LoggedOnEventArgs> OnSuccessfullyLoggedIn;

        public LoginService(LoginManager loginManager)
        {
            m_loginManager = loginManager ?? throw new ArgumentNullException(nameof(loginManager));
            m_loginManager.LoggedOn += OnEngineLoggedOn;
            m_loginManager.LogonFailed += OnEngineLogonFailed;
            m_loginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
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
            MessageBox.Show(
                e.FormattedErrorMessage,
                "Unable to connect",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
            => OnSuccessfullyLoggedIn?.Invoke(this, e);

        public void Logon(string username, string password, string server = "")
            => m_loginManager.LogOn(server, username, password);
    }
}
