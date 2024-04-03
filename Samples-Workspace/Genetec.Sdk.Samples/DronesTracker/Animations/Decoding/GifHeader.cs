// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.IO;

namespace DronesTracker.Animations.Decoding
{
    internal class GifHeader : GifBlock
    {
        #region Public Properties

        public GifLogicalScreenDescriptor LogicalScreenDescriptor { get; private set; }
        public string Signature { get; private set; }
        public string Version { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal override GifBlockKind Kind => GifBlockKind.Other;

        #endregion Internal Properties

        #region Private Constructors

        private GifHeader()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifHeader ReadHeader(Stream stream)
        {
            var header = new GifHeader();
            header.Read(stream);
            return header;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream)
        {
            Signature = GifHelpers.ReadString(stream, 3);
            if (Signature != "GIF")
                throw GifHelpers.InvalidSignatureException(Signature);
            Version = GifHelpers.ReadString(stream, 3);
            if (Version != "87a" && Version != "89a")
                throw GifHelpers.UnsupportedVersionException(Version);
            LogicalScreenDescriptor = GifLogicalScreenDescriptor.ReadLogicalScreenDescriptor(stream);
        }

        #endregion Private Methods
    }
}