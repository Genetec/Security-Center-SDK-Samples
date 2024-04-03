// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.IO;
using System.Text;

namespace DronesTracker.Animations.Decoding
{
    internal class GifCommentExtension : GifExtension
    {
        #region Internal Fields

        internal const int ExtensionLabel = 0xFE;

        #endregion Internal Fields

        #region Public Properties

        public string Text { get; private set; }

        #endregion Public Properties

        #region Internal Properties

        internal override GifBlockKind Kind => GifBlockKind.SpecialPurpose;

        #endregion Internal Properties

        #region Private Constructors

        private GifCommentExtension()
        {
        }

        #endregion Private Constructors

        #region Internal Methods

        internal static GifCommentExtension ReadComment(Stream stream)
        {
            var comment = new GifCommentExtension();
            comment.Read(stream);
            return comment;
        }

        #endregion Internal Methods

        #region Private Methods

        private void Read(Stream stream)
        {
            // Note: at this point, the label (0xFE) has already been read

            var bytes = GifHelpers.ReadDataBlocks(stream, false);
            if (bytes != null)
                Text = Encoding.ASCII.GetString(bytes);
        }

        #endregion Private Methods
    }
}