using Newtonsoft.Json.Bson;
using SWSM.Core;

using SWSM.SCM.Interface.Enums;

namespace SWSW.Core.Tests
{
    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void GetStartupType()
        {
            var x =  WindowsServicesInfo.GetAllSystemServices(true); 
        }

        [TestMethod]
        public void StartServiceTest()
        {
            //HotKeyServiceUWP
            ServiceManager sm = new ServiceManager();
            var resultStart = sm.ChangeServiceState("WirelessKB850NotificationService", ServiceExecStatus.Running, new ServiceChangeStateOptions { waitForResult=true, enableIfDisabled=true, targetStartupModeAfterEnabled = ServiceStartMode.Automatic } );
        }

        [TestMethod]
        public void StopServiceTest()
        {
            ServiceManager sm = new ServiceManager();
            string serviceName = "WirelessKB850NotificationService";
            var result = sm.ChangeServiceState(serviceName, ServiceExecStatus.Stopped, new ServiceChangeStateOptions { waitForResult = true });
        }

        [TestMethod]
        public void BuildTree()
        {
            WindowsServicesInfo.GetAllSystemServices(false, null);
        }
    }
}
