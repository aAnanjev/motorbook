using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DM;

namespace CarFixed.DS.DAL
{
    public interface IGarageDR : IGenericDataRepository<Garage>
    {
        IList<GarageSelectByQuoteDistance_Result> GetGaragesForQuote(int quoteId, double radius);

    }

    public interface IQuoteGarageOptionDR : IGenericDataRepository<QuoteGarageOption> { }


    public class GarageDR : CarFixedBaseDR<Garage>, IGarageDR
    {
        public IList<GarageSelectByQuoteDistance_Result> GetGaragesForQuote(int quoteId, double radius)
        {
            using (var context = new CarFixedEntities())
            {
                return context.GarageSelectByQuoteDistance(quoteId, radius).ToList();
            }
        }
    }

    public class QuoteGarageOptionDR : CarFixedBaseDR<QuoteGarageOption>, IQuoteGarageOptionDR { }

}
