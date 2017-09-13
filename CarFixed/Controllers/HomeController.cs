using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

using CarFixed.DS.DM;
using CarFixed.DS.BLL;
using CarFixed.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using CarFixed.Core;

namespace CarFixed.Controllers
{
    public class HomeController : Controller
    {
        private GarageBL _GarageBL = null;
        private UserBL _UserBL = null;
        private VehicleBL _VehicleBL = null;

        public HomeController()
        {
            _GarageBL = new GarageBL();
            _UserBL = new UserBL();
            _VehicleBL = new VehicleBL();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult DoVRMLookup(string vrm)
        {
            VehicleBL vehicleBl = new VehicleBL();
            //List<Vehicle> vehicles = vehicleBl.GetAllVehicles();
            VrmResponse vrmResponse = vehicleBl.GetVrmLightResponseByVrm(vrm);
            Session["CurrentVrm"] = vrm;
            Session["VrmResponse"] = vrmResponse;

            if (vrmResponse != null)
            {
                Session["VrmLookupResult"] = vrmResponse;
                return Json(new { status = "Confirmed" });
            }
            else
                return Json(new { status = "Failed" });
        }

        public ActionResult QuoteForm(bool reset = false)
        {
            if (reset)
            {
                Session["VrmLookupResult"] = null;
            }

            QuoteModel model = new QuoteModel();
            if (Session["VrmLookupResult"] == null)
            {
                model.make = "";
                model.modeltype = "";
                model.vrm = "";
                model.year = "";
                model.fuelType = "";
            }
            else
            {

                VrmResponse vehicle = (VrmResponse)Session["VrmLookupResult"];

                model.list = new List<string>();
                model.subList = new List<string>();
                model.make = vehicle.Make;
                model.modeltype = vehicle.Model;
                model.vrm = vehicle.Vrm;
                model.year = vehicle.Year;
                model.fuelType = vehicle.FuelType;

            }

            if (model != null)
                return PartialView("_QuoteForm", model);
            else
                return null;
            
        }
        [HttpPost]
        public async Task<ActionResult> QuoteFormSubmit(QuoteModel model)
        {
            int tempYear = -1;
            string userId = String.Empty;

            ApplicationUser user = null;

            if (model.IsNewUser)
            {
                user = new ApplicationUser { UserName = model.Email, Email = model.Email, PhoneNumber = model.Telephone};

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    userId = user.Id;
                }
                else
                {
                    //TODO: can we return and display errors????
                    //redirect?????
                    return Json(new { success = false, msg = result.Errors }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                user = UserManager.FindByName(model.ExistingUserEmail);

                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                userId = user.Id;
            }

            if (!String.IsNullOrEmpty(userId))
            {
                
                CarFixedUser cfUser = _UserBL.GetCarFixedUser(userId);
                Vehicle vehicle = null;

                if (cfUser == null)
                {
                    UserManager.AddToRole(user.Id, "User");
                    int num;
                    cfUser = new CarFixedUser();
                    cfUser.EntityState = EntityState.Added;
                    cfUser.UserID = userId;
                    cfUser.DateAdded = DateTime.Now;
                    cfUser.FirstName = model.Firstname;
                    cfUser.LastName = model.Lastname;
                    vehicle = new Vehicle();
                    vehicle.EntityState = EntityState.Added;
                    vehicle.VRM = model.vrm;
                    vehicle.Manufacturer = model.make;
                    vehicle.Model = model.modeltype;
                    vehicle.FuelType = model.fuelType;
                    if (Int32.TryParse(model.year, out num))
                        vehicle.Year = num;
                    vehicle.DateAdded = DateTime.Now;

                    cfUser.Vehicles.Add(vehicle);

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                else
                {
                    cfUser.EntityState = EntityState.Modified;

                    if (cfUser.Vehicles.ToList().Exists(v => v.VRM == model.vrm) == false)
                    {
                        vehicle = new Vehicle();
                        vehicle.EntityState = EntityState.Added;
                        vehicle.VRM = model.vrm;
                        vehicle.DateAdded = DateTime.Now;
                        vehicle.IsDisplayed = true;

                        cfUser.Vehicles.Add(vehicle);
                    }
                    else
                    {
                        vehicle = cfUser.Vehicles.ToList().Find(v => v.VRM == model.vrm);
                        cfUser.Vehicles.ToList().Find(v => v.VRM == model.vrm).EntityState = EntityState.Modified;
                        cfUser.Vehicles.ToList().Find(v => v.VRM == model.vrm).IsDisplayed = true;
                    }
                }

                if (cfUser.Address == null)
                {
                    cfUser.Address = new Address();
                    cfUser.Address.EntityState = EntityState.Added;                    
                }
                else
                    cfUser.Address.EntityState = EntityState.Modified;

                if ((String.IsNullOrEmpty(cfUser.Address.Postcode)) ||
                    cfUser.Address.Postcode.ToUpper().Replace(" ", "") != model.Postcode.ToUpper().Replace(" ", "") ||
                    cfUser.Address.Longitude.HasValue == false ||
                    cfUser.Address.Latitude.HasValue == false)
                {
                    cfUser.Address.Postcode = model.Postcode.ToUpper().Replace(" ", "");

                    PostcodeLatitudeLongitude latLon = GeoLocationService.GetLatitudeAndLongitude(model.Postcode);

                    if (latLon.HasValue)
                    {
                        cfUser.Address.Latitude = Convert.ToDecimal(latLon.Latitude);
                        cfUser.Address.Longitude = Convert.ToDecimal(latLon.Longitude);
                    }
                }

                QuoteItemBasic item = new QuoteItemBasic();
                item.EntityState = EntityState.Added;
                item.DateAdded = DateTime.Now;
                item.BasicSubCategoryID = model.SubCategoryId;
                item.AdditionalInfo = model.Description;

                Quote quote = new Quote();
                quote.EntityState = EntityState.Added;
                quote.DateAdded = DateTime.Now;
                quote.QuoteItemBasics.Add(item);
                quote.QuoteUrgencyID = (int)QuoteUrgency.QuoteUrgencyEnum.Medium;
                quote.StatusID = (int)Quote.StatusEnum.RequestedByUser;

                vehicle.Quotes.Add(quote);           

                _UserBL.UpdateCarFixedUser(cfUser);

                //TODO: Move to some sort of job engine

                List<GarageSelectByQuoteDistance_Result> garageOptions =_GarageBL.GetGaragesForQuote(quote.QuoteID, Convert.ToInt32(ConfigurationManager.AppSettings["DefaultGarageRadius"])).ToList();
                List<QuoteGarageOption> quoteGarageOptions = new List<QuoteGarageOption>();

                foreach (GarageSelectByQuoteDistance_Result option in garageOptions)
                {
                    QuoteGarageOption opt = new QuoteGarageOption();
                    opt.EntityState = EntityState.Added;
                    opt.DateAdded = DateTime.Now;
                    opt.QuoteID = quote.QuoteID;
                    opt.GarageID = option.GarageID;
                    opt.Distance = option.Distance.Value;
                    opt.IsQuoteSubmitted = false;
                    quoteGarageOptions.Add(opt);
                }

                if (quoteGarageOptions.Count() > 0)
                    _GarageBL.UpdateQuoteGarageOption(quoteGarageOptions.ToArray());
            }
           

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult TermsAndConditions()
        {
            return View();
        }

        ActionResult Validation()
        {
            return Json(new { msg = false });
        }
    }
}