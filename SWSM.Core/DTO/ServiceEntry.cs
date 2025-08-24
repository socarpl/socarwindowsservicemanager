using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.Core.DTO
{
    public enum StartupMode { Undefined, AutomaticDelayed, Automatic, Manual, Disabled }
    public enum ServiceStateType { Undefined, Running, Paused, Stopped } //we are not interested in pending states because we set target state and "wait" for it
    public class ServiceEntry
    {
        public StartupMode ServiceStartupType { get; set; }
        public ServiceStateType ServiceStateType { get; set; }
        public String ServiceName { get; set; }
        public String ServiceDisplayName { get; set; }
        public String ServiceCommandLine { get; set; }

        public override string ToString()
        {
            return String.Format("{0} x {1} ({2})", ServiceName, ServiceStateType, ServiceStartupType); 
        }

    }
}
