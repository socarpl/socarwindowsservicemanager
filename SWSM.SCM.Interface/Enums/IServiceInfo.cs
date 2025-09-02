using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.SCM.Interface.Enums
{
    public interface IServiceInfo
    {
        OperationResult GetDisplayName(string ServiceName);

        OperationResult ServiceExist(string ServiceName);


    }
}
