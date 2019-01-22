using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DeliveryUI.Code;

namespace DeliveryUI.Models
{
    public class DeliveryModels : IUIMessenger
    {
        // Table header used for Index, Action, Update and Deactivate.
        public string TableHeader { get; set; }

        // Used to communicate with the user.
        public string Message { get; set; }

        #region "MODEL_FOR_DELIVERY_TYPES"
        [Required(ErrorMessage = "Select a delivery method.")]
        [Display(Name = "Delivery type")]
        public string DeliveryField { get; set; }

        public class DeliveryTypes
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        public IEnumerable<DeliveryTypes> Deliveries { get; set; }
        #endregion

        [Required(ErrorMessage = "Sender name or email is required.")]
        [Display(Name = "Sender")]
        public string from { get; set; }

        [Required(ErrorMessage = "Recipient name or email is required.")]
        [Display(Name = "Recipient")]
        public string to { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [Display(Name = "Message")]
        public string message { get; set; }

        [Display(Name = "Select to activate")]
        public bool active { get; set; }

        public int deliveriesId { get; set; }

        public int deliveryId { get; set; }

        // Used to get list of delivery items from the database to update.
        public class UpdateDelivery
        {
            public int deliveriesId { get; set; }

            public int deliveryId { get; set; }

            public string from { get; set; }

            public string to { get; set; }
            public string message { get; set; }
            public bool active { get; set; }
        }
    }
}