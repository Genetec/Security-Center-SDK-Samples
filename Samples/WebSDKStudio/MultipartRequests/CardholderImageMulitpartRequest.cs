using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace WebSDKStudio.MultipartRequests
{
    internal class CardholderImageMulitpartRequest : MultipartRequest
    {
        /// <summary>
        /// Guid of the cardholder to set the picture to.
        /// </summary>
        internal protected Guid m_cardholderGuid;

        /// <summary>
        /// The image to send.
        /// </summary>
        internal protected Image m_image;

        /// <summary>
        /// The Image Name, extracted from the Path to the image.
        /// </summary>
        private readonly string m_imageName;

        public CardholderImageMulitpartRequest(string image, string uriServer, Guid cardholderGuid, WebSdkStudioWindow webSdkStudioWindow)
        {
            m_image = Image.FromFile(image);
            m_uriServer = uriServer;
            var imageSplit = image.Split('\\');
            m_imageName = imageSplit.Last();
            m_cardholderGuid = cardholderGuid;
            m_webSdkStudioWindow = webSdkStudioWindow;
        }

        public void UploadData()
        {
            //String parameters
            var myPicture = ImageToBase64String(m_image);
            var myPictureArray = Encoding.GetBytes(myPicture);

            m_postParameters.Add("$myPicture", new FileParameter(myPictureArray, m_imageName, "image/png"));
            m_postUrl = m_uriServer + "entity?q=entity=" + m_cardholderGuid + ",Picture=$myPicture";

            MultipartFormDataPost();
        }

        private static string ImageToBase64String(Image image)
        {
            var imageBytes = ImageToByteArray(image);
            return Convert.ToBase64String(imageBytes);
        }

        private static byte[] ImageToByteArray(Image image)
        {
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
