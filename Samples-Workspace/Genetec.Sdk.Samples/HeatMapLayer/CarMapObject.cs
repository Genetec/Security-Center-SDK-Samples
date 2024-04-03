// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using Genetec.Sdk.Entities.Maps;
using HeatMapLayer.Views;

namespace HeatMapLayer
{
    [MapObject("{42E01283-94A5-4FBB-9779-CA3E93B28E17}", typeof(CarMapObjectView), false)]
    public sealed class CarMapObject : MapObject
    {

        #region Private Fields

        private static readonly Guid CarMapObjectId = new Guid("{42E01283-94A5-4FBB-9779-CA3E93B28E17}");

        #endregion Private Fields

        #region Public Properties

        public string RouteFile { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public CarMapObject()
            : base(CarMapObjectId) { }

        #endregion Public Constructors

    }
}