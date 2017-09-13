using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFixed.DS.DM
{
    public partial class QuoteGarageOption : IEntity
    {
        public enum StatusEnum
        {
            Sent = 9,
            Dismissed = 10
        }

        public EntityState EntityState { get; set; }

        public bool IsSelected { get; set; }
    }
}
