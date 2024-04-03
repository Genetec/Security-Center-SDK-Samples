// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

namespace DronesTracker.Animations.Decoding
{
    internal struct GifColor
    {
        #region Public Properties

        public byte B { get; }

        public byte G { get; }

        public byte R { get; }

        #endregion Public Properties

        #region Internal Constructors

        internal GifColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        #endregion Internal Constructors

        #region Public Methods

        public override string ToString()
        {
            return $"#{R:x2}{G:x2}{B:x2}";
        }

        #endregion Public Methods
    }
}