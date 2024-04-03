// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Collections.Generic;
using System.IO;

namespace DronesTracker.Animations.Decoding
{
    internal abstract class GifExtension : GifBlock
    {
        #region Internal Fields

        internal const int ExtensionIntroducer = 0x21;

        #endregion Internal Fields

        #region Internal Methods

        internal static GifExtension ReadExtension(Stream stream, IEnumerable<GifExtension> controlExtensions, bool metadataOnly)
        {
            // Note: at this point, the Extension Introducer (0x21) has already been read

            int label = stream.ReadByte();
            if (label < 0)
                throw GifHelpers.UnexpectedEndOfStreamException();
            switch (label)
            {
                case GifGraphicControlExtension.ExtensionLabel:
                    return GifGraphicControlExtension.ReadGraphicsControl(stream);

                case GifCommentExtension.ExtensionLabel:
                    return GifCommentExtension.ReadComment(stream);

                case GifPlainTextExtension.ExtensionLabel:
                    return GifPlainTextExtension.ReadPlainText(stream, controlExtensions, metadataOnly);

                case GifApplicationExtension.ExtensionLabel:
                    return GifApplicationExtension.ReadApplication(stream);

                default:
                    throw GifHelpers.UnknownExtensionTypeException(label);
            }
        }

        #endregion Internal Methods
    }
}