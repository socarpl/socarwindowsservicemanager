
using Microsoft.Win32;

using System.Management;
using System.Security.Authentication.ExtendedProtection;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using SWSM.SCM.Interface.Enums;
using SWSM.SCM.Interface;


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


        private IServiceExecStateProvider ISCMExec;
        private IServiceInfo ISCMInfo;

        /// <summary>
        /// Changes the state of a specified Windows service to the desired state.
        /// </summary>
        /// <param name="serviceName">The short name of the Windows service to change state for.</param>
        /// <param name="newState">The desired state to change the service to. See <see cref="ServiceStateType"/>.</param>
        /// <param name="options">Options controlling how the state change is performed. See <see cref="ServiceChangeStateOptions"/>.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the outcome of the operation.</returns>
        public OperationResult ChangeServiceState(string serviceName, ServiceExecStatus newState, ServiceChangeStateOptions? options)
        {
            _log?.LogInformation("ChangeServiceState called with serviceName: {ServiceName}, newState: {NewState}, options: {@Options}", serviceName, newState, options);

            try
            {
                if (options == null) options = ServiceChangeStateOptions.GetDefaultOption();

                // If name of service is incorrect then failure is reported.
                if (!ISCMInfo.ServiceExist(serviceName).Result<bool>())
                    return OperationResult.Failure($"Service '{serviceName}' does not exist.");

                // Switch based on newState and execute corresponding ISCMExec method
                OperationResult result;
                switch (newState)
                {
                    case ServiceExecStatus.Running:
                        result = ISCMExec.StartService(serviceName);
                        break;
                    case ServiceExecStatus.Stopped:
                        result = ISCMExec.StopService(serviceName);
                        break;
                    case ServiceExecStatus.Paused:
                        result = ISCMExec.PauseService(serviceName);
                        break;
                    default:
                        return OperationResult.Failure("Unsupported service state requested.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _log?.LogError(ex, "Error changing service state for {ServiceName} to {NewState}", serviceName, newState);
                return OperationResult.Failure($"Exception: {ex.GetType().Name} - {ex.Message}");
            }
        }

        public OperationResult ChangeServiceStartMode(string serviceName, SCM.Interface.Enums.ServiceStartMode targetStartupMode)
        {
            if (!ISCMInfo.ServiceExist(serviceName).Result<bool>())
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
                        case SCM.Interface.Enums.ServiceStartMode.Automatic:
                            key.SetValue("Start", 2, RegistryValueKind.DWord);
                            key.DeleteValue("DelayedAutoStart", false);
                            break;
                        case SCM.Interface.Enums.ServiceStartMode.AutomaticDelayed:
                            key.SetValue("Start", 2, RegistryValueKind.DWord);
                            key.SetValue("DelayedAutoStart", 1, RegistryValueKind.DWord);
                            break;
                        case SCM.Interface.Enums.ServiceStartMode.Manual:
                            key.SetValue("Start", 3, RegistryValueKind.DWord);
                            key.DeleteValue("DelayedAutoStart", false);
                            break;
                        case SCM.Interface.Enums.ServiceStartMode.Disabled:
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
