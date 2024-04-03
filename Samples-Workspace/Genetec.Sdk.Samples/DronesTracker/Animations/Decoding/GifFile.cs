// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DronesTracker.Animations.Decoding
{
    internal class GifFile
    {
        #region Public Properties

        public IList<GifExtension> Extensions { get; set; }
        public IList<GifFrame> Frames { get; set; }
        public GifColor[] GlobalColorTable { get; set; }
        public GifHeader Header { get; private set; }
        public ushort RepeatCount { get; set; }

        #endregion Public Properties

        #region Private Constructors

        private GifFile()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifFile ReadGifFile(Stream stream, bool metadataOnly)
        {
            var file = new GifFile();
            file.Read(stream, metadataOnly);
            return file;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream, bool metadataOnly)
        {
            Header = GifHeader.ReadHeader(stream);

            if (Header.LogicalScreenDescriptor.HasGlobalColorTable)
            {
                GlobalColorTable = GifHelpers.ReadColorTable(stream, Header.LogicalScreenDescriptor.GlobalColorTableSize);
            }
            ReadFrames(stream, metadataOnly);

            var netscapeExtension =
                            Extensions
                                .OfType<GifApplicationExtension>()
                                .FirstOrDefault(GifHelpers.IsNetscapeExtension);

            RepeatCount = netscapeExtension != null ? GifHelpers.GetRepeatCount(netscapeExtension) : (ushort)1;
        }

        private void ReadFrames(Stream stream, bool metadataOnly)
        {
            var frames = new List<GifFrame>();
            var controlExtensions = new List<GifExtension>();
            var specialExtensions = new List<GifExtension>();
            while (true)
            {
                var block = GifBlock.ReadBlock(stream, controlExtensions, metadataOnly);

                if (block.Kind == GifBlockKind.GraphicRendering)
                    controlExtensions = new List<GifExtension>();

                if (block is GifFrame frame)
                {
                    frames.Add(frame);
                }
                else if (block is GifExtension extension)
                {
                    switch (extension.Kind)
                    {
                        case GifBlockKind.Control:
                            controlExtensions.Add(extension);
                            break;

                        case GifBlockKind.SpecialPurpose:
                            specialExtensions.Add(extension);
                            break;

                        case GifBlockKind.GraphicRendering:
                            break;

                        case GifBlockKind.Other:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (block is GifTrailer)
                {
                    break;
                }
            }

            Frames = frames.AsReadOnly();
            Extensions = specialExtensions.AsReadOnly();
        }

        #endregion Private Methods
    }
}