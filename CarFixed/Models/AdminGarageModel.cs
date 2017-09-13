using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DM;

namespace CarFixed.Models
{
    public class AdminGarageModelView
    {
        public List<Garage> Garages { get; set; }
    }

    public class AdminGarageModelEdit
    {
        public List<Status> GarageStatuses { get; set; }
        public Garage Garage { get; set; }
    }
}
