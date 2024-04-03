// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Xml.Linq;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components.MapObjectProvider;

namespace HeatMapLayer.Providers
{
    internal struct CarCoordinates
    {

        #region Public Properties

        public List<GeoCoordinate> Coordinates { get; }

        public bool IsLooping { get; }

        #endregion Public Properties

        #region Public Constructors

        public CarCoordinates(List<GeoCoordinate> coordinates, bool isLooping)
        {
            Coordinates = coordinates;
            IsLooping = isLooping;
        }

        #endregion Public Constructors
    }

    public sealed class CarMapObjectProvider : MapObjectProvider, IDisposable
    {

        #region Private Fields

        private readonly Dictionary<CarMapObject, CarCoordinates> m_cars = new Dictionary<CarMapObject, CarCoordinates>();

        private readonly List<Thread> m_threads = new List<Thread>();

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{52B67BBC-6FD4-414A-8F59-49BC27C9EBF7}"));

        #endregion Private Fields

        #region Public Events

        public event EventHandler<GeoCoordinate> NewPosition;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => nameof(CarMapObjectProvider);

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Constructors

        public CarMapObjectProvider()
        {
            CreateCars();

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

        public override IList<MapObject> Query(MapObjectProviderContext context)
        {
            var map = Workspace.Sdk.GetEntity(context.MapId) as Map;

            // We only provide accidents for geo referenced maps
            if ((map != null) && map.IsGeoReferenced)
            {
                var result = new List<MapObject>(m_cars.Keys);
                return result;
            }
            return null;
        }

        #endregion Public Methods

        #region Private Methods

        private void CreateCars()
        {
            lock (m_cars)
            {
                const double defaultLat = 45.442397;
                const double defaultLon = -73.656514;

                const string route1 = @"Resources\route1.txt";
                const string route2 = @"Resources\route2.txt";
                const string route3 = @"Resources\route3.txt";

                var car1 = new CarMapObject { RouteFile = route1, Latitude = defaultLat, Longitude = defaultLon };
                var car2 = new CarMapObject { RouteFile = route2, Latitude = defaultLat, Longitude = defaultLon };
                var car3 = new CarMapObject { RouteFile = route3, Latitude = defaultLat, Longitude = defaultLon };

                m_cars.Add(car1, new CarCoordinates(new List<GeoCoordinate>(), true));
                m_cars.Add(car2, new CarCoordinates(new List<GeoCoordinate>(), false));
                m_cars.Add(car3, new CarCoordinates(new List<GeoCoordinate>(), true));
            }
        }

        private void OnUpdateCoordinatesThreadStart(object o)
        {
            if (o is CarMapObject car)
            {
                CarCoordinates carCoordinates;

                lock (m_cars)
                {
                    if (!m_cars.TryGetValue(car, out carCoordinates))
                    {
                        return;
                    }
                }

                while (true)
                {
                    if (!carCoordinates.IsLooping)
                    {
                        carCoordinates.Coordinates.Reverse();
                    }

                    foreach (var coord in carCoordinates.Coordinates)
                    {
                        car.Latitude = coord.Latitude;
                        car.Longitude = coord.Longitude;

                        NewPosition?.Invoke(this, coord);

                        Thread.Sleep(1500);
                    }
                    Thread.Sleep(1500);
                }
            }
        }

        private void ParseRoutes()
        {
            try
            {
                var executableDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (executableDirectory != null)
                {
                    lock (m_cars)
                    {
                        foreach (var car in m_cars)
                        {
                            var filePath = Path.Combine(executableDirectory, car.Key.RouteFile);
                            if (System.IO.File.Exists(filePath))
                            {
                                var doc = XDocument.Load(filePath);

                                foreach (var des in doc.Descendants("rtept"))
                                {
                                    var latitude = des.Attribute("lat").Value;
                                    var longitude = des.Attribute("lon").Value;

                                    var coor = new GeoCoordinate(double.Parse(latitude, CultureInfo.InvariantCulture),
                                        double.Parse(longitude, CultureInfo.InvariantCulture));
                                    car.Value.Coordinates.Add(coor);
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

            lock (m_cars)
            {
                var index = 1;
                foreach (var car in m_cars)
                {
                    var thread = new Thread(OnUpdateCoordinatesThreadStart)
                    {
                        Name = "Car thread #" + index,
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