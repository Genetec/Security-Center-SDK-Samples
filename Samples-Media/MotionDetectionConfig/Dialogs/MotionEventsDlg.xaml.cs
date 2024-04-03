using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.CustomEvents;

// ==========================================================================
// Copyright (C) 2012 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for November 9:
//  1729 – Spain, France and Great Britain sign the Treaty of Seville.
//  1872 – The Great Boston Fire of 1872.
//  1887 – The United States receives rights to Pearl Harbor, Hawaii.
// ==========================================================================
namespace MotionDetectionConfig.Dialogs
{
    #region Classes

    /// <summary>
    /// Interaction logic for MotionEventsDlg.xaml
    /// </summary>
    public partial class MotionEventsDlg : Window
    {
        #region Constants

        /// <summary>
        /// The list of event items
        /// </summary>
        private readonly ObservableCollection<MotionEventItem> m_eventItems = new ObservableCollection<MotionEventItem>();

        #endregion

        #region Fields

        /// <summary>
        /// Flag indicating if we are currently initializing the control
        /// </summary>
        private bool m_isInitializing;

        #endregion

        #region Properties

        /// <summary>
        /// The list of event items used in the UI
        /// </summary>
        public ReadOnlyObservableCollection<MotionEventItem> MotionEvents
        {
            get { return new ReadOnlyObservableCollection<MotionEventItem>(m_eventItems); }
        }

        /// <summary>
        /// The id of the motion off event
        /// </summary>
        public int MotionOffEvent { get; private set; }

        /// <summary>
        /// The id of the motion on event
        /// </summary>
        public int MotionOnEvent { get; private set; }

        #endregion

        #region Nested Classes and Structures

        /// <summary>
        /// Class used to represent an event
        /// </summary>
        sealed public class MotionEventItem
        {
            #region Properties

            /// <summary>
            /// Value representing the ID of the event
            /// </summary>
            public int EventId { get; private set; }

            /// <summary>
            /// Value representing the name of the event
            /// </summary>
            public string EventName { get; private set; }

            #endregion

            #region Constructors

            public MotionEventItem(int id, string name)
            {
                EventId = id;
                EventName = name;
            }

            #endregion
        }

        #endregion

        #region Constructors

        public MotionEventsDlg()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnButtonOkClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnComboBoxMotionOffEventSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_isInitializing)
            {
                return;
            }

            if (m_cbMotionOffEvent.SelectedItem != null)
            {
                MotionEventItem item = m_cbMotionOffEvent.SelectedItem as MotionEventItem;
                if (item != null)
                {
                    MotionOffEvent = item.EventId;
                }
            }
        }

        private void OnComboBoxMotionOnEventSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_isInitializing)
            {
                return;
            }

            if (m_cbMotionOnEvent.SelectedItem != null)
            {
                MotionEventItem item = m_cbMotionOnEvent.SelectedItem as MotionEventItem;
                if (item != null)
                {
                    MotionOnEvent = item.EventId;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the control with the SDK Engine instance and the values from the configuration
        /// </summary>
        /// <param name="sdkEngine">The instance of the SDK Engine</param>
        /// <param name="motionOnEvent">The id of the motion on event from the configuration</param>
        /// <param name="motionOffEvent">The id of the motion off event from the configuration</param>
        public void Initialize(Engine sdkEngine, int motionOnEvent, int motionOffEvent)
        {
            MotionOnEvent = motionOnEvent;
            MotionOffEvent = motionOffEvent;

            m_isInitializing = true;
            try
            {
                //Get the system configuration entity
                var systemConfiguration = sdkEngine.GetEntity<SystemConfiguration>(SdkGuids.SystemConfiguration);
                if (systemConfiguration != null)
                {
                    var customEventService = systemConfiguration.CustomEventService;
                    if (customEventService.CustomEvents != null)
                    {
                        //Add the default events
                        m_eventItems.Add(new MotionEventItem(0, "None"));
                        m_eventItems.Add(new MotionEventItem(Camera.DefaultMotionDetectionMotionOnEvent, "Motion on"));
                        m_eventItems.Add(new MotionEventItem(Camera.DefaultMotionDetectionMotionOffEvent, "Motion off"));

                        //Add each custom event of the system in the list
                        foreach (CustomEvent customEvent in customEventService.CustomEvents)
                        {
                            //Make sure the to save custom event ids with a negative value
                            MotionEventItem item = new MotionEventItem(-customEvent.Id, customEvent.Name);
                            m_eventItems.Add(item);

                            if (motionOnEvent == item.EventId)
                            {
                                //If this event is the motion on event from the config, select it
                                m_cbMotionOnEvent.SelectedItem = item;
                            }

                            if (motionOffEvent == item.EventId)
                            {
                                //If this event is the motion off event from the config, select it
                                m_cbMotionOffEvent.SelectedItem = item;
                            }
                        }

                        MotionEventItem motionOnItem = m_cbMotionOnEvent.SelectedItem as MotionEventItem;
                        if ((motionOnItem == null) || (motionOnItem.EventId != MotionOnEvent))
                        {
                            //If the motion on event is not selected, select the default value
                            m_cbMotionOnEvent.SelectedItem = m_eventItems[1];
                            MotionOnEvent = Camera.DefaultMotionDetectionMotionOnEvent;
                        }

                        MotionEventItem motionOffItem = m_cbMotionOffEvent.SelectedItem as MotionEventItem;
                        if ((motionOffItem == null) || (motionOffItem.EventId != MotionOffEvent))
                        {
                            //If the motion off event is not selected, select the default value
                            m_cbMotionOffEvent.SelectedItem = m_eventItems[2];
                            MotionOffEvent = Camera.DefaultMotionDetectionMotionOffEvent;
                        }
                    }
                }
            }
            finally
            {
                m_isInitializing = false;
            }
        }

        #endregion
    }

    #endregion
}

