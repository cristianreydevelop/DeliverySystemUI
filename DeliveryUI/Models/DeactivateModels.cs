using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeliveryUI.Code;

namespace DeliveryUI.Models
{
    public class DeactivateModels : IUIMessenger
    {
        #region "MODEL_TO_DEACTIVATE"
        public string ActiveDeliveries { get; set; }

        public class Deactivates
        {
            public string deliveriesid { get; set; }
            public string info { get; set; }
        }

        // Table header used for Index, Action, Update and Deactivate.
        public string TableHeader { get; set; }

        // Used to communicate with the user.
        public string Message { get; set; }

        public IEnumerable<Deactivates> AllDeactivates { get; set; }
        #endregion
    }
}