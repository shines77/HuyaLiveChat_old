using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuyaLive
{
    class TimeStamp
    {
        static public uint now()
        {
            DateTime time_1970_01_01 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(time_1970_01_01);
            DateTime nowTime = DateTime.Now;
            uint unixTime = (uint)System.Math.Round((nowTime - startTime).TotalSeconds,
                                                    MidpointRounding.AwayFromZero);
            return unixTime;
        }

        static public long now_ms()
        {
            DateTime time_1970_01_01 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(time_1970_01_01);
            DateTime nowTime = DateTime.Now;
            long unixTime_ms = (long)System.Math.Round((nowTime - startTime).TotalMilliseconds,
                                                       MidpointRounding.AwayFromZero);
            return unixTime_ms;
        }

        static public DateTime toDateTime(long timestamp)
        {
            DateTime time_1970_01_01 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(time_1970_01_01);
            long timestamp_us = timestamp * 10000000;
            TimeSpan elpasedTime = new TimeSpan(timestamp_us);
            DateTime nowTime = startTime.Add(elpasedTime);
            return nowTime;
        }
    }
}
