namespace SWSM.SCM.Interface
{
    public interface IServiceExecutionState
    {
        void StartService(string ServiceName);
        void StopService(string ServiceName);

        void PauseService(string ServiceName);
        void ContinueService(string ServiceName);


    }
}
