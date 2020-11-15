using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCitylife.Models
{
    public partial class ApartmentPhoto
    {
        /// <summary>
        /// If a thumbnail exists - returns its file path. Otherwise - returns the file path of the main photo. (that may also be used as a thumbnail)
        /// </summary>
        public string thumbnailOrFilePath
        {
            get
            {
                if (this.ThumbnailPath != "")
                {
                    return this.ThumbnailPath;
                }
                else
                {
                    return this.FilePath;
                }
            }
        }
    }
}
