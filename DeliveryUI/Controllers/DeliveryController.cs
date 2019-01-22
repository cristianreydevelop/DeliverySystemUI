using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Text;
using System.IO;
using DeliveryUI.Code;
using DeliveryUI.Models;

namespace DeliveryUI.Controllers
{
    public class DeliveryController : Controller
    {
        [HttpPost]
        public ActionResult SaveDelivery()
        {
            string Msg = string.Empty;

            if (ModelState.IsValid)
            {
                string Method = string.Empty;
                string ApiEnd = string.Empty;
                string RequestBody = "deliveryid=" + Request.Form["DeliveryField"] +
                                     "&from=" + Request.Form["from"] +
                                     "&to=" + Request.Form["to"] +
                                     "&message=" + Request.Form["message"] +
                                     "&active=" + Request.Form["active"].Contains("true").ToString();

                // InsertDelivery.deliveriesId is only used when updating or deleting a delivery.
                switch (string.IsNullOrEmpty(Request.Form["deliveriesId"]) || Request.Form["deliveriesId"] == "0")
                {
                    case true:
                        // We are creating a new delivery.
                        Method = "POST";
                        ApiEnd = Settings.SaveDeliveryEndPoint;

                        break;
                    default:
                        // We are editing an existing delivery.
                        Method = "PUT";
                        ApiEnd = Settings.UpdateDeliveryEndPoint;
                        RequestBody += "&deliveriesId=" + Request.Form["deliveriesId"];

                        break;
                }

                UIDeliveryHelper.CallAPIPost(Method, ApiEnd, "application/x-www-form-urlencoded", RequestBody);
                Msg = UIDeliveryHelper.Message;
            }

            Actions model = new Actions
            {
                AllActions = UIDeliveryHelper.GetActions(),
                TableHeader = UIDeliveryHelper.Message,
                Message = Msg
            };

            return View("~/Views/Actions/Action.cshtml", model);
        }

        // Brings up the main form of the application to Save and Edit deliveries.
        [HttpGet]
        public ActionResult Index()
        {
            DeliveryModels.UpdateDelivery ud = new DeliveryModels.UpdateDelivery();
            string Msg = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["ActiveDeliveries"]))
            {
                ud = UIDeliveryHelper.GetDelivery(Convert.ToInt32(Request.QueryString["ActiveDeliveries"]));
                Msg = UIDeliveryHelper.Message;
            }

            DeliveryModels model = new DeliveryModels
            {
                Deliveries = UIDeliveryHelper.GetDelTypes(),
                Message = Msg,
                deliveriesId = ud.deliveriesId,
                deliveryId = ud.deliveryId,
                from = ud.from,
                to = ud.to,
                message = ud.message,
                active = ud.active,
                TableHeader = string.IsNullOrEmpty(Request.Form["ActiveDeliveries"]) ? Settings.CreateDelivery : Settings.UpdateDelivery
            };

            return View(model);
        }

        //
        // GET: /Delivery/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Delivery/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Delivery/Create

        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Delivery/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Delivery/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Delivery/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Delivery/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
