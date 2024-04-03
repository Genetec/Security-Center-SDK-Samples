// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.IO;

namespace DronesTracker.Animations.Decoding
{
    // label 0xF9
    internal class GifGraphicControlExtension : GifExtension
    {

        #region Internal Fields

        internal const int ExtensionLabel = 0xF9;

        #endregion Internal Fields

        #region Public Properties

        public int BlockSize { get; private set; }
        public int Delay { get; private set; }
        public int DisposalMethod { get; private set; }
        public bool HasTransparency { get; private set; }
        public int TransparencyIndex { get; private set; }
        public bool UserInput { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal override GifBlockKind Kind => GifBlockKind.Control;

        #endregion Internal Properties

        #region Private Constructors

        private GifGraphicControlExtension()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifGraphicControlExtension ReadGraphicsControl(Stream stream)
        {
            var ext = new GifGraphicControlExtension();
            ext.Read(stream);
            return ext;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream)
        {
            // Note: at this point, the label (0xF9) has already been read

            var bytes = new byte[6];
            stream.ReadAll(bytes, 0, bytes.Length);
            BlockSize = bytes[0]; // should always be 4
            if (BlockSize != 4)
                throw GifHelpers.InvalidBlockSizeException("Graphic Control Extension", 4, BlockSize);
            var packedFields = bytes[1];
            DisposalMethod = (packedFields & 0x1C) >> 2;
            UserInput = (packedFields & 0x02) != 0;
            HasTransparency = (packedFields & 0x01) != 0;
            Delay = BitConverter.ToUInt16(bytes, 2) * 10; // milliseconds
            TransparencyIndex = bytes[4];
        }

        #endregion Private Methods

    }
}