namespace NinKode.Common.Utilities
{
    using System;
    using System.Linq;

    /// <summary>
    /// Creates time in the Norwegian timezone
    /// </summary>
    public static class NorwayDateTime
    {
        private const string LinuxTimezone = "Europe/Oslo";
        private const string WindowsTimezone = "W. Europe Standard Time";

        /// <summary>
        /// Norwegian timezone
        /// </summary>
        public static DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, GetTimeZoneInfo());

        /// <summary>
        /// Converts any time to Norwegian timezone
        /// </summary>
        /// <param name="dtg"></param>
        /// <returns></returns>
        public static DateTime Convert(DateTime dtg)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(dtg.ToUniversalTime(), GetTimeZoneInfo());
        }

        /// <summary>
        /// Parse string and convert time to Norwegian timezone
        /// </summary>
        /// <param name="dtg"></param>
        /// <returns></returns>
        public static DateTime Parse(string dtg)
        {
            return Convert(DateTime.Parse(dtg));
        }

        private static TimeZoneInfo GetTimeZoneInfo()
        {
            return TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.Id.Equals(LinuxTimezone)) ??
                   TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.Id.Equals(WindowsTimezone)) ??
                   TimeZoneInfo.Local;
        }
    }
}
