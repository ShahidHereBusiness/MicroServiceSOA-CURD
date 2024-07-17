using System.Data;

namespace MicroServicesSOA.SOA
{
    public static class Diminish
    {
        /// <summary>
        /// Select Tupples From SqlLite
        /// </summary>
        public static DataTable? AppInstance { get; set; }
        public static string? connectionString { get; set; }
        /// <summary>
        /// Database File Tracking
        /// </summary>
        public static string? AppDiminishPath { get; set; }
        /// <summary>
        /// Billion APIs 
        /// </summary>
        public static string? AppCode { get; set; }
        /// <summary>
        /// Table Name
        /// </summary>
        public static string? AppName { get; set; }
        /// <summary>
        /// Record Instance Figure Sprint Wise
        /// </summary>
        public static double? AppTurnaround { get; set; }
        /// <summary>
        /// Logs Instance of API Pipe '|' delimeted
        /// </summary>
        public static string? AppLogs { get; set; }
        /// <summary>
        /// Table Name PostFix
        /// </summary>
        public static string? AppNameSuffix { get; set; }
        /// <summary>
        /// Expiry Time Frame
        /// </summary>
        public static int AccountExpiry { get; set; } = 30;
    }
}