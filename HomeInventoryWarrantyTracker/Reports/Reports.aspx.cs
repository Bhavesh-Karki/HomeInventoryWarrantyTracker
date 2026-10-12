using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace HomeInventoryWarrantyTracker.Reports
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                LoadInventoryStatistics(userId);
                LoadWarrantyStatistics(userId);
                LoadCategoryStatistics(userId);
                LoadLocationStatistics(userId);
            }
        }

        private void LoadInventoryStatistics(int userId)
        {
            // Total Items
            string totalItemsQuery = "SELECT COUNT(*) FROM InventoryItems WHERE UserID = @UserID AND IsDeleted = 0";
            object totalItemsObj = DatabaseHelper.ExecuteScalar(totalItemsQuery, new SqlParameter("@UserID", userId));
            int totalItems = totalItemsObj != DBNull.Value ? Convert.ToInt32(totalItemsObj) : 0;
            lblTotalItems.Text = totalItems.ToString();

            // Total Value
            string totalValueQuery = "SELECT SUM(PurchasePrice) FROM InventoryItems WHERE UserID = @UserID AND IsDeleted = 0";
            object totalValueObj = DatabaseHelper.ExecuteScalar(totalValueQuery, new SqlParameter("@UserID", userId));
            decimal totalValue = totalValueObj != DBNull.Value ? Convert.ToDecimal(totalValueObj) : 0;
            lblTotalValue.Text = string.Format("\u20B9{0:#,##0.00}", totalValue);

            // Average Price
            string avgPriceQuery = "SELECT AVG(PurchasePrice) FROM InventoryItems WHERE UserID = @UserID AND IsDeleted = 0 AND PurchasePrice > 0";
            object avgPriceObj = DatabaseHelper.ExecuteScalar(avgPriceQuery, new SqlParameter("@UserID", userId));
            decimal avgPrice = avgPriceObj != DBNull.Value ? Convert.ToDecimal(avgPriceObj) : 0;
            lblAvgPrice.Text = string.Format("\u20B9{0:#,##0.00}", avgPrice);

            // Top Category
            string topCatQuery = @"
                SELECT TOP 1 c.CategoryName 
                FROM InventoryItems i 
                INNER JOIN Categories c ON i.CategoryID = c.CategoryID 
                WHERE i.UserID = @UserID AND i.IsDeleted = 0 
                GROUP BY c.CategoryName 
                ORDER BY COUNT(*) DESC";
            object topCatObj = DatabaseHelper.ExecuteScalar(topCatQuery, new SqlParameter("@UserID", userId));
            lblTopCategory.Text = topCatObj != null && topCatObj != DBNull.Value ? topCatObj.ToString() : "N/A";
        }

        private void LoadWarrantyStatistics(int userId)
        {
            DateTime today = DateTime.Now.Date;
            DateTime next30 = today.AddDays(30);

            // Active Warranties
            string activeQuery = @"
                SELECT COUNT(*) FROM Warranties w 
                INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                WHERE i.UserID = @UserID AND i.IsDeleted = 0 AND w.ExpiryDate >= @Today";
            int activeCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(activeQuery, 
                new SqlParameter("@UserID", userId), new SqlParameter("@Today", today)));
            lblActiveWarranties.Text = activeCount.ToString();

            // Expiring Warranties
            string expiringQuery = @"
                SELECT COUNT(*) FROM Warranties w 
                INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                WHERE i.UserID = @UserID AND i.IsDeleted = 0 
                AND w.ExpiryDate >= @Today AND w.ExpiryDate <= @Next30";
            int expiringCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(expiringQuery, 
                new SqlParameter("@UserID", userId), new SqlParameter("@Today", today), new SqlParameter("@Next30", next30)));
            lblExpiringWarranties.Text = expiringCount.ToString();

            // Expired Warranties
            string expiredQuery = @"
                SELECT COUNT(*) FROM Warranties w 
                INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                WHERE i.UserID = @UserID AND i.IsDeleted = 0 AND w.ExpiryDate < @Today";
            int expiredCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(expiredQuery, 
                new SqlParameter("@UserID", userId), new SqlParameter("@Today", today)));
            lblExpiredWarranties.Text = expiredCount.ToString();

            // Items Without Warranty
            string noWarrantyQuery = @"
                SELECT COUNT(*) FROM InventoryItems i 
                LEFT JOIN Warranties w ON i.ItemID = w.ItemID 
                WHERE i.UserID = @UserID AND i.IsDeleted = 0 AND w.WarrantyID IS NULL";
            int noWarrantyCount = Convert.ToInt32(DatabaseHelper.ExecuteScalar(noWarrantyQuery, 
                new SqlParameter("@UserID", userId)));
            lblNoWarranty.Text = noWarrantyCount.ToString();
        }

        private void LoadCategoryStatistics(int userId)
        {
            string query = @"
                SELECT c.CategoryName, COUNT(i.ItemID) as ItemCount, ISNULL(SUM(i.PurchasePrice),0) as TotalValue 
                FROM Categories c 
                LEFT JOIN InventoryItems i ON c.CategoryID = i.CategoryID AND i.UserID = @UserID AND i.IsDeleted = 0 
                GROUP BY c.CategoryName 
                ORDER BY ItemCount DESC";

            DataTable dt = DatabaseHelper.GetDataTable(query, new SqlParameter("@UserID", userId));
            gvCategoryStats.DataSource = dt;
            gvCategoryStats.DataBind();
        }

        private void LoadLocationStatistics(int userId)
        {
            string query = @"
                SELECT ISNULL(Location, 'Unspecified') AS Location, COUNT(*) as ItemCount, ISNULL(SUM(PurchasePrice),0) as TotalValue 
                FROM InventoryItems 
                WHERE UserID = @UserID AND IsDeleted = 0 
                GROUP BY Location 
                ORDER BY ItemCount DESC";

            DataTable dt = DatabaseHelper.GetDataTable(query, new SqlParameter("@UserID", userId));
            gvLocationStats.DataSource = dt;
            gvLocationStats.DataBind();
        }
    }
}
