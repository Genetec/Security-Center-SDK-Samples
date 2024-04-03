// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;

namespace TimelineProvider.Extensions
{

    /// <summary>
    /// DateTime extensions
    /// </summary>
    public static class DateTimeExtensions
    {

        #region Public Methods

        /// <summary>
        /// Method to trim the milliseconds of a DateTime.
        /// </summary>
        /// <param name="dt">The DateTime to trim.</param>
        /// <returns>The trimmed DateTime.</returns>
        public static DateTime TrimMilliseconds(this DateTime dt) =>
            new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, 0, dt.Kind);

        #endregion Public Methods

    }

}