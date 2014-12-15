using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchedule.Classes
{
    class EventData
    {
        public string eventName;
        public string eventType;
        public DateTimeOffset FromDate;
        public DateTimeOffset ToDate;
        public TimeSpan FromTime;
        public TimeSpan ToTime;
        public string cityName;
        public string CompleteAddress;
        public string EventDescription;
    }
}
