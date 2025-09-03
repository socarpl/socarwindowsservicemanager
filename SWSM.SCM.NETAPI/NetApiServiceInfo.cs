using SWSM.SCM.Interface;
using SWSM.SCM.Interface.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.SCM.NETAPI
{
    public class NetApiServiceInfo : IServiceInfoProvider
    {

        public ServiceExecStatus GetCurrentExecutionState(string ServiceName)
        {
            try
            {
                using (ServiceController sc = new ServiceController(ServiceName))
                {
                    return sc.Status switch
                    {
                        ServiceControllerStatus.Stopped => ServiceExecStatus.Stopped,
                        ServiceControllerStatus.StartPending => ServiceExecStatus.StartPending,
                        ServiceControllerStatus.StopPending => ServiceExecStatus.StopPending,
                        ServiceControllerStatus.Running => ServiceExecStatus.Running,
                        ServiceControllerStatus.ContinuePending => ServiceExecStatus.ContinuePending,
                        ServiceControllerStatus.PausePending => ServiceExecStatus.PausePending,
                        ServiceControllerStatus.Paused => ServiceExecStatus.Paused,
                        _ => throw new InvalidOperationException($"Unknown service status: {sc.Status}")
                    };
                }
            }
            catch (InvalidOperationException)
            {
                // Service not found or cannot be accessed
                return ServiceExecStatus.Stopped;
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // Access denied or service does not exist
                return ServiceExecStatus.Stopped;
            }
        }

        /// <summary>
        /// Retrieves the display name of a Windows service given its service name.
        /// </summary>
        /// <param name="ServiceName">The system name of the service.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> containing the display name in <c>OperationResultData</c> if successful,
        /// or an error message if the operation fails.
        /// </returns>
        public OperationResult GetDisplayName(string ServiceName)
        {
            try
            {
                using (ServiceController sc = new ServiceController(ServiceName))
                {
                    string displayName = sc.DisplayName;
                    var result = OperationResult.Success($"Display name for service '{ServiceName}' retrieved successfully.");
                    result.ResultData = displayName;
                    return result;
                }
            }
            catch (InvalidOperationException ex)
            {
                return OperationResult.Failure($"Failed to get display name for service '{ServiceName}': {ex.Message}");
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                return OperationResult.Failure($"Failed to get display name for service '{ServiceName}': {ex.Message}");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"An unexpected error occurred while getting display name for service '{ServiceName}': {ex.Message}");
            }
        }

    }
}
