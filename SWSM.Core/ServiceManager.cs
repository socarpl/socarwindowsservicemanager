
using Microsoft.Win32;
using SWSM.Core.DTO;
using System.Management;
using System.Security.Authentication.ExtendedProtection;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;


namespace SWSM.Core
{
    public class ServiceManager
    {

        public ServiceManager() { }

        public ServiceManager(ILogger<ServiceManager> log)
        {
            _log = log;
        }
    
        private ILogger _log;
     

        /// <summary>
        /// Changes the state of a specified Windows service to the desired state.
        /// </summary>
        /// <param name="serviceName">The short name of the Windows service to change state for.</param>
        /// <param name="newState">The desired state to change the service to. See <see cref="ServiceStateType"/>.</param>
        /// <param name="options">Options controlling how the state change is performed. See <see cref="ServiceChangeStateOptions"/>.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the outcome of the operation.</returns>
        public OperationResult ChangeServiceState(string serviceName, ServiceStateType newState, ServiceChangeStateOptions? options)
        {
            _log?.LogInformation("ChangeServiceState called with serviceName: {ServiceName}, newState: {NewState}, options: {@Options}", serviceName, newState, options);

            try
            {
                if (options == null) options = ServiceChangeStateOptions.GetDefaultOption(); //default

                //If for some reason provided state is Undefined there is no action taken 
                if (newState == ServiceStateType.Undefined)
                    return OperationResult.Failure(serviceName + " cannot be set to undefined state. No action taken.");

                //If name of service is incorrect then failure is reported.
                if (!WindowsServicesInfo.ServiceExist(serviceName))
                    return OperationResult.Failure($"Service '{serviceName}' does not exist.");

                // If target state is same as current or service is in transition from one state to another (pendingXYZ) no action should be taken.
                #region Status matching 
                using (ServiceController service = new ServiceController(serviceName))
                {
                    _log?.LogInformation("Current status of service {ServiceName} is {ServiceStatus}", serviceName, service.Status);
                    switch (service.Status)
                    {
                        case ServiceControllerStatus.StartPending:
                        case ServiceControllerStatus.ContinuePending:
                        case ServiceControllerStatus.PausePending:
                        case ServiceControllerStatus.StopPending:
                            return OperationResult.Failure($"Service '{serviceName}' is already in a pending state. Cannot change state to {newState}.");

                        case ServiceControllerStatus.Running when newState == ServiceStateType.Running:
                            return OperationResult.Success($"Service '{serviceName}' is already running. No action taken.");

                        case ServiceControllerStatus.Paused when newState == ServiceStateType.Paused:
                            return OperationResult.Success($"Service '{serviceName}' is already paused. No action taken.");

                        case ServiceControllerStatus.Stopped when newState == ServiceStateType.Stopped:
                            return OperationResult.Success($"Service '{serviceName}' is already stopped. No action taken.");

                        case ServiceControllerStatus.Running:
                        case ServiceControllerStatus.Paused:
                        case ServiceControllerStatus.Stopped:
                            break; // Continue to state change logic

                        default:
                            throw new Exception("This should not be thrown ever as this code should not be reachable");
                    }
                    #endregion

                    // Execute state change
                    #region status state change 
                    switch (newState)
                    {
                        case ServiceStateType.Running:
                            if (ServiceInfo.GetServiceCurrentStartupMode(serviceName) == StartupMode.Disabled && options.enableIfDisabled)
                            {
                                OperationResult opStatus = ChangeStartupMode(serviceName, options.targetStartupModeAfterEnabled);
                                if (opStatus.IsFailure)
                                    return OperationResult.Failure($"Failed to change startup mode for service '{serviceName}' before starting it.");
                                service.Refresh();
                            }
                            service.Start();

                            if (options.waitForResult) service.WaitForStatus(ServiceControllerStatus.Running);
                            break;
                        case ServiceStateType.Stopped:
                            service.Stop();
                            if (options.waitForResult) service.WaitForStatus(ServiceControllerStatus.Stopped);
                            break;
                        case ServiceStateType.Paused:
                            service.Pause();
                            if (options.waitForResult) service.WaitForStatus(ServiceControllerStatus.Paused);
                            break;
                        default:
                            throw new Exception("Unreachable code reached.");

                    }
                    #endregion

                    return OperationResult.Success("State changed successfuly");
                }
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "Error changing service state for {ServiceName} to {NewState}", serviceName, newState);
                return OperationResult.Failure(ex.Message);
            }
        }

        public OperationResult ChangeStartupMode(string serviceName, StartupMode targetStartupMode)
        {
            if (!WindowsServicesInfo.ServiceExist(serviceName))
                return OperationResult.Failure($"Service '{serviceName}' does not exist.");

            if (targetStartupMode == ServiceInfo.GetServiceCurrentStartupMode(serviceName))
            {
                return OperationResult.Success($"Service '{serviceName}' is already set to {targetStartupMode} startup mode. No action taken.");
            }

            try
            {
                string registryPath = $@"SYSTEM\CurrentControlSet\Services\{serviceName}";
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryPath, writable: true))
                {
                    if (key == null)
                        return OperationResult.Failure($"Could not open registry key for service '{serviceName}'.");

                    switch (targetStartupMode)
                    {
                        case StartupMode.Automatic:
                            key.SetValue("Start", 2, RegistryValueKind.DWord);
                            key.DeleteValue("DelayedAutoStart", false);
                            break;
                        case StartupMode.AutomaticDelayed:
                            key.SetValue("Start", 2, RegistryValueKind.DWord);
                            key.SetValue("DelayedAutoStart", 1, RegistryValueKind.DWord);
                            break;
                        case StartupMode.Manual:
                            key.SetValue("Start", 3, RegistryValueKind.DWord);
                            key.DeleteValue("DelayedAutoStart", false);
                            break;
                        case StartupMode.Disabled:
                            key.SetValue("Start", 4, RegistryValueKind.DWord);
                            key.DeleteValue("DelayedAutoStart", false);
                            break;
                        default:
                            return OperationResult.Failure("Unsupported startup mode.");
                    }
                }
                return OperationResult.Success("State changed successfuly");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Failed to change startup mode: {ex.Message}");
            }
        }
       


     

    }

}
