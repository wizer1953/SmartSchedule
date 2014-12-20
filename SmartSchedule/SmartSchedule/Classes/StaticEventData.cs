using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartSchedule.Classes
{
    public class StaticEventData
    {
        public static string eventName { get; set; }
        public static string eventType { get; set; }
        public static DateTimeOffset ToDate { get; set; }
        public static TimeSpan ToTime { get; set; }
        public static string cityName { get; set; }
        public static string cityId { get; set; }
        public static string CompleteAddress { get; set; }
        public static string EventDescription { get; set; }
    }
}
