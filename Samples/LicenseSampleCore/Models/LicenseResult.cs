namespace LicenseSampleCore.Models
{
    public class LicenseResult
    {
        /// <summary>
        /// License Key
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Current usage of license
        /// </summary>
        public int CurrentCount { get; set; }

        /// <summary>
        /// Max usage of license
        /// </summary>
        public int MaxCount { get; set; }

        public LicenseResult() { }
        public LicenseResult(string type, int currentCount, int maxCount)
        {
            Type = type;
            CurrentCount = currentCount;
            MaxCount = maxCount;
        }

        public override string ToString()
        {
            return $"{Type} : {CurrentCount} / {MaxCount}";
        }
    }
}
