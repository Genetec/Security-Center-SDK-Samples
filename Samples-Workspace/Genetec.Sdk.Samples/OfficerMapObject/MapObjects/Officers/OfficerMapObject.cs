// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Entities.Maps;
using System;

namespace OfficerMapObject.MapObjects.Officers
{
    [MapObject(IdString, typeof(OfficerMapObjectView), false)]
    public sealed class OfficerMapObject : MapObject
    {
        #region Public Fields

        public static readonly Guid OfficerLayerId = new Guid("{F5EE347D-EE1F-4C37-B417-50C0C7D9EE8A}");

        #endregion Public Fields

        #region Private Fields

        private const string IdString = "{BE5A991D-7F7F-4B9E-932E-C49103C90628}";
        private new static readonly Guid Id = new Guid(IdString);

        #endregion Private Fields

        #region Public Properties

        public override bool IsClusterable => true;

        public override Guid LayerId => OfficerLayerId;

        #endregion Public Properties

        #region Public Constructors

        public OfficerMapObject()
            : base(Id)
        {
        }

        public OfficerMapObject(double latitude, double longitude)
            : base(Id)
        {
            Latitude = latitude;
            Longitude = longitude;
            LinkedEntity = new Guid("c7297bd9-ba9c-4211-b4e6-d79c152d3a59");
        }

        #endregion Public Constructors
    }
}