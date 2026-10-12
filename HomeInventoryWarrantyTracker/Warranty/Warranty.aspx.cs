using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace HomeInventoryWarrantyTracker.Warranty
{
    public partial class Warranty : System.Web.UI.Page
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
                ViewState["SortExpression"] = "ExpiryDate";
                ViewState["SortDirection"] = "ASC";
                LoadWarranties();
            }
        }

        private void LoadWarranties()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            string query = @"
                SELECT w.WarrantyID, i.ProductName, i.Brand, w.WarrantyType, 
                       w.ProviderName, w.StartDate, w.ExpiryDate 
                FROM Warranties w 
                INNER JOIN InventoryItems i ON w.ItemID = i.ItemID 
                WHERE i.UserID = @UserID AND i.IsDeleted = 0";

            DataTable dt = DatabaseHelper.GetDataTable(query, new SqlParameter("@UserID", userId));

            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.Sort = ViewState["SortExpression"].ToString() + " " + ViewState["SortDirection"].ToString();
                gvWarranties.DataSource = dv;
                gvWarranties.DataBind();
            }
        }

        protected void gvWarranties_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortDirection = "ASC";

            if (ViewState["SortExpression"].ToString() == sortExpression)
            {
                sortDirection = ViewState["SortDirection"].ToString() == "ASC" ? "DESC" : "ASC";
            }

            ViewState["SortExpression"] = sortExpression;
            ViewState["SortDirection"] = sortDirection;

            LoadWarranties();
        }

        protected void gvWarranties_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvWarranties.PageIndex = e.NewPageIndex;
            LoadWarranties();
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

        protected string GetWarrantyStatusBadge(object expiryDateObj)
        {
            if (expiryDateObj == DBNull.Value || expiryDateObj == null)
                return "<span class=\"status-badge status-none\">None</span>";

            DateTime expiryDate = Convert.ToDateTime(expiryDateObj);
            TimeSpan diff = expiryDate.Date - DateTime.Now.Date;
            int days = (int)diff.TotalDays;

            if (days < 0)
                return "<span class=\"status-badge status-expired\">Expired</span>";
            else if (days <= 30)
                return "<span class=\"status-badge status-expiring\">Expiring Soon</span>";
            else
                return "<span class=\"status-badge status-active\">Active</span>";
        }
    }
}
