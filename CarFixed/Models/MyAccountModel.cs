using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CarFixed.DS.DM;

namespace CarFixed.Models
{
    public class MyAccountModel
    {
        public CarFixedUser User { get; set; }

        

        public Details details { get; set; }
        public Settings settings { get; set; }
        public Security security { get; set; }
    }

    public class Details
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string DateAdded { get; set; }
    }

    public class Settings
    {
        public bool Newsletters { get; set; }
    }

    public class Security
    {
        public string Password { get; set; }
    }
}