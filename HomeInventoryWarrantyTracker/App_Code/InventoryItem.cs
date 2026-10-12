using System;

namespace HomeInventoryWarrantyTracker.App_Code
{
    public class InventoryItem
    {
        public int ItemID { get; set; }
        public int UserID { get; set; }
        public int CategoryID { get; set; }
        public int? SellerID { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string ModelNumber { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public string Location { get; set; }
        public string ItemCondition { get; set; }
        public string Description { get; set; }
        public DateTime? WarrantyStartDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
