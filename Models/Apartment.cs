using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Apartment
    {
        public Apartment()
        {
            ApartmentDays = new HashSet<ApartmentDay>();
            ApartmentPhotoes = new HashSet<ApartmentPhoto>();
            Orders = new HashSet<Order>();
            Pricings = new HashSet<Pricing>();
        }

        public int Id { get; set; }
        public short Number { get; set; }
        public string NameKey { get; set; }
        public string DescriptionKey { get; set; }
        public string AddressKey { get; set; }
        public short Size { get; set; }
        public string FeaturesKeys { get; set; }
        public int Type { get; set; }

        public virtual ICollection<ApartmentDay> ApartmentDays { get; set; }
        public virtual ICollection<ApartmentPhoto> ApartmentPhotoes { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Pricing> Pricings { get; set; }
    }
}
