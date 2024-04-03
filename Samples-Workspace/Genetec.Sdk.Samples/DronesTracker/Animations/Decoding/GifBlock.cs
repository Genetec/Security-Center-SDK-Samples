// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Collections.Generic;
using System.IO;

namespace DronesTracker.Animations.Decoding
{
    internal abstract class GifBlock
    {
        #region Internal Properties

        internal abstract GifBlockKind Kind { get; }

        #endregion Internal Properties

        #region Internal Methods

        internal static GifBlock ReadBlock(Stream stream, IEnumerable<GifExtension> controlExtensions, bool metadataOnly)
        {
            var blockId = stream.ReadByte();
            if (blockId < 0)
                throw GifHelpers.UnexpectedEndOfStreamException();
            switch (blockId)
            {
                case GifExtension.ExtensionIntroducer:
                    return GifExtension.ReadExtension(stream, controlExtensions, metadataOnly);

                case GifFrame.ImageSeparator:
                    return GifFrame.ReadFrame(stream, controlExtensions, metadataOnly);

                case GifTrailer.TrailerByte:
                    return GifTrailer.ReadTrailer();

                default:
                    throw GifHelpers.UnknownBlockTypeException(blockId);
            }
        }

        #endregion Internal Methods
    }
}