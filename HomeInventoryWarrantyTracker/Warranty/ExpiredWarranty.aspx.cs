using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;

namespace HomeInventoryWarrantyTracker.Warranty
{
    public partial class ExpiredWarranty : System.Web.UI.Page
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
                LoadExpiredWarranties();
            }
        }

        private void LoadExpiredWarranties()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            DateTime today = DateTime.Now.Date;

            string query = @"
                SELECT w.WarrantyID, i.ProductName, w.ProviderName, w.ExpiryDate, w.ContactInformation
                FROM Warranties w 
                INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                WHERE i.UserID = @UserID AND i.IsDeleted = 0 
                AND w.ExpiryDate < @Today 
                ORDER BY w.ExpiryDate DESC";

            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Today", today)
            };

            DataTable dt = DatabaseHelper.GetDataTable(query, parameters);
            gvExpired.DataSource = dt;
            gvExpired.DataBind();
        }

        protected int GetDaysExpired(object expiryDateObj)
        {
            if (expiryDateObj == DBNull.Value || expiryDateObj == null)
                return 0;

            DateTime expiryDate = Convert.ToDateTime(expiryDateObj);
            TimeSpan diff = DateTime.Now.Date - expiryDate.Date;
            return (int)diff.TotalDays;
        }
    }
}
