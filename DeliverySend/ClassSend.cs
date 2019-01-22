using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using DeliveryGlobalSettings;
using System.Net;
using System.IO;

namespace DeliverySend
{
    public class ClassSend
    {
        public bool HTTPPost(string Url, string Message)
        {
            bool retVal = false;

            HttpWebRequest webRequest = null;
            int intStatusCode = 0;
            string strStatus = string.Empty;

            webRequest = (HttpWebRequest)WebRequest.Create(Url);

            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";

            bool blnRetVal = false;

            Stream os = null;
            try
            {
                byte[] Data = Encoding.UTF8.GetBytes(Message); // "deliveryid=" + Request.Form["InsertDelivery.deliveryId"] + "&from=" + Request.Form["InsertDelivery.from"] + "&to=" + Request.Form["InsertDelivery.to"] + "&message=" + Request.Form["InsertDelivery.message"] + "&active=" + Request.Form["InsertDelivery.active"].Contains("true").ToString());
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
                        retVal = false;

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

                            // string strErrorMessage = "|ORIGINAL_SOURCE" + wex.Source + " --> Error occurred in " + CLASS_NAME + "." + PROCEDURE_NAME + "|ERRORINFOError reported from: " + CLASS_NAME + "." + PROCEDURE_NAME + "|CLASSNAME" + CLASS_NAME + "|REQEMEDIAID0" + "|REQUISITIONID0|FEEDID" + p_intAutoFeedID.ToString() + "|EMEDIAID" + p_strEmediaID + "|ERRORLEVEL2|WRITESTOPLIGHT1|FILENAME" + FileName + "|PARAMUSED" + p_strParamUsed + "|PARAMTYPE16|PROCESSGUID" + Guid;
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
                            throw ex2;
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
            }

            return retVal;
        }

        public bool SendMail(string from, string to, string subject, string message, string username, string password)
        {
            bool retVal = false;

            try
            {
                using (MailMessage MailMsg = new MailMessage())
                {
                    MailMsg.From = new MailAddress(from);
                    MailMsg.To.Add(to);
                    MailMsg.Subject = subject;
                    MailMsg.Body = message;

                    using (SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com"))
                    {
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential(username, password);
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(MailMsg);
                    }
                }
                retVal = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }

            return retVal;
        }

        public bool FTP(string FTPAddress, string UserName, string Password, string FileName, byte[] Data, bool UsePassiveFTP, bool UseFTPES)
        {
            if (!FTPAddress.StartsWith("ftp://"))
            {
                FTPAddress = "ftp://" + FTPAddress;
            }

            if (!FTPAddress.EndsWith("/"))
            {
                FTPAddress += "/";
            }

            string uri = FTPAddress + FileName; // "ftp://internal-ftp1.hodesiq.com/Huntington.xml"; // "ftp://" + ftpServerIP + "/" + fileInf.Name;

            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri)); //(new Uri("ftp://" + ftpServerIP + "/" + fileInf.Name));

            // Provide the WebPermission Credintials
            reqFTP.Credentials = new NetworkCredential(UserName, Password);

            //ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            //SSL Certificate
            if (UseFTPES == true)
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                reqFTP.EnableSsl = true;
                //WebResponse webResponse = reqFTP.GetResponse();
            }

            // By default KeepAlive is true, where the control connection is not closed
            // after a command is executed.
            reqFTP.KeepAlive = false;

            // Specify the command to be executed.
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.Timeout = 3600000;
            reqFTP.UsePassive = UsePassiveFTP;
            
            // Specify the data transfer type.
            reqFTP.UseBinary = true;

            // Notify the server about the size of the uploaded file
            reqFTP.ContentLength = Data.Length; // fileInf.Length;

            Stream strm = null;

            try
            {
                // Stream to which the file to be upload is written
                strm = reqFTP.GetRequestStream();

                Stream strmRdr = new MemoryStream(Data);

                const int buffLength = 2048;
                byte[] buff = new byte[buffLength];

                // Read from the file stream 2kb at a time
                int contentLen = strmRdr.Read(buff, 0, buffLength);
                long intNoOfBuffers = reqFTP.ContentLength / buffLength;
                long intLastBuffer = reqFTP.ContentLength - intNoOfBuffers * buffLength;

                if (intLastBuffer > 0)
                {
                    intNoOfBuffers += 1;
                }

                long intCountBuffers = 0;

                // Till Stream content ends
                while (contentLen != 0)
                {
                    intCountBuffers += 1;
                    // Write Content from the file stream to the FTP Upload Stream
                    strm.Write(buff, 0, contentLen);
                    
                    contentLen = strmRdr.Read(buff, 0, buffLength);
                }

                strmRdr.Close();
                strmRdr.Dispose();
                strmRdr = null;
                //}
            }
            catch (ProtocolViolationException pex)
            {
                try
                {
                    // TODO: Report error.
                }
                catch (Exception ex2)
                {
                    // TODO: Report error.
                }

                return false;
            }
            catch (WebException wex)
            {
                FtpWebResponse strGetErrorResponse = ((FtpWebResponse)wex.Response);
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

                    intCode = (int)strGetErrorResponse.StatusCode;
                    strDescription = strGetErrorResponse.StatusDescription;
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }

                try
                {
                    // TODO: Report error.
                }
                catch (Exception ex2)
                {
                    // TODO: Report error.
                }

                return false;
            }
            catch (Exception ex)
            {
                try
                {
                    // TODO: Report error.
                }
                catch (Exception ex2)
                {
                    throw ex2;
                }

                return false;
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                }
            }

            return true;
        }

        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
