// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries.Video;
using Genetec.Sdk.Workspace;
using ModuleSample.Controls.Timeline.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Threading;

namespace ModuleSample.Controls.Timeline.DataProvider
{

    /// <summary>
    /// Timeline data provider using the sdk
    /// </summary>
    public class SdkTimelineDataProvider : ITimelineDataProvider
    {

        #region Private Fields

        /// <summary>
        /// The current dispatcher, used to synchronized on the UI thread
        /// </summary>
        private readonly Dispatcher m_dispatcher;

        #endregion Private Fields

        #region Public Events

        /// <summary>
        /// Event fired when motions are received
        /// </summary>
        public event EventHandler MotionsReceived;

        /// <summary>
        /// Event fired when sequences are received
        /// </summary>
        public event EventHandler SequencesReceived;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets the current begin time
        /// </summary>
        public DateTime BeginTime { get; private set; }

        /// <summary>
        /// Gets and sets the current camera
        /// </summary>
        public Guid Camera { get; set; }

        /// <summary>
        /// Gets the current end time
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// Gets the received motions
        /// </summary>
        public List<ITimelineEvent> Motions { get; private set; }

        /// <summary>
        /// Gets the received sequences
        /// </summary>
        public List<ITimelineEvent> Sequences { get; private set; }

        /// <summary>
        /// Gets the camera time zone
        /// </summary>
        public TimeZoneInfo TimeZone
        {
            get
            {
                TimeZoneInfo timezone = null;
                Camera camera = Workspace.Sdk.GetEntity(Camera) as Camera;
                if (camera != null)
                {
                    timezone = camera.TimeZone;
                }
                return timezone;
            }
        }

        #endregion Public Properties

        #region Private Properties

        private IEngine Sdk { get; set; }

        private Workspace Workspace { get; set; }

        #endregion Private Properties

        #region Public Constructors

        public SdkTimelineDataProvider()
        {
            m_dispatcher = Dispatcher.CurrentDispatcher;
            Sequences = new List<ITimelineEvent>();
            Motions = new List<ITimelineEvent>();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Initialize the provider with the workspace
        /// </summary>
        /// <param name="workspace"></param>
        public void Initialize(Workspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException("workspace");

            Workspace = workspace;
            Sdk = workspace.Sdk;
        }

        /// <summary>
        /// Set the current timeline range
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public void SetTimelineRange(DateTime begin, DateTime end)
        {
            BeginTime = begin;
            EndTime = end;
            QuerySequences();
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Build the list of motion events from the data table received.
        /// Motion events are trimmed to fit in the sequences ranges.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private List<ITimelineEvent> BuildMotionEvents(DataTable dataTable)
        {
            List<ITimelineEvent> motions = new List<ITimelineEvent>();
            int nRowCount = 0;
            foreach (DataRow row in dataTable.Rows)
            {
                DateTime eventTime = (DateTime)row["EventTime"];
                uint value = (uint)row["Value"];
                TimeSpan duration = TimeSpan.Zero;

                if (nRowCount < dataTable.Rows.Count - 1)
                {
                    // retrieve the next result event time to know where the current motion ends.
                    DataRow nextRow = dataTable.Rows[++nRowCount];
                    if (nextRow != null)
                    {
                        duration = (DateTime)nextRow["EventTime"] - eventTime;
                    }
                }
                else
                {
                    duration = new TimeSpan(0, 0, 20);
                }

                // Sequence where the motion starts
                ITimelineEvent sequenceIn = Sequences.Find(seq => seq.EventTime <= eventTime &&
                                                                  seq.EventTime + seq.Duration > eventTime);

                // Sequence where the motion ends
                ITimelineEvent sequenceOut = Sequences.Find(seq => seq.EventTime <= eventTime + duration &&
                                                                   seq.EventTime + seq.Duration > eventTime + duration);

                // If the In and Out sequences are the same, it means that the motion is included completely in a sequence,
                // or completly not in a sequence
                if (sequenceIn == sequenceOut)
                {
                    motions.Add(new Motion(eventTime, duration, value));
                }
                else
                {
                    // add motion event from start of event to end of sequence
                    if (sequenceIn != null)
                    {
                        duration = sequenceIn.EventTime + sequenceIn.Duration - eventTime;
                        motions.Add(new Motion(eventTime, duration, value));
                    }
                    // add motion event from the start of sequence to the end of event
                    else if (sequenceOut != null)
                    {
                        duration = (eventTime + duration) - sequenceOut.EventTime;
                        motions.Add(new Motion(sequenceOut.EventTime, duration, value));
                    }
                }
            }

            return motions;
        }

        /// <summary>
        /// Build the list of sequence events from the data table received.
        /// </summary>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        private List<ITimelineEvent> BuildSequenceEvents(DataTable dataTable)
        {
            List<ITimelineEvent> sequences = new List<ITimelineEvent>();
            foreach (DataRow row in dataTable.Rows)
            {
                sequences.Add(new Sequence((DateTime)row["StartTime"], (DateTime)row["EndTime"]));
            }
            return sequences;
        }

        private void OnMotionsQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            Action action = delegate
            {
                Motions = BuildMotionEvents(e.Data);

                if (MotionsReceived != null)
                    MotionsReceived(this, EventArgs.Empty);
            };
            m_dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
        }

        private void OnSequencesQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            Action action = delegate
            {
                Sequences = BuildSequenceEvents(e.Data);

                if (SequencesReceived != null)
                    SequencesReceived(this, EventArgs.Empty);

                QueryMotions();
            };
            m_dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
        }
        /// <summary>
        /// Launch motion query
        /// </summary>
        private void QueryMotions()
        {
            MotionEventQuery query = Sdk.ReportManager.CreateReportQuery(ReportType.MotionEvent, null) as MotionEventQuery;

            query.Cameras.Add(Camera);
            query.TimeRange.SetTimeRange(BeginTime, EndTime);
            query.SortOrder = OrderByType.Ascending;
            query.MaximumResultCount = 50000;
            query.QueryCompleted += OnMotionsQueryCompleted;

            query.BeginQuery(null, null);
        }

        /// <summary>
        /// Launch sequence query
        /// </summary>
        private void QuerySequences()
        {
            SequenceQuery query = Sdk.ReportManager.CreateReportQuery(ReportType.VideoSequence, null) as SequenceQuery;

            query.Cameras.Add(Camera);
            query.TimeRange.SetTimeRange(BeginTime, EndTime);
            query.SortOrder = OrderByType.Ascending;
            query.MaximumResultCount = 50000;
            query.QueryCompleted += OnSequencesQueryCompleted;

            query.BeginQuery(null, null);
        }

        #endregion Private Methods

    }

}