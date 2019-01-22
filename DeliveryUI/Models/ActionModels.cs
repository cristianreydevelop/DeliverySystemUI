using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeliveryUI.Code;

namespace DeliveryUI.Models
{
    public class Actions : IUIMessenger
    {
        #region "MODEL_FOR_ACTIONS"
        public string ActionsField { get; set; }

        public class ActionsTypes
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        // Table header used for Index, Action, Update and Deactivate.
        public string TableHeader { get; set; }

        // Used to communicate with the user.
        public string Message { get; set; }

        public IEnumerable<ActionsTypes> AllActions { get; set; }
        #endregion
    }
}