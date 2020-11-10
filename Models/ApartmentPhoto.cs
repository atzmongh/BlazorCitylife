using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class ApartmentPhoto
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public short Type { get; set; }
        public short Orientation { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public short SortOrder { get; set; }
        public string ThumbnailPath { get; set; }
        public bool ForDesktop { get; set; }
        public bool ForMobile { get; set; }
        public int ApartmentId { get; set; }

        public virtual Apartment Apartment { get; set; }
    }
}
