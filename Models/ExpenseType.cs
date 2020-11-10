using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class ExpenseType
    {
        public ExpenseType()
        {
            Expenses = new HashSet<Expense>();
        }

        public int Id { get; set; }
        public string NameKey { get; set; }
        public string DescriptionKey { get; set; }

        public virtual ICollection<Expense> Expenses { get; set; }
    }
}
