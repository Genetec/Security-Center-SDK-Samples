// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.IO;

namespace DronesTracker.Animations.Decoding
{
    internal class GifImageDescriptor
    {
        #region Public Properties

        public bool HasLocalColorTable { get; private set; }
        public int Height { get; private set; }
        public bool Interlace { get; private set; }
        public bool IsLocalColorTableSorted { get; private set; }
        public int Left { get; private set; }
        public int LocalColorTableSize { get; private set; }
        public int Top { get; private set; }
        public int Width { get; private set; }

        #endregion Public Properties

        #region Private Constructors

        private GifImageDescriptor()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifImageDescriptor ReadImageDescriptor(Stream stream)
        {
            var descriptor = new GifImageDescriptor();
            descriptor.Read(stream);
            return descriptor;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream)
        {
            var bytes = new byte[9];
            stream.ReadAll(bytes, 0, bytes.Length);
            Left = BitConverter.ToUInt16(bytes, 0);
            Top = BitConverter.ToUInt16(bytes, 2);
            Width = BitConverter.ToUInt16(bytes, 4);
            Height = BitConverter.ToUInt16(bytes, 6);
            var packedFields = bytes[8];
            HasLocalColorTable = (packedFields & 0x80) != 0;
            Interlace = (packedFields & 0x40) != 0;
            IsLocalColorTableSorted = (packedFields & 0x20) != 0;
            LocalColorTableSize = 1 << ((packedFields & 0x07) + 1);
        }

        #endregion Private Methods
    }
}