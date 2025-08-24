
using Microsoft.Win32;
using SWSM.Core.DTO;
using System.Management;
using System.Security.Authentication.ExtendedProtection;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;


namespace SWSM.Core
{
    public static class ServicesManager
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
        public static List<ServiceEntry> GetAllSystemServices(bool QueryWMIForCommandLine = false,Action<string, int, int>? ProgressUpdate = null)
        {
            // Get all services on the system
            var allservices = ServiceController.GetServices();
            var ret = new ServiceEntry[allservices.Length];
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
                var ws = new ServiceEntry
                {
                    ServiceStartupType = ServicesManager.GetServiceStartupType(service),
                    ServiceName = service.ServiceName,
                    ServiceDisplayName = service.DisplayName
                };

                // Optionally query WMI for the service command line
                if (QueryWMIForCommandLine)
                    ws.ServiceCommandLine = ServicesManager.GetCommandLine(service.ServiceName);

                // Store the result
                ret[i] = ws;
            });

            // Return the list of service entries
            return ret.ToList();
        }

        /// <summary>
        /// Gets Windows Service command line from WMI
        /// </summary>
        /// <param name="serviceName">The windows service short name</param>
        /// <returns>Command line used to start the service</returns>
        public static string GetCommandLine(string serviceName)
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
            catch {  }

            return "Exception occured during fetching command line";
        }

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

        public static ServiceControllerStatus GetServiceCurrentState(String ServiceName)
        {
            if (!ServiceExist(ServiceName))
                throw new Exception($"Service '{ServiceName}' does not exist.");
            using (ServiceController service = new ServiceController(ServiceName))
            {
                return service.Status;
            }
        }

        public static OperationResult ChangeServiceState(string serviceName, ServiceStateType newState, ServiceChangeStateOptions? options)
        {
            try
            {
                if (options == null) options = new ServiceChangeStateOptions(); //default

                //If for some reason provided state is Undefined there is no action taken 
                if (newState == ServiceStateType.Undefined)
                    return OperationResult.NoAction(serviceName + " cannot be set to undefined state. No action taken.");

                //If name of service is incorrect then failure is reported.
                if (!ServiceExist(serviceName))
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
                            if (GetServiceCurrentStartupMode(serviceName) == StartupMode.Disabled
                                && options.enableIfDisabled)
                            {
                                if (!ChangeStartupMode(serviceName, options.targetModeWhenEnabled).IsSuccess)
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
            if (!ServiceExist(serviceName))
                return OperationResult.Failure($"Service '{serviceName}' does not exist.");

            if (targetStartupMode == GetServiceCurrentStartupMode(serviceName))
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
        public static StartupMode GetServiceCurrentStartupMode(string serviceName)
        {
            try
            {
                using (var service = new ServiceController(serviceName))
                {
                    if (service == null)
                        return StartupMode.Undefined;

                    // Check for delayed auto start
                    if (IsDelayedAutoStart(service.ServiceName))
                        return StartupMode.AutomaticDelayed;

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
                return StartupMode.Undefined;
            }
        }

        public static bool IsTriggerStart(string serviceName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}\TriggerInfo"))
            {
                return key != null;
            }
        }

        public static StartupMode GetServiceStartupType(ServiceController service)
        {

            if (service == null)
                return StartupMode.Undefined;

            if (IsDelayedAutoStart(service.ServiceName))
                return StartupMode.AutomaticDelayed;


            var startMode = service.StartType;
            switch (startMode)
            {
                case ServiceStartMode.Automatic: return StartupMode.Automatic;
                case ServiceStartMode.Manual: return StartupMode.Manual;
                case ServiceStartMode.Disabled: return StartupMode.Disabled;
                default: return StartupMode.Undefined;
            }
          ;
        }

        private static bool IsDelayedAutoStart(string serviceName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey($@"SYSTEM\CurrentControlSet\Services\{serviceName}"))
            {
                object val = key?.GetValue("DelayedAutoStart");
                return val != null && Convert.ToInt32(val) == 1;
            }
        }
    }

}
