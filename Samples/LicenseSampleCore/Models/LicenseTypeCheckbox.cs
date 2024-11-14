namespace LicenseSampleCore.Models
{
    public class LicenseTypeCheckbox
    {
        /// <summary>
        /// License label (key of the license) 
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Item is checked in list
        /// </summary>
        public bool Checked { get; set; }
    }
}
