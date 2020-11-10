using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class ApartmentDay
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public bool IsCleaned { get; set; }
        public int Revenue { get; set; }
        public DateTime Date { get; set; }
        public decimal PriceFactor { get; set; }
        public string CurrencyCurrencyCode { get; set; }
        public int ApartmentId { get; set; }
        public int? OrderId { get; set; }

        public virtual Apartment Apartment { get; set; }
        public virtual Currency CurrencyCurrencyCodeNavigation { get; set; }
        public virtual Order Order { get; set; }
    }
}
