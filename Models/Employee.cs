using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeWorkDays = new HashSet<EmployeeWorkDay>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<EmployeeWorkDay> EmployeeWorkDays { get; set; }
    }
}
