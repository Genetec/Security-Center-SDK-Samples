#region Using Statements
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Forms;

using Genetec.Sdk;
using Genetec.Sdk.Credentials;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.CustomFields;
using Genetec.Sdk.Events.AccessPoint;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Samples.SamplesLibrary;
#endregion

// ==========================================================================
// Copyright (C) 1989-2007 by Genetec Information Systems, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples
{
    public partial class MainDlg : Form
    {
        #region Fields

        /// <summary>
        /// Represent the SDK class used to control Security Center
        /// </summary>
        private Engine m_sdkEngine = new Engine();

        /// <summary>
        /// Represent the door entity set as filter
        /// </summary>
        private Door m_boDoor;

        #endregion

        #region Properties
        #endregion

        #region Nested Classes and Structures

        private sealed class CredentialViewItem : ListViewItem
        {
            #region Fields

            private CredentialFormat m_objCredentialFormat;

            #endregion

            #region Properties

            public CredentialFormat CredentialFormat
            {
                get
                {
                    return m_objCredentialFormat;
                }
            }

            #endregion

            #region Constructors

            public CredentialViewItem(AccessPointCredentialUnknownEvent e)
            {
                m_objCredentialFormat = CredentialFormat.Deserialize(e.XmlCredential);

                // Sdk times are always in UTC mode
                Text = e.Timestamp.ToLocalTime().ToString();
                SubItems.Add( m_objCredentialFormat.ToString() );
            }

            #endregion
        }

        #endregion

        #region Enumerations
        #endregion

        #region Events and Delegates
        #endregion

        #region Constructors

        public MainDlg()
        {
            InitializeComponent();

            m_dtArrival.MinDate = m_dtDeparture.MinDate = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            
            SubscribeEngine();
            ValidateState();

            OnRadioAutomatic_CheckedChanged(null, null);
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private and Protected Methods

        /// <summary>
        /// Subscribe to the Engine events
        /// </summary>
        private void SubscribeEngine()
        {
            m_sdkEngine.LoginManager.LoggedOn += new EventHandler<LoggedOnEventArgs>( OnLoggedOn );
            m_sdkEngine.LoginManager.LoggedOff += new EventHandler<LoggedOffEventArgs>( OnLoggedOff );
            m_sdkEngine.EventReceived += new EventHandler<EventReceivedEventArgs>( OnEventReceived );
        }

        /// <summary>
        /// Unsubscribe from the Engine events
        /// </summary>
        private void UnsubscribeEngine()
        {
            m_sdkEngine.LoginManager.LoggedOn -= new EventHandler<LoggedOnEventArgs>( OnLoggedOn );
            m_sdkEngine.LoginManager.LoggedOff -= new EventHandler<LoggedOffEventArgs>( OnLoggedOff );
            m_sdkEngine.EventReceived -= new EventHandler<EventReceivedEventArgs>( OnEventReceived );
        }

        /// <summary>
        /// Download all the entities in the Sdk cache. Note that download all the
        /// entities in the Sdk cache we cause poor performances on large system
        /// over 100000 entities.
        /// </summary>
        private void DownloadAll()
        {
            // Create a query to download Security Center's entities in the SDK cache
            EntityConfigurationQuery objQuery = m_sdkEngine.ReportManager.CreateReportQuery( ReportType.EntityConfiguration ) as EntityConfigurationQuery;

            // Add all the entity type in the query
            foreach ( EntityType eEntityType in Enum.GetValues( typeof( EntityType ) ) )
            {
                objQuery.EntityTypeFilter.Add( eEntityType );
            }

            // Launch the query. Note that when the queried entities we be downloaded 
            // in the Sdk cache, the EntityAdded event will be called.
            objQuery.BeginQuery(null, null);
        }

        private void Reset()
        {
            m_tbFirstName.Text = String.Empty;
            m_tbLastName.Text = String.Empty;
            m_lvCredentials.Items.Clear();
        }

        private void ValidateState()
        {
            bool bValidStatePage0 = ( m_tbFirstName.TextLength > 0 ) && ( m_tbLastName.TextLength > 0 );
            m_objCredentialPage.Enabled = bValidStatePage0;

            bool bValidStatePage1 = false;
            if (m_rbAutomatic.Checked == true)
            {
                bValidStatePage1 = bValidStatePage0 && (m_lvCredentials.SelectedItems.Count > 0);
            }
            else
            {
                bValidStatePage1 = bValidStatePage0 && (m_cbFormat.SelectedIndex != -1) ;
            }
            
            m_objSummaryPage.Enabled = bValidStatePage1;

            switch ( m_objTabControl.SelectedIndex )
            {
                //-------------------------------------------------------------
                // Information page
                //-------------------------------------------------------------
                case 0:
                    {
                        m_btnPrevious.Visible = false;
                        m_btnNext.Visible = true;
                        m_btnCreate.Visible = false;

                        m_btnNext.Enabled = bValidStatePage0;
                    } break;

                //-------------------------------------------------------------
                // Credential page
                //-------------------------------------------------------------
                case 1:
                    {
                        m_btnPrevious.Visible = true;
                        m_btnNext.Visible = false;
                        m_btnCreate.Visible = true;

                        m_btnCreate.Enabled = bValidStatePage1;
                    } break;

                //-------------------------------------------------------------
                // Summary page
                //-------------------------------------------------------------
                case 2:
                    {
                        m_btnPrevious.Visible = false;
                        m_btnNext.Visible = false;
                        m_btnCreate.Visible = false;
                    } break;

                default:
                    System.Diagnostics.Debug.Assert( false );
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Event Handlers

        #region Dialog Events

        private void OnButtonConnect_Click(object sender, EventArgs e)
        {
            using ( LogonDlg dlg = new LogonDlg() )
            {
                dlg.Initialize( m_sdkEngine );
                dlg.ShowDialog( this );
            }
        }

        private void OnButtonDisconnect_Click(object sender, EventArgs e)
        {
            m_sdkEngine.LoginManager.LogOff();
        }

        private void OnButtonPrevious_Click(object sender, EventArgs e)
        {
            m_objTabControl.SelectedIndex--;
        }

        private void OnButtonNext_Click(object sender, EventArgs e)
        {
            m_objTabControl.SelectedIndex++;
        }

        private void OnButtonCreate_Click(object sender, EventArgs e)
        {
            // Goto the summary page
            m_objTabControl.SelectedTab = m_objSummaryPage;

            try
            {
                m_sdkEngine.TransactionManager.CreateTransaction();

                string strCompleteName = String.Format( "{0} {1}", m_tbFirstName.Text, m_tbLastName.Text );

                //-----------------------------------------------------------------
                // Create the credentials
                //-----------------------------------------------------------------
                string strCredentialName = String.Format( "{0}'s credential", strCompleteName );
                Collection<Credential> colCredentials = new Collection<Credential>();

                if (m_rbAutomatic.Checked == true)
                {
                    foreach (CredentialViewItem lvItem in m_lvCredentials.SelectedItems)
                    {
                        // Create the credential
                        Credential boCredential = m_sdkEngine.CreateEntity(strCredentialName, EntityType.Credential) as Credential;

                        // Set the credential format & number
                        boCredential.Format = lvItem.CredentialFormat;

                        // Add the newly created credential to the collection
                        colCredentials.Add(boCredential);
                    }
                }
                else
                {
                    // Create the credential
                    Credential boCredential = m_sdkEngine.CreateEntity(strCredentialName, EntityType.Credential) as Credential;

                    string value = (string)m_cbFormat.SelectedItem;
                    switch (value)
                    {
                        case "Standard 26 bits":
                            boCredential.Format = new WiegandStandardCredentialFormat(Convert.ToInt32(m_nupFacility.Value), Convert.ToInt32(m_nupCardID.Value));
                            break;
                        case "H10302":
                            boCredential.Format = new WiegandH10302CredentialFormat(Convert.ToInt32(m_nupCardID.Value));
                            break;
                        case "H10304":
                            boCredential.Format = new WiegandH10304CredentialFormat(Convert.ToInt32(m_nupFacility.Value), Convert.ToInt32(m_nupCardID.Value));
                            break;
                        case "H10306":
                            boCredential.Format = new WiegandH10306CredentialFormat(Convert.ToInt32(m_nupFacility.Value), Convert.ToInt32(m_nupCardID.Value));
                            break;
                        case "HID Corporate 1000":
                            boCredential.Format = new WiegandCorporate1000CredentialFormat(Convert.ToInt32(m_nupFacility.Value), Convert.ToInt32(m_nupCardID.Value));
                            break;
                    }

                    // Add the newly created credential to the collection
                    colCredentials.Add(boCredential);
                }

                //-----------------------------------------------------------------
                // Create the cardholder / visitor
                //-----------------------------------------------------------------
                if ( m_rbCardholder.Checked )
                {
                    Cardholder boCardholder = m_sdkEngine.CreateEntity( strCompleteName, EntityType.Cardholder ) as Cardholder;
                    boCardholder.FirstName = m_tbFirstName.Text;
                    boCardholder.LastName = m_tbLastName.Text;

                    // Associate each credential with the newly created cardholder
                    foreach ( Credential boCredential in colCredentials )
                    {
                        // Add the credential to the cardholder
                        boCardholder.Credentials.Add( boCredential );
                    }
                }
                else
                {
                    Visitor boVisitor = m_sdkEngine.CreateEntity( strCompleteName, EntityType.Visitor ) as Visitor;

                    boVisitor.FirstName = m_tbFirstName.Text;
                    boVisitor.LastName = m_tbLastName.Text;

                    if (m_dtArrival.Value >= m_dtDeparture.Value)
                    {
                        m_lblResult.Text = "Failed! Arrival date must be less than Departure date.";
                        m_sdkEngine.TransactionManager.RollbackTransaction();
                        return;
                    }

                    boVisitor.Arrival = m_dtArrival.Value;
                    boVisitor.Departure = m_dtDeparture.Value;

                    // Associate each credential with the newly created visitor
                    foreach ( Credential boCredential in colCredentials )
                    {
                        // Add the credential to the visitor
                        boVisitor.Credentials.Add( boCredential );
                    }
                }

                m_sdkEngine.TransactionManager.CommitTransaction();
                m_lblResult.Text = "Success!";
            }
            catch(Genetec.Sdk.SdkException ex)
            {
                m_lblResult.Text = ex.ErrorCode.ToString();
            }
        }

        private void OnRadioEntityType_Click(object sender, EventArgs e)
        {
            m_pnlVisitor.Visible = m_rbVisitor.Checked;
        }

        private void OnButtonFilter_Click(object sender, EventArgs e)
        {
            using ( SearchDlg dlg = new SearchDlg() )
            {
                dlg.EntityTypeFilter.Clear();
                dlg.EntityTypeFilter.Add( EntityType.Door );
                dlg.MutlipleSelection = false;
                dlg.Initialize( m_sdkEngine );

                if ( dlg.ShowDialog( this ) == DialogResult.OK )
                {
                    if ( dlg.SelectedItems.Count == 1 )
                    {
                        m_boDoor = m_sdkEngine.GetEntity( dlg.SelectedItems[0] ) as Door;

                        m_lblFilter.Text = m_boDoor.Name;
                    }
                }
            }
        }

        private void OnUserInterfaceChanged(object sender, EventArgs e)
        {
            ValidateState();
        }

        private void OnRadioAutomatic_CheckedChanged(object sender, EventArgs e)
        {
            m_btnFilter.Enabled = m_rbAutomatic.Checked;
            m_lvCredentials.Enabled = m_rbAutomatic.Checked; ;
            m_cbFormat.Enabled = !m_rbAutomatic.Checked; ;
            m_nupFacility.Enabled = !m_rbAutomatic.Checked;
            m_nupCardID.Enabled = !m_rbAutomatic.Checked;
            ValidateState();
        }

        private void OnComboFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nupFacility.Visible = true;
            m_nupCardID.Visible = true;
            string value = (string)m_cbFormat.SelectedItem;
            switch (value)
            {
                case "Standard 26 bits":
                    m_nupFacility.Maximum = WiegandStandardCredentialFormat.MaximumFacilityId;
                    m_nupCardID.Maximum = WiegandStandardCredentialFormat.MaximumCardId;
                    break;
                case "H10302":
                    m_nupCardID.Maximum = WiegandH10302CredentialFormat.MaximumCardId;
                    m_nupFacility.Visible = false;
                    break;
                case "H10304":
                    m_nupFacility.Maximum = WiegandH10304CredentialFormat.MaximumFacilityId;
                    m_nupCardID.Maximum = WiegandH10304CredentialFormat.MaximumCardId;
                    break;
                case "H10306":
                    m_nupFacility.Maximum = WiegandH10306CredentialFormat.MaximumFacilityId;
                    m_nupCardID.Maximum = WiegandH10306CredentialFormat.MaximumCardId;
                    break;
                case "HID Corporate 1000":
                    m_nupFacility.Maximum = WiegandCorporate1000CredentialFormat.MaximumFacilityId;
                    m_nupCardID.Maximum = WiegandCorporate1000CredentialFormat.MaximumCardId;
                    break;
            }
            ValidateState();
        }

        #endregion

        #region SDK Events

        private void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_btnConnect.Enabled = false;
            m_btnDisconnect.Enabled = true;
            m_objTabControl.Enabled = true;
            m_pnlBottom.Enabled = true;

            DownloadAll();
        }

        private void OnLoggedOff(object sender, LoggedOffEventArgs e)
        {
            if (e.AutoReconnect == false)
            {
                m_btnConnect.Enabled = true;
                m_btnDisconnect.Enabled = false;
                m_objTabControl.Enabled = false;
                m_pnlBottom.Enabled = false;
            }
        }

        private void OnEventReceived(object sender, EventReceivedEventArgs e)
        {
            if ( m_boDoor == null )
                return;

            Entity boSource = m_sdkEngine.GetEntity( e.SourceGuid );
            Guid g1 = m_boDoor.Guid;
            Guid g2 = boSource.Guid;
            if ( m_boDoor == boSource ) 
            {
                switch ( e.EventType )
                {
                    case EventType.AccessUnknownCredential:
                        {
                            AccessPointCredentialUnknownEvent objUnknownEvent = e.Event as AccessPointCredentialUnknownEvent;
                            m_lvCredentials.Items.Add( new CredentialViewItem( objUnknownEvent ) );
                        } break;
                }
            }
        }

        #endregion

        #endregion

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Always dispose the SDK object when closing the application
            if (m_sdkEngine != null)
            {
                // Unsubscribe from the Engine events
                UnsubscribeEngine();

                m_sdkEngine.Dispose();
                m_sdkEngine = null;
            }
        }
    }
}