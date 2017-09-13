using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFixed.DS.DM
{
    public class VrmResponse
    {
        public int GCodeId { get; set; }
        public string Vrm { get; set; }
        public string Vin { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string FuelType { get; set; }
        public string EngineSize { get; set; }
        public string BHP { get; set; }

        public string Colour { get; set; }
        public MotResponse LastMot { get; set; }
        
        public List<String> MidCodes { get; set; }

        public DateTime TaxRenewalDate { get; set; }

        public VrmResponse()
        {
            this.MidCodes = new List<string>();
        }
    }

    public class MotResponse
    {
        public DateTime Date { get; set; }
        public DateTime RenewalDate { get; set; }
        public string Result { get; set; }
        public int OdometerReading { get; set; }
        public string OdomoterUnits { get; set; }
        public List<string> Advisories { get; set; }
        public List<string> Fails { get; set; }

        public MotResponse()
        {
            this.Advisories = new List<string>();
            this.Fails = new List<string>();
        }

    }
}
