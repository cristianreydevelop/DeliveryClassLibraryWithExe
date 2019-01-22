using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryControl;

namespace ExecuteDelivery
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassControl control = new ClassControl();

            control.RunCommandLine(18, "");
        }
    }
}
