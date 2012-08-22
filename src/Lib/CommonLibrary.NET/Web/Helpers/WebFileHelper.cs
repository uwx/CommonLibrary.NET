using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using ComLib.Extensions;

namespace ComLib.Web.Helpers
{
    /// <summary>
    /// Helper class for handling uploaded files.
    /// </summary>
    public class WebFileHelper
    {
        /// <summary>
        /// Checks if any media files were uploaded by checking the size of each posted file.
        /// </summary>
        /// <param name="request">Http request.</param>
        /// <returns>True if media files were uploaded.</returns>
        public static bool HasFiles(HttpRequestBase request)
        {
            if (request.Files == null || request.Files.Count == 0)
                return false;

            // Now check if files were uploaded.
            for (int ndx = 0; ndx < request.Files.Count; ndx++)
            {
                var file = request.Files[ndx];
                if (file.ContentLength > 0)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Checks if any media files were uploaded by checking the size of each posted file.
        /// </summary>
        /// <param name="request">Http request.</param>
        /// <param name="maxSizeInK">The maximum size of each file.</param>
        /// <param name="maxNumFiles">The maximum number of files allowed.</param>
        /// <param name="errorStorage">Callback for adding errors</param>
        /// <returns>True if media files were uploaded.</returns>
        public static bool ValidateMediaFiles(HttpRequestBase request, int maxSizeInK, int maxNumFiles, Action<string, string> errorStorage)
        {
            // No files ?
            if (request.Files == null || request.Files.Count == 0)
                return true;

            // Max files exceeded?
            if (request.Files.Count > maxNumFiles)
            {
                if (errorStorage != null) 
                    errorStorage("Media files", "No more than " + maxNumFiles + " can be uploaded.");
                
                return false;
            }

            bool isValid = true;
            bool hasErrorStorage = errorStorage != null;

            // Now check if files were uploaded.
            for (int ndx = 0; ndx < request.Files.Count; ndx++)
            {
                var file = request.Files[ndx];
                var size = file.ContentLength.KiloBytes();
                var name = file.FileName;
                if (size > maxSizeInK)
                {
                    if(hasErrorStorage)
                        errorStorage("File : " + (ndx + 1), "File " + name + " with size " + size + " exceeds maximum size of " + maxSizeInK + " kilobytes");

                    isValid = false;
                }
            }
            return isValid;
        }
    }
}
