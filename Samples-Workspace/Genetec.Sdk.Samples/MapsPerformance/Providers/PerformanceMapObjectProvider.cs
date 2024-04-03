// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Workspace.Components.MapObjectProvider;

namespace MapsPerformance.Providers
{
    public sealed class PerformanceMapObjectProvider : MapObjectProvider
    {

        #region Private Fields

        private const int Count = 100000;
        private static readonly List<CameraMapObject> s_buffer = new List<CameraMapObject>();
        private readonly List<Guid> m_camerasId = new List<Guid>();

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{CB4BF49C-CE50-423A-9941-AEE46742024D}"));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Performance map object provider";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        public override IList<MapObject> Query(MapObjectProviderContext context)
        {
            var map = Workspace.Sdk.GetEntity(context.MapId) as Map;

            // We only provide accidents for geo referenced maps
            if ((map != null) && map.IsGeoReferenced)
            {
                return new List<MapObject>(s_buffer);
            }

            return null;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Initialize()
        {
            Workspace.Sdk.LoggedOn += OnSdkLoggedOn;
            Workspace.Sdk.LoggedOff += OnSdkLoggedOff;

            if (Workspace.Sdk.IsConnected)
            {
                Setup();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnSdkLoggedOff(object sender, LoggedOffEventArgs e)
        {
            lock (s_buffer)
            {
                s_buffer.Clear();
                m_camerasId.Clear();
            }
        }

        private void OnSdkLoggedOn(object sender, LoggedOnEventArgs e)
        {
            WaitCallback pFunc = delegate
            {
                Setup();
            };
            ThreadPool.QueueUserWorkItem(pFunc);
        }
        private void Setup()
        {
            var query = Workspace.Sdk.ReportManager.CreateReportQuery(ReportType.CameraConfiguration) as CameraConfigurationQuery;
            if (query != null)
            {
                var result = query.Query();

                lock (s_buffer)
                {
                    m_camerasId.Clear();
                    m_camerasId.Capacity = result.Data.Rows.Count;

                    foreach (DataRow row in result.Data.Rows)
                    {
                        var cameraId = (Guid)row[0];
                        m_camerasId.Add(cameraId);
                    }
                }
            }

            lock (s_buffer)
            {
                s_buffer.Clear();
                s_buffer.Capacity = Count;

                var rnd = new Random(Environment.TickCount);
                for (var i = 0; i < Count; ++i)
                {
                    var longitude = rnd.NextDouble() * 360 - 180;
                    var latitude = rnd.NextDouble() * 170 - 85;

                    var mapObject = new CameraMapObject { Latitude = latitude, Longitude = longitude };

                    if (m_camerasId.Count > 0)
                    {
                        mapObject.LinkedEntity = m_camerasId[rnd.Next(m_camerasId.Count - 1)];
                    }

                    s_buffer.Add(mapObject);
                }
            }

            Invalidate(Guid.Empty, new List<MapObject>(s_buffer), null, null);
        }

        #endregion Private Methods

    }
}