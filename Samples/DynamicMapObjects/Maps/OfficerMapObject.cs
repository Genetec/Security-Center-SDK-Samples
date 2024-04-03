using Genetec.Sdk.Entities.Maps;
using System;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

namespace DynamicMapObjects.Maps
{

    #region Classes

    [MapObject(IdString, typeof(OfficerMapObjectView), false)]
    public sealed class OfficerMapObject : MapObject
    {
        #region Constants

        private new static readonly Guid Id = new Guid(IdString);

        private const string IdString = "{CCD7A714-2E93-4BB6-8C30-3A8C38A8E6B6}";

        public static readonly Guid OfficerLayerId = new Guid("{F5EE347D-EE1F-4C37-B417-50C0C7D9EE8A}");

        #endregion

        #region Properties

        public override bool IsClusterable { get { return true; } }

        public override Guid LayerId { get { return OfficerLayerId; } }

        #endregion

        #region Constructors

        public OfficerMapObject() : base(Id)
        {
        }

        public OfficerMapObject(Guid linkedEntity) : base(Id)
        {
            LinkedEntity = linkedEntity;
        }

        #endregion
    }
    #endregion
}
