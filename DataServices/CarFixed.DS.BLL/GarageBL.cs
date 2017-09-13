using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CarFixed.DS.DAL;
using CarFixed.DS.DM;

namespace CarFixed.DS.BLL
{
    public interface IGarageBL
    {
        IList<Garage> GetGarages();
        Garage GetGarage(int garageId);
        void UpdateGarage(params Garage[] garages);
        IList<GarageSelectByQuoteDistance_Result> GetGaragesForQuote(int quoteId, double radius);
        void UpdateQuoteGarageOption(params QuoteGarageOption[] options);

        QuoteGarageOption GetQuoteGarageOption(int quoteGarageOptionId);
    }

    public class GarageBL : IGarageBL
    {
        #region Variables

        private IGarageDR _GarageDR = null;
        private IQuoteGarageOptionDR _QuoteGarageOptionDR = null;

        #endregion Variables

        #region Construction

        public GarageBL()
        {
            _GarageDR = new GarageDR();
            _QuoteGarageOptionDR = new QuoteGarageOptionDR();
        }

        #endregion Construction

        #region Public Methods

        public IList<Garage> GetGarages()
        {
            return _GarageDR.GetAll(g => g.Status);
        }

        public Garage GetGarage(int garageId)
        {
            return _GarageDR.GetSingle(g => g.GarageID == garageId, g => g.Address);
        }

        public IList<GarageSelectByQuoteDistance_Result> GetGaragesForQuote(int quoteId, double radius)
        {
            return _GarageDR.GetGaragesForQuote(quoteId, radius);
        }

        public void UpdateGarage(params Garage[] garages)
        {
            _GarageDR.Update(garages);
        }

        public void UpdateQuoteGarageOption(params QuoteGarageOption[] options)
        {
            _QuoteGarageOptionDR.Update(options);
        }

        public QuoteGarageOption GetQuoteGarageOption(int quoteGarageOptionId)
        {
            return _QuoteGarageOptionDR.GetSingle(o => o.QuoteGarageOptionID == quoteGarageOptionId);
        }
        

        #endregion Public Methods
    }
}
