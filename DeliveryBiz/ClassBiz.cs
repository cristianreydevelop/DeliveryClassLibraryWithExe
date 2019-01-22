using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryData;
using DeliveryGlobalSettings;
using DeliverySend;
using System.Data;
using System.Data.SqlClient;

namespace DeliveryBiz
{
    public class ClassBiz
    {
        private string CLASS_NAME = System.Reflection.Assembly.GetExecutingAssembly().ToString() + "/DeliveryBiz.ClassBiz";

        public bool RunDelivery(int Param)
        {
            ClassData Deliveries = new ClassData();
            ClassSend SendMsg = new ClassSend();

            DataTable dt = Deliveries.GetDeliveries(Param);

            for (int cntr = 0; cntr < dt.Rows.Count; cntr++)
            {
                DataRow dr = dt.Rows[cntr];
                
                if (dr["deliveryname"].ToString() == ClassGlobalSettings.DELIVERY_TYPES.EMAIL)
                {
                    SendMsg.SendMail(dr["from"].ToString(), dr["to"].ToString(), dr["message"].ToString(), dr["message"].ToString(), ClassGlobalSettings.TestEmailUname, ClassGlobalSettings.TestEmailPwd);
                }
                else if ((dr["deliveryname"].ToString() == ClassGlobalSettings.DELIVERY_TYPES.HTTP) || (dr["deliveryname"].ToString() == ClassGlobalSettings.DELIVERY_TYPES.HTTPS))
                {
                    SendMsg.HTTPPost(dr["url"].ToString(), dr["message"].ToString());
                }
                else if (dr["deliveryname"].ToString() == ClassGlobalSettings.DELIVERY_TYPES.FTP)
                {
                    byte[] Data = null;
                    SendMsg.FTP(dr["to"].ToString(), "", "", "", Data, false, false);
                }
            }

            dt.Dispose();
            dt = null;

            return true;
        } // public bool RunDelivery(int Param, bool FromSchedule)
    }
}
