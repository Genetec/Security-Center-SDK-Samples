// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

namespace DronesTracker.Animations.Decoding
{
    internal class GifTrailer : GifBlock
    {
        #region Internal Fields

        internal const int TrailerByte = 0x3B;

        #endregion Internal Fields

        #region Internal Properties

        internal override GifBlockKind Kind => GifBlockKind.Other;

        #endregion Internal Properties

        #region Private Constructors

        private GifTrailer()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifTrailer ReadTrailer()
        {
            return new GifTrailer();
        }

        #endregion Internal Methods
    }
}