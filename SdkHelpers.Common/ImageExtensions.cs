using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for March 31:
//  1885 – The United Kingdom establishes the Bechuanaland Protectorate.
//  1909 – Serbia accepts Austrian control over Bosnia and Herzegovina.
//  1931 – An earthquake destroys Managua, Nicaragua, killing 2,000.
// ==========================================================================
namespace SdkHelpers.Common
{
    #region Classes

    public static class ImageExtensions
    {
        #region Nested Classes and Structures

        internal static class NativeMethods
        {
            [DllImport("gdi32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            internal static extern bool DeleteObject(IntPtr hObject);
        }

        #endregion

        #region Public Methods

        public static string BitmapImageToBase64Sting(this BitmapImage image, BitmapEncoder encoder)
        {
            encoder.Frames.Add(BitmapFrame.Create(image));
            using (var ms = new MemoryStream())
            {
                encoder.Save(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static System.Drawing.Bitmap BitmapSourceToBitmap(BitmapSource bitmapsource, BitmapEncoder encoder)
        {
            using (var outStream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapsource));
                encoder.Save(outStream);
                return new System.Drawing.Bitmap(outStream);
            }
        }

        public static BitmapImage BitmapToBitmapImage(this System.Drawing.Bitmap bitmap, ImageFormat format)
        {
            var image = new BitmapImage();
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, format);

                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
            }

            return image;
        }

        public static BitmapImage StringBase64ToBitmapImage(this string base64BitmapImage)
        {
            byte[] data = Convert.FromBase64String(base64BitmapImage);
            BitmapImage bitmapImage = null;
            if (data.Any())
            {
                bitmapImage = new BitmapImage();
                var stream = new MemoryStream(data);

                try
                {
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = stream;
                    bitmapImage.CacheOption = BitmapCacheOption.None;
                    bitmapImage.EndInit();
                }
                catch (NotSupportedException)
                {
                    bitmapImage = null;
                }
            }
            return bitmapImage;
        }

        #endregion
    }

    #endregion
}
