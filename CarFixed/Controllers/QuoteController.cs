using CarFixed.DS.BLL;
using CarFixed.DS.DM;
using CarFixed.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using AutoGuru.Assistant.ThirdParty.AutoData.Models;
using CarFixed.DS.BLL;
using AutoGuru.Assistant;
using AutoGuru.Data.Vehicle.Services;


namespace CarFixed.Controllers
{
    public class QuoteController : Controller
    {
        // GET: Quote
        public ActionResult Index()
        {
            QuoteModel model = new QuoteModel();

            if (Session["VrmLookupResult"] != null)
            {
                VrmResponse vehicle = (VrmResponse)Session["VrmLookupResult"];

                model.vrm = vehicle.Vrm;
                model.make = vehicle.Make;
                model.modeltype = vehicle.Model;
                model.year = vehicle.Year;
                model.fuelType = vehicle.FuelType;
            }
            else
            {

            }

            model.list = new List<string>();
            model.subList = new List<string>();

            model.list.Add("Databse");
            model.list.Add("Entries");
            model.list.Add("Go");
            model.list.Add("Here!");
            model.subList.Add("Databse");
            model.subList.Add("Entries");
            model.subList.Add("Go");
            model.subList.Add("Here!");

            return View(model);
        }

        public JsonResult GetCategories()
        {
            CategoryBL categoryBl = new CategoryBL();

            List<BasicCategory> cats = categoryBl.GetAllBasicCategories().ToList();

            return Json(cats, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubCategories(int categoryId)
        {
            CategoryBL categoryBl = new CategoryBL();

            List<BasicSubCategory> cats = categoryBl.GetBasicSubCategories(categoryId).ToList();

            foreach (BasicSubCategory cat in cats)
            {
                if (cat.BasicSubCategoryGroup != null)
                    cat.BasicSubCategoryGroup.BasicSubCategories = null;
            }

            return Json(cats, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBasicSubCategoryRepairEstimatedTimes(int subCategoryId)
        {
            CategoryBL categoryBl = new CategoryBL();
            RepairTimeBL repairTimeBl = new RepairTimeBL();

            BasicSubCategoryRepairModel model = new BasicSubCategoryRepairModel();
            model.RepairRefs = categoryBl.GetBasicSubCategoryRepairRef(subCategoryId).ToList();

            if (Session["VrmResponse"] != null && Session["VrmResponse"] is VrmResponse)
            {
                VrmResponse vrmResponse = Session["VrmResponse"] as VrmResponse;
                model.RelevantRepairs = repairTimeBl.GetRepairs(vrmResponse.MidCodes, vrmResponse.GCodeId, model.RepairRefs);
            }

            return PartialView("_BasicSubCategoryRepairPartial", model);
        }
        public ActionResult GetBasicSubCategoryServiceEstimatedTimes(int subCategoryId)
        {
            BasicSubCategoryServiceModel model = new BasicSubCategoryServiceModel();
            if (Session["VrmResponse"] != null && Session["VrmResponse"] is VrmResponse)
            {
                VrmResponse vrmResponse = Session["VrmResponse"] as VrmResponse;
                ServiceBL serviceBl = new ServiceBL();

                model.ServiceWrapper =serviceBl.GetServices(vrmResponse.Vrm, 12000, 12000);
            }
            return PartialView("_BasicSubCategoryServicePartial", model);

        }

        [HttpPost]
        public ActionResult DoVRMLookup(string vrm)
        {
            VehicleBL vehicleBl = new VehicleBL();
            //List<Vehicle> vehicles = vehicleBl.GetAllVehicles();
            VrmResponse vrmResponse = vehicleBl.GetVrmLightResponseByVrm(vrm);
            Session["CurrentVrm"] = vrm;

            if (vrmResponse != null)
            {
                Session["VrmLookupResult"] = vrmResponse;
                var json = new { data = vrmResponse, status = "Confirmed"};
                return Json(json);
            }
            else
                return Json(new { status = "Failed" });
        }
    }
}