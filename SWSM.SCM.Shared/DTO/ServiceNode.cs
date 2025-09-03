using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using SWSM.SCM.Interface.Enums;

namespace SWSM.SCM.Shared.DTO
{   
    public class ServiceNode
    {
        public ServiceNode()
        { 
           DependsOn = new List<String>();
           DependantServices = new List<string>();
        }
        public List<String> DependsOn { get; set; }        
        public List<String> DependantServices { get; set; }        
        public ServiceStartMode StartupType { get; set; }
        public ServiceExecStatus StateType { get; set; }
        public String ServiceName { get; set; }
        public String ServiceDisplayName { get; set; }
        public String ServiceCommandLine { get; set; }

        public override string ToString()
        {
            return String.Format("{0} x {1} ({2}) DependsOn: {3} services", ServiceName, StateType, StartupType, this.DependsOn.Count); 
        }

    }
}
