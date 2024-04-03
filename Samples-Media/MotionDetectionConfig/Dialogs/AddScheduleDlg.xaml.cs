using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;

// ==========================================================================
// Copyright (C) 2012 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for November 7:
//  680 – The Sixth Ecumenical Council commences in Constantinople.
//  1907 – Delta Sigma Pi is founded at New York University.
//  1914 – The first issue of The New Republic magazine is published.
// ==========================================================================
namespace MotionDetectionConfig.Dialogs
{
    #region Classes

    /// <summary>
    /// Interaction logic for AddScheduleDlg.xaml
    /// </summary>
    public partial class AddScheduleDlg : Window
    {
        #region Constants

        public static readonly DependencyProperty SelectedScheduleProperty =
            DependencyProperty.Register
            ("SelectedSchedule", typeof(Schedule), typeof(AddScheduleDlg),
            new PropertyMetadata(null));

        #endregion

        #region Properties

        /// <summary>
        /// The currently selected schedule
        /// </summary>
        public Schedule SelectedSchedule
        {
            get { return (Schedule)GetValue(SelectedScheduleProperty); }
            set { SetValue(SelectedScheduleProperty, value); }
        }

        #endregion

        #region Constructors

        public AddScheduleDlg()
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

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the dialog
        /// </summary>
        /// <param name="sdkEngine">The Engine instance used to query the system</param>
        /// <param name="excludedSchedules">A list of schedules to exclude from the results disapled in the combo box</param>
        public void Initialize(Engine sdkEngine, List<Guid> excludedSchedules)
        {
            //Create a new query to fetch all the schedules of the system (should not have many)
            EntityConfigurationQuery query = sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            if (query != null)
            {
                query.EntityTypeFilter.Add(EntityType.Schedule);

                //Launch the query
                QueryCompletedEventArgs results = query.Query();

                //Parse the results
                foreach (DataRow row in results.Data.Rows)
                {
                    Guid guid = (Guid)row[0];
                    if (excludedSchedules.Contains(guid))
                    {
                        //If the schedule is in our excleded list, skip it
                        continue;
                    }

                    //Get the schedule and add it to the combo box
                    Schedule schedule = (Schedule)sdkEngine.GetEntity(guid);
                    if (schedule != null)
                    {
                        m_cbSchedules.Items.Add(schedule);
                    }
                }

                if (m_cbSchedules.Items.Count > 0)
                {
                    //Select the first available schedule by default
                    m_cbSchedules.SelectedIndex = 0;
                }
            }
        }

        #endregion
    }

    #endregion
}

