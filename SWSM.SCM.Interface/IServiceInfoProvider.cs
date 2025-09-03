using SWSM.SCM.Interface.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.SCM.Interface
{
    public interface IServiceInfoProvider
    {
        ServiceExecStatus GetCurrentExecutionState(string ServiceName);

        OperationResult GetDisplayName(string ServiceName); 

    }
}
