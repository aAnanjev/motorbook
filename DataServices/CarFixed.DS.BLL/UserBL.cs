using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DAL;
using CarFixed.DS.DM;

namespace CarFixed.DS.BLL
{
    public interface IUserBL
    {
        CarFixedUser GetCarFixedUser(string userId);
        CarFixedUser GetCarFixedUserForProfile(string userId);
        CarFixedUser GetCarFixedUser(int carFixedUserId);


        void UpdateCarFixedUser(params CarFixedUser[] cfUsers);

    }

    public class UserBL : IUserBL
    {
        #region Variables

        private ICarFixedUserDR _CarFixedUserDR = null;

        #endregion Variables

        #region Construction

        public UserBL()
        {
            _CarFixedUserDR = new CarFixedUserDR();
        }

        #endregion Construction

        #region Public Methods

        public CarFixedUser GetCarFixedUser(string userId)
        {
            CarFixedUser user = _CarFixedUserDR.GetSingle(u => u.UserID == userId, u => u.Vehicles, u => u.Address);
            
            return user;
        }

        public CarFixedUser GetCarFixedUserForProfile(string userId)
        {
            CarFixedUser user = _CarFixedUserDR.GetSingle(u => u.UserID == userId, u => u.Vehicles, u => u.Address);

            if (user != null)
                user.Vehicles = user.Vehicles.ToList().Where(v => v.IsDisplayed == true).ToList();

            return user;
        }

        public CarFixedUser GetCarFixedUser(int carFixedUserId)
        {
            return _CarFixedUserDR.GetSingle(u => u.CarFixedUserID == carFixedUserId, u => u.Vehicles, u => u.Address);
        }

        public void UpdateCarFixedUser(params CarFixedUser[] cfUsers)
        {
            _CarFixedUserDR.Update(cfUsers);
        }

        #endregion Public Methods
    }
}
