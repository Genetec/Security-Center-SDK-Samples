using DynamicMapObjects.Maps;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Components.MapObjectProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Stream = System.IO.Stream;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace DynamicMapObjects.Components
{
    #region Classes

    public sealed class OfficerMapObjectProvider : MapObjectProvider, IDisposable
    {
        #region Constants

        private readonly List<OfficerObject> m_officerList = new List<OfficerObject>();

        private readonly List<List<GeoCoordinate>> m_officerRouteList = new List<List<GeoCoordinate>>();

        #endregion

        #region Fields

        private bool m_moveOfficers = true;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name
        {
            get { return "Officers map object provider"; }
        }

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId
        {
            get { return new Guid("{5DFE38E1-6920-4728-A514-ACA4135EDCA6}"); }
        }

        #endregion

        #region Constructors

        public OfficerMapObjectProvider(Workspace workspace)
        {
            Initialize(workspace);
            workspace.Sdk.LoginManager.LoggedOn += OnEngineLoggedOn;
        }

        #endregion

        #region Destructors and Dispose Methods

        public void Dispose()
        {
            m_moveOfficers = false;
        }

        #endregion

        #region Event Handlers

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs loggedOnEventArgs)
        {
            // Fetch the Id of a camera to link to the officers
            Guid cameraToLink = Guid.Empty;
            EntityConfigurationQuery query = Workspace.Sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            if (query != null)
            {
                query.EntityTypeFilter.Add(EntityType.Camera);
                QueryCompletedEventArgs results = query.Query();
                foreach (DataRow row in results.Data.Rows)
                {
                    if (Workspace.Sdk.GetEntity((Guid)row[0]).IsOnline)
                    {
                        cameraToLink = (Guid)row[0];
                        break;
                    }
                }
            }
            for (int i = 0; i < 5; i++)
            {
                var officer = new OfficerMapObject(cameraToLink);
                m_officerList.Add(new OfficerObject(officer, i * 2));
            }

            Task.Run(() => MoveOfficers());
        }

        #endregion

        #region Public Methods

        public override IList<MapObject> Query(MapObjectProviderContext context)
        {
            var map = Workspace.Sdk.GetEntity(context.MapId) as Map;

            // Provide officers for geo referenced maps
            if ((map != null) && map.IsGeoReferenced)
            {
                var result = new List<MapObject>();
                result.AddRange(m_officerList.Select(i => i.Officer)); // Here, the viewArea could be used to return only the MapObjects inside the displayed bounds
                return result;
            }

            return null;
        }

        #endregion

        #region Private Methods

        private void MoveOfficers()
        {
            ParseOfficerRoutes();
            foreach (var officerObject in m_officerList)
            {
                officerObject.Routes = m_officerRouteList;
            }
            while (m_moveOfficers)
            {
                foreach (var officerObject in m_officerList)
                {
                    officerObject.NextStep();
                }
                Invalidate(Guid.Empty, null, null, m_officerList.Select(i => (MapObject)i.Officer).ToList());
                //Time between coordinates jumps
                Thread.Sleep(150);
            }
        }

        private void ParseOfficerRoutes()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                for (int i = 1; i < 11; i++)
                {
                    var resourceName = "DynamicMapObjects.Resources.Officerroute" + i + ".txt";
                    using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        var doc = XDocument.Load(stream);
                        var officerRoute = new List<GeoCoordinate>();
                        foreach (var des in doc.Descendants("rtept"))
                        {
                            var latitude = des.Attribute("lat").Value;
                            var longitude = des.Attribute("lon").Value;

                            officerRoute.Add(new GeoCoordinate(double.Parse(latitude, CultureInfo.InvariantCulture), double.Parse(longitude, CultureInfo.InvariantCulture)));
                        }
                        m_officerRouteList.Add(officerRoute);
                    }
                }
            }
            catch (Exception ex)
            {
                // Protect against parsing exceptions
                Console.WriteLine(ex);
            }
        }

        #endregion
    }

    #endregion
}

