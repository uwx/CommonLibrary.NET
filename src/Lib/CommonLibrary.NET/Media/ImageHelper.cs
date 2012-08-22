using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace ComLib.MediaSupport
{
    /// <summary>
    /// Helper class for graphics processing.
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// Convert byte arry to an image object.
        /// </summary>
        /// <param name="content">The bytes representing the image.</param>
        /// <returns>Image created from bytes.</returns>
        public static Image ConvertToImage(byte[] content)
        {
            MemoryStream ms = new MemoryStream(content);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }


        /// <summary>
        /// Converts an image to a byte array.
        /// </summary>
        /// <param name="image">Image to convert.</param>
        /// <returns>Array of bytes with converted image.</returns>
        public byte[] ConvertToBytes(Image image)
        {            
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }


        /// <summary>
        /// Converts an image to a byte array.
        /// </summary>
        /// <param name="image">Image to convert.</param>
        /// <param name="format">The format of the image.</param>
        /// <returns>Array of bytes with converted image.</returns>
        public byte[] ConvertToBytes(Image image, ImageFormat format)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }


        /// <summary>
        /// Convert the image contents as a byte[] into a thumbnail represented by another byte[].
        /// </summary>
        /// <param name="imageContents">The byte[] for the original contents.</param>
        /// <param name="thumbNailHeigth">Height for thumbnail</param>
        /// <param name="thumbNailWidth">Width for thumbnail</param>
        /// <returns>Processing results.</returns>
        public static BoolMessageItem<byte[]> ConvertToThumbNail(byte[] imageContents, int thumbNailWidth, int thumbNailHeigth)
        {
            byte[] thumbnailBytes = null;
            BoolMessageItem<byte[]> result = null;
            
            if (thumbNailWidth == 0) thumbNailWidth = 50;
            if (thumbNailHeigth == 0) thumbNailHeigth = 40;

            try
            {   
                using (Image image = ConvertToImage(imageContents))
                {
                    // Now generate the thumbnail.
                    using (Image thumbnail = image.GetThumbnailImage(50, 40, null, new System.IntPtr()))
                    {
                        // The below code converts an Image object to a byte array
                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            thumbnailBytes = ms.ToArray();
                            result = new BoolMessageItem<byte[]>(thumbnailBytes, true, string.Empty);
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = new BoolMessageItem<byte[]>(null, false, "Error converting to thumbnail.");
            }
            return result;
        }


        /// <summary>
        /// Returns the height and width of the image.
        /// </summary>
        /// <param name="imageContents">Byte array with converted image.</param>
        /// <returns>Tuple with height and width.</returns>
        public static Tuple2<int, int> GetDimensions(byte[] imageContents)
        {
            Image image = ConvertToImage(imageContents);
            int width = Convert.ToInt32(image.PhysicalDimension.Width);
            int height = Convert.ToInt32(image.PhysicalDimension.Height);
            return new Tuple2<int, int>(height, width);
        }
    }
}
