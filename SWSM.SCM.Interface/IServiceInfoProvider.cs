using SWSM.SCM.Interface.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.SCM.Interface
{
    /// <summary>
    /// Provides methods to retrieve information about a service, such as its execution state and display name.
    /// </summary>
    public interface IServiceInfoProvider
    {
        /// <summary>
        /// Gets the current execution state of the specified service.
        /// </summary>
        /// <param name="ServiceName">The name of the service.</param>
        /// <returns>The current <see cref="ServiceExecStatus"/> of the service.</returns>
        ServiceExecStatus GetCurrentExecutionState(string ServiceName);

        /// <summary>
        /// Gets the display name of the specified service.
        /// </summary>
        /// <param name="ServiceName">The name of the service.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> containing the display name if successful, or an error message if not.
        /// </returns>
        OperationResult GetDisplayName(string ServiceName);
    }
}
