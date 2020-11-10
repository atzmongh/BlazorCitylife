using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Guest
    {
        public Guest()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }

        public virtual Country CountryCodeNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
