using System;

namespace HomeInventoryWarrantyTracker.App_Code
{
    public class WarrantyModel
    {
        public int WarrantyID { get; set; }
        public int ItemID { get; set; }
        public string WarrantyType { get; set; }
        public string ProviderName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int? WarrantyPeriodMonths { get; set; }
        public string WarrantyTerms { get; set; }
        public string ContactInformation { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
