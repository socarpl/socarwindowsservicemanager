using SWSM.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.Core
{
    public class ServiceChangeStateOptions
    {

        /// <summary>
        /// If true, the method will wait for the service to reach the target state before returning.   
        /// </summary>
        public bool waitForResult { get; set; } = false;
        /// <summary>
        /// If service startup mode is Disabled, this property defines whether to enable the service.
        /// </summary>
        public bool enableIfDisabled { get; set; } = false;
        /// <summary>
        /// if enableIfDisabled is true, this property defines the startup mode to set for the service. 
        /// </summary>
        public StartupMode targetModeWhenEnabled { get; set; } = StartupMode.Manual;   
    }
}
