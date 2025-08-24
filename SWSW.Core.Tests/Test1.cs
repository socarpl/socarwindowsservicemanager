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
            var x =  ServicesManager.GetAllSystemServices(); 
        }

        [TestMethod]
        public void StartServiceTest()
        {
            //HotKeyServiceUWP            
            var resultStart = ServicesManager.ChangeServiceState("WirelessKB850NotificationService", ServiceStateType.Running, new ServiceChangeStateOptions { waitForResult=true, enableIfDisabled=true, targetModeWhenEnabled = StartupMode.Automatic } );
        }

        [TestMethod]
        public void StopServiceTest()
        {
            string serviceName = "WirelessKB850NotificationService";
            var result = ServicesManager.ChangeServiceState(serviceName, ServiceStateType.Stopped, new ServiceChangeStateOptions { waitForResult = true });
        }
    }
}
