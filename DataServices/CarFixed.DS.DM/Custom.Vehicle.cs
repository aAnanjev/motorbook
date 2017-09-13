using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFixed.DS.DM
{
    public partial class Vehicle :IEntity
    {
        public EntityState EntityState { get; set; }

        public VrmResponse VrmResponse { get; set; }
    }
}
