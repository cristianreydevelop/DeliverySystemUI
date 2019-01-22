using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeliveryUI.Code;
using DeliveryUI.Models;

namespace DeliveryUI.Controllers
{
    public class UpdateController : Controller
    {
        //
        // GET: /Update/
        [HttpGet]
        public ActionResult GetUpdateList()
        {
            // Leverage Deactivate models as it is exactly what we need.
            DeactivateModels model = new DeactivateModels
            {
                AllDeactivates = UIDeliveryHelper.GetDeliveries(),
                TableHeader = Settings.DeliveryToUpdate,
                Message = UIDeliveryHelper.Message
            };
            // This activates a dropdown on the UI.

            return View(model);
        }

        public ActionResult UpdateDelivery()
        {
            return View();
        }
    }
}
