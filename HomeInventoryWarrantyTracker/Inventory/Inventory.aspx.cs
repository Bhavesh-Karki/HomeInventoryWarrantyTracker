using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace HomeInventoryWarrantyTracker.Inventory
{
    public partial class Inventory : System.Web.UI.Page
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
                ViewState["SortExpression"] = "ProductName";
                ViewState["SortDirection"] = "ASC";
                
                LoadCategoryFilter();
                LoadInventory();
                LoadTreeView();
            }
        }

        private void LoadCategoryFilter()
        {
            string query = "SELECT CategoryID, CategoryName, IsActive FROM Categories ORDER BY CategoryName";
            DataTable dt = DatabaseHelper.GetDataTable(query);
            ddlCategoryFilter.Items.Clear();
            ddlCategoryFilter.Items.Add(new ListItem("All Categories", ""));
            foreach (DataRow row in dt.Rows)
            {
                string text = row["CategoryName"].ToString();
                if (!Convert.ToBoolean(row["IsActive"]))
                {
                    text += " (Discontinued)";
                }
                ddlCategoryFilter.Items.Add(new ListItem(text, row["CategoryID"].ToString()));
            }
        }

        private void LoadInventory()
        {
            int userId = Convert.ToInt32(Session["UserID"]);
            
            string query = @"
                SELECT i.ItemID, i.ProductName, c.CategoryName, i.Brand, 
                       i.PurchasePrice, i.PurchaseDate, i.Location,
                       w.ExpiryDate as WarrantyExpiryDate
                FROM InventoryItems i
                LEFT JOIN Categories c ON i.CategoryID = c.CategoryID
                LEFT JOIN Warranties w ON i.ItemID = w.ItemID
                WHERE i.UserID = @UserID AND i.IsDeleted = 0 ";

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@UserID", userId));

            // Search
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                query += " AND (i.ProductName LIKE @Search OR i.Brand LIKE @Search OR i.SerialNumber LIKE @Search) ";
                parameters.Add(new SqlParameter("@Search", "%" + txtSearch.Text.Trim() + "%"));
            }

            // Filters
            if (!string.IsNullOrEmpty(ddlCategoryFilter.SelectedValue))
            {
                query += " AND i.CategoryID = @CategoryID ";
                parameters.Add(new SqlParameter("@CategoryID", ddlCategoryFilter.SelectedValue));
            }

            if (!string.IsNullOrEmpty(ddlLocationFilter.SelectedValue))
            {
                query += " AND i.Location = @Location ";
                parameters.Add(new SqlParameter("@Location", ddlLocationFilter.SelectedValue));
            }

            // Warranty Filter
            string warrantyFilter = ddlWarrantyFilter.SelectedValue;
            if (warrantyFilter == "Active")
            {
                query += " AND w.ExpiryDate > GETDATE() ";
            }
            else if (warrantyFilter == "Expiring")
            {
                query += " AND w.ExpiryDate > GETDATE() AND w.ExpiryDate <= DATEADD(day, 30, GETDATE()) ";
            }
            else if (warrantyFilter == "Expired")
            {
                query += " AND w.ExpiryDate < GETDATE() ";
            }
            else if (warrantyFilter == "None")
            {
                query += " AND w.WarrantyID IS NULL ";
            }

            // Sorting
            query += $" ORDER BY {ViewState["SortExpression"]} {ViewState["SortDirection"]}";

            DataTable dt = DatabaseHelper.GetDataTable(query, parameters.ToArray());
            gvInventory.DataSource = dt;
            gvInventory.DataBind();
        }

        private void LoadTreeView()
        {
            tvCategories.Nodes.Clear();
            int userId = Convert.ToInt32(Session["UserID"]);

            string catQuery = @"SELECT DISTINCT c.CategoryID, c.CategoryName 
                                FROM Categories c 
                                INNER JOIN InventoryItems i ON c.CategoryID = i.CategoryID 
                                WHERE i.UserID = @UserID AND i.IsDeleted = 0";
            
            DataTable dtCategories = DatabaseHelper.GetDataTable(catQuery, new SqlParameter("@UserID", userId));

            string itemQuery = "SELECT ItemID, CategoryID, ProductName FROM InventoryItems WHERE UserID = @UserID AND IsDeleted = 0";
            DataTable dtItems = DatabaseHelper.GetDataTable(itemQuery, new SqlParameter("@UserID", userId));

            foreach (DataRow catRow in dtCategories.Rows)
            {
                int catId = Convert.ToInt32(catRow["CategoryID"]);
                TreeNode catNode = new TreeNode(catRow["CategoryName"].ToString(), catId.ToString());
                catNode.SelectAction = TreeNodeSelectAction.None;

                DataRow[] items = dtItems.Select($"CategoryID = {catId}");
                foreach (DataRow itemRow in items)
                {
                    TreeNode itemNode = new TreeNode(itemRow["ProductName"].ToString(), itemRow["ItemID"].ToString());
                    itemNode.NavigateUrl = $"~/Inventory/ItemDetails.aspx?id={itemRow["ItemID"]}";
                    catNode.ChildNodes.Add(itemNode);
                }

                tvCategories.Nodes.Add(catNode);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvInventory.PageIndex = 0;
            LoadInventory();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            ddlCategoryFilter.SelectedIndex = 0;
            ddlWarrantyFilter.SelectedIndex = 0;
            ddlLocationFilter.SelectedIndex = 0;
            gvInventory.PageIndex = 0;
            LoadInventory();
        }

        protected void Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvInventory.PageIndex = 0;
            LoadInventory();
        }

        protected void gvInventory_Sorting(object sender, GridViewSortEventArgs e)
        {
            string currentSortDirection = ViewState["SortDirection"].ToString();
            
            if (ViewState["SortExpression"].ToString() == e.SortExpression)
            {
                ViewState["SortDirection"] = currentSortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                ViewState["SortDirection"] = "ASC";
                ViewState["SortExpression"] = e.SortExpression;
            }
            
            LoadInventory();
        }

        protected void gvInventory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvInventory.PageIndex = e.NewPageIndex;
            LoadInventory();
        }

        protected void gvInventory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewItem")
            {
                string itemId = e.CommandArgument.ToString();
                Response.Redirect($"~/Inventory/ItemDetails.aspx?id={itemId}");
            }
        }

        protected void gvInventory_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int itemId = Convert.ToInt32(gvInventory.DataKeys[e.RowIndex].Value);
                int userId = Convert.ToInt32(Session["UserID"]);

                string query = @"
                    BEGIN TRANSACTION;
                    DELETE FROM Warranties WHERE ItemID = @ItemID;
                    DELETE FROM InventoryItems WHERE ItemID = @ItemID AND UserID = @UserID;
                    COMMIT TRANSACTION;";

                DatabaseHelper.ExecuteNonQuery(query, 
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@UserID", userId));

                lblMessage.Text = "Item successfully deleted.";
                LoadInventory();
                LoadTreeView();
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error deleting item: " + ex.Message;
            }
        }

        protected void gvInventory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int itemId = Convert.ToInt32(gvInventory.SelectedDataKey.Value);
            Response.Redirect($"~/Inventory/ItemDetails.aspx?id={itemId}");
        }

        protected string GetWarrantyStatusBadge(object expiryDate)
        {
            if (expiryDate == DBNull.Value || expiryDate == null)
            {
                return "<span class='status-badge status-none'>No Warranty</span>";
            }

            DateTime expiry = Convert.ToDateTime(expiryDate);
            int daysRemaining = (expiry - DateTime.Now).Days;

            if (daysRemaining < 0)
            {
                return "<span class='status-badge status-expired'>Expired</span>";
            }
            else if (daysRemaining <= 30)
            {
                return "<span class='status-badge status-expiring'>Expiring Soon</span>";
            }
            else
            {
                return "<span class='status-badge status-active'>Active</span>";
            }
        }
    }
}
