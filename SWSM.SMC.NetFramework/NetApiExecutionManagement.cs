using SWSM.SCM.Interface;
using SWSM.SCM.Interface.Enums;
using System.ServiceProcess;

namespace SWSM.SMC.NETAPI
{
    public class NetApiExecutionManagement : IServiceExecState
    {

        public NetApiExecutionManagement() { 
        
        
        }
        public NetApiExecutionManagement(bool waitForStatus, int waitForStatusSeconds)
        {
            WaitForStatus = waitForStatus;
            WaitForStatusSeconds = waitForStatusSeconds;
        }   

        public bool WaitForStatus { get; set; } = true;
        public int WaitForStatusSeconds { get; set; } = 30;

        private OperationResult SetState(string serviceName, SCMOperation operation)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    TimeSpan waitTime = TimeSpan.FromSeconds(WaitForStatusSeconds);
                    switch (operation)
                    {
                        case SCMOperation.Start:
                            if (sc.Status == ServiceControllerStatus.Running)
                                return OperationResult.Success($"Service '{serviceName}' is already running.");
                            sc.Start();
                            if (WaitForStatus)
                                sc.WaitForStatus(ServiceControllerStatus.Running, waitTime);
                            return OperationResult.Success($"Service '{serviceName}' started successfully.");
                        case SCMOperation.Stop:
                            if (sc.Status == ServiceControllerStatus.Stopped)
                                return OperationResult.Success($"Service '{serviceName}' is already stopped.");
                            sc.Stop();
                            if (WaitForStatus)
                                sc.WaitForStatus(ServiceControllerStatus.Stopped, waitTime);
                            return OperationResult.Success($"Service '{serviceName}' stopped successfully.");
                        case SCMOperation.Pause:
                            if (sc.CanPauseAndContinue)
                            {
                                if (sc.Status == ServiceControllerStatus.Paused)
                                    return OperationResult.Success($"Service '{serviceName}' is already paused.");
                                sc.Pause();
                                if (WaitForStatus)
                                    sc.WaitForStatus(ServiceControllerStatus.Paused, waitTime);
                                return OperationResult.Success($"Service '{serviceName}' paused successfully.");
                            }
                            else
                            {
                                return OperationResult.Failure($"Service '{serviceName}' cannot be paused.");
                            }
                        case SCMOperation.Continue:
                            if (sc.CanPauseAndContinue)
                            {
                                if (sc.Status == ServiceControllerStatus.Running)
                                    return OperationResult.Success($"Service '{serviceName}' is already running.");
                                sc.Continue();
                                if (WaitForStatus)
                                    sc.WaitForStatus(ServiceControllerStatus.Running, waitTime);
                                return OperationResult.Success($"Service '{serviceName}' continued successfully.");
                            }
                            else
                            {
                                return OperationResult.Failure($"Service '{serviceName}' cannot be continued.");
                            }
                        default:
                            return OperationResult.Failure("Unknown operation.");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                return OperationResult.Failure($"Failed to perform '{operation}' on service '{serviceName}': {ex.Message}");
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                return OperationResult.Failure($"Failed to perform '{operation}' on service '{serviceName}': {ex.Message}");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"An unexpected error occurred while performing '{operation}' on service '{serviceName}': {ex.Message}");
            }
        }

        

        public OperationResult StartService(string serviceName)
        {
           return this.SetState(serviceName, SCMOperation.Start);
        }

        public OperationResult StopService(string serviceName)
        {
            return this.SetState(serviceName, SCMOperation.Stop);
        }

        public OperationResult PauseService(string serviceName)
        {
            return this.SetState(serviceName, SCMOperation.Pause);
        }

        public OperationResult ContinueService(string serviceName)
        {
            return this.SetState(serviceName, SCMOperation.Continue);
        }

        public ServiceExecStatus GetCurrentExecutionState(string serviceName)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
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
        /// <param name="serviceName">The system name of the service.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> containing the display name in <c>OperationResultData</c> if successful,
        /// or an error message if the operation fails.
        /// </returns>
        public OperationResult GetDisplayName(string serviceName)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    string displayName = sc.DisplayName;
                    var result = OperationResult.Success($"Display name for service '{serviceName}' retrieved successfully.");
                    result.ResultData = displayName;
                    return result;
                }
            }
            catch (InvalidOperationException ex)
            {
                return OperationResult.Failure($"Failed to get display name for service '{serviceName}': {ex.Message}");
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                return OperationResult.Failure($"Failed to get display name for service '{serviceName}': {ex.Message}");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"An unexpected error occurred while getting display name for service '{serviceName}': {ex.Message}");
            }
        }
    }
}
