using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using SWSM.SCM.Interface.Enums;

namespace SWSM.Core.DTO
{
    
    public enum ServiceStateType { Stopped, Running, Paused  } //we are not interested in pending states because we set target state and "wait" for it
    public class ServiceNode
    {
        public ServiceNode()
        { 
           DependsOn = new List<String>();
           DependantServices = new List<string>();
        }
        public List<String> DependsOn { get; set; }        
        public List<String> DependantServices { get; set; }
        public ServiceController? SourceSC { get; set; } //optional, may be null
        public ServiceStartType ServiceStartupType { get; set; }
        public ServiceStateType ServiceStateType { get; set; }
        public String ServiceName { get; set; }
        public String ServiceDisplayName { get; set; }
        public String ServiceCommandLine { get; set; }


        public override string ToString()
        {
            return String.Format("{0} x {1} ({2}) DependsOn: {3} services", ServiceName, ServiceStateType, ServiceStartupType, this.DependsOn.Count); 
        }

    }
}
