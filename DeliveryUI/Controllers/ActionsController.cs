using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeliveryUI.Models;
using DeliveryUI.Code;

namespace DeliveryUI.Controllers
{
    public class ActionsController : Controller
    {

        //
        // GET: /Actions/
        [HttpGet]
        public ActionResult Action()
        {
            Actions model = new Actions
            {
                AllActions = UIDeliveryHelper.GetActions(),
                TableHeader = UIDeliveryHelper.Message,
                Message = UIDeliveryHelper.CheckApi()
            };

            return View(model);
        }

    }
}
