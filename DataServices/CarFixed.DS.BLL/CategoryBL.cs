using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DAL;
using CarFixed.DS.DM;

namespace CarFixed.DS.BLL
{
    public interface ICategoryBL
    {
        IList<BasicCategory> GetAllBasicCategories();

        IList<BasicSubCategory> GetBasicSubCategories(int basicCategoryId);
        IList<BasicSubCategoryRepairRef> GetBasicSubCategoryRepairRef(int basicCategoryId);

    }

    public class CategoryBL : ICategoryBL
    {
        #region Variables

        private IBasicCategoryDR _BasicCategoryDR = null;
        private IBasicSubCategoryDR _BasicSubCategoryDR = null;
        private IBasicSubCategoryRepairRefDR _BasicSubCategoryRepairRefDR = null;

        #endregion Variables

        #region Construction

        public CategoryBL()
        {
            _BasicCategoryDR = new BasicCategoryDR();
            _BasicSubCategoryDR = new BasicSubCategoryDR();
            _BasicSubCategoryRepairRefDR = new BasicSubCategoryRepairRefDR();
        }

        #endregion Construction

        #region Public Methods

        public IList<BasicCategory> GetAllBasicCategories()
        {
            return _BasicCategoryDR.GetAll();
        }

        public IList<BasicSubCategory> GetBasicSubCategories(int basicCategoryId)
        {
            return _BasicSubCategoryDR.GetList(
                c => c.BasicCategoryID == basicCategoryId, 
                c => c.BasicSubCategoryGroup)
                .OrderBy(c => c.BasicSubCategoryGroup.Order).ThenBy(c => c.Title).ToList();
        }

        public IList<BasicSubCategoryRepairRef> GetBasicSubCategoryRepairRef(int basicCategoryId)
        {
            return _BasicSubCategoryRepairRefDR.GetList(r => r.BasicSubCategoryID == basicCategoryId);
        }

        #endregion Public Methods
    }
}
