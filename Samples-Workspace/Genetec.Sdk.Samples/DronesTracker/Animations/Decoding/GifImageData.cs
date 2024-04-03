// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.IO;

namespace DronesTracker.Animations.Decoding
{
    internal class GifImageData
    {
        #region Public Properties

        public byte[] CompressedData { get; set; }
        public byte LzwMinimumCodeSize { get; set; }

        #endregion Public Properties

        #region Private Constructors

        private GifImageData()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifImageData ReadImageData(Stream stream, bool metadataOnly)
        {
            var imgData = new GifImageData();
            imgData.Read(stream, metadataOnly);
            return imgData;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream, bool metadataOnly)
        {
            LzwMinimumCodeSize = (byte)stream.ReadByte();
            CompressedData = GifHelpers.ReadDataBlocks(stream, metadataOnly);
        }

        #endregion Private Methods
    }
}