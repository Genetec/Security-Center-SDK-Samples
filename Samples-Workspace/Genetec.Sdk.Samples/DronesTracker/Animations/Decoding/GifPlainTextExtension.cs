// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DronesTracker.Animations.Decoding
{
    // label 0x01
    internal class GifPlainTextExtension : GifExtension
    {
        #region Internal Fields

        internal const int ExtensionLabel = 0x01;

        #endregion Internal Fields

        #region Public Properties

        public int BackgroundColorIndex { get; private set; }
        public int BlockSize { get; private set; }
        public int CellHeight { get; private set; }
        public int CellWidth { get; private set; }
        public IList<GifExtension> Extensions { get; private set; }
        public int ForegroundColorIndex { get; private set; }
        public int Height { get; private set; }
        public int Left { get; private set; }
        public string Text { get; private set; }
        public int Top { get; private set; }
        public int Width { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal override GifBlockKind Kind => GifBlockKind.GraphicRendering;

        #endregion Internal Properties

        #region Private Constructors

        private GifPlainTextExtension()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifPlainTextExtension ReadPlainText(Stream stream, IEnumerable<GifExtension> controlExtensions, bool metadataOnly)
        {
            var plainText = new GifPlainTextExtension();
            plainText.Read(stream, controlExtensions, metadataOnly);
            return plainText;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream, IEnumerable<GifExtension> controlExtensions, bool metadataOnly)
        {
            // Note: at this point, the label (0x01) has already been read

            var bytes = new byte[13];
            stream.ReadAll(bytes, 0, bytes.Length);

            BlockSize = bytes[0];
            if (BlockSize != 12)
                throw GifHelpers.InvalidBlockSizeException("Plain Text Extension", 12, BlockSize);

            Left = BitConverter.ToUInt16(bytes, 1);
            Top = BitConverter.ToUInt16(bytes, 3);
            Width = BitConverter.ToUInt16(bytes, 5);
            Height = BitConverter.ToUInt16(bytes, 7);
            CellWidth = bytes[9];
            CellHeight = bytes[10];
            ForegroundColorIndex = bytes[11];
            BackgroundColorIndex = bytes[12];

            var dataBytes = GifHelpers.ReadDataBlocks(stream, metadataOnly);
            Text = Encoding.ASCII.GetString(dataBytes);
            Extensions = controlExtensions.ToList().AsReadOnly();
        }

        #endregion Private Methods
    }
}