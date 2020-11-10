using System;
using System.Collections.Generic;

namespace BlazorCitylife.Models
{
    public partial class Order
    {
        public Order()
        {
            ApartmentDays = new HashSet<ApartmentDay>();
        }

        public int Id { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public int DayCount { get; set; }
        public int AdultCount { get; set; }
        public int ChildrenCount { get; set; }
        public string ExpectedArrival { get; set; }
        public string SpecialRequest { get; set; }
        public int Status { get; set; }
        public string ConfirmationNumber { get; set; }
        public string CancellationNumber { get; set; }
        public int Price { get; set; }
        public int AmountPaid { get; set; }
        public string BookedBy { get; set; }
        public int OrderColor { get; set; }
        public string StaffComments { get; set; }
        public string CurrencyCurrencyCode { get; set; }
        public int ApartmentId { get; set; }
        public int GuestId { get; set; }

        public virtual Apartment Apartment { get; set; }
        public virtual Currency CurrencyCurrencyCodeNavigation { get; set; }
        public virtual Guest Guest { get; set; }
        public virtual ICollection<ApartmentDay> ApartmentDays { get; set; }
    }
}
