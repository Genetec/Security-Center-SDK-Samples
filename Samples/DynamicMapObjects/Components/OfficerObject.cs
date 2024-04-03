using DynamicMapObjects.Maps;
using Genetec.Sdk;
using System.Collections.Generic;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace DynamicMapObjects.Components
{
    #region Classes

    class OfficerObject
    {
        #region Constants

        private readonly OfficerMapObject m_officer;

        #endregion

        #region Fields

        private int m_currentRouteStep;

        private bool m_reverse;

        private int m_routeIndex;

        #endregion

        #region Properties

        public OfficerMapObject Officer
        {
            get { return m_officer; }
        }

        public List<List<GeoCoordinate>> Routes { get; set; }

        #endregion

        #region Constructors

        public OfficerObject(OfficerMapObject officer, int index)
        {
            m_officer = officer;
            m_routeIndex = index;
            m_reverse = false;
            m_currentRouteStep = 0;
        }

        #endregion

        #region Public Methods

        public void NextStep()
        {
            if (Routes.Count <= 0)
                return;

            var currentRoute = new List<GeoCoordinate>(Routes[m_routeIndex]);

            if (currentRoute.Count <= 0)
                return;

            //If the officer is on it's way back, reverse the route
            if (m_reverse)
            {
                currentRoute.Reverse();
            }
            //Set the officer's coordinates
            m_officer.Latitude = currentRoute[m_currentRouteStep].Latitude;
            m_officer.Longitude = currentRoute[m_currentRouteStep].Longitude;
            m_currentRouteStep++;

            //If we're at our destination
            if (m_currentRouteStep == currentRoute.Count)
            {
                //Start a new route from the beginning
                m_currentRouteStep = 0;
                //If we were on our way back to the starting point, get the next route 
                if (m_reverse)
                {
                    m_reverse = false;
                    m_routeIndex++;
                }
                //Go back to the starting p=oint
                else
                {
                    m_reverse = true;
                }
            }
            //Go back to the first route when the end of the list is reached
            if (m_routeIndex == Routes.Count)
            {
                m_routeIndex = 0;
            }
        }

        #endregion
    }

    #endregion
}

