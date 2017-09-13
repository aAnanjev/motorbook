using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoGuru.Data;
using AutoGuru.Data.Vehicle.Services;

namespace CarFixed.DS.BLL
{
    public interface IServiceBL
    {
        ServiceRegimeWrapper GetServices(string vrm, int currentMileage, int annualMileage);

    }

    public class ServiceBL : IServiceBL
    {
        public ServiceRegimeWrapper GetServices(string vrm, int currentMileage, int annualMileage)
        {
            return ServiceRegime.GetAutoDataRegimes(vrm, 1, currentMileage, annualMileage, false);
        }
    }
}
