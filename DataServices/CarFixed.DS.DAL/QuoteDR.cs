using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

using CarFixed.DS.DM;

namespace CarFixed.DS.DAL
{
    public interface IQuoteDR : IGenericDataRepository<Quote>
    {
        IList<Quote> GetQuoteOptions(int garageId);
    }


    public class QuoteDR : CarFixedBaseDR<Quote>, IQuoteDR
    {
        public IList<Quote> GetQuoteOptions(int garageId)
        {
            IList<Quote> quotes = null;

            using (var context = new CarFixedEntities())
            {
                
                context.Configuration.ProxyCreationEnabled = false;

                var result = (from qgo in context.QuoteGarageOptions
                              join q in context.Quotes on qgo.QuoteID equals q.QuoteID
                              where q.StatusID == (int)Quote.StatusEnum.RequestedByUser
                              select q).Distinct();

                result.Include("Vehicle");
                result.Include("QuoteItemBasics");
                result.Include("QuoteItemBasics.BasicSubCategory");

                quotes = result.ToList();
            }

            return quotes;
        }
    }
}
