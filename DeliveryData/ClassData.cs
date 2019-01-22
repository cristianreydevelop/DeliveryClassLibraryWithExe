using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DeliveryGlobalSettings;
using DeliveryErrors;

namespace DeliveryData
{
    public class ClassData : IDisposable
    {
        private string CLASS_NAME = System.Reflection.Assembly.GetExecutingAssembly().ToString() + "/DeliveryData.ClassData";

        public DataTable GetSchedule(string RunTime)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cnn = new SqlConnection(ClassGlobalSettings.GetConnString))
                {
                    cnn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cnn;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.getschedules";

                        cmd.Parameters.Add("@time", SqlDbType.VarChar, 8).Value = RunTime;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                ClassErrors Errors = new ClassErrors(null, sqlex);

                // We dont have deliveries id at this point.
                logerror(0, string.Empty, sqlex.Message, Errors.StackTrace);
            }
            catch (Exception ex)
            {
                ClassErrors Errors = new ClassErrors(ex, null);

                // We dont have deliveries id at this point.
                logerror(0, string.Empty, ex.Message, Errors.StackTrace);
            }

            return dt;
        }

        public DataTable GetDeliveries(int DeliveriesId)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection cnn = new SqlConnection(ClassGlobalSettings.GetConnString))
                {
                    cnn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cnn;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.getdeliveryforrun";

                        cmd.Parameters.Add("@deliveriesid", SqlDbType.Int, -1).Value = DeliveriesId;

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                ClassErrors Errors = new ClassErrors(null, sqlex);

                logerror(DeliveriesId, string.Empty, sqlex.Message, Errors.StackTrace);
            }
            catch (Exception ex)
            {
                ClassErrors Errors = new ClassErrors(ex, null);

                logerror(DeliveriesId, string.Empty, ex.Message, Errors.StackTrace);
            }

            return dt;
        }

        public void logerror(int deliveriesid, string deliverytype, string errordescription, string stacktrace)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection(ClassGlobalSettings.GetConnString))
                {
                    cnn.Open();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = cnn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "dbo.logerror";

                        cmd.Parameters.Add("@deliveriesid", SqlDbType.Int, -1).Value = deliveriesid;
                        cmd.Parameters.Add("@deliverytype", SqlDbType.VarChar, 5).Value = deliverytype.Length > 0 ? deliverytype : null;
                        cmd.Parameters.Add("@errdescription", SqlDbType.VarChar, -1).Value = errordescription;
                        cmd.Parameters.Add("@stacktrace", SqlDbType.VarChar, -1).Value = stacktrace;

                        cmd.ExecuteNonQuery();
                    } // using (SqlCommand cmd = new SqlCommand())
                } // using(SqlConnection cnn = new SqlConnection
            } // try
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {

        }
    }
}
