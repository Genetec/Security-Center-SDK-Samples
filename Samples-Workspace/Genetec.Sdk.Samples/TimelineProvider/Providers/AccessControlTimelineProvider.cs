// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Genetec.Sdk;
using Genetec.Sdk.Events.AccessPoint;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Queries.AccessControl;
using Genetec.Sdk.Workspace.Pages.Contents;
using TimelineProvider.Events;

namespace TimelineProvider.Providers
{
    public sealed class AccessControlTimelineProvider : Genetec.Sdk.Workspace.Components.TimelineProvider.TimelineProvider, IDisposable
    {

        #region Private Fields

        /// <summary>
        /// We keep the list of cardholders to make sure we get all the events.
        /// The hashset is simply to not have duplicates.
        /// </summary>
        private readonly HashSet<Guid> m_cardholders = new HashSet<Guid>();

        private bool m_disposed;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the application's workspace
        /// </summary>
        public Genetec.Sdk.Workspace.Workspace Workspace { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (m_disposed)
                return;

            if (disposing)
            {
                Workspace.Sdk.EntitiesAdded -= OnSdkEntitiesAdded;
                Workspace.Sdk.EntitiesRemoved -= SdkOnEntitiesRemoved;
                Workspace.Sdk.EventReceived -= OnSdkEventReceived;
            }

            m_disposed = true;
        }

        /// <summary>
        /// Initialize the timeline provider
        /// </summary>
        /// <param name="workspace">Application's workspace</param>
        public void Initialize(Genetec.Sdk.Workspace.Workspace workspace)
        {
            Workspace = workspace ?? throw new ArgumentException("Workspace must not be null", nameof(workspace));
            Workspace.Sdk.EventReceived += OnSdkEventReceived;
            Workspace.Sdk.EntitiesAdded += OnSdkEntitiesAdded;
            Workspace.Sdk.EntitiesRemoved += SdkOnEntitiesRemoved;

            var entityConfigurationQuery = Workspace.Sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            if (entityConfigurationQuery == null) return;
            entityConfigurationQuery.EntityTypeFilter.Add(EntityType.Cardholder);
            entityConfigurationQuery.DownloadAllRelatedData = true;
            entityConfigurationQuery.BeginQuery(OnAllCardholdersQueryCompleted, entityConfigurationQuery);
        }

        /// <summary>
        /// Query timeline event for the specified content within the specified time range
        /// </summary>
        /// <param name="contentGroup">Content group currently hooked in the timeline</param>
        /// <param name="startTime">Timeline range start time</param>
        /// <param name="endTime">Timeline range end time</param>
        public override void Query(ContentGroup contentGroup, DateTime startTime, DateTime endTime)
        {
            if (!(contentGroup?.Current is VideoContent videoContent))
                return;

            if (!(Workspace.Sdk.GetEntity(videoContent.EntityId) is Camera))
                return;

            if (!(Workspace.Sdk.ReportManager.CreateReportQuery(ReportType.CardholderActivity) is CardholderActivityQuery cardholderActivityQuery))
                return;

            m_cardholders.ToList().ForEach(x => cardholderActivityQuery.Cardholders.Add(x));
            cardholderActivityQuery.TimeRange.SetTimeRange(startTime, endTime);
            cardholderActivityQuery.BeginQuery(OnTimedQueryCompleted, cardholderActivityQuery);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAllCardholdersQueryCompleted(IAsyncResult ar)
        {
            var query = (EntityConfigurationQuery)ar.AsyncState;
            var results = query.EndQuery(ar);

            var cardholderId = results.Data.Rows
                .Cast<DataRow>()
                .Select(row => (Guid)row[0])
                .ToList();

            cardholderId.ForEach(x => m_cardholders.Add(x));
        }

        private void OnSdkEntitiesAdded(object sender, EntitiesAddedEventArgs e)
            => e.Entities.ToList().ForEach(x =>
            {
                if (x.EntityType == EntityType.Cardholder)
                    m_cardholders.Add(x.EntityGuid);
            });

        private void OnSdkEventReceived(object sender, EventReceivedEventArgs e)
        {
            if (!(e.Event is AccessEvent accessEvent))
                return;

            InsertEvent(new AccessTimelineEvent(Workspace,
                accessEvent.Cardholder,
                accessEvent.Timestamp,
                accessEvent.Type == EventType.AccessGranted));
        }

        private void OnTimedQueryCompleted(IAsyncResult ar)
        {
            var query = (CardholderActivityQuery)ar.AsyncState;
            var results = query.EndQuery(ar);

            foreach (DataRow dataRow in results.Data.Rows)
                InsertEvent(new AccessTimelineEvent(Workspace,
                        (Guid)dataRow[AccessControlReportQuery.CardholderGuidColumnName],
                        (DateTime)dataRow[AccessControlReportQuery.TimestampColumnName],
                        (EventType)dataRow[AccessControlReportQuery.EventTypeColumnName] == EventType.AccessGranted));
        }

        private void SdkOnEntitiesRemoved(object sender, EntitiesRemovedEventArgs e)
            => e.Entities.ToList().ForEach(x =>
            {
                if (x.EntityType == EntityType.Cardholder)
                    m_cardholders.Remove(x.EntityGuid);
            });

        #endregion Private Methods

        #region Private Destructors

        ~AccessControlTimelineProvider()
            => Dispose(false);

        #endregion Private Destructors

    }
}