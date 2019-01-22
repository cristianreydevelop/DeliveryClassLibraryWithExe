using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
namespace DeliveryErrors
{
    public class ClassErrors
    {
        private string _sTrace = string.Empty;
        private int _cNumber = 0;
        private int _rNumber = 0;

        public int ColNumber
        {
            get
            {
                return ColNumber;
            }

            set
            {
                ColNumber = value;
            }
        }

        public int LineNumber
        {
            get
            {
                return _rNumber;
            }
        }

        public string StackTrace
        {
            get
            {
                return _sTrace;
            }

            set
            {
                _sTrace = value;
            }
        }

        public ClassErrors(Exception Error, SqlException sqlError)
        {
            StackTrace st = null;
            string ExName = null;

            if (sqlError == null)
            {
                st = new StackTrace(Error, true);
                
                ExName = Error.GetType().Name;
            }
            else
            {
                st = new StackTrace(sqlError, true);

                ExName = sqlError.GetType().Name;
            }

            StackFrame[] Frames = null;
            Frames = st.GetFrames();
            _sTrace = st.ToString();
            string[] strFrames = new string[st.FrameCount];
            string[] SplitFrames = null;

            for (int intFrCntr = 0; intFrCntr < st.FrameCount; intFrCntr += 1)
            {
                strFrames[intFrCntr] = Frames[intFrCntr].ToString();
                SplitFrames = strFrames[intFrCntr].Split(':');

                if (_rNumber == 0)
                {
                    _rNumber = Convert.ToInt32(SplitFrames[SplitFrames.Length - 2]);
                    _cNumber = Convert.ToInt32(SplitFrames[SplitFrames.Length - 1]);
                }
            }
        }
    }
}
