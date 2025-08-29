using Microsoft.Win32;
using SWSM.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.Core
{
    public class ServiceInfo
    {
        /// <summary>
        /// Does given service start is triggered by something?
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static bool IsServiceStarteByTrigger(string serviceName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}\TriggerInfo"))
            {
                return key != null;
            }
        }

        /// <summary>
        /// Gets the startup mode of a Windows service using a ServiceController instance.
        /// </summary>
        /// <param name="service">The ServiceController instance representing the service.</param>
        /// <returns>
        /// The <see cref="StartupMode"/> representing the startup mode of the service.
        /// Returns <see cref="StartupMode.Undefined"/> if the service is null.
        /// </returns>
        public static StartupMode GetServiceStartupType(ServiceController service)
        {
            if (service == null)
                return StartupMode.Undefined;

            // Check if the service is set to delayed automatic start
            if (IsDelayedAutoStart(service.ServiceName))
                return StartupMode.AutomaticDelayed;

            // Determine the startup mode based on the service's StartType
            var startMode = service.StartType;
            switch (startMode)
            {
                case ServiceStartMode.Automatic: return StartupMode.Automatic;
                case ServiceStartMode.Manual: return StartupMode.Manual;
                case ServiceStartMode.Disabled: return StartupMode.Disabled;
                default: return StartupMode.Undefined;
            }
        }

        /// <summary>
        /// Checks if the specified service is configured for delayed automatic start.
        /// </summary>
        /// <param name="serviceName">The short name of the Windows service.</param>
        /// <returns>
        /// True if the service is set to delayed automatic start; otherwise, false.
        /// </returns>
        private static bool IsDelayedAutoStart(string serviceName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}"))
            {
                object val = key?.GetValue("DelayedAutoStart");
                return val != null && Convert.ToInt32(val) == 1;
            }
        }

        /// <summary>
        /// Gets Windows Service command line from WMI
        /// </summary>
        /// <param name="serviceName">The windows service short name</param>
        /// <returns>Command line used to start the service</returns>
        public static string GetServiceCommandLine(string serviceName)
        {
            try
            {
                string query = $"SELECT * FROM Win32_Service WHERE Name = '{serviceName}'";

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                using (ManagementObjectCollection results = searcher.Get())
                {
                    var serviceMO = results.Cast<ManagementObject>().FirstOrDefault();
                    return serviceMO?["PathName"]?.ToString();
                }
            }
            catch { }

            return "Exception occured during fetching command line";
        }

        /// <summary>
        /// Gets the current startup mode of a Windows service by its name.
        /// </summary>
        /// <param name="serviceName">The short name of the Windows service.</param>
        /// <returns>
        /// The <see cref="StartupMode"/> representing the current startup mode of the service.
        /// Returns <see cref="StartupMode.Undefined"/> if the service cannot be found or an error occurs.
        /// </returns>
        public static StartupMode GetServiceCurrentStartupMode(string serviceName)
        {
            try
            {
                // Create a ServiceController instance for the specified service
                using (var service = new ServiceController(serviceName))
                {
                    if (service == null)
                        return StartupMode.Undefined;

                    // Check if the service is set to delayed automatic start
                    if (IsDelayedAutoStart(service.ServiceName))
                        return StartupMode.AutomaticDelayed;

                    // Determine the startup mode based on the service's StartType
                    switch (service.StartType)
                    {
                        case ServiceStartMode.Automatic:
                            return StartupMode.Automatic;
                        case ServiceStartMode.Manual:
                            return StartupMode.Manual;
                        case ServiceStartMode.Disabled:
                            return StartupMode.Disabled;
                        default:
                            return StartupMode.Undefined;
                    }
                }
            }
            catch
            {
                // Return Undefined if any exception occurs (e.g., service not found)
                return StartupMode.Undefined;
            }
        }
    }
}
