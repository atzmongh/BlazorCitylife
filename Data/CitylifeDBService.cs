using BlazorCitylife.Models;
using BlazorCitylife.Shared;
using CityLife;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorCitylife.Data
{
    public class CitylifeDBService
    {
        public Employee LoggedinUser;
        public NavMenu NavMenuComponent;
        public DateTime FromDate = FakeDateTime.DateNow.AddDays(-1);
        public bool UserIsLoggedin => LoggedinUser != null;
        public string UserName => UserIsLoggedin ? LoggedinUser.UserName : "";
       
           

        public Employee UserLoggedin(string userName, string password)
        {
            using(var db = new citylifedb8_blContext())
            {
                Employee theEmployee = db.Employee.SingleOrDefault(anEmp=>anEmp.UserName == userName);
                if (theEmployee != null && theEmployee.PasswordHash == password)
                {
                    LoggedinUser = theEmployee;
                }
                else
                {
                    theEmployee = null;
                }
                return theEmployee;  //(or null if no user is logged in)
            }
        }

        /// <summary>
        /// This is the horizontal version of the dashboard. It shows the orders of all apartments for 31 days, since the date entered by the user, 
        /// or since today. (the default)
        /// </summary>
        /// <param name="fromDate">Date entered in the date picker, or null</param>
        /// <param name="wideDashboard">When "on" means that the user wants to get the wide version of the dashboard.
        /// In this mode, each column is wider (about 4 normal columns), we show only 3 days, and we put more data into\
        /// each order element.</param>
        /// <returns></returns>
        public void s21Dashboard(DateTime startDate, string wideDashboard = "off")
        {
            this.FromDate = startDate;

            //Currntly for both normal mode and wide mode we dislay 31 days. The number of days can be set here.
            //We display one month. The calculation is: the number of days displayed are equal to the month length of the starting month. 
            //In most cases this will yield an end date which is one less that the starting date. The only exception is January 30 and 31, which will 
            //end at 1 or 2 of March.
            //For example: start date    end date
            //             10/11/2019    9/12/2019
            //             31/12/2019    30/1/2020
            //             30/1/2019     1/3/2019 (except for leap year, where it will be 29/2/2020)
            //             1/11/2019     30/11/2019
            //             1/12/2019     31/12/2019
            //             1/2/2020      29/2/2020 (2020 is leap year)
            int dashboardDays = FakeDateTime.monthLength(startDate);

            
                List<Money> revenuePerDay = null;
                List<Money> expensePerDay = null;
                List<string> expenseTypes = null;
                EmployeeWorkDay[] empWorkDaysArray = null;
                List<Employee> maidList = null;
                List<Money> revenuePerApartment = null;
                List<double> aveargeDaysPerApartment = null;
                List<int> percentOccupancyPerApartment = null;
                var apartmentDayBlocks = s21dashboardPreparation(
                    startDate,
                    dashboardDays,
                    ref revenuePerDay,
                    ref expensePerDay,
                    ref expenseTypes,
                    ref revenuePerApartment,
                    ref aveargeDaysPerApartment,
                    ref percentOccupancyPerApartment,
                    ref empWorkDaysArray,
                    ref maidList);

                if (Session["lastOrderDetails"] != null)
                {
                    ViewBag.highlightOrderId = (int)Session["lastOrderDetails"];
                }
                else
                {
                    ViewBag.highlightOrderId = 0;
                }
                return View("s21Dashboard");
           



        }

        /// <summary>
        /// The function reads the orders for all apartments starting from the date set by the user and for a full month (30 or 31 days,
        /// and for February 28 or 29 days)
        /// </summary>
        ///<param name="fromDate">starting date of the dashboard</param>
        ///<param name="days">the number of days we wish to display.(depends on the starting month)</param>
        ///<param name="revenuePerDay">An output parameter - will contain the list of revenues per day</param>
        ///<param name="expensePerDay">total expenses for each day</param>
        ///<param name="percentOccupancyPerApartment">Number of days the apartment is occupied divided by total number of days (rounded to 
        ///whole percent)</param>
        ///<param name="revenuePerApartment">Total revenue per apartment for that month</param>
        ///<param name="averageDaysPerApartment">contains the average number of days per rent for each apartment. We include in this average
        ///only rents that have started in the displayed month. IF a rent started in that month and spans to the next month
        ///the average takes into account the total time for that rent - not only the portion will falls in this month</param>
        ///<param name="empWorkDaysArray">An array containing an employeeWorkDay record for each day in the month.
        ///Days for which no record found - will be null. Days for which more than one recrod found - will contain
        ///the last record. </param>
        ///<param name="maidList">A list of maids (employees of role="maid") - ouitput parameter</param>
        /// <returns>List of apartment orders. For each apartment:
        /// list of DayBlocks.
        /// A dayBlock is either a single free day, or an order which can span 1 or more days. Note that a day block may not 
        /// be identical to the corresponding order because the order may start before the "fromDate" or end after the "fromDate+31".
        /// Note  that the list contains first all real apartments, then "waiting" apartments</returns>
        public List<List<DayBlock>> s21dashboardPreparation(DateTime fromDate,
            int days,
            ref List<Money> revenuePerDay,
            ref List<Money> expensePerDay,
            ref List<string> expenseTypes,
            ref List<Money> revenuePerApartment,
            ref List<double> averageDaysPerApartment,
            ref List<int> percentOccupancyPerApartment,
            ref EmployeeWorkDay[] empWorkDaysArray,
            ref List<Employee> maidList)
        {
            //for each apartment
            //for each day from from_date to from_date+3
            //lastOrderId = 0;
            //orderLength = 0;
            //apartmentDay = read apartmentDay record for apartment and day
            //if record does not exist or record status == free - this is a free day - add a free block
            //order = apartmentDay->order
            //if order.isFirstDay - create a new busy block
            //add the day to the busy block
            //if order.isLastDay - write busy block
            //write last busy block

            //a 2 dimensional array - a list of apartments, and for each apartment - a list of day blocks
            //where each day block is either a free day or an order.
            citylifedb8_blContext db = new citylifedb8_blContext();
            var apartmentDayBlocks = new List<List<DayBlock>>();
            revenuePerDay = new List<Money>();
            expensePerDay = new List<Money>();
            for (int i = 0; i < days; i++)
            {
                revenuePerDay.Add(new Money(0m, "UAH"));
            }


            var lastDate = fromDate.AddDays(days - 1);
            revenuePerApartment = new List<Money>();
            averageDaysPerApartment = new List<double>();
            percentOccupancyPerApartment = new List<int>();

            //Calculate the expenses for each date in the range
            for (DateTime aDate = fromDate; aDate <= lastDate; aDate = aDate.AddDays(1))
            {
                //Read all expenses for the date and sum them
                var expenseListCentsForDate = (from expense in db.Expense
                                               where expense.Date == aDate
                                               select expense.Amount);   //The expenses are kept as cents in the DB
                int expensesCentsForDate = 0;
                if (expenseListCentsForDate.Count() > 0)
                {
                    expensesCentsForDate = expenseListCentsForDate.Sum();
                }
                Money expensesForDate = new Money(expensesCentsForDate, "UAH");
                expensePerDay.Add(expensesForDate);
            }

            //Sort apartments by type (first all "normal" apartments then the "waiting" apartments), then by their number
            var sortedApartments = from anApartment in db.Apartment
                                   orderby anApartment.Type, anApartment.Number
                                   select anApartment;
            Order anOrder = new Order()   //create a fictitious order with id = 0
            {
                Id = 0
            };

            foreach (var anApartment in sortedApartments)
            {
                var dayBlocks = new List<DayBlock>();
                DayBlock aDayBlock = null;
                int dayNumber = 0;
                Money apartmentRevenue = new Money(0m, "UAH");
                double apartmentOccupiedDays = 0;   //Use float for the percentage calculation later
                double totalRents = 0.0;    //Counter for how many orders are during the month for that apartment.
                                            //We keep it as double in order to calculate the average (which should be in float)
                int totalRentDays = 0;      //the total amount of rented days for that apartment in that month. We take into account only rents that started
                                            //in the displayed month.
                                            //Get all apartment days of the current apartment for the desired month
                var apartmentDaysForMonth = (from theApartmentDay in db.ApartmentDay
                                             where theApartmentDay.Apartment.Id == anApartment.Id && theApartmentDay.Date >= fromDate && theApartmentDay.Date <= lastDate
                                             orderby theApartmentDay.Date
                                             select theApartmentDay).ToList();
                int apartmentDaysI = 0;
                ApartmentDay anApartmentDay;
                int apartmentDaysCount = apartmentDaysForMonth.Count();
                for (var aDate = fromDate; aDate <= lastDate; aDate = aDate.AddDays(1))
                {
                    if (apartmentDaysCount > apartmentDaysI && apartmentDaysForMonth[apartmentDaysI].Date == aDate)
                    {
                        //The current apartmentDays record matches the on-hand date - an apartmentDay exists
                        anApartmentDay = apartmentDaysForMonth[apartmentDaysI];
                        apartmentDaysI++;
                    }
                    else
                    {
                        //An apartment day does not exist - it will be null
                        anApartmentDay = null;
                    }

                    if (anApartmentDay == null || anApartmentDay.Status == (int)ApartOccuStatus.Free)
                    {
                        //This is a free day
                        if (aDayBlock != null)
                        {
                            //Although this should not occur (assuming that the apartmentDays table matches the orders checkin and checkout dates)
                            //But anyway - we will write the dayBlock to the list
                            dayBlocks.Add(aDayBlock);
                        }
                        aDayBlock = new DayBlock() { apartmentNumber = anApartment.Number, status = OrderStatus.Free, firstDate = aDate.ToString("yyyy-MM-dd") };//"Free" status denotes a free day
                        dayBlocks.Add(aDayBlock);
                        aDayBlock = null;
                    }
                    else
                    {
                        //this is a busy day. Read the order record
                        if (anOrder.Id != anApartmentDay.Order.Id)
                        {
                            //We did not read this order yet - read it
                            anOrder = db.Order.Single(record => record.Id == anApartmentDay.Order.Id);
                            //Check if this order started today
                            if (anOrder.CheckinDate == aDate)
                            {
                                //take this order into account for calculation of average days per rent
                                totalRents++;
                                totalRentDays += anOrder.DayCount;
                            }
                            else
                            {
                                //This is a new order but it started before the first day of the dispalyed month - do not take it into consideration
                                //for calculating average rent days.
                            }
                        }
                        else
                        {
                            //the order is for more than one day. We have already read this order in the previous cycle in the date loop
                        }
                        //At this point anOrder contains the order pointed by the on-hand apartmentDay record
                        //Add the revenue of that day to the total revenu per day
                        revenuePerDay[dayNumber] += anApartmentDay.revenueAsMoney();
                        apartmentRevenue += anApartmentDay.revenueAsMoney();
                        apartmentOccupiedDays++;
                        if (aDayBlock == null)
                        {
                            //This is the first time we see this order - open a new DayBlock object. Note that if the report starts from 
                            //1/12/2019 and we have an order that started on 39/11/2019 and continued to 4/12/2019 - then the first time we 
                            //encounted this order is not in the checkin date of it.
                            aDayBlock = new DayBlock(anOrder)
                            {
                                days = 1,
                                firstDate = aDate.ToString("yyyy-MM-dd"),
                            };
                        }
                        else
                        {
                            //This is a continuation of the order - increment the number of days
                            aDayBlock.days++;
                        }
                        if (anOrder.isLastDay(aDate))
                        {
                            //This is the last day of the order - write the day block
                            dayBlocks.Add(aDayBlock);
                            aDayBlock = null;
                        }
                    }
                    dayNumber++;
                }
                //At this point we finished going on all dates for a specific apartment. It is possible that the last DayBlock was not yet written
                //if its checkout date is beyond the last day of the report (our report is from 1/12/2019 till 31/12/2019, but the checkout date
                //of the last order is 2/1/2020)
                if (aDayBlock != null)
                {
                    dayBlocks.Add(aDayBlock);
                    aDayBlock = null;
                }
                //Add the dayBlocks list into the apartmentDayBlocks. Check if it is a "waiting" apartment.
                apartmentDayBlocks.Add(dayBlocks);

                //Add the apartment revenue and apartment occupacy percentage - only for "normal" apartments
                revenuePerApartment.Add(apartmentRevenue);
                double apartmentOccupancyPercent = apartmentOccupiedDays / days * 100.0;
                int apartmentOccupancyPercentRounded = (int)Math.Round(apartmentOccupancyPercent);
                percentOccupancyPerApartment.Add(apartmentOccupancyPercentRounded);

                //calculate the average rent days per apartment
                double averageRentDays = 0.0;
                if (totalRents > 0)
                {
                    averageRentDays = totalRentDays / totalRents;
                }
                averageDaysPerApartment.Add(averageRentDays);
            }
            //At this point the apartmentDayBlocks variable contaiins a list of list of day blocks 

            //Calculate the list of employee work days. The list contains a single record for each day (or null, if no employee is assigned
            //for that day). If 2 employees are assigned for the same day - only one is taken (the last one)
            //empWorkDaysList = new List<EmployeeWorkDay>();
            empWorkDaysArray = new EmployeeWorkDay[days];
            var empWorkDays = from anEmpWorkDay in db.EmployeeWorkDay
                              where anEmpWorkDay.DateAndTime >= fromDate && anEmpWorkDay.DateAndTime <= lastDate
                              orderby anEmpWorkDay.DateAndTime
                              select anEmpWorkDay;
            foreach (var anEmpWorkDays in empWorkDays)
            {
                int dayNumber = (int)Math.Round((anEmpWorkDays.DateAndTime.Date - fromDate).TotalDays, 0);
                empWorkDaysArray[dayNumber] = anEmpWorkDays;
            }

            maidList = db.Employee.Where(emp => emp.Role == "maid").ToList();  //Add all maids to the maid list

            expenseTypes = (from expenseType in db.ExpenseType
                            select expenseType.NameKey).ToList();



            return apartmentDayBlocks;

        }
    }
}
