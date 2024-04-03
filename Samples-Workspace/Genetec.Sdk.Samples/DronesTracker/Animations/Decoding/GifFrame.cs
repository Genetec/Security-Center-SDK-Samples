// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DronesTracker.Animations.Decoding
{
    internal class GifFrame : GifBlock
    {
        #region Internal Fields

        internal const int ImageSeparator = 0x2C;

        #endregion Internal Fields

        #region Public Properties

        public GifImageDescriptor Descriptor { get; private set; }
        public IList<GifExtension> Extensions { get; private set; }
        public GifImageData ImageData { get; private set; }
        public GifColor[] LocalColorTable { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal override GifBlockKind Kind => GifBlockKind.GraphicRendering;

        #endregion Internal Properties

        #region Private Constructors

        private GifFrame()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifFrame ReadFrame(Stream stream, IEnumerable<GifExtension> controlExtensions, bool metadataOnly)
        {
            var frame = new GifFrame();

            frame.Read(stream, controlExtensions, metadataOnly);

            return frame;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream, IEnumerable<GifExtension> controlExtensions, bool metadataOnly)
        {
            // Note: at this point, the Image Separator (0x2C) has already been read

            Descriptor = GifImageDescriptor.ReadImageDescriptor(stream);
            if (Descriptor.HasLocalColorTable)
            {
                LocalColorTable = GifHelpers.ReadColorTable(stream, Descriptor.LocalColorTableSize);
            }
            ImageData = GifImageData.ReadImageData(stream, metadataOnly);
            Extensions = controlExtensions.ToList().AsReadOnly();
        }

        #endregion Private Methods
    }
}