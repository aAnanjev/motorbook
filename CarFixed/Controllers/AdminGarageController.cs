using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CarFixed.Models;
using CarFixed.DS.BLL;
using CarFixed.DS.DM;

namespace CarFixed.Controllers
{
    public class AdminGarageController : Controller
    {
        private IGarageBL _GarageBL = null;
        private IStatusBL _StatusBL = null;

        public AdminGarageController()
        {
            _GarageBL = new GarageBL();
            _StatusBL = new StatusBL();
        }

        public ActionResult ViewGarages()
        {
            AdminGarageModelView model = new AdminGarageModelView();

            model.Garages = _GarageBL.GetGarages().ToList();

            return View(model);
        }

        public ActionResult EditGarage(int garageId)
        {
            AdminGarageModelEdit model = new AdminGarageModelEdit();

            model.GarageStatuses = _StatusBL.GetGarageStatuses().ToList();
            model.Garage = _GarageBL.GetGarage(garageId);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditGarage(AdminGarageModelEdit model)
        {
            Garage garage = _GarageBL.GetGarage(model.Garage.GarageID);

            garage.Name = model.Garage.Name;
            garage.StatusID = model.Garage.StatusID;
            garage.EntityState = EntityState.Modified;

            _GarageBL.UpdateGarage(garage);

            return RedirectToAction("ViewGarages");
        }
    }
}