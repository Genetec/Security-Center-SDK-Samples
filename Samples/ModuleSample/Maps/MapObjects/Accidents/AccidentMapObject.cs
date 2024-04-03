// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Entities.Maps;
using System;

namespace ModuleSample.Maps.MapObjects.Accidents
{
    [MapObject(IdString, typeof(AccidentMapObjectView), false)]
    public class AccidentMapObject : MapObject
    {

        #region Public Fields

        public const string AccidentLayerName = "Accidents";
        public static readonly Guid AccidentsLayerId = new Guid("{4DBDF995-4818-4EC0-8DC4-315E78041234}");

        #endregion Public Fields

        #region Private Fields

        private const string IdString = "{E31354E7-1343-4159-B2B8-77C06A4E020A}";
        private static readonly Guid AccidentMapObjectId = new Guid(IdString);

        #endregion Private Fields

        #region Public Properties

        public string Description { get; private set; }

        public override Guid LayerId => AccidentsLayerId;

        #endregion Public Properties

        #region Public Constructors

        public AccidentMapObject(double latitude, double longitude, string description)
            : base(AccidentMapObjectId)
        {
            Latitude = latitude;
            Longitude = longitude;
            Description = description;
        }

        #endregion Public Constructors

    }
}