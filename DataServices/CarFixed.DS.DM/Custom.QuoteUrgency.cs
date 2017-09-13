using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFixed.DS.DM
{
    public partial class QuoteUrgency : IEntity
    {
        public EntityState EntityState { get; set; }

        public enum QuoteUrgencyEnum
        {
            Low = 25,
            Medium = 50,
            High = 75
        }
    }
}
