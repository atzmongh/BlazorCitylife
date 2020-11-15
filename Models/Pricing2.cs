using CityLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCitylife.Models
{
    public partial class Pricing
    {
        public Money priceWeekendAsMoney()
        {
            return new Money(this.PriceWeekEnd, this.CurrencyCurrencyCode);
        }

        public Money priceWeekdayAsMoney()
        {
            return new Money(this.PriceWeekDay, this.CurrencyCurrencyCode);
        }
    }
}
