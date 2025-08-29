using SWSM.Core.DTO;

namespace SWSM.ProfileManagement
{
    public class ServicesProfile
    {

        public ServicesProfile()
        {
            ServicesTargetState = new List<ServiceEntry>();
        }
        public string Name { get; set; }

        public List<ServiceEntry> ServicesTargetState { get; set; }
        
    }
}
