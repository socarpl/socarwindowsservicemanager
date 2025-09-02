using SWSM.SCM.Interface.Enums;

namespace SWSM.SCM.Interface
{
    public interface IServiceExecState
    {

        OperationResult StartService(string ServiceName);
        OperationResult StopService(string ServiceName);

        OperationResult PauseService(string ServiceName);
        OperationResult ContinueService(string ServiceName);

        ServiceExecStatus GetCurrentExecutionState(string ServiceName);  


    }
}
