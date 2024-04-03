using Genetec.Sdk.Entities.Maps;
using System;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject.Maps
{
    /// <summary>
    /// Class that represents the identity of an accident map object.
    /// </summary>
    public sealed class AccidentMapObjectIdentifier
    {
        public const string StringIdentifier = "{F3147E96-923F-4C75-944C-2CF9E76AA844}";
        public static readonly Guid Identifier = new Guid(StringIdentifier);
    }

    /// <summary>
    /// The accident map object.
    /// </summary>
    [MapObject(AccidentMapObjectIdentifier.StringIdentifier, false)]
    public sealed class AccidentMapObject : MapObject
    {
        /// <summary>
        /// Gets or sets the descriptions of the accident map object.
        /// </summary>
        public string Description
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="AccidentMapObject"/> class.
        /// </summary>
        /// <param name="latitude">The latitude of the accident map object.</param>
        /// <param name="longitude">The longitude of the accident map object.</param>
        /// <param name="description">The description of the accident map object.</param>
        public AccidentMapObject(double latitude, double longitude, string description)
            : base(AccidentMapObjectIdentifier.Identifier)
        {
            Latitude = latitude;
            Longitude = longitude;
            Description = description;
        }
    }
}