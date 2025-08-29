using Newtonsoft.Json.Bson;
using SWSM.Core;
using SWSM.Core.DTO;

namespace SWSW.Core.Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void GetStartupType()
        {
            var x =  WindowsServicesInfo.GetAllSystemServices(); 
        }

        [TestMethod]
        public void StartServiceTest()
        {
            //HotKeyServiceUWP            
            var resultStart = ServiceManager.ChangeServiceState("WirelessKB850NotificationService", ServiceStateType.Running, new ServiceChangeStateOptions { waitForResult=true, enableIfDisabled=true, targetStartupModeAfterEnabled = StartupMode.Automatic } );
        }

        [TestMethod]
        public void StopServiceTest()
        {
            string serviceName = "WirelessKB850NotificationService";
            var result = ServiceManager.ChangeServiceState(serviceName, ServiceStateType.Stopped, new ServiceChangeStateOptions { waitForResult = true });
        }
    }
}
