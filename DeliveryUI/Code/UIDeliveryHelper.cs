using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using DeliveryUI.Models;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Configuration;

namespace DeliveryUI.Code
{
    public static class UIDeliveryHelper
    {
        private static string themessage = string.Empty;

        public static String Message 
        { 
            get
            {
                return themessage;
            }
        }

        public static string CheckApi()
        {
            GetDelivery(0);

            return themessage;
        }

        public static List<DeliveryModels.DeliveryTypes> GetDelTypes()
        {
            themessage = string.Empty;

            List<DeliveryModels.DeliveryTypes> DelTypes = new List<DeliveryModels.DeliveryTypes>();
            DeliveryModels.DeliveryTypes DelItem = new DeliveryModels.DeliveryTypes();

            DelTypes.Add(new DeliveryModels.DeliveryTypes { ID = "100", Name = "EMAIL" });
            DelTypes.Add(new DeliveryModels.DeliveryTypes { ID = "101", Name = "FTP"} );
            DelTypes.Add(new DeliveryModels.DeliveryTypes { ID = "102", Name = "SFTP"} );
            DelTypes.Add(new DeliveryModels.DeliveryTypes { ID = "103", Name = "HTTP"} );
            DelTypes.Add(new DeliveryModels.DeliveryTypes { ID = "104", Name = "HTTPS"} );

            return DelTypes;
        }

        public static List<Actions.ActionsTypes> GetActions()
        {
            themessage = string.Empty;

            List<Actions.ActionsTypes> ActionTypes = new List<Actions.ActionsTypes>();
            Actions.ActionsTypes ActionItem = new Actions.ActionsTypes();

            ActionTypes.Add(new Actions.ActionsTypes { ID = "1", Name = "Create a Delivery" });
            ActionTypes.Add(new Actions.ActionsTypes { ID = "2", Name = "Update a Delivery" });
            ActionTypes.Add(new Actions.ActionsTypes { ID = "3", Name = "Deactivate a Delivery" });

            return ActionTypes;
        }

        public static void CallAPIPost(string Method, string ApiEndPoint, string ContentType, string RequestBody)
        {
            switch(Method)
            {
                case "PUT":
                    themessage = Settings.DeliveryUpdated;

                    break;
                case "DELETE":
                    themessage = Settings.DeliveryDeactivated;

                    break;
                default:
                    // POST
                    themessage = Settings.DeliveryCreated;

                    break;
            }   

            HttpWebRequest webRequest = null;
            int intStatusCode = 0;
            string strStatus = string.Empty;

            webRequest = (HttpWebRequest)WebRequest.Create(ApiEndPoint);

            webRequest.ContentType = ContentType;
            webRequest.Method = Method;

            bool blnRetVal = false;

            Stream os = null;
            try
            {
                
                byte[] Data = Encoding.UTF8.GetBytes(RequestBody); // "deliveryid=" + Request.Form["InsertDelivery.deliveryId"] + "&from=" + Request.Form["InsertDelivery.from"] + "&to=" + Request.Form["InsertDelivery.to"] + "&message=" + Request.Form["InsertDelivery.message"] + "&active=" + Request.Form["InsertDelivery.active"].Contains("true").ToString());
                webRequest.ContentLength = Data.Length;   //Count bytes to send

                os = webRequest.GetRequestStream();
                os.Write(Data, 0, Data.Length);         //Send it
                
                blnRetVal = true;

                if (os != null)
                {
                    os.Flush();
                    os.Close();
                    os = null;
                }

                // if blnRetVal is true then sending was successful.
                string Result = string.Empty;
                if (blnRetVal == true)
                {
                    //WebResponse webResponse = webRequest.GetResponse();
                    try
                    {
                        using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                        {
                            if (webResponse == null)
                            {
                                Exception exInner = new Exception("Unable to get a response back for a posting " + 0.ToString() + "\r\nFeedID " + 0.ToString() + "\r\nThe webResponse object is null");
                                throw new Exception(string.Empty, exInner);
                            }

                            intStatusCode = (int)webResponse.StatusCode;
                            strStatus = webResponse.StatusDescription;

                            using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                            {
                               // retVal = Json.Decode<List<DeliveryModels.GetDeliveries>>(sr.ReadToEnd());
                            }
                        }
                    }
                    catch (WebException wex)
                    {
                        HttpWebResponse strGetErrorResponse = ((HttpWebResponse)wex.Response);
                        Stream stream = null;

                        int intCode = 0;
                        string result = string.Empty;
                        string strDescription = string.Empty;

                        // Media id 10137 is for some reason erroring here.  ErrorID 602236
                        try
                        {
                            stream = strGetErrorResponse.GetResponseStream();

                            Encoding encoding = System.Text.Encoding.Default; // System.Text.Encoding.GetEncoding("utf-8");

                            StreamReader streamReader = new StreamReader(stream, encoding);

                            result = streamReader.ReadToEnd();

                            if (result.Contains("PRIMARY KEY constraint"))
                                themessage = "THERE IS ALREADY DELIVERY WITH THE SAME DELIVERY TYPE, SENDER AND RECIPIENT";
                            else
                                themessage = Settings.GenericErrorMessage;

                            intCode = (int)strGetErrorResponse.StatusCode;
                            strDescription = strGetErrorResponse.StatusDescription;

                            // TODO: Log error.
                            // string strErrorMessage = "|ORIGINAL_SOURCE" + wex.Source + " --> Error occurred in " + CLASS_NAME + "." + PROCEDURE_NAME + "|ERRORINFOError reported from: " + CLASS_NAME + "." + PROCEDURE_NAME + "|CLASSNAME" + CLASS_NAME + "|REQEMEDIAID0" + "|REQUISITIONID0|FEEDID" + p_intAutoFeedID.ToString() + "|EMEDIAID" + p_strEmediaID + "|ERRORLEVEL2|WRITESTOPLIGHT1|FILENAME" + p_strFileName + "|PARAMUSED" + p_strParamUsed + "|PARAMTYPE16|PROCESSGUID" + p_strGuid;
                            // ClassErrors clsErr = new ClassErrors(wex.Message + strErrorMessage, wex, wex.Source);

                            /*try
                            {
                                clsData.ReportError(clsErr.ErrorInfo, clsErr.Source, clsErr.Message, clsErr.LineNumber, clsErr.AfErrorNumber, clsErr.ErrorNumber, clsErr.ConnectionString, clsErr.Filename, clsErr.AutoFeedID, clsErr.EmediaID, clsErr.ReqEmediaID.ToString(), clsErr.DeliveryType, clsErr.EmailAddress, clsErr.FTPAddress, clsErr.URLAddress, clsErr.LocalCopyAdddress, clsErr.ParamUsed, clsErr.RequisitionID, clsErr.ReqEmediaID, ClassStaticData.MachineName, clsErr.StoredProcedure, clsErr.ParamType, clsErr.ErrorLevel, clsErr.WriteStopLight, string.Empty, clsErr.UsesSubID, clsErr.SubID, clsErr.ProcessGuid, clsErr.GetStackTrace, clsErr.GetStackFrames, clsErr.InnerExceptionMessage, clsErr.ExceptionType, clsErr.ColumnNumber);
                            }
                            catch (Exception ex2)
                            {
                                ClassStaticData.EmailMessage(ClassStaticData.GetFromEmail, ClassStaticData.GetDevEmail, string.Empty, ClassStaticData.Msg_UnexpectedError, "Original error follows:\r\n\r\nFrom: " + System.Reflection.Assembly.GetExecutingAssembly().ToString() + "\r\nConnectionString: " + ClassStaticData.MASTER_SOURCE + "\r\n\r\n" + "Error Message: " + wex.Message + "\r\n\r\n" + wex.Source + "\r\n\r\nNew error follows:\r\n" + ex2.Message + "\r\n" + ex2.Source + "\r\nNew error inner exception: " + ex2.InnerException + "\r\n" + "\r\n\r\nAdditional information follows:\r\nAutoFeedID: " + clsErr.AutoFeedID.ToString() + "\r\n\r\nEmediaID: " + clsErr.EmediaID + "\r\n\r\nCIDList: " + clsErr.CIDList + "\r\n\r\nErrorTime: " + System.DateTime.Now.ToLongTimeString());
                            }*/
                        }
                        catch (Exception ex2)
                        {
                            themessage = Settings.GenericErrorMessage;
                            // TODO: Log error.
                            // throw ex2;
                        }
                    }
                }

                webRequest = null;

                if (intStatusCode > 204)
                {
                    /*Exception exInner = new Exception("Unable to post job " + p_intReqEmediaID.ToString() + "\r\nStatusCode " + intStatusCode.ToString() + "\r\nStatus " + strStatus + "\r\nFeedID " + p_intAutoFeedID.ToString() + "\r\nThe site has returned this error " + Result);
                    throw new Exception(string.Empty, exInner);*/
                }

                /*Store result value here*/
            }
            finally
            {
                if (os != null)
                {
                    os.Flush();
                    os.Close();
                    os = null;
                }

                /*if (clsData != null)
                {
                    clsData.Dispose();
                }

                clsData = null;*/
            }

            webRequest = null;
        }

