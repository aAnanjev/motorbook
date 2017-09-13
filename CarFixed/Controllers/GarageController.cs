using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using CarFixed.Models;
using CarFixed.DS.BLL;
using CarFixed.DS.DM;

namespace CarFixed.Controllers
{
    public class GarageController : Controller
    {
        private IQuoteBL _QuoteBL = null;
        private IUserBL _UserBL = null;
        private IGarageBL _GarageBL = null;

        public GarageController()
        {
            _QuoteBL = new QuoteBL();
            _UserBL = new UserBL();
            _GarageBL = new GarageBL();
        }

        // GET: Garage
        public ActionResult Index()
        {
            GarageIndexModel model = new GarageIndexModel();
            CarFixedUser user = _UserBL.GetCarFixedUser(this.User.Identity.GetUserId());

            if (user.GarageID.HasValue)
            {
                model.QuoteOptions = _QuoteBL.GetQuoteOptionsForGarage(user.GarageID.Value).ToList();
                model.SubmittedQuoteOptions = _QuoteBL.GetQuoteOptionsForGarage(user.GarageID.Value, true).ToList();
            }

            return View(model);
        }

        public ActionResult SaveGarageDashboard(GarageIndexModel model)
        {
            List<QuoteGarageOption> optionsToUpdate = new List<QuoteGarageOption>();

            foreach (QuoteGarageOption option in model.QuoteOptions.FindAll(o => o.IsSelected == true))
            {
                QuoteGarageOption dbOption = _GarageBL.GetQuoteGarageOption(option.QuoteGarageOptionID);
                dbOption.EntityState = EntityState.Modified;
                dbOption.IsQuoteSubmitted = true;
                dbOption.DateQuoteSubmitted = DateTime.Now;
                dbOption.QuoteSubmittedValue = option.QuoteSubmittedValue;
                dbOption.QuoteSubmittedMessage = option.QuoteSubmittedMessage;

                optionsToUpdate.Add(dbOption);
            }

            _GarageBL.UpdateQuoteGarageOption(optionsToUpdate.ToArray());


            return RedirectToAction("Index");
        }

        public ActionResult SaveQuoteGarageOption(string button, QuoteGarageOption option)
        {

            QuoteGarageOption dbOption = _GarageBL.GetQuoteGarageOption(option.QuoteGarageOptionID);
            dbOption.EntityState = EntityState.Modified;
            dbOption.IsQuoteSubmitted = true;
            dbOption.DateQuoteSubmitted = DateTime.Now;
            dbOption.QuoteSubmittedValue = option.QuoteSubmittedValue;
            dbOption.QuoteSubmittedMessage = option.QuoteSubmittedMessage;

            if (button == "1")
                dbOption.StatusID = (int)QuoteGarageOption.StatusEnum.Sent;
            else
                dbOption.StatusID = (int)QuoteGarageOption.StatusEnum.Dismissed;


            _GarageBL.UpdateQuoteGarageOption(dbOption);


            return RedirectToAction("Index");
        }
    }
}