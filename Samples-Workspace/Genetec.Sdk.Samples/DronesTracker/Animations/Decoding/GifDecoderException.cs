// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;

namespace DronesTracker.Animations.Decoding
{
    [Serializable]
    internal class GifDecoderException : Exception
    {
        #region Internal Constructors

        internal GifDecoderException()
        {
        }

        internal GifDecoderException(string message) : base(message)
        {
        }

        internal GifDecoderException(string message, Exception inner) : base(message, inner)
        {
        }

        #endregion Internal Constructors

        #region Protected Constructors

        protected GifDecoderException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        #endregion Protected Constructors
    }
}