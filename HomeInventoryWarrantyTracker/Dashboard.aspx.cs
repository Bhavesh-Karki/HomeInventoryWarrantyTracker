using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeInventoryWarrantyTracker
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Auth check
            if (Session["UserID"] == null)
            {
                Response.Redirect("~/Account/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                LoadDashboardStatistics(userId);
                LoadWarrantyAlerts(userId);
                LoadRecentInventory(userId);
                LoadApplicationVisits();
            }
        }

        private void LoadDashboardStatistics(int userId)
        {
            // Total Items
            object totalItems = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM InventoryItems WHERE UserID = @UserID AND IsDeleted = 0",
                new SqlParameter("@UserID", userId));
            lblTotalItems.Text = Convert.ToInt32(totalItems).ToString();

            // Total Value
            object totalValue = DatabaseHelper.ExecuteScalar(
                "SELECT ISNULL(SUM(PurchasePrice), 0) FROM InventoryItems WHERE UserID = @UserID AND IsDeleted = 0",
                new SqlParameter("@UserID", userId));
            lblTotalValue.Text = string.Format("\u20B9{0:N2}", Convert.ToDecimal(totalValue));

            // Active Warranties (expiry > today + 30 days)
            object activeCount = DatabaseHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM Warranties w 
                  INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                  WHERE i.UserID = @UserID AND i.IsDeleted = 0 AND w.ExpiryDate > DATEADD(DAY, 30, GETDATE())",
                new SqlParameter("@UserID", userId));
            lblActiveWarranties.Text = Convert.ToInt32(activeCount).ToString();

            // Expiring Soon (within 30 days)
            object expiringCount = DatabaseHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM Warranties w 
                  INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                  WHERE i.UserID = @UserID AND i.IsDeleted = 0 
                  AND w.ExpiryDate >= CAST(GETDATE() AS DATE) AND w.ExpiryDate <= DATEADD(DAY, 30, GETDATE())",
                new SqlParameter("@UserID", userId));
            lblExpiringSoon.Text = Convert.ToInt32(expiringCount).ToString();

            // Expired
            object expiredCount = DatabaseHelper.ExecuteScalar(
                @"SELECT COUNT(*) FROM Warranties w 
                  INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                  WHERE i.UserID = @UserID AND i.IsDeleted = 0 AND w.ExpiryDate < CAST(GETDATE() AS DATE)",
                new SqlParameter("@UserID", userId));
            lblExpired.Text = Convert.ToInt32(expiredCount).ToString();

            // Categories
            object catCount = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM Categories WHERE IsActive = 1");
            lblCategories.Text = Convert.ToInt32(catCount).ToString();
        }

        private void LoadWarrantyAlerts(int userId)
        {
            DataTable dt = DatabaseHelper.GetDataTable(
                @"SELECT i.ProductName, DATEDIFF(DAY, CAST(GETDATE() AS DATE), w.ExpiryDate) AS RemainingDays
                  FROM Warranties w 
                  INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                  WHERE i.UserID = @UserID AND i.IsDeleted = 0 
                  AND w.ExpiryDate >= CAST(GETDATE() AS DATE) AND w.ExpiryDate <= DATEADD(DAY, 30, GETDATE())
                  ORDER BY w.ExpiryDate ASC",
                new SqlParameter("@UserID", userId));
            
            rptWarrantyAlerts.DataSource = dt;
            rptWarrantyAlerts.DataBind();
            
            pnlNoAlerts.Visible = dt.Rows.Count == 0;
        }

        private void LoadRecentInventory(int userId)
        {
            DataTable dt = DatabaseHelper.GetDataTable(
                @"SELECT TOP 5 i.ItemID, i.ProductName, c.CategoryName, i.Brand, i.PurchaseDate, 
                  i.PurchasePrice, i.WarrantyExpiryDate
                  FROM InventoryItems i 
                  LEFT JOIN Categories c ON i.CategoryID = c.CategoryID 
                  WHERE i.UserID = @UserID AND i.IsDeleted = 0 
                  ORDER BY i.CreatedDate DESC",
                new SqlParameter("@UserID", userId));
            
            gvRecentItems.DataSource = dt;
            gvRecentItems.DataBind();
        }

        private void LoadApplicationVisits()
        {
            if (Application["ApplicationVisits"] != null)
            {
                lblVisitCount.Text = Application["ApplicationVisits"].ToString();
            }
        }

        // Helper method for warranty status in GridView
        protected string GetWarrantyStatus(object warrantyExpiryDate)
        {
            if (warrantyExpiryDate == null || warrantyExpiryDate == DBNull.Value)
                return "<span class='status-badge status-none'>No Warranty</span>";
            
            DateTime expiry = Convert.ToDateTime(warrantyExpiryDate);
            DateTime today = DateTime.Today;
            
            if (expiry < today)
                return "<span class='status-badge status-expired'>Expired</span>";
            else if (expiry <= today.AddDays(30))
                return "<span class='status-badge status-expiring'>Expiring Soon</span>";
            else
                return "<span class='status-badge status-active'>Active</span>";
        }
    }
}
