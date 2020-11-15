using CityLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCitylife.Models
{
    public partial class ApartmentDay
    {
        public Money revenueAsMoney()
        {
            decimal priceDecimal = ((decimal)this.Revenue) / 100;     //The price is kept in cents in the DB.
            Money priceMoney = new Money(priceDecimal, this.CurrencyCurrencyCode);
            return priceMoney;
        }
    }
}
