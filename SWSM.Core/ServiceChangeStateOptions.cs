using SWSM.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.Core
{
    /// <summary>
    /// Define set of options that are used during ServiceStateChange
    /// </summary>
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
        /// If target service that has to be started is currently Disabled, this property defines what startup mode should be set when enabling the service.
        /// </summary>
        public StartupMode targetStartupModeAfterEnabled { get; set; } = StartupMode.Manual;

        public static ServiceChangeStateOptions? GetDefaultOption()
        {
            return new ServiceChangeStateOptions();
        }
    }
}
