// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows.Media;
using Genetec.Sdk.Entities.Maps;

namespace DronesTracker.Maps
{
    [MapObject("{42E01283-94A5-4FBB-9779-CA3E93B28E17}", typeof(DemoMapObjectView), false)]
    public sealed class DemoMapObject : MapObject
    {

        #region Private Fields

        private static readonly Guid DemoMapObjectId = new Guid("{42E01283-94A5-4FBB-9779-CA3E93B28E17}");

        #endregion Private Fields

        #region Public Properties

        public Color Color { get; }

        public ImageSource Image { get; }

        /// <summary>
        /// Gets a flag indicating if the object can be clustered
        /// </summary>
        public override bool IsClusterable { get; }

        public string Name { get; }

        public string RouteFile { get; }

        #endregion Public Properties

        #region Public Constructors

        public DemoMapObject(string name, ImageSource image, string routeFile, Color color, bool isClusterable)
            : base(DemoMapObjectId)
        {
            Name = name;
            Image = image;
            RouteFile = routeFile;
            Color = color;
            IsClusterable = isClusterable;
        }

        #endregion Public Constructors

    }
}