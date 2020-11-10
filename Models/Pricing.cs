using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Pricing
    {
        public int Id { get; set; }
        public short Adults { get; set; }
        public short Children { get; set; }
        public int PriceWeekDay { get; set; }
        public int PriceWeekEnd { get; set; }
        public int ApartmentId { get; set; }
        public string CurrencyCurrencyCode { get; set; }

        public virtual Apartment Apartment { get; set; }
        public virtual Currency CurrencyCurrencyCodeNavigation { get; set; }
    }
}
