using BlazorCitylife.Models;
using CityLife;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorCitylife.Data
{
    public enum OrderStatus : int
    {
        Created = 0,
        CheckedIn = 1,
        CheckedOut = 2,
        Cancelled = 3,
        Free = 9,
        Waiting_list = 4,
        Waiting_deletion = 5
    }
    public enum Color : int
    {
        Red = 0,
        Orange = 1,
        Green = 2,
        Blue = 3,
        Gray = 4
    }

    /// <summary>
    /// This is a base class for classes that define the fields of a form. They should contain all properties of the form, plus an "isValid"
    /// method that checks the form for validity. Each invalid property will get an error message in the errorMessage dictionary.
    /// where the key is the property name. For example, if the address field is empty, we will add a <key,value> pair to the dictioanry:
    /// key="address", value="address cannot be empty"
    /// </summary>
    public abstract class formData
    {
        protected Dictionary<string, string> errorMessage = new Dictionary<string, string>();
        /// <summary>
        /// Returns true if the form is valid. If not valid - adds error messages to the dictionary.
        /// </summary>
        /// <returns></returns>
        public abstract bool isValid();
        /// <summary>
        /// Returns the translated error messages for a specific property, or empty string if the field was valid
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="tBox"></param>
        /// <returns></returns>
        public string errorMessageOf(string fieldName, TranslateBox tBox)
        {
            try
            {
                return tBox.translate(this.errorMessage[fieldName]);
            }
            catch (Exception)
            {
                return "";
            }
        }
        public void setErrorMessageFor(string fieldName, string message)
        {
            errorMessage.Add(fieldName, message);
        }
        /// <summary>
        /// The method returns the word "error" if the field has an error. This will create the CSS class "error"
        /// It returns empty string otherwise
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public string errorOf(string fieldName)
        {
            return this.errorMessage.ContainsKey(fieldName) ? "error" : "";
        }

    }
    /// <summary>
    /// Note that the class inherits methods errorMessageOf and errorOf
    /// </summary>
    public class OrderData : formData
    {
        public int orderId;
        public string name;
        public string phone;
        public string email;
        public string country;
        public int adults;
        public int children;
        public DateTime checkin;
        public DateTime checkout;
        public int nights;
        public string price;
        public string paid;
        public string unpaid;
        public string expectedArrival;
        public string comments;
        public string bookedBy;
        public int apartmentNumber;
        public string confirmationNumber;
        public OrderStatus status;
        public Color orderColor;
        public string staffComments;

        public OrderData()
        {
            status = OrderStatus.Created;
            paid = "0";
            confirmationNumber = "0";
            orderColor = Color.Red;
        }
        public OrderData(Order anOrder)
        {
            orderId = anOrder.Id;
            name = anOrder.Guest.Name;
            phone = anOrder.Guest.Phone;
            email = anOrder.Guest.Email;
            Country guestCountry = anOrder.Guest.CountryCodeNavigation;
            if (guestCountry == null)
            {
                country = null;
            }
            else
            {
                country = guestCountry.Name;
            }
            adults = anOrder.AdultCount;
            children = anOrder.ChildrenCount;
            checkin = anOrder.CheckinDate;
            checkout = anOrder.CheckoutDate;
            nights = anOrder.nights;
            Money priceM = anOrder.priceAsMoney();
            Money paidM = anOrder.amountPaidAsMoney();
            Money unpaidM = priceM - paidM;

            price = priceM.toMoneyString();
            paid = paidM.toMoneyString();
            unpaid = unpaidM.toMoneyString();
            expectedArrival = anOrder.ExpectedArrival;
            comments = anOrder.SpecialRequest;
            bookedBy = anOrder.BookedBy;
            apartmentNumber = anOrder.Apartment.Number;
            confirmationNumber = anOrder.ConfirmationNumber;
            status = (OrderStatus)anOrder.Status;
            orderColor = (Color)anOrder.OrderColor;
            staffComments = anOrder.StaffComments;
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The function checks the booking data for validity. There is a difference if the booking was made by a guest or by admin.
        /// If by guest - there will be more checks then if done by admin.
        /// </summary>
        /// <returns></returns>
        public override bool isValid()
        {
            this.errorMessage.Clear();
            if (this.name == null || this.name == "")
            {
                this.errorMessage.Add("name", "Please enter name");
            }

            if (this.country == "" && this.bookedBy == "Guest")
            {
                //A guest must specify country
                this.errorMessage.Add("country", "Please select country");
            }
            else if (this.country == "")
            {
                //Booking was done by admin and not by a guest - it is OK not to have country
            }
            else
            {
                //Cuntry is not empty - check that it exists in our DB
                citylifedb8_blContext db = new citylifedb8_blContext();
                Country theCountry = db.Country.SingleOrDefault(a => a.name == this.country);
                if (theCountry == null)
                {
                    //Although we do not expect such a case, as our country list contains all countries, and the user can only choose from the list
                    this.errorMessage.Add("country", "This country does not exist in our country list");
                }
            }
            if (this.bookedBy == "Guest" && !OrderData.IsValidEmail(this.email))    //Email is mandatory for a guest, but not for admin
            {
                this.errorMessage.Add("email", "Please enter a valid email address");
            }
            //When a guest enters a phone number - it should be between 10 to 13 digits. no special characters allowed in the number.
            //THIS IS AN INTERIM SOLUTION - WE SHOULD LOOK FOR A SUITABLE LIBRARY TO VALIDATE PHONE NUMBER
            if (this.bookedBy == "Guest" && !Regex.Match(this.phone, @"^(\+?[0-9]{10,13})$").Success)
            {
                this.errorMessage.Add("phone", "Please enter a valid phone number");
            }
            if (adults < 1)
            {
                this.errorMessage.Add("adults", "Number of adults must be at least 1");
            }
            if (children < 0)
            {
                this.errorMessage.Add("children", "number of children cannot be negative");
            }
            if (checkout <= checkin)
            {
                this.errorMessage.Add("checkin", "Checkout date cannot be before or equal to checkin date");
            }
            try
            {
                Money priceM = new Money(this.price);
            }
            catch (AppException)
            {
                //The price amount could not be converted to Money - it is not legal as money (either the currency does not exist or non numeric
                this.errorMessage.Add("price", "Invalid price");
            }
            try
            {
                Money paidM = new Money(this.paid);
            }
            catch (AppException)
            {
                //The paid amount could not be converted to Money - it is not legal as money (either the currency does not exist or non numeric
                this.errorMessage.Add("paid", "Invalid amount");
            }

            return errorMessage.Count() == 0;
        }
    }

    /// <summary>
    /// A day block is either a single free day, or a group of occupied days for a single apartment and a single guest.
    /// it adds 2 properties:
    /// days - that may be different than the "nights" property of OrderData, because if the report starts after the beginning or ends before the
    /// end of the order - the "days" property will reflect the number of days the order will be shown in the report.
    /// firstDate - the first date in the report. May not be the same as checkIn property
    ///  
    /// </summary>
    public class DayBlock : OrderData
    {
        public DayBlock()
        { }
        public DayBlock(Order anOrder) : base(anOrder)
        { }
        public int days;
        public string firstDate;
    }

    public class RevenueAndOccupancy
    {
        public Money revenue { get; private set; } = new Money(0m, "UAH");
        public int occupiedDays { get; private set; } = 0;

        private int totalDaysInMonth = 30;
        /// <summary>
        /// number of days in the month represented by this 
        /// </summary>
        /// <param name="totalDays"></param>
        public RevenueAndOccupancy(int totalDays)
        {
            if (totalDays > 0)
            {
                totalDaysInMonth = totalDays;
            }
        }
        public double percentOccupancy()
        {
            return (double)occupiedDays / totalDaysInMonth;
        }
        /// <summary>
        /// Add revenue for a single day. We assume that each day where we had a revenue - is also an occupied
        /// day, so we increase the occupied days count.
        /// </summary>
        /// <param name="amount"></param>
        public void addDaysRevenue(Money amount)
        {
            revenue += amount;
            occupiedDays++;
        }
    }
}
