using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterSubmission.Data
{
    public interface IKubeMQSettings
    {
        string ChannelName { get; set; }
        string ClientID { get; set; }
        string KubeMQServerAddress { get; set; }
    }
    public class KubeMQSettings : IKubeMQSettings
    {
        public string ChannelName { get; set; }
        public string ClientID { get; set; }
        public string KubeMQServerAddress { get; set; }
    }
}
