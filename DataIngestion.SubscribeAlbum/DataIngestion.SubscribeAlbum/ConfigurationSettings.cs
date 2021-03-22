using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataIngestion.SubscribeAlbum
{
    public class ConfigurationSettings
    {

        public string DaprPort { get; set; }
        public bool EventsOn { get; set; }
        public string BrokerName { get; set; }
    }
}
