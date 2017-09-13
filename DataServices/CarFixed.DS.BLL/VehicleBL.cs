using System.Collections.Generic;
using System.Linq;

using CarFixed.DS.DAL;
using CarFixed.DS.DM;
using CarFixed.DS.API.AutuGuru;

namespace CarFixed.DS.BLL
{
    public interface IVehicleBL
    {
        List<Vehicle> GetAllVehicles();
        Vehicle GetVehicleByVrm(string vrm);
        VrmResponse GetVrmResponseByVrm(string vrm);
        List<Vehicle> GetVehiclessByManufacturer(string manufacturer);
        bool RemoveVehicle(int id);
        Vehicle GetVehicleById(int id);
    }

    public class VehicleBL
    {
        #region Properties

        private IVehicleDR _VehicleDR = null;

        private AgApiVrmLookup _AgApiVrmLookup = null;

        #endregion Properties

        #region Construction

        public VehicleBL()
        {
            _VehicleDR = new VehicleDR();
            _AgApiVrmLookup = new AgApiVrmLookup();
        }

        #endregion Construction

        #region Public Methods

        public List<Vehicle> GetAllVehicles()
        {
           return _VehicleDR.GetAll().ToList();
        }

        public Vehicle GetVehicleByVrm(string vrm)
        {
            return _VehicleDR.GetSingle(v => v.VRM == vrm);
        }

        public Vehicle GetVehicleByVrm(string vrm, int carFixedUserID)
        {
            return _VehicleDR.GetSingle(v => v.VRM == vrm && v.CarFixedUserID == carFixedUserID);
        }

        public VrmResponse GetVrmResponseByVrm(string vrm)
        {
            return _AgApiVrmLookup.VrmLookup(vrm);
        }

        public VrmResponse GetVrmLightResponseByVrm(string vrm)
        {
            return _AgApiVrmLookup.VrmLookupLight(vrm);
        }

        public List<Vehicle> GetVehiclessByManufacturer(string manufacturer)
        {
            return _VehicleDR.GetList(v => v.Manufacturer == manufacturer).ToList();
        }

        public Vehicle GetVehicleById(int id)
        {
            return _VehicleDR.GetSingle(v => v.VehicleID == id);
        }

        public bool RemoveVehicle(int id)
        {
            Vehicle vehicle = GetVehicleById(id);
            vehicle.EntityState = EntityState.Modified;
            vehicle.IsDisplayed = false;

            try {
                _VehicleDR.Update(vehicle);
                return true;
            }
            catch { return false; }
        }

        #endregion Public Methods
    }
}
