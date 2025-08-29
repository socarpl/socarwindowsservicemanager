
using Microsoft.Win32;
using SWSM.Core.DTO;
using System.Management;
using System.Security.Authentication.ExtendedProtection;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;


namespace SWSM.Core
{
    public static class ServiceManager
    {


        /// <summary>
        /// Changes the state of a specified Windows service to the desired state.
        /// </summary>
        /// <param name="serviceName">The short name of the Windows service to change state for.</param>
        /// <param name="newState">The desired state to change the service to. See <see cref="ServiceStateType"/>.</param>
        /// <param name="options">Options controlling how the state change is performed. See <see cref="ServiceChangeStateOptions"/>.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the outcome of the operation.</returns>
        public static OperationResult ChangeServiceState(string serviceName, ServiceStateType newState, ServiceChangeStateOptions? options)
        {
            try
            {
                if (options == null) options = ServiceChangeStateOptions.GetDefaultOption(); //default

                //If for some reason provided state is Undefined there is no action taken 
                if (newState == ServiceStateType.Undefined)
                    return OperationResult.NoAction(serviceName + " cannot be set to undefined state. No action taken.");

                //If name of service is incorrect then failure is reported.
                if (!WindowsServicesInfo.ServiceExist(serviceName))
                    return OperationResult.Failure($"Service '{serviceName}' does not exist.");

                // If target state is same as current or service is in transition from one state to another (pendingXYZ) no action should be taken.
                #region Status matching 
                using (ServiceController service = new ServiceController(serviceName))
                {



                    switch (service.Status)
                    {
                        case ServiceControllerStatus.StartPending:
                        case ServiceControllerStatus.ContinuePending:
                        case ServiceControllerStatus.PausePending:
                        case ServiceControllerStatus.StopPending:
                            return OperationResult.NoAction($"Service '{serviceName}' is already in a pending state. Cannot change state to {newState}.");

                        case ServiceControllerStatus.Running:
                            {
                                if (newState == ServiceStateType.Running)
                                    return OperationResult.NoAction($"Service '{serviceName}' is already running. No action taken.");
                            }
                            break;

                        case ServiceControllerStatus.Paused:
                            {
                                if (newState == ServiceStateType.Paused)
                                    return OperationResult.NoAction($"Service '{serviceName}' is already paused. No action taken.");
                            }
                            break;

                        case ServiceControllerStatus.Stopped:
                            {
                                if (newState == ServiceStateType.Stopped)
                                    return OperationResult.NoAction($"Service '{serviceName}' is already stopped. No action taken.");
                            }
                            break;
                        default: throw new Exception("This should not be thrown ever as this code should not be reachable");
                    }
                    #endregion

                // Execute state change
                #region status state change 
                switch (newState)
                {
                    case ServiceStateType.Running:
                        if (ServiceInfo.GetServiceCurrentStartupMode(serviceName) == StartupMode.Disabled && options.enableIfDisabled)
                        {
                            if (!ChangeStartupMode(serviceName, options.targetStartupModeAfterEnabled).IsSuccess)
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

                return OperationResult.Success;    
                }
            }
            catch (Exception ex)
            {
                return OperationResult.Failure(ex.Message);
            }
        }

        public static OperationResult ChangeStartupMode(string serviceName, StartupMode targetStartupMode)
        {
            if (!WindowsServicesInfo.ServiceExist(serviceName))
                return OperationResult.Failure($"Service '{serviceName}' does not exist.");

            if (targetStartupMode == ServiceInfo.GetServiceCurrentStartupMode(serviceName))
            {
                return OperationResult.NoAction($"Service '{serviceName}' is already set to {targetStartupMode} startup mode. No action taken.");
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
                return OperationResult.Success;
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Failed to change startup mode: {ex.Message}");
            }
        }
       


     

    }

}
