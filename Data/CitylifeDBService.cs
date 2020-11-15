using BlazorCitylife.Models;
using BlazorCitylife.Shared;
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
        public  NavMenu NavMenuComponent;
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
    }
}
