using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace HomeInventoryWarrantyTracker.Warranty
{
    public partial class ExpiringWarranty : System.Web.UI.Page
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
                LoadExpiringWarranties();
            }
        }

        private void LoadExpiringWarranties()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            DateTime today = DateTime.Now.Date;
            DateTime next30Days = today.AddDays(30);

            string query = @"
                SELECT w.WarrantyID, i.ProductName, w.ProviderName, w.ExpiryDate, w.ContactInformation
                FROM Warranties w 
                INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                WHERE i.UserID = @UserID AND i.IsDeleted = 0 
                AND w.ExpiryDate >= @Today AND w.ExpiryDate <= @Next30 
                ORDER BY w.ExpiryDate ASC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Today", today),
                new SqlParameter("@Next30", next30Days)
            };

            DataTable dt = DatabaseHelper.GetDataTable(query, parameters);
            gvExpiring.DataSource = dt;
            gvExpiring.DataBind();
        }

        protected string GetRemainingDays(object expiryDateObj)
        {
            if (expiryDateObj == DBNull.Value || expiryDateObj == null)
                return "N/A";

            DateTime expiryDate = Convert.ToDateTime(expiryDateObj);
            TimeSpan diff = expiryDate.Date - DateTime.Now.Date;
            int days = (int)diff.TotalDays;

            if (days > 0)
                return $"{days} days";
            else if (days == 0)
                return "Expires today";
            else
                return $"Expired by {Math.Abs(days)} days";
        }
    }
}
