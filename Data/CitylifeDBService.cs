using BlazorCitylife.Models;
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
        public Employee UserLoggedin(string userName, string password)
        {
            using(var db = new citylifedb8_blContext())
            {
                Employee theEmployee = db.Employee.SingleOrDefault(anEmp=>anEmp.UserName == userName);
                if (theEmployee == null)
                {
                    return null;
                }
                else
                {
                    if (theEmployee.PasswordHash != password)
                    {
                        return null;
                    }
                }
                //If we got here - we have an authenticated user
                LoggedinUser = theEmployee;
                return theEmployee;
            }
        }
    }
}
