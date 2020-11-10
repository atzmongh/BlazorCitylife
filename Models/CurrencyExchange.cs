using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class CurrencyExchange
    {
        public int Id { get; set; }
        public decimal ConversionRate { get; set; }
        public DateTime Date { get; set; }
        public string FromCurrencyCurrencyCode { get; set; }
        public string ToCurrencyCurrencyCode { get; set; }

        public virtual Currency FromCurrencyCurrencyCodeNavigation { get; set; }
        public virtual Currency ToCurrencyCurrencyCodeNavigation { get; set; }
    }
}
