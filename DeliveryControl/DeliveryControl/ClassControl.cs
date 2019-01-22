using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryBiz;
using DeliveryData;
using System.Data;
using System.Data.SqlClient;

namespace DeliveryControl
{
    public class ClassControl  //: IDisposable
    {
        private string CLASS_NAME = System.Reflection.Assembly.GetExecutingAssembly().ToString() + "/DeliveryControl.ClassControl";

        public bool RunCommandLine(int Param, string RunTime)
        {
            ClassBiz ExecuteProcess = new ClassBiz();

            if (RunTime.Length > 0)
            {
                ClassData data = new ClassData();

                DataTable dt = data.GetSchedule(RunTime);

                for (int cntr = 0; cntr < dt.Rows.Count ; cntr++)
                {
                    DataRow dr = dt.Rows[cntr];

                    // TODO: Implement Asynchronous calling here.
                    ExecuteProcess.RunDelivery(Convert.ToInt32(dr["deliveriesid"]));
                }
            }
            else
                ExecuteProcess.RunDelivery(Param);

            return true;
        }
    }
}
