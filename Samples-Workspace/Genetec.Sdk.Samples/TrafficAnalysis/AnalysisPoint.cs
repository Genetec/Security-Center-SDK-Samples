// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;

namespace TrafficAnalysis
{
    public sealed class AnalysisPoint
    {

        #region Public Properties

        public DateTime Timestamp { get; }

        public double Value { get; }

        #endregion Public Properties

        #region Public Constructors

        public AnalysisPoint(DateTime timestamp, double value)
        {
            Timestamp = timestamp;
            Value = value;
        }

        #endregion Public Constructors
    }
}