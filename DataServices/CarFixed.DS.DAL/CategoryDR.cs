using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DM;

namespace CarFixed.DS.DAL
{
    public interface IBasicCategoryDR : IGenericDataRepository<BasicCategory> { }
    public interface IBasicSubCategoryDR : IGenericDataRepository<BasicSubCategory> { }
    public interface IBasicSubCategoryRepairRefDR : IGenericDataRepository<BasicSubCategoryRepairRef> { }


    public class BasicCategoryDR : CarFixedBaseDR<BasicCategory>, IBasicCategoryDR { }
    public class BasicSubCategoryDR : CarFixedBaseDR<BasicSubCategory>, IBasicSubCategoryDR { }
    public class BasicSubCategoryRepairRefDR : CarFixedBaseDR<BasicSubCategoryRepairRef>, IBasicSubCategoryRepairRefDR { }
}
