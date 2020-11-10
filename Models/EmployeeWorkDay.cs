using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class EmployeeWorkDay
    {
        public int Id { get; set; }
        public DateTime DateAndTime { get; set; }
        public int Hours { get; set; }
        public int SalaryCents { get; set; }
        public bool IsSalaryDay { get; set; }
        public int EmployeeId { get; set; }
        public string CurrencyCurrencyCode { get; set; }

        public virtual Currency CurrencyCurrencyCodeNavigation { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
