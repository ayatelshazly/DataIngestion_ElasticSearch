using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events
{
    public static class EventExtension
    {
        public static Event CreateEvent(this Event evt, object data)
        {
            evt.Data = data;
            return evt;
        }
    }
}
