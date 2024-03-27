using Genetec.Sdk.EventsArgs;
using System;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    public partial class LogonDlg : Form
    {
        #region Fields

        /// <summary>
        /// Represent the SDK class used to control Security Center
        /// </summary>
        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public LogonDlg()
        {
            InitializeComponent();

            m_directoryInput.Text = Environment.MachineName;
        }

        #endregion

        #region Event Handlers

        private void OnButtonConnectClick(object sender, EventArgs e)
        {
            if (m_sdkEngine == null)
                throw new InvalidOperationException("Call Initialize first");

            m_connect.Enabled = false;

            // Connect to Security Center
            m_sdkEngine.LoginManager.BeginLogOn(m_directoryInput.Text, m_usernameInput.Text, m_passwordInput.Text);
        }

        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            DialogResult result = MessageBox.Show("The identity of the Directory server cannot be verified. \n" +
                                                  "The certificate is not from a trusted certifying authority. \n" +
                                                  "Do you trust this server?", "Secure Communication", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.AcceptDirectory = true;
            }
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            m_connect.Enabled = true;
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_connect.Enabled = false;
            Close();
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            m_state.Text = e.FailureCode.ToString();
            m_connect.Enabled = true;
        }

        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            m_state.Text = e.Status.ToString();
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdkEngine)
        {
            m_sdkEngine = sdkEngine;

            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;

        }

        #endregion
    }

    #endregion
}

