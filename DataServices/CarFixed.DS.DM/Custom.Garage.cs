using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFixed.DS.DM
{
    public partial class Garage : IEntity
    {
        public enum StatusEnum
        {
            Live = 1,
            Archived = 2,
            AwaitingApproval = 3
        }

        public EntityState EntityState { get; set; }
    }

    public partial class GarageService:IEntity
    {
        public EntityState EntityState { get; set; }
    }
}
