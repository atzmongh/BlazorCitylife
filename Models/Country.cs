using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Country
    {
        public Country()
        {
            Guests = new HashSet<Guest>();
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }

        public virtual ICollection<Guest> Guests { get; set; }
    }
}
