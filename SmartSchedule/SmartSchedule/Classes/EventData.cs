using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchedule.Classes
{
    public class EventData
    {
        public string eventName { get; set; }
        public string eventType { get; set; }
        public DateTimeOffset ToDate { get; set; }
        public TimeSpan ToTime { get; set; }
        public string cityName { get; set; }
        public string CompleteAddress { get; set; }
        public string EventDescription { get; set; }
       /*
        public string eventName;
        public string eventType;
        public string FromDate;
        public string ToDate;
        public string FromTime;
        public string ToTime;
        public string cityName;
        public string CompleteAddress;
        public string EventDescription;
        * */
    }
}
