using GalaSoft.MvvmLight.CommandWpf;
using Genetec.Sdk;
using Genetec.Sdk.Credentials;
using Genetec.Sdk.Entities;
using System;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace CardholderAndCredentialStatusSample
{
    public class CredentialStatus
    {
        #region Fields

        private Engine m_sdkEngine;

        private Credential m_credential;

        private RelayCommand m_activateNowCommand;

        private RelayCommand m_activateFutureCommand;

        private RelayCommand m_activatePeriodCommand;

        private RelayCommand m_deactivateNowCommand;

        private RelayCommand m_deactivateFutureCommand;

        private RelayCommand m_expireOnFirstUseCommand;

        private RelayCommand m_expirationToNeverCommand;

        private RelayCommand m_expireWhenNotUsedCommand;

        private RelayCommand m_lostCommand;

        private RelayCommand m_stolenCommand;

        private RelayCommand m_properties;

        #endregion

        #region Properties

        public ICommand ActivateNowCommand => m_activateNowCommand ?? (m_activateNowCommand = new RelayCommand(ActivateNow));
        public ICommand ActivateFutureCommand => m_activateFutureCommand ?? (m_activateFutureCommand = new RelayCommand(ActivateFuture));
        public ICommand ActivatePeriodCommand => m_activatePeriodCommand ?? (m_activatePeriodCommand = new RelayCommand(ActivatePeriod));
        public ICommand DeactivateNowCommand => m_deactivateNowCommand ?? (m_deactivateNowCommand = new RelayCommand(DeactivateNow));
        public ICommand DeactivateFutureCommand => m_deactivateFutureCommand ?? (m_deactivateFutureCommand = new RelayCommand(DeactivateFuture));
        public ICommand ExpireOnFirstUseCommand => m_expireOnFirstUseCommand ?? (m_expireOnFirstUseCommand = new RelayCommand(ExpireOnFirstUse));
        public ICommand ExpirationToNeverCommand => m_expirationToNeverCommand ?? (m_expirationToNeverCommand = new RelayCommand(ExpirationToNever));
        public ICommand ExpireWhenNotUsedCommand => m_expireWhenNotUsedCommand ?? (m_expireWhenNotUsedCommand = new RelayCommand(ExpireWhenNotUsed));
        public ICommand LostCommand => m_lostCommand ?? (m_lostCommand = new RelayCommand(Lost));
        public ICommand StolenCommand => m_stolenCommand ?? (m_stolenCommand = new RelayCommand(Stolen));
        public ICommand PropertiesCommand => m_properties ?? (m_properties = new RelayCommand(Properties));

        #endregion

        #region Public Methods

        public void Initialize(Engine engine)
        {
            m_sdkEngine = engine;
        }

        public void Dispose()
        {
            if (m_credential != null)
                m_sdkEngine.EntityManager.DeleteEntity(m_credential);
        }

        public void ActivateNow()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.Activate();
        }

        public void ActivateFuture()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.Activate(DateTime.UtcNow.AddSeconds(30));
        }

        public void ActivatePeriod()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.Activate(DateTime.UtcNow.AddSeconds(15), DateTime.UtcNow.AddSeconds(45));
        }

        public void DeactivateNow()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.Deactivate();
        }

        public void DeactivateFuture()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.Deactivate(DateTime.UtcNow.AddSeconds(45));
        }

        public void ExpireOnFirstUse()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.ExpireOnFirstUseInDays(1);
        }

        public void ExpirationToNever()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.SetExpirationToNever();
        }

        public void ExpireWhenNotUsed()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.ExpireWhenNotUsedInDays(3);
        }

        public void Lost()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.Lost();
        }

        public void Stolen()
        {
            if (m_credential == null)
                CreateCredential();

            m_credential.Status.Stolen();
        }

        public void Properties()
        {
            if (m_credential == null)
                CreateCredential();

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("Credential Properties: \n\n");

            var activationDate = m_credential.Status.ActivationDate;
            if (activationDate == null)
                stringBuilder.AppendLine("Activation date: Null");
            else
            {
                var adt = DateTime.SpecifyKind((DateTime)activationDate, DateTimeKind.Utc);
                stringBuilder.AppendLine($"Activation date: {adt.ToLocalTime()}");
            }

            var activationType = m_credential.Status.ActivationType;
            stringBuilder.AppendLine($"Activation type: {activationType}");

            var expirationDate = m_credential.Status.ExpirationDate;
            if (expirationDate == null)
                stringBuilder.AppendLine("Expiration date: Null");
            else
            {
                var edt = DateTime.SpecifyKind((DateTime)expirationDate, DateTimeKind.Utc);
                stringBuilder.AppendLine($"Expiration date: {edt.ToLocalTime()}");
            }

            var expirationType = m_credential.Status.ExpirationType;
            stringBuilder.AppendLine($"Expiration type: {expirationType}");

            var expirationDuration = m_credential.Status.ExpirationDuration;
            stringBuilder.AppendLine(expirationDuration == null ? "Expiration duration: Null" : $"Expiration duration: {expirationDuration}");

            var state = m_credential.Status.State;
            stringBuilder.AppendLine($"State: {state}");

            MessageBox.Show(stringBuilder.ToString());
        }

        #endregion

        #region Private Methods

        private void CreateCredential()
        {
            var credentialBuilder = m_sdkEngine.EntityManager.GetCredentialBuilder();
            credentialBuilder.SetName($"Credential {DateTime.Now}");
            var format = new WiegandStandardCredentialFormat(1, 1);
            credentialBuilder.SetFormat(format);
            m_credential = credentialBuilder.Build();
        }

        #endregion
    }
}
