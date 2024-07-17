namespace MicroServicesSOA.SOAV
{
    internal static class Compute
    {
        /// <summary>
        /// Solution Developer:
        /// Difference b/w two DateTime Objects
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        internal static string ComputeTimeSpan(DateTime start, DateTime? end = null)
        {
            end = (end == null) ? DateTime.Now : end;
            return (end - start).ToString() ?? string.Empty;
        }
    }
}
