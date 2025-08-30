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
    public class WindowsServicesInfo
    {
        /// <summary>
        /// Gets current state of all Windows Services on the system.
        /// </summary>
        /// <param name="QueryWMIForCommandLine">
        /// If true, queries WMI for the command line used to start each service.
        /// </param>
        /// <param name="ProgressUpdate">
        /// Optional callback for progress updates. Invoked as ProgressUpdate(serviceName, currentIndex, totalCount).
        /// </param>
        /// <returns>
        /// List of ServiceEntry objects representing the current Windows Services and their state.
        /// </returns>
        public static List<ServiceNode> GetAllSystemServices(bool QueryWMIForCommandLine = false, Action<string, int, int>? ProgressUpdate = null)
        {
            // Get all services on the system
            var allservices = ServiceController.GetServices();
            var ret = new ServiceNode[allservices.Length];
            int totalProgress = 0;

            // Process each service in parallel for performance
            Parallel.For(0, allservices.Length, i =>
            {
                var service = allservices[i];
                System.Diagnostics.Debug.WriteLine("Getting: " + service.ServiceName);

                // Update progress counter and invoke callback if provided
                Interlocked.Increment(ref totalProgress);
                ProgressUpdate?.Invoke(service.ServiceName, totalProgress, allservices.Length);

                // Create a ServiceEntry for the current service
                var ws = new ServiceNode
                {
                    ServiceStartupType = ServiceInfo.GetServiceStartupType(service),
                    ServiceName = service.ServiceName,
                    ServiceDisplayName = service.DisplayName,
                    SourceSC = service,
                    DependsOn = service.ServicesDependedOn.Select(s => s.ServiceName).ToList(),
                    DependantServices = service.DependentServices.Select(s => s.ServiceName).ToList()
                };

                

                // Optionally query WMI for the service command line
                if (QueryWMIForCommandLine)
                    ws.ServiceCommandLine = ServiceInfo.GetServiceCommandLine(service.ServiceName);

                // Store the result
                ret[i] = ws;
            });


            // Return the list of service entries
            return ret.ToList();
        }

        /// <summary>
        /// Checks if service exist based on short name of the service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool ServiceExist(string serviceName)
        {
            try
            {
                using (ServiceController service = new ServiceController(serviceName))
                {
                    return true; // If we can create a ServiceController, the service exists
                }
            }
            catch (InvalidOperationException)
            {
                return false; // Service does not exist
            }
            catch (Exception ex)
            {
                throw new Exception($"Error checking service existence: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves the current status of a specified Windows service.
        /// </summary>
        /// <param name="ServiceName">The short name of the Windows service.</param>
        /// <returns>The current <see cref="ServiceControllerStatus"/> of the service.</returns>
        /// <exception cref="Exception">Thrown if the specified service does not exist.</exception>
        public static ServiceControllerStatus GetServiceCurrentState(String ServiceName)
        {
            if (!ServiceExist(ServiceName))
                throw new Exception($"Service '{ServiceName}' does not exist.");
            using (ServiceController service = new ServiceController(ServiceName))
            {
                return service.Status;
            }
        }

      



    }
}
