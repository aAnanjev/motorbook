using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DM;

namespace CarFixed.DS.DAL
{
    public interface IStatusDR : IGenericDataRepository<Status> { }

    public class StatusDR : CarFixedBaseDR<Status>, IStatusDR { }
}
