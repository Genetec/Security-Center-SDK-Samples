// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using DronesTracker.Maps.Layers;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components.MapObjectProvider;

namespace DronesTracker.Maps
{
    internal struct SimulatedRoute
    {

        #region Public Properties

        public List<GeoCoordinate> Coordinates { get; }

        public bool IsLooping { get; }

        #endregion Public Properties

        #region Public Constructors

        public SimulatedRoute(List<GeoCoordinate> coordinates, bool isLooping)
        {
            Coordinates = coordinates;
            IsLooping = isLooping;
        }

        #endregion Public Constructors

    }

    public sealed class DemoMapObjectProvider : MapObjectProvider, IDisposable
    {

        #region Private Fields

        private readonly Dictionary<DemoMapObject, SimulatedRoute> m_simulators = new Dictionary<DemoMapObject, SimulatedRoute>();
        private readonly List<Thread> m_threads = new List<Thread>();

        #endregion Private Fields

        #region Public Events

        public event EventHandler<GeoCoordinate> NewPosition;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Demo map object provider";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => new Guid("{CD719862-D4D3-491E-A7FD-F058F9481AC0}");

        /// <summary>
        /// Gets or Sets the time in ms to wait between simulation
        /// </summary>
        public int WaitTime
        {
            get;
            private set;
        }

        #endregion Public Properties

        #region Public Constructors

        public DemoMapObjectProvider()
        {
            LoadConfiguration();

            WaitCallback pFunc = delegate { Start(); };
            ThreadPool.QueueUserWorkItem(pFunc);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            foreach (var thread in m_threads)
            {
                thread.Abort();
            }
            m_threads.Clear();
        }

        [Obsolete("Use IList<MapObject> Query(MapObjectProviderContext context) instead")]
        public override IList<MapObject> Query(Guid mapId, GeoBounds viewArea)
        {
            var map = Workspace.Sdk.GetEntity(mapId) as Map;

            // We only provide accidents for geo referenced maps
            if ((map != null) && map.IsGeoReferenced)
            {
                var result = new List<MapObject>(m_simulators.Keys);
                return result;
            }

            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadConfiguration()
        {
            var dllPath = Assembly.GetExecutingAssembly().Location;
            if (!string.IsNullOrEmpty(dllPath))
            {
                var directory = Path.GetDirectoryName(dllPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    var xmlFilePath = Path.Combine(directory, "Configuration.xml");
                    var config = ConfigurationXml.FromFile(xmlFilePath);
                    if (config != null)
                    {
                        MotionHeatMapLayerBuilder.IsEnabled = config.ShowHeatMap;
                        WaitTime = config.WaitTime;

                        if (config.Simulators != null)
                        {
                            lock (m_simulators)
                            {
                                const double defaultLat = 45.442397;
                                const double defaultLon = -73.656514;

                                foreach (var mapObject in config.Simulators.Select(simulator => new DemoMapObject(simulator.Name, simulator.GetImageSource(),
                                    simulator.RouteFile, simulator.GetColor(), simulator.IsClusterable)
                                {
                                    Latitude = defaultLat,
                                    Longitude = defaultLon
                                }))
                                {
                                    m_simulators.Add(mapObject, new SimulatedRoute(new List<GeoCoordinate>(), true));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void OnUpdateCoordinatesThreadStart(object o)
        {
            if (o is DemoMapObject car)
            {
                SimulatedRoute simulatedRoute;

                lock (m_simulators)
                {
                    if (!m_simulators.TryGetValue(car, out simulatedRoute))
                    {
                        return;
                    }
                }

                while (true)
                {
                    if (!simulatedRoute.IsLooping)
                    {
                        simulatedRoute.Coordinates.Reverse();
                    }

                    foreach (var coord in simulatedRoute.Coordinates)
                    {
                        car.Latitude = coord.Latitude;
                        car.Longitude = coord.Longitude;

                        NewPosition?.Invoke(this, coord);

                        Thread.Sleep(WaitTime);
                    }
                    Thread.Sleep(WaitTime);
                }
            }
        }

        private void ParseRoutes()
        {
            try
            {
                string executableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (executableDirectory != null)
                {
                    lock (m_simulators)
                    {
                        foreach (var car in m_simulators)
                        {
                            var filePath = Path.Combine(executableDirectory, car.Key.RouteFile);
                            if (System.IO.File.Exists(filePath))
                            {
                                var doc = XDocument.Load(filePath);

                                foreach (var des in doc.Descendants("rtept"))
                                {
                                    var latitude = des.Attribute("lat")?.Value;
                                    var longitude = des.Attribute("lon")?.Value;

                                    if (longitude != null && latitude != null)
                                    {
                                        var coordinate = new GeoCoordinate(double.Parse(latitude, CultureInfo.InvariantCulture),
                                                double.Parse(longitude, CultureInfo.InvariantCulture));
                                        car.Value.Coordinates.Add(coordinate);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Protect against parsing exceptions
                Console.WriteLine(ex);
            }
        }

        private void Start()
        {
            ParseRoutes();

            lock (m_simulators)
            {
                int index = 1;
                foreach (var car in m_simulators)
                {
                    var thread = new Thread(OnUpdateCoordinatesThreadStart)
                    {
                        Name = "Demo thread #" + index,
                        IsBackground = true
                    };
                    thread.Start(car.Key);
                    m_threads.Add(thread);

                    index++;
                }
            }
        }

        #endregion Private Methods

    }
}