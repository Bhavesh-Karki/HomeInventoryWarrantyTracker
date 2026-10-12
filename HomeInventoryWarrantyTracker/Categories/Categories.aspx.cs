using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeInventoryWarrantyTracker.Categories
{
    public partial class Categories : System.Web.UI.Page
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
                LoadCategories();
            }
        }

        private void LoadCategories()
        {
            string filter = ddlFilterStatus != null ? ddlFilterStatus.SelectedValue : "ALL";
            string query = "SELECT CategoryID, CategoryName, Description, IsActive FROM Categories ";
            
            if (filter == "1")
            {
                query += "WHERE IsActive = 1 ";
            }
            else if (filter == "0")
            {
                query += "WHERE IsActive = 0 ";
            }
            
            query += "ORDER BY IsActive DESC, CategoryName ASC";
            
            DataTable dt = DatabaseHelper.GetDataTable(query);
            gvCategories.DataSource = dt;
            gvCategories.DataBind();
        }

        protected void ddlFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCategories.PageIndex = 0;
            LoadCategories();
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string categoryName = txtCategoryName.Text.Trim();
                string description = txtDescription.Text.Trim();
                int initialStatus = 1;
                if (ddlAddStatus != null && int.TryParse(ddlAddStatus.SelectedValue, out int st))
                {
                    initialStatus = st;
                }

                // Check uniqueness
                string checkQuery = "SELECT COUNT(1) FROM Categories WHERE CategoryName = @CategoryName";
                int exists = Convert.ToInt32(DatabaseHelper.ExecuteScalar(checkQuery, new SqlParameter("@CategoryName", categoryName)));

                if (exists > 0)
                {
                    ShowMessage("Category already exists.", false);
                    return;
                }

                string insertQuery = "INSERT INTO Categories (CategoryName, Description, IsActive) VALUES (@CategoryName, @Description, @IsActive)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CategoryName", categoryName),
                    new SqlParameter("@Description", description),
                    new SqlParameter("@IsActive", initialStatus)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);
                if (rowsAffected > 0)
                {
                    ShowMessage("Category added successfully.", true);
                    txtCategoryName.Text = "";
                    txtDescription.Text = "";
                    if (ddlAddStatus != null) ddlAddStatus.SelectedValue = "1";
                    LoadCategories();
                }
                else
                {
                    ShowMessage("Error adding category.", false);
                }
            }
        }

        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ToggleStatus")
            {
                int categoryId = Convert.ToInt32(e.CommandArgument);
                
                string checkStatusQuery = "SELECT CategoryName, IsActive FROM Categories WHERE CategoryID = @CategoryID";
                DataTable dt = DatabaseHelper.GetDataTable(checkStatusQuery, new SqlParameter("@CategoryID", categoryId));
                
                if (dt.Rows.Count > 0)
                {
                    string catName = dt.Rows[0]["CategoryName"].ToString();
                    bool currentStatus = Convert.ToBoolean(dt.Rows[0]["IsActive"]);
                    bool newStatus = !currentStatus;
                    
                    string updateQuery = "UPDATE Categories SET IsActive = @NewStatus WHERE CategoryID = @CategoryID";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@NewStatus", newStatus ? 1 : 0),
                        new SqlParameter("@CategoryID", categoryId)
                    };
                    
                    DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
                    LoadCategories();
                    
                    if (newStatus)
                    {
                        ShowMessage($"Category '{catName}' is now ACTIVE (available for new purchases).", true);
                    }
                    else
                    {
                        ShowMessage($"Category '{catName}' is now DISCONTINUED. Existing items are preserved, but this category won't appear when adding new purchases.", true);
                    }
                }
            }
        }

        protected void gvCategories_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList ddlEditStatus = (DropDownList)e.Row.FindControl("ddlEditStatus");
                if (ddlEditStatus != null && DataBinder.Eval(e.Row.DataItem, "IsActive") != null)
                {
                    bool isActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsActive"));
                    ddlEditStatus.SelectedValue = isActive ? "1" : "0";
                }
            }
        }

        protected void gvCategories_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCategories.EditIndex = e.NewEditIndex;
            LoadCategories();
        }

        protected void gvCategories_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCategories.EditIndex = -1;
            LoadCategories();
        }

        protected void gvCategories_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);
            TextBox txtEditCat = (TextBox)gvCategories.Rows[e.RowIndex].FindControl("txtEditCategoryName");
            TextBox txtEditDesc = (TextBox)gvCategories.Rows[e.RowIndex].FindControl("txtEditDescription");
            DropDownList ddlStatus = (DropDownList)gvCategories.Rows[e.RowIndex].FindControl("ddlEditStatus");

            string categoryName = txtEditCat != null ? txtEditCat.Text.Trim() : "";
            string description = txtEditDesc != null ? txtEditDesc.Text.Trim() : "";
            int isActive = ddlStatus != null ? Convert.ToInt32(ddlStatus.SelectedValue) : 1;

            if (string.IsNullOrEmpty(categoryName))
            {
                ShowMessage("Category Name cannot be empty.", false);
                return;
            }

            string updateQuery = "UPDATE Categories SET CategoryName = @CategoryName, Description = @Description, IsActive = @IsActive WHERE CategoryID = @CategoryID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@CategoryName", categoryName),
                new SqlParameter("@Description", description),
                new SqlParameter("@IsActive", isActive),
                new SqlParameter("@CategoryID", categoryId)
            };

            DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
            
            gvCategories.EditIndex = -1;
            LoadCategories();
            ShowMessage("Category updated successfully.", true);
        }

        protected void gvCategories_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int categoryId = Convert.ToInt32(gvCategories.DataKeys[e.RowIndex].Value);

                string deleteQuery = @"
                    BEGIN TRANSACTION;
                    DELETE FROM Warranties WHERE ItemID IN (SELECT ItemID FROM InventoryItems WHERE CategoryID = @CategoryID);
                    DELETE FROM InventoryItems WHERE CategoryID = @CategoryID;
                    DELETE FROM Categories WHERE CategoryID = @CategoryID;
                    COMMIT TRANSACTION;";

                DatabaseHelper.ExecuteNonQuery(deleteQuery, new SqlParameter("@CategoryID", categoryId));
                LoadCategories();
                ShowMessage("Category and associated items deleted successfully.", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting category: " + ex.Message, false);
            }
        }

        protected void gvCategories_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategories.PageIndex = e.NewPageIndex;
            LoadCategories();
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = isSuccess ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }
    }
}
