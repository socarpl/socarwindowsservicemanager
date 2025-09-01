using SWSM.SCM.Interface;
using SWSM.SCM.Interface.Enums;
using System.ServiceProcess;

namespace SWSM.SMC.NetFramework
{
    public class ExecutionManagement : IServiceExecState
    {

        private OperationResult SetState(string serviceName, SCMOperation operation)
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    switch (operation)
                    {
                        case SCMOperation.Start:
                            if (sc.Status == ServiceControllerStatus.Running)
                                return OperationResult.Success($"Service '{serviceName}' is already running.");
                            sc.Start();
                            sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                            return OperationResult.Success($"Service '{serviceName}' started successfully.");
                        case SCMOperation.Stop:
                            if (sc.Status == ServiceControllerStatus.Stopped)
                                return OperationResult.Success($"Service '{serviceName}' is already stopped.");
                            sc.Stop();
                            sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                            return OperationResult.Success($"Service '{serviceName}' stopped successfully.");
                        case SCMOperation.Pause:
                            if (sc.CanPauseAndContinue)
                            {
                                if (sc.Status == ServiceControllerStatus.Paused)
                                    return OperationResult.Success($"Service '{serviceName}' is already paused.");
                                sc.Pause();
                                sc.WaitForStatus(ServiceControllerStatus.Paused, TimeSpan.FromSeconds(30));
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
                                    return OperationResult.Success($"Service '{serviceName}' is already running."); // Updated to use Success instead of NoAction
                                sc.Continue();
                                sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
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
    }
}
