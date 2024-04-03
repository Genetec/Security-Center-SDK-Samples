// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Genetec.Sdk.Workspace.Services;

namespace TrafficAnalysis.Services
{
    public interface IAnalysisService : IService
    {

        #region Public Methods

        IList<AnalysisPoint> GetAnalysisData(Guid cameraId, DateTime start, DateTime end);

        #endregion Public Methods

    }

    public sealed class AnalysisService : IAnalysisService
    {

        #region Private Fields

        private readonly Dictionary<Guid, List<AnalysisPoint>> m_buffer = new Dictionary<Guid, List<AnalysisPoint>>();

        #endregion Private Fields

        #region Public Properties

        public Genetec.Sdk.Workspace.Workspace Workspace { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public IList<AnalysisPoint> GetAnalysisData(Guid cameraId, DateTime start, DateTime end)
        {
            List<AnalysisPoint> data;

            lock (m_buffer)
            {
                if (!m_buffer.TryGetValue(cameraId, out data))
                {
                    data = GenerateFakeData();
                    m_buffer.Add(cameraId, data);
                }
            }

            return data.Where(x => x.Timestamp > start && x.Timestamp < end).ToList();
        }

        public void Initialize(Genetec.Sdk.Workspace.Workspace workspace) => Workspace = workspace;

        #endregion Public Methods

        #region Private Methods

        private List<AnalysisPoint> GenerateFakeData()
        {
            const int count = 10000;
            const double minValue = 0.0;
            const double maxValue = 50.0;
            const double maxIncrement = 5.0;
            var data = new List<AnalysisPoint>(count);

            var dt = DateTime.UtcNow - TimeSpan.FromMinutes(5);
            var random = new Random(Environment.TickCount);

            var lastValue = random.NextDouble() * maxValue;
            for (var i = 0; i < count; i++)
            {
                var increment = (random.NextDouble() - 0.5) * maxIncrement;

                // Provoke a mutation
                if (i % 30 == 0)
                {
                    increment *= 3.0;
                }

                var value = lastValue + increment;

                // Provoke a mutation
                if (i % 300 == 0)
                {
                    value = random.NextDouble() * maxValue / 2;
                }

                value = Math.Min(value, maxValue);
                value = Math.Max(value, minValue);

                var pt = new AnalysisPoint(dt, value);
                data.Add(pt);

                lastValue = value;
                dt += TimeSpan.FromSeconds(1);
            }

            return data;
        }

        #endregion Private Methods
    }
}