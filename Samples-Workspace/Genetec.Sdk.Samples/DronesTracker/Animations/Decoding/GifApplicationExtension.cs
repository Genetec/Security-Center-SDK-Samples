// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.IO;
using System.Text;

namespace DronesTracker.Animations.Decoding
{
    // label 0xFF
    internal class GifApplicationExtension : GifExtension
    {
        #region Internal Fields

        internal const int ExtensionLabel = 0xFF;

        #endregion Internal Fields

        #region Public Properties

        public string ApplicationIdentifier { get; private set; }
        public byte[] AuthenticationCode { get; private set; }
        public int BlockSize { get; private set; }
        public byte[] Data { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal override GifBlockKind Kind => GifBlockKind.SpecialPurpose;

        #endregion Internal Properties

        #region Private Constructors

        private GifApplicationExtension()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifApplicationExtension ReadApplication(Stream stream)
        {
            var ext = new GifApplicationExtension();
            ext.Read(stream);
            return ext;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream)
        {
            // Note: at this point, the label (0xFF) has already been read

            var bytes = new byte[12];
            stream.ReadAll(bytes, 0, bytes.Length);
            BlockSize = bytes[0]; // should always be 11
            if (BlockSize != 11)
                throw GifHelpers.InvalidBlockSizeException("Application Extension", 11, BlockSize);

            ApplicationIdentifier = Encoding.ASCII.GetString(bytes, 1, 8);
            var authCode = new byte[3];
            Array.Copy(bytes, 9, authCode, 0, 3);
            AuthenticationCode = authCode;
            Data = GifHelpers.ReadDataBlocks(stream, false);
        }

        #endregion Private Methods
    }
}