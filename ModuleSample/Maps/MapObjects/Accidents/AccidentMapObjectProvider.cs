// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components.MapObjectProvider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace ModuleSample.Maps.MapObjects.Accidents
{
    public class AccidentMapObjectProvider : MapObjectProvider, IDisposable
    {

        #region Private Fields

        private static readonly ObservableCollection<AccidentMapObject> s_accidents = new ObservableCollection<AccidentMapObject>();
        private static readonly List<AccidentMapObject> s_scaleTest = new List<AccidentMapObject>();
        private readonly List<GeoCoordinate> m_towingRoute = new List<GeoCoordinate>();
        private Thread m_thread;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Accident map object provider";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => new Guid("{16EB953C-9E17-42DE-BBE8-87F769415E12}");

        #endregion Public Properties

        #region Public Constructors

        public AccidentMapObjectProvider()
        {
            m_thread = new Thread(OnThreadStart) {Name = "Accident provider", IsBackground = true};
            m_thread.Start();
        }

        #endregion Public Constructors

        #region Public Methods

        public static void AddAccident(AccidentMapObject incident)
        {
            lock (s_accidents)
            {
                s_accidents.Add(incident);
            }
        }

        public static ReadOnlyObservableCollection<AccidentMapObject> GetAccidents()
        {
            return new ReadOnlyObservableCollection<AccidentMapObject>(s_accidents);
        }

        public static void RemoveAccident(AccidentMapObject incident)
        {
            lock (s_accidents)
            {
                s_accidents.Remove(incident);
            }
        }
        public void Dispose()
        {
            s_accidents.CollectionChanged -= OnAccidentsCollectionChanged;

            if (m_thread != null)
            {
                m_thread.Abort();
                m_thread = null;
            }
        }

        public override IList<MapObject> Query(MapObjectProviderContext context)
        {
            var map = Workspace.Sdk.GetEntity(context.MapId) as Map;

            // We only provide accidents for geo referenced maps
            if ((map != null) && map.IsGeoReferenced)
            {
                var result = new List<MapObject>(s_accidents);
                result.AddRange(s_scaleTest);
                return result;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAccidentsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var addedItems = new List<MapObject>();
            var removedItems = new List<MapObject>();

            if (e.NewItems != null)
            {
                addedItems.AddRange(e.NewItems.Cast<MapObject>());
            }

            if (e.OldItems != null)
            {
                removedItems.AddRange(e.OldItems.Cast<MapObject>());
            }

            Invalidate(Guid.Empty, addedItems, removedItems, null);
        }

        private void OnThreadStart()
        {
            ParseTowingRoute();
            RetrieveIncidents();
            s_accidents.CollectionChanged += OnAccidentsCollectionChanged;

            var enableScaleTest = false;
            if (enableScaleTest)
            {
                SetupScaleTest(5000);
            }

            var isReversed = false;

            var isRunning = true;
            while (isRunning)
            {
                var route = new List<GeoCoordinate>(m_towingRoute);
                if (isReversed)
                {
                    route.Reverse();
                }

                // Otherwise 100% CPU when no coordinates in the route.
                Thread.Sleep(300);

                if (enableScaleTest)
                {
                    var rnd = new Random(Environment.TickCount);
                    lock (s_scaleTest)
                    {
                        foreach (var mapObj in s_scaleTest)
                        {
                            var longitude = rnd.NextDouble() * 360 - 180;
                            var latitude = rnd.NextDouble() * 170 - 85;
                            mapObj.Latitude = latitude;
                            mapObj.Longitude = longitude;
                        }
                    }
                }

                isReversed = !isReversed;
            }
        }

        private void ParseTowingRoute()
        {
            try
            {
                var filePath = Path.Combine(System.Windows.Forms.Application.StartupPath, "towingroute.txt");
                if (System.IO.File.Exists(filePath))
                {
                    var doc = XDocument.Load(filePath);

                    foreach (var des in doc.Descendants("rtept"))
                    {
                        var latitude = des.Attribute("lat").Value;
                        var longitude = des.Attribute("lon").Value;

                        m_towingRoute.Add(new GeoCoordinate(double.Parse(latitude, CultureInfo.InvariantCulture), double.Parse(longitude, CultureInfo.InvariantCulture)));
                    }
                }
            }
            catch (Exception ex)
            {
                // Protect against parsing exceptions
                Console.WriteLine(ex);
            }
        }

        private static void RetrieveIncidents()
        {
            lock (s_accidents)
            {
                var accident01 = new AccidentMapObject(45.469069, -73.523598, "Accident on Champlain bridge.\r\nReported via Waze app.");
                s_accidents.Add(accident01);

                var accident02 = new AccidentMapObject(45.521400, -73.525474, "Accident on Jacques-Cartier bridge.\r\nReported via Waze app.");
                s_accidents.Add(accident02);

                var accident03 = new AccidentMapObject(45.511280, -73.764424, "Accident on Autoroute Chomedey.\r\nReported via Waze app.");
                s_accidents.Add(accident03);

                var accident04 = new AccidentMapObject(43.679429, -79.617244, "Toronto #1");
                s_accidents.Add(accident04);

                var accident05 = new AccidentMapObject(43.676422, -79.611782, "Toronto #2");
                s_accidents.Add(accident05);

                var accident06 = new AccidentMapObject(43.681317, -79.609050, "Toronto #3");
                s_accidents.Add(accident06);
            }
        }

        private void SetupScaleTest(int count)
        {
            var rnd = new Random(Environment.TickCount);
            lock (s_scaleTest)
            {
                s_scaleTest.Clear();
                s_scaleTest.Capacity = count;

                for (var i = 0; i < count; ++i)
                {
                    var longitude = rnd.NextDouble() * 360 - 180;
                    var latitude = rnd.NextDouble() * 170 - 85;

                    var accident = new AccidentMapObject(latitude, longitude, "Accident #" + i);
                    s_scaleTest.Add(accident);
                }
            }

            Invalidate(Guid.Empty, new List<MapObject>(s_scaleTest), null, null);
        }

        #endregion Private Methods
    }
}