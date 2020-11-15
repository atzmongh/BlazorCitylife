using CityLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCitylife.Models
{
    public partial class Expense
    {
        public Money expenseAsMoney()
        {
            decimal expenseDecimal = ((decimal)this.Amount) / 100;     //The price is kept in cents in the DB.
            Money expenseMoney = new Money(expenseDecimal, this.CurrencyCurrencyCode);
            return expenseMoney;
        }
    }
}
