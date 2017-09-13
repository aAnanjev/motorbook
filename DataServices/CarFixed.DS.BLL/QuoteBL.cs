using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DAL;
using CarFixed.DS.DM;

namespace CarFixed.DS.BLL
{
    public interface IQuoteBL
    {
        IList<QuoteGarageOption> GetSubmittedQuoteOptions(int quoteId);

        IList<Quote> GetQuotes(string userId);
        IList<QuoteGarageOption> GetQuoteOptionsForGarage(int garageId, bool getSubmittedQuotes = false);

    }

    public class QuoteBL : IQuoteBL
    {
        #region Variables

        private IQuoteDR _QuoteDR = null;
        private IQuoteGarageOptionDR _QuoteGarageOptionDR = null;

        #endregion Variables

        #region Construction

        public QuoteBL()
        {
            _QuoteDR = new QuoteDR();
            _QuoteGarageOptionDR = new QuoteGarageOptionDR();
        }

        #endregion Construction

        #region Public Methods

        public IList<Quote> GetQuotes(string userId)
        {
            return _QuoteDR.GetListStrParams(q => q.Vehicle.CarFixedUser.UserID == userId,
                "Vehicle",
                "QuoteItemBasics",
                "QuoteItemBasics.BasicSubCategory",
                "QuoteItemBasics.BasicSubCategory.BasicCategory");
        }

        //public IList<Quote> GetQuoteOptionsForGarage(int garageId)
        //{
        //    return _QuoteDR.GetQuoteOptions(garageId);
        //}

        public IList<QuoteGarageOption> GetSubmittedQuoteOptions(int quoteId)
        {
            List<QuoteGarageOption> options = null;

            options = _QuoteGarageOptionDR.GetListStrParams(
                q => q.QuoteID == quoteId && q.StatusID == (int)QuoteGarageOption.StatusEnum.Sent && q.Quote.StatusID == (int)Quote.StatusEnum.RequestedByUser,
                "Garage",
                "Garage.Address",
                "Quote.QuoteItemBasics",
                "Quote.QuoteItemBasics.BasicSubCategory",
                "Quote.QuoteItemBasics.BasicSubCategory.BasicCategory").ToList();

            return options;
        }

        public IList<QuoteGarageOption> GetQuoteOptionsForGarage(int garageId, bool getSubmittedQuotes = false)
        {
            List<QuoteGarageOption> options = null;

            if (getSubmittedQuotes)
            {
                options = _QuoteGarageOptionDR.GetListStrParams(
                    q => q.GarageID == garageId && q.StatusID == (int)QuoteGarageOption.StatusEnum.Sent && q.Quote.StatusID == (int)Quote.StatusEnum.RequestedByUser,
                    "Quote",
                    "Quote.Vehicle",
                    "Quote.QuoteItemBasics",
                    "Quote.QuoteItemBasics.BasicSubCategory",
                    "Quote.QuoteItemBasics.BasicSubCategory.BasicCategory").ToList();
            }
            else
            {
                options = _QuoteGarageOptionDR.GetListStrParams(
                    q => q.GarageID == garageId && q.StatusID == null && q.Quote.StatusID == (int)Quote.StatusEnum.RequestedByUser,
                    "Quote",
                    "Quote.Vehicle",
                    "Quote.QuoteItemBasics",
                    "Quote.QuoteItemBasics.BasicSubCategory",
                    "Quote.QuoteItemBasics.BasicSubCategory.BasicCategory").ToList();
            }

            return options;
        }


        #endregion Public Methods
    }
}
