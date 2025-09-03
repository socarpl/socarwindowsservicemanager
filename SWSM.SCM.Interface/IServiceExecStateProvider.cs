using SWSM.SCM.Interface.Enums;

namespace SWSM.SCM.Interface
{
    public interface IServiceExecStateProvider
    {

        OperationResult StartService(string ServiceName);
        OperationResult StopService(string ServiceName);

        OperationResult PauseService(string ServiceName);
        OperationResult ContinueService(string ServiceName);

        


    }
}
