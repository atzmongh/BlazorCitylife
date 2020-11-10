using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Currency
    {
        public Currency()
        {
            ApartmentDays = new HashSet<ApartmentDay>();
            CurrencyExchangeFromCurrencyCurrencyCodeNavigation = new HashSet<CurrencyExchange>();
            CurrencyExchangeToCurrencyCurrencyCodeNavigation = new HashSet<CurrencyExchange>();
            EmployeeWorkDays = new HashSet<EmployeeWorkDay>();
            Expenses = new HashSet<Expense>();
            Orders = new HashSet<Order>();
            Pricings = new HashSet<Pricing>();
        }

        public string CurrencyCode { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApartmentDay> ApartmentDays { get; set; }
        public virtual ICollection<CurrencyExchange> CurrencyExchangeFromCurrencyCurrencyCodeNavigation { get; set; }
        public virtual ICollection<CurrencyExchange> CurrencyExchangeToCurrencyCurrencyCodeNavigation { get; set; }
        public virtual ICollection<EmployeeWorkDay> EmployeeWorkDays { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Pricing> Pricings { get; set; }
    }
}
