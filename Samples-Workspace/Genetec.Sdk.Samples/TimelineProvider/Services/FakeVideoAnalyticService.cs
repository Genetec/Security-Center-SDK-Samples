// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Genetec.Sdk;

namespace TimelineProvider.Services
{
    /// <summary>
    /// This class fakes video analyzed events for the Sample.
    /// </summary>
    internal class FakeVideoAnalyticService
    {

        #region Private Fields

        private const int MaximumChanceToStopAnEvent = 100;

        private const int MaximumChangeToBeAnEvent = 10000;

        private const int MaximumDecreaseChance = 4;

        /// <summary>
        /// If you want more events displayed, change this constant to 9000.
        /// (Every 10 seconds there should be 1 event.)
        /// </summary>
        private const int MinimumChanceToBeAnEvent = 9999;

        private static readonly object Lock = new object();

        private static FakeVideoAnalyticService s_fakeVideoAnalyticService;

        /// <summary>
        /// This field makes sure we don't try to find events if we already processed that time.
        /// </summary>
        private List<DateTimeRange> m_analyzedTime = new List<DateTimeRange>();

        #endregion Private Fields

        #region Private Constructors

        private FakeVideoAnalyticService()
        {
        }

        #endregion Private Constructors

        #region Public Methods

        /// <summary>
        /// Singleton pattern.
        /// </summary>
        /// <returns>Instance of the class FakeVideoAnalyticService.</returns>
        public static FakeVideoAnalyticService GetInstance()
        {
            if (s_fakeVideoAnalyticService == null)
            {
                lock (Lock)
                {
                    if (s_fakeVideoAnalyticService == null)
                        s_fakeVideoAnalyticService = new FakeVideoAnalyticService();
                }
            }
            return s_fakeVideoAnalyticService;
        }

        /// <summary>
        /// This method is used to select how many events are at a certain time.
        /// In the real world, this is where the real video analyze would be done.
        /// </summary>
        /// <param name="cameraGuid">The Guid of the Camera.</param>
        /// <param name="startTime">The start time for the analyze.</param>
        /// <param name="endTime">The end time for the analyze.</param>
        /// <returns>The faked events from the TimeRange.</returns>
        public Dictionary<DateTime, long> GetRandomEvents(Guid cameraGuid, DateTime startTime, DateTime endTime)
        {
            // Not using pluginGuid and cameraGuid as this is a fake video analytic.
            var dictEvents = new Dictionary<DateTime, long>();
            var rnd = new Random();
            var currentAnalyzeTime = startTime;

            // Since we fake it, we want more then 1 instance to show up in the Timeline.
            // So this var is used for that.
            var chanceToStopTheEvent = MaximumChanceToStopAnEvent;
            var lastTimeWasEvent = false;
            var isAnEvent = false;

            while (endTime > currentAnalyzeTime)
            {
                var alreadyProcessed = false;
                var dateTimeRange = m_analyzedTime.Where(x => x.ContainsTime(currentAnalyzeTime)).ToList();
                if (dateTimeRange.Any())
                {
                    alreadyProcessed = true;
                    // Increase the time.
                    currentAnalyzeTime = dateTimeRange.FirstOrDefault()?.EndTime.AddSeconds(1) ?? DateTime.MinValue;
                }

                if (!alreadyProcessed)
                {
                    if (lastTimeWasEvent)
                    {
                        var rollToStopBeingAnEvent = rnd.Next(0, MaximumChanceToStopAnEvent);
                        if (rollToStopBeingAnEvent > chanceToStopTheEvent)
                        {
                            // Event has been stopped, so we set back the defaults
                            lastTimeWasEvent = false;
                            isAnEvent = false;
                            chanceToStopTheEvent = MaximumChanceToStopAnEvent;
                        }
                        else
                        {
                            // The Fake Algo told us the event is not done yet.
                            // Decrease the chance to stop the event to make it easier to stop it next time.
                            chanceToStopTheEvent = chanceToStopTheEvent - rnd.Next(0, MaximumDecreaseChance);
                        }
                    }
                    else
                    {
                        // Try to get a new Event going.
                        if (rnd.Next(0, MaximumChangeToBeAnEvent) >= MinimumChanceToBeAnEvent)
                        {
                            lastTimeWasEvent = true;
                            isAnEvent = true;
                        }
                    }

                    // Is this frame an event?
                    if (isAnEvent)
                    {
                        var eventCount = rnd.Next(1, 10);
                        dictEvents.Add(currentAnalyzeTime, eventCount);
                        // Increase the time.
                        currentAnalyzeTime = currentAnalyzeTime.AddSeconds(2);
                    }
                    else
                    {
                        // Increase the time.
                        currentAnalyzeTime = currentAnalyzeTime.AddSeconds(1);
                    }
                }
            }

            m_analyzedTime.Add(new DateTimeRange(startTime, endTime));
            m_analyzedTime = DateTimeRange.ResolveOverlaps(m_analyzedTime).ToList();

            // Return all the events triggered and their count.
            return dictEvents;
        }

        #endregion Public Methods

    }
}