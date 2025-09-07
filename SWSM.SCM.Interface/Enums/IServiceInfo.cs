using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.SCM.Interface.Enums
{
    /// <summary>
    /// Defines methods for retrieving information about services.
    /// </summary>
    public interface IServiceInfo
    {
        /// <summary>
        /// Gets the display name of a service by its service name.
        /// </summary>
        /// <param name="ServiceName">The name of the service.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> containing the display name if found, or an error message if not.
        /// </returns>
        OperationResult GetDisplayName(string ServiceName);

        /// <summary>
        /// Checks whether a service exists by its service name.
        /// </summary>
        /// <param name="ServiceName">The name of the service to check.</param>
        /// <returns>
        /// An <see cref="OperationResult"/> indicating whether the service exists.
        /// </returns>
        OperationResult ServiceExist(string ServiceName);
    }
}
