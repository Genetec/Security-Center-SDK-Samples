using Genetec.Sdk;
using LicenseSampleCore.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace LicenseSampleCore
{
    public partial class MainWindow : Window
    {
        #region Constants

        /// <summary>
        /// The Engine of the Sdk.
        /// </summary>
        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Fields

        public ObservableCollection<LicenseTypeCheckbox> LicenseTypes { get; set; } = new ObservableCollection<LicenseTypeCheckbox>();
        public ObservableCollection<LicenseResult> LicenseResult { get; set; } = new ObservableCollection<LicenseResult>();
        public static readonly DependencyProperty IsSdkEngineConnectedProperty = DependencyProperty.Register(
                    "IsSdkEngineConnected", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));
        public bool IsSdkEngineConnected
        {
            get { return (bool)GetValue(IsSdkEngineConnectedProperty); }
            set { SetValue(IsSdkEngineConnectedProperty, value); }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            // Logon to Sdk engine
            string server = "localhost";
            string username = "admin";
            string password = "";
            m_sdkEngine.LoggedOn += (s, e) => LoggedOn();
            m_sdkEngine.LoginManager.LogOn(server, username, password);
        }

        #region Private Methods

        void LoggedOn()
        {
            InitLicenseTypes().ConfigureAwait(false);
            IsSdkEngineConnected = true;
        }

        /// <summary>
        /// Get labels for the list of all licenses 
        /// ** This is the only way to get the keys for single licenses (LicenseManager.GetLicenseItemUsageAsync(itemNames)) **
        /// </summary>
        /// <returns></returns>
        async Task InitLicenseTypes()
        {
            LicenseTypes.Clear();
            var licenseDict = await m_sdkEngine.LicenseManager.GetEveryLicenseItemUsageAsync();
            foreach (var license in licenseDict)
            {
                LicenseTypes.Add(new LicenseTypeCheckbox() { Label = license.Key, Checked = false });
            }
        }

        #endregion

        #region XAML Methods

        /// <summary>
        /// Get specified Licenses with LicenseManager.GetLicenseItemUsageAsync()
        /// This gives CurrentCount and Maximum Count
        /// </summary>
        private async void ButtonGetSpecific_Click(object sender, RoutedEventArgs e)
        {
            LicenseResult.Clear();
            foreach (var selected in LicenseTypes.Where(t => t.Checked))
            {
                var license = await m_sdkEngine.LicenseManager.GetLicenseItemUsageAsync(selected.Label);
                LicenseResult.Add(new LicenseResult(selected.Label, license.CurrentCount, license.MaximumCount));
            }
        }

        /// <summary>
        /// Get all licenses LicenseManager.GetEveryLicenseItemUsageAsync()
        /// This gives CurrentCount and MaximumCount
        /// </summary>
        /// <returns></returns>
        private async void ButtonGetAll_Click(object sender, RoutedEventArgs e)
        {
            LicenseResult.Clear();
            var licenseDict = await m_sdkEngine.LicenseManager.GetEveryLicenseItemUsageAsync();
            foreach (var license in licenseDict)
            {
                LicenseResult.Add(new LicenseResult(license.Key, license.Value.CurrentCount, license.Value.MaximumCount));
            }
        }

        #endregion
    }
}
