using SWSM.SCM.Interface.Enums;

namespace SWSM.SCM.Interface
{
    /// <summary>
    /// Provides methods to control the execution state of a service, such as starting, stopping, pausing, and continuing.
    /// </summary>
    public interface IServiceExecStateProvider
    {
        /// <summary>
        /// Starts the specified service.
        /// </summary>
        /// <param name="ServiceName">The name of the service to start.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the outcome of the operation.</returns>
        OperationResult StartService(string ServiceName);

        /// <summary>
        /// Stops the specified service.
        /// </summary>
        /// <param name="ServiceName">The name of the service to stop.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the outcome of the operation.</returns>
        OperationResult StopService(string ServiceName);

        /// <summary>
        /// Pauses the specified service.
        /// </summary>
        /// <param name="ServiceName">The name of the service to pause.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the outcome of the operation.</returns>
        OperationResult PauseService(string ServiceName);

        /// <summary>
        /// Continues the specified paused service.
        /// </summary>
        /// <param name="ServiceName">The name of the service to continue.</param>
        /// <returns>An <see cref="OperationResult"/> indicating the outcome of the operation.</returns>
        OperationResult ContinueService(string ServiceName);
    }
}
