using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DM;

namespace CarFixed.Models
{
    public class GarageIndexModel
    {
        public List<QuoteGarageOption> QuoteOptions { get; set; }
        public List<QuoteGarageOption> SubmittedQuoteOptions { get; set; }
    }
}
