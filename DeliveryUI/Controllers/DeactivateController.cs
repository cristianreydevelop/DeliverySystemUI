using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeliveryUI.Models;
using DeliveryUI.Code;

namespace DeliveryUI.Controllers
{
    public class DeactivateController : Controller
    {
        //
        // GET: /Deactivate/
        [HttpGet]
        public ActionResult GetDeactivateList()
        {
            DeactivateModels model = new DeactivateModels
            {
                AllDeactivates = UIDeliveryHelper.GetDeliveries(),
                TableHeader = Settings.DeliveryToDeactivate,
                Message = UIDeliveryHelper.Message
            };
            
            return View(model);
        }

        [HttpPost]
        public ActionResult DeactivateDelivery()
        {
            string Msg = string.Empty;

            if (ModelState.IsValid)
            {
                // Web browser does not support put. Accept post and change as needed.
                UIDeliveryHelper.CallAPIPost("DELETE", Settings.UpdateDeliveryEndPoint, "application/x-www-form-urlencoded", "deliveriesId=" + Request.Form["ActiveDeliveries"].ToString());
                Msg = UIDeliveryHelper.Message;
            }

            Actions model = new Actions
            {
                AllActions = UIDeliveryHelper.GetActions(),
                Message = Msg,
                TableHeader = UIDeliveryHelper.Message
            };

            return View("~/Views/Actions/Action.cshtml", model);
        }
    }
}
