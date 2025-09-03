using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWSM.SCM.Interface
{
    public interface IServiceStartTypeProvider
    {
        void GetServiceStartType(String ServiceName);
    }
}
