using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DAL;
using CarFixed.DS.DM;

namespace CarFixed.DS.BLL
{
    public interface IStatusBL
    {
        IList<Status> GetGarageStatuses();

    }

    public class StatusBL : IStatusBL
    {
        #region Variables

        private IStatusDR _StatusDR = null;

        #endregion Variables

        #region Construction

        public StatusBL()
        {
            _StatusDR = new StatusDR();
        }

        #endregion Construction

        #region Public Methods

        public IList<Status> GetGarageStatuses()
        {
            return _StatusDR.GetList(s => s.IsGarageStatus == true).OrderBy(s => s.Order).ToList();
        }

        #endregion Public Methods
    }
}
