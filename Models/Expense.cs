using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Expense
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public string CurrencyCurrencyCode { get; set; }
        public string Description { get; set; }
        public int ExpenseTypeId { get; set; }

        public virtual Currency CurrencyCurrencyCodeNavigation { get; set; }
        public virtual ExpenseType ExpenseType { get; set; }
    }
}
