using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.MediaSupport
{
    /// <summary>
    /// Helper class for checking media formats ( audio, video, image ).
    /// </summary>
    public class MediaHelper
    {
        private static IDictionary<string, bool> _audioFormats = new Dictionary<string, bool>();
        private static IDictionary<string, bool> _videoFormats = new Dictionary<string, bool>();
        private static IDictionary<string, bool> _imageFormats = new Dictionary<string, bool>();


        /// <summary>
        /// Initialize the formats for audio, video, image.
        /// </summary>
        static MediaHelper()
        {
            _audioFormats["wav"] = true;
            _audioFormats["mp3"] = true;
            _audioFormats["m4p"] = true;
            _audioFormats["wma"] = true;
            _audioFormats["aiff"] = true;
            _audioFormats["au"] = true;
            _audioFormats[".wav"] = true;
            _audioFormats[".mp3"] = true;
            _audioFormats[".m4p"] = true;
            _audioFormats[".wma"] = true;
            _audioFormats[".aiff"] = true;
            _audioFormats[".au"] = true;

            _videoFormats["mpeg"] = true;
            _videoFormats[".mpeg"] = true;

            _imageFormats["gif"] = true;
            _imageFormats["tiff"] = true;
            _imageFormats["jpg"] = true;
            _imageFormats["jpeg"] = true;
            _imageFormats["bmp"] = true;
            _imageFormats["png"] = true;
            _imageFormats["raw"] = true;
            _imageFormats["jfif"] = true;
            _imageFormats[".gif"] = true;
            _imageFormats[".tiff"] = true;
            _imageFormats[".jpg"] = true;
            _imageFormats[".jpeg"] = true;
            _imageFormats[".bmp"] = true;
            _imageFormats[".png"] = true;
            _imageFormats[".raw"] = true;
            _imageFormats[".jfif"] = true;
        }


        /// <summary>
        /// Whether or not the format supplied is an audio format.
        /// </summary>
        /// <param name="format">Name of format.</param>
        /// <returns>True if it's an audio format.</returns>
        public static bool IsAudioFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) return false;

            format = format.Trim().ToLower();
            return _audioFormats.ContainsKey(format);
        }


        /// <summary>
        /// Whether or not the format supplied is an audio format.
        /// </summary>
        /// <param name="format">Name of format.</param>
        /// <returns>True if it's a video format.</returns>
        public static bool IsVideoFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) return false;

            format = format.Trim().ToLower();
            return _videoFormats.ContainsKey(format);
        }


        /// <summary>
        /// Whether or not the format supplied is an audio format.
        /// </summary>
        /// <param name="format">Name of format.</param>
        /// <returns>True if it's an image format.</returns>
        public static bool IsImageFormat(string format)
        {
            if (string.IsNullOrEmpty(format)) return false;

            format = format.Trim().ToLower();
            return _imageFormats.ContainsKey(format);
        }
    }
}