        public static DeliveryModels.UpdateDelivery GetDelivery(int DeliveriesId)
        {
            themessage = string.Empty;

            DeliveryModels.UpdateDelivery retVal = null;

            HttpWebRequest webRequest = null;
            int intStatusCode = 0;
            string strStatus = string.Empty;

            webRequest = (HttpWebRequest)WebRequest.Create("http://localhost:50628/api/Delivery/GetDelivery/?DeliveriesId=" + DeliveriesId.ToString());

            webRequest.Method = "get";

            bool blnRetVal = false;

            Stream os = null;
            try
            {
                blnRetVal = true;

                if (os != null)
                {
                    os.Flush();
                    os.Close();
                    os = null;
                }

                // if blnRetVal is true then sending was successful.
                string Result = string.Empty;

                if (blnRetVal == true)
                {
                    //WebResponse webResponse = webRequest.GetResponse();
                    try
                    {
                        using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                        {
                            if (webResponse == null)
                            {
                                themessage = Settings.APIGenericErrorMessage;

                                Exception exInner = new Exception("Unable to get a response back for a posting " + 0.ToString() + "\r\nFeedID " + 0.ToString() + "\r\nThe webResponse object is null");
                                throw new Exception(string.Empty, exInner);
                            }

                            intStatusCode = (int)webResponse.StatusCode;
                            strStatus = webResponse.StatusDescription;

                            using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                            {
                                retVal = Json.Decode<DeliveryModels.UpdateDelivery>(sr.ReadToEnd());
                            }
                        }
                    }
                    catch (WebException wex)
                    {
                        HttpWebResponse strGetErrorResponse = ((HttpWebResponse)wex.Response);
                        Stream stream = null;

                        int intCode = 0;
                        string result = string.Empty;
                        string strDescription = string.Empty;

                        try
                        {
                            stream = strGetErrorResponse.GetResponseStream();

                            Encoding encoding = System.Text.Encoding.Default; // System.Text.Encoding.GetEncoding("utf-8");

                            StreamReader streamReader = new StreamReader(stream, encoding);

                            result = streamReader.ReadToEnd();

                            themessage = Settings.APIGenericErrorMessage;

                            intCode = (int)strGetErrorResponse.StatusCode;
                            strDescription = strGetErrorResponse.StatusDescription;

                            // TODO: Log error.
                            //string strErrorMessage = "|ORIGINAL_SOURCE" + wex.Source + " --> Error occurred in " + CLASS_NAME + "." + PROCEDURE_NAME + "|ERRORINFOError reported from: " + CLASS_NAME + "." + PROCEDURE_NAME + "|CLASSNAME" + CLASS_NAME + "|REQEMEDIAID0" + "|REQUISITIONID0|FEEDID" + p_intAutoFeedID.ToString() + "|EMEDIAID" + p_strEmediaID + "|ERRORLEVEL2|WRITESTOPLIGHT1|FILENAME" + p_strFileName + "|PARAMUSED" + p_strParamUsed + "|PARAMTYPE16|PROCESSGUID" + p_strGuid;
                            //ClassErrors clsErr = new ClassErrors(wex.Message + strErrorMessage, wex, wex.Source);

                            /*try
                            {
                                clsData.ReportError(clsErr.ErrorInfo, clsErr.Source, clsErr.Message, clsErr.LineNumber, clsErr.AfErrorNumber, clsErr.ErrorNumber, clsErr.ConnectionString, clsErr.Filename, clsErr.AutoFeedID, clsErr.EmediaID, clsErr.ReqEmediaID.ToString(), clsErr.DeliveryType, clsErr.EmailAddress, clsErr.FTPAddress, clsErr.URLAddress, clsErr.LocalCopyAdddress, clsErr.ParamUsed, clsErr.RequisitionID, clsErr.ReqEmediaID, ClassStaticData.MachineName, clsErr.StoredProcedure, clsErr.ParamType, clsErr.ErrorLevel, clsErr.WriteStopLight, string.Empty, clsErr.UsesSubID, clsErr.SubID, clsErr.ProcessGuid, clsErr.GetStackTrace, clsErr.GetStackFrames, clsErr.InnerExceptionMessage, clsErr.ExceptionType, clsErr.ColumnNumber);
                            }
                            catch (Exception ex2)
                            {
                                ClassStaticData.EmailMessage(ClassStaticData.GetFromEmail, ClassStaticData.GetDevEmail, string.Empty, ClassStaticData.Msg_UnexpectedError, "Original error follows:\r\n\r\nFrom: " + System.Reflection.Assembly.GetExecutingAssembly().ToString() + "\r\nConnectionString: " + ClassStaticData.MASTER_SOURCE + "\r\n\r\n" + "Error Message: " + wex.Message + "\r\n\r\n" + wex.Source + "\r\n\r\nNew error follows:\r\n" + ex2.Message + "\r\n" + ex2.Source + "\r\nNew error inner exception: " + ex2.InnerException + "\r\n" + "\r\n\r\nAdditional information follows:\r\nAutoFeedID: " + clsErr.AutoFeedID.ToString() + "\r\n\r\nEmediaID: " + clsErr.EmediaID + "\r\n\r\nCIDList: " + clsErr.CIDList + "\r\n\r\nErrorTime: " + System.DateTime.Now.ToLongTimeString());
                            }*/
                        }
                        catch (Exception ex2)
                        {
                            themessage = Settings.APIGenericErrorMessage;

                            // TODO: Log error.
                            // throw ex2;
                        }
                    }
                }

                webRequest = null;

                if (intStatusCode > 204)
                {
                    themessage = Settings.APIGenericErrorMessage;

                    // TODO: Log error.

                    /*Exception exInner = new Exception("Unable to post job " + p_intReqEmediaID.ToString() + "\r\nStatusCode " + intStatusCode.ToString() + "\r\nStatus " + strStatus + "\r\nFeedID " + p_intAutoFeedID.ToString() + "\r\nThe site has returned this error " + Result);
                    throw new Exception(string.Empty, exInner);*/
                }

                /*Store result value here*/
            }
            finally
            {
                if (os != null)
                {
                    os.Flush();
                    os.Close();
                    os = null;
                }

                /*if (clsData != null)
                {
                    clsData.Dispose();
                }

                clsData = null;*/
            }

            webRequest = null;

            return retVal;
        }

