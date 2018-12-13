using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleRestService.Models
{
    public class Person
    {
        #region Private fields
        private string firstName;
        private int id;
        private double payrate;
        private string lastname;
        private DateTime enddate;
        private DateTime startdate;
        #endregion

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        } 

        public string LastName
        {
            get { return lastname; }
            set { lastname = value; }
        }

        public double PayRate
        {
            get { return payrate; }
            set { payrate = value; }
        }
        public DateTime StartDate
        {
            get { return startdate; }
            set { startdate = value; }
        }

        public DateTime EndDate
        {
            get { return enddate; }
            set { enddate = value; }
        }
    }
}