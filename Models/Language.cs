using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Language
    {
        public Language()
        {
            Translations = new HashSet<Translation>();
        }

        public string LanguageCode { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }
    }
}