        public static List<DeactivateModels.Deactivates> GetDeliveries()
        {
            themessage = string.Empty;

            List<DeactivateModels.Deactivates> retVal = null;

            HttpWebRequest webRequest = null;
            int intStatusCode = 0;
            string strStatus = string.Empty;

            webRequest = (HttpWebRequest)WebRequest.Create(Settings.GetDeliveriesEndPoint);

            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "get";

            bool blnRetVal = false;

            Stream os = null;
            try
            {
                blnRetVal = true;

                if (os != null)
                {
                    os.Flush();
                    os.Close();
                    os = null;
                }

                // if blnRetVal is true then sending was successful.
                string Result = string.Empty;

                if (blnRetVal == true)
                {
                    //WebResponse webResponse = webRequest.GetResponse();
                    try
                    {
                        using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                        {
                            if (webResponse == null)
                            {
                                Exception exInner = new Exception("Unable to get a response back for a posting " + 0.ToString() + "\r\nFeedID " + 0.ToString() + "\r\nThe webResponse object is null");
                                throw new Exception(string.Empty, exInner);
                            }

                            intStatusCode = (int)webResponse.StatusCode;
                            strStatus = webResponse.StatusDescription;

                            using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                            {
                                retVal = Json.Decode<List<DeactivateModels.Deactivates>>(sr.ReadToEnd());

                                if (retVal.Count == 0)
                                {
                                    retVal.Add(new DeactivateModels.Deactivates { deliveriesid = "", info = "There are no active deliveries." });
                                }
                            }
                        }
                    }
                    catch (WebException wex)
                    {
                        HttpWebResponse strGetErrorResponse = ((HttpWebResponse)wex.Response);
                        Stream stream = null;

                        int intCode = 0;
                        string result = string.Empty;
                        string strDescription = string.Empty;

                        // Media id 10137 is for some reason erroring here.  ErrorID 602236
                        try
                        {
                            stream = strGetErrorResponse.GetResponseStream();

                            Encoding encoding = System.Text.Encoding.Default; // System.Text.Encoding.GetEncoding("utf-8");

                            StreamReader streamReader = new StreamReader(stream, encoding);

                            result = streamReader.ReadToEnd();

                            intCode = (int)strGetErrorResponse.StatusCode;
                            strDescription = strGetErrorResponse.StatusDescription;

                            themessage = Settings.APIGenericErrorMessage;
                            //string strErrorMessage = "|ORIGINAL_SOURCE" + wex.Source + " --> Error occurred in " + CLASS_NAME + "." + PROCEDURE_NAME + "|ERRORINFOError reported from: " + CLASS_NAME + "." + PROCEDURE_NAME + "|CLASSNAME" + CLASS_NAME + "|REQEMEDIAID0" + "|REQUISITIONID0|FEEDID" + p_intAutoFeedID.ToString() + "|EMEDIAID" + p_strEmediaID + "|ERRORLEVEL2|WRITESTOPLIGHT1|FILENAME" + p_strFileName + "|PARAMUSED" + p_strParamUsed + "|PARAMTYPE16|PROCESSGUID" + p_strGuid;
                            //ClassErrors clsErr = new ClassErrors(wex.Message + strErrorMessage, wex, wex.Source);

                            // TODO: Report or log error.
                            /*try
                            {
                                clsData.ReportError(clsErr.ErrorInfo, clsErr.Source, clsErr.Message, clsErr.LineNumber, clsErr.AfErrorNumber, clsErr.ErrorNumber, clsErr.ConnectionString, clsErr.Filename, clsErr.AutoFeedID, clsErr.EmediaID, clsErr.ReqEmediaID.ToString(), clsErr.DeliveryType, clsErr.EmailAddress, clsErr.FTPAddress, clsErr.URLAddress, clsErr.LocalCopyAdddress, clsErr.ParamUsed, clsErr.RequisitionID, clsErr.ReqEmediaID, ClassStaticData.MachineName, clsErr.StoredProcedure, clsErr.ParamType, clsErr.ErrorLevel, clsErr.WriteStopLight, string.Empty, clsErr.UsesSubID, clsErr.SubID, clsErr.ProcessGuid, clsErr.GetStackTrace, clsErr.GetStackFrames, clsErr.InnerExceptionMessage, clsErr.ExceptionType, clsErr.ColumnNumber);
                            }
                            catch (Exception ex2)
                            {
                                ClassStaticData.EmailMessage(ClassStaticData.GetFromEmail, ClassStaticData.GetDevEmail, string.Empty, ClassStaticData.Msg_UnexpectedError, "Original error follows:\r\n\r\nFrom: " + System.Reflection.Assembly.GetExecutingAssembly().ToString() + "\r\nConnectionString: " + ClassStaticData.MASTER_SOURCE + "\r\n\r\n" + "Error Message: " + wex.Message + "\r\n\r\n" + wex.Source + "\r\n\r\nNew error follows:\r\n" + ex2.Message + "\r\n" + ex2.Source + "\r\nNew error inner exception: " + ex2.InnerException + "\r\n" + "\r\n\r\nAdditional information follows:\r\nAutoFeedID: " + clsErr.AutoFeedID.ToString() + "\r\n\r\nEmediaID: " + clsErr.EmediaID + "\r\n\r\nCIDList: " + clsErr.CIDList + "\r\n\r\nErrorTime: " + System.DateTime.Now.ToLongTimeString());
                            }*/
                        }
                        catch (Exception ex2)
                        {
                            themessage = Settings.APIGenericErrorMessage;

                            // TODO: Report error.
                            // throw ex2;
                        }
                    }
                }

                webRequest = null;

                if (intStatusCode > 204)
                {
                    themessage = Settings.APIGenericErrorMessage;
                    /*Exception exInner = new Exception("Unable to post job " + p_intReqEmediaID.ToString() + "\r\nStatusCode " + intStatusCode.ToString() + "\r\nStatus " + strStatus + "\r\nFeedID " + p_intAutoFeedID.ToString() + "\r\nThe site has returned this error " + Result);
                    throw new Exception(string.Empty, exInner);*/
                }

                /*Store result value here*/
            }
            finally
            {
                if (os != null)
                {
                    os.Flush();
                    os.Close();
                    os = null;
                }

                /*if (clsData != null)
                {
                    clsData.Dispose();
                }

                clsData = null;*/
            }

            webRequest = null;

            return retVal;
        }
    }
}