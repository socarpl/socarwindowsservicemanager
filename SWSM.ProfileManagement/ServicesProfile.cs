

using SWSM.SCM.Shared.DTO;

namespace SWSM.ProfileManagement
{
    public class ServicesProfile
    {

        public ServicesProfile()
        {
            ServicesTargetState = new List<ServiceNode>();
        }
        public string Name { get; set; }

        public List<ServiceNode> ServicesTargetState { get; set; }
        
    }
}
