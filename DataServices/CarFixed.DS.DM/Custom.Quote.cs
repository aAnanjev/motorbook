using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFixed.DS.DM
{
    public partial class Quote : IEntity
    {
        public enum StatusEnum
        {
            RequestedByUser = 4,
            GaragesAssigned = 5,
            GarageChosen = 6,
            WorkComplete = 7,
            Cancelled = 8
        }

        public EntityState EntityState { get; set; }

        public List<GarageSelectByQuoteDistance_Result> TempGarageOptions { get; set; }
    }
}
