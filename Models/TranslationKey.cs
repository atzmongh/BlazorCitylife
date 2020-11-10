using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class TranslationKey
    {
        public TranslationKey()
        {
            Translations = new HashSet<Translation>();
        }

        public int Id { get; set; }
        public string TransKey { get; set; }
        public bool IsUsed { get; set; }
        public string FilePath { get; set; }
        public int? LineNumber { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }
    }
}
