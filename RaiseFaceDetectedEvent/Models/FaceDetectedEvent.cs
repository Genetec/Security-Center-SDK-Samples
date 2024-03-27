using System.Windows.Media.Imaging;

namespace RaiseFaceDetectedEvent.Models
{
    #region Classes

    public class FaceDetectedEvent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the age associated with this video analytics face detected event.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int Age { get; set; }

        /// <summary>
        /// Gets or sets the confidence associated with this video analytics face detected event.
        /// </summary>
        /// <value>
        /// The confidence ration.
        /// </value>
        public double ConfidenceRatio { get; set; }

        /// <summary>
        /// Gets or sets the face image associated with this video analytics event.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public BitmapImage Image { get; set; }

        /// <summary>
        /// Gets or sets the metadata related to the Face Detection Event..
        /// </summary>
        /// <value>
        /// The metadata.
        /// </value>
        public string Metadata { get; set; }

        #endregion
    }

    #endregion
}

