using CityLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCitylife.Models
{
    public partial class Order
    {
        public bool isFirstDay(DateTime aDate)
        {
            return this.checkinDate == aDate;
        }
        /// <summary>
        /// Last day is the day before the checkout date. For example: if the guest checked in on 1/12/2019 and checkout on 4/12/2019
        /// then: 
        ///             isFirstDay  isLastDate
        /// 1/12/2019 - true        false
        /// 2/12/2019   false       false
        /// 3/12/2019   false       true
        /// 4/12/2019   fakse       false
        /// 
        /// If the guest checked in on 1/12/2019 and checkout on 2/12/2019, then:
        /// 1/12/2019 - true        true
        /// 2/12/2019   false       false
        /// </summary>
        /// <param name="aDate"></param>
        /// <returns></returns>
        public bool isLastDay(DateTime aDate)
        {
            return this.CheckoutDate == aDate.AddDays(1);
        }

        public Money priceAsMoney()
        {
            return new Money(this.Price, this.CurrencyCurrencyCode);
        }

        public Money amountPaidAsMoney()
        {
            return new Money(this.AmountPaid, this.CurrencyCurrencyCode);
        }

        public int nights
        {
            get
            {
                return Order.dateDifference(this.CheckoutDate, this.CheckinDate);
            }

        }
        public static int dateDifference(DateTime checkoutDate, DateTime checkinDate)
        {
            var nightsSpan = (checkoutDate - checkinDate);
            var nightCount = (int)Math.Round(nightsSpan.TotalDays, 0);
            return nightCount;
        }
    }
}
