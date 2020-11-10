using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Translation
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int TranslationKeyId { get; set; }
        public string LanguageLanguageCode { get; set; }

        public virtual Language LanguageLanguageCodeNavigation { get; set; }
        public virtual TranslationKey TranslationKey { get; set; }
    }
}
