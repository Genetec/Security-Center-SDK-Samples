// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Linq;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Pages.Contents;
using TimelineProvider.Events;
using TimelineProvider.Extensions;
using TimelineProvider.Services;

namespace TimelineProvider.Providers
{
    public class RandomEventsTimelineProvider : Genetec.Sdk.Workspace.Components.TimelineProvider.TimelineProvider
    {

        #region Private Fields

        /// <summary>
        /// The fake video analytic service.
        /// </summary>
        private readonly FakeVideoAnalyticService m_fakeVideoAnalyticService = FakeVideoAnalyticService.GetInstance();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the application's workspace
        /// </summary>
        public Genetec.Sdk.Workspace.Workspace Workspace { get; private set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Initialize the timeline provider
        /// </summary>
        /// <param name="workspace">Application's workspace</param>
        public void Initialize(Genetec.Sdk.Workspace.Workspace workspace)
            => Workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));

        /// <summary>
        /// Query timeline event for the specified content within the specified time range
        /// </summary>
        /// <param name="contentGroup">Content group currently hooked in the timeline</param>
        /// <param name="startTime">Timeline range start time</param>
        /// <param name="endTime">Timeline range end time</param>
        public override void Query(ContentGroup contentGroup, DateTime startTime, DateTime endTime)
        {
            // Making sure the is a videoContent content group.
            if (!(contentGroup?.Current is VideoContent videoContent))
                return;

            // Making sure the videoContent contains a camera. We won't use the camera in this sample.
            if (!(Workspace.Sdk.GetEntity(videoContent.EntityId) is Camera))
                return;

            // Trimming the times.
            startTime = startTime.TrimMilliseconds();
            endTime = endTime.TrimMilliseconds();

            // Making sure we don't create events in the future.
            if (endTime > DateTime.UtcNow.TrimMilliseconds())
                endTime = DateTime.UtcNow.TrimMilliseconds();

            // Retrieving the random events.
            RetrieveAndInsertRandomEvents(videoContent, startTime, endTime);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Method to retrieve the fake video events from a fake analyze.
        /// </summary>
        /// <remarks>
        /// If you want to retrieve more events, you can by changing the appropriate constant value in the <see cref="FakeVideoAnalyticService"/>.
        /// </remarks>
        /// <param name="videoContent">The video content</param>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <returns>A task</returns>
        private void RetrieveAndInsertRandomEvents(VideoContent videoContent, DateTime startTime, DateTime endTime)
        {
            var randomEvents = m_fakeVideoAnalyticService.GetRandomEvents(videoContent.EntityId, startTime, endTime);
            if (randomEvents != null && randomEvents.Count > 0)
            {
                // Insert all of them at the same time.
                InsertEvents(randomEvents.Select(e => new RandomTimelineEvent(e.Key, e.Value)));
            }
        }

        #endregion Private Methods

    }
}