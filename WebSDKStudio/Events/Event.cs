using System;
using System.Xml.Linq;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace WebSDKStudio.Events
{
    #region Classes

    /// <summary>
    /// Class Event
    /// </summary>
    public class Event
    {
        #region Properties

        /// <summary>
        /// Name of the Event.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// TimeStamp of when it happens.
        /// </summary>
        public DateTime EventTimeStamp { get; set; }

        /// <summary>
        /// The source of the Event
        /// </summary>
        public string Source { get; set; }

        #endregion
    }

    #endregion
}

