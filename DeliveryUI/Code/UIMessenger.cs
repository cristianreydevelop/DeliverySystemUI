using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace DeliveryUI.Code
{
    public interface IUIMessenger
    {
        string TableHeader { get; set; }
        string Message { get; set; }
    }

    public static class Settings
    {
        public static string ActiveDeliveryTypes
        {
            get
            {
                return ConfigurationManager.AppSettings["ACTIVE_DELIVERY_TYPES"];
            }
        }

        public static string ActionsList
        {
            get
            {
                return ConfigurationManager.AppSettings["ACTIONS_LIST"];
            }
        }

        public static string DeliveriesList
        {
            get
            {
                return ConfigurationManager.AppSettings["DELIVERIES_LIST"];
            }
        }

        public static string GenericErrorMessage
        {
            get
            {
                return ConfigurationManager.AppSettings["GENERIC_ERROR_MESSAGE"];
            }
        }

        public static string APIGenericErrorMessage
        {
            get
            {
                return ConfigurationManager.AppSettings["API_GENERIC_ERROR_MESSAGE"];
            }
        }

        public static string CreateDelivery
        {
            get
            {
                return ConfigurationManager.AppSettings["CREATE_DELIVERY"];
            }
        }

        public static string DeliveryCreated
        {
            get
            {
                return ConfigurationManager.AppSettings["DELIVERY_CREATED"];
            }
        }

        public static string DeliveryDeactivated
        {
            get
            {
                return ConfigurationManager.AppSettings["DELIVERY_DEACTIVATED"];
            }
        }

        public static string DeliveryUpdated
        {
            get
            {
                return ConfigurationManager.AppSettings["DELIVERY_UPDATED"];
            }
        }

        public static string DeliveryToUpdate
        {
            get
            {
                return ConfigurationManager.AppSettings["SELECT_DELIVERY_UPDATE"];
            }
        }

        public static string DeliveryToDeactivate
        {
            get
            {
                return ConfigurationManager.AppSettings["SELECT_DELIVERY_DEACTIVATE"];
            }
        }

        public static string UpdateDelivery
        {
            get
            {
                return ConfigurationManager.AppSettings["UPDATE_DELIVERY"];
            }
        }

        public static string SaveDeliveryEndPoint
        {
            get
            {
                return ConfigurationManager.AppSettings["SAVE_DELIVERY_END_POINT"];
            }
        }

        public static string UpdateDeliveryEndPoint
        {
            get
            {
                return ConfigurationManager.AppSettings["UPDATE_DELIVERY_END_POINT"];
            }
        }

        public static string GetDeliveriesEndPoint
        {
            get
            {
                return ConfigurationManager.AppSettings["GET_DELIVERIES_END_POINT"];
            }
        }
    }
}