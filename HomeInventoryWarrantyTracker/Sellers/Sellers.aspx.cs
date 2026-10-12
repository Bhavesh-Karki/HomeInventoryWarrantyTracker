using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace HomeInventoryWarrantyTracker.Sellers
{
    public partial class Sellers : System.Web.UI.Page
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
                LoadSellers();
            }
        }

        private void LoadSellers()
        {
            string query = "SELECT SellerID, SellerName, ContactNumber, Email, Address, CreatedDate FROM Sellers ORDER BY SellerName";
            DataTable dt = DatabaseHelper.GetDataTable(query);
            gvSellers.DataSource = dt;
            gvSellers.DataBind();
        }

        protected void btnAddSeller_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string sellerName = txtSellerName.Text.Trim();
                string contact = txtContactNumber.Text.Trim();
                string email = txtSellerEmail.Text.Trim();
                string address = txtAddress.Text.Trim();

                string insertQuery = @"INSERT INTO Sellers (SellerName, ContactNumber, Email, Address, CreatedDate) 
                                       VALUES (@SellerName, @ContactNumber, @Email, @Address, @CreatedDate)";
                
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@SellerName", sellerName),
                    new SqlParameter("@ContactNumber", string.IsNullOrEmpty(contact) ? (object)DBNull.Value : contact),
                    new SqlParameter("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email),
                    new SqlParameter("@Address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address),
                    new SqlParameter("@CreatedDate", DateTime.Now)
                };

                int rowsAffected = DatabaseHelper.ExecuteNonQuery(insertQuery, parameters);
                if (rowsAffected > 0)
                {
                    ShowMessage("Seller added successfully.", true);
                    txtSellerName.Text = "";
                    txtContactNumber.Text = "";
                    txtSellerEmail.Text = "";
                    txtAddress.Text = "";
                    LoadSellers();
                }
                else
                {
                    ShowMessage("Error adding seller.", false);
                }
            }
        }

        protected void gvSellers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvSellers.EditIndex = e.NewEditIndex;
            LoadSellers();
        }

        protected void gvSellers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvSellers.EditIndex = -1;
            LoadSellers();
        }

        protected void gvSellers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int sellerId = Convert.ToInt32(gvSellers.DataKeys[e.RowIndex].Value);
            string sellerName = ((TextBox)gvSellers.Rows[e.RowIndex].Cells[1].Controls[0]).Text.Trim();
            string contact = ((TextBox)gvSellers.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();
            string email = ((TextBox)gvSellers.Rows[e.RowIndex].Cells[3].Controls[0]).Text.Trim();
            string address = ((TextBox)gvSellers.Rows[e.RowIndex].Cells[4].Controls[0]).Text.Trim();

            if (string.IsNullOrEmpty(sellerName))
            {
                ShowMessage("Seller Name cannot be empty.", false);
                return;
            }

            string updateQuery = @"UPDATE Sellers 
                                   SET SellerName = @SellerName, 
                                       ContactNumber = @ContactNumber, 
                                       Email = @Email, 
                                       Address = @Address 
                                   WHERE SellerID = @SellerID";
            
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@SellerName", sellerName),
                new SqlParameter("@ContactNumber", string.IsNullOrEmpty(contact) ? (object)DBNull.Value : contact),
                new SqlParameter("@Email", string.IsNullOrEmpty(email) ? (object)DBNull.Value : email),
                new SqlParameter("@Address", string.IsNullOrEmpty(address) ? (object)DBNull.Value : address),
                new SqlParameter("@SellerID", sellerId)
            };

            DatabaseHelper.ExecuteNonQuery(updateQuery, parameters);
            
            gvSellers.EditIndex = -1;
            LoadSellers();
            ShowMessage("Seller updated successfully.", true);
        }

        protected void gvSellers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int sellerId = Convert.ToInt32(gvSellers.DataKeys[e.RowIndex].Value);

                string deleteQuery = @"
                    BEGIN TRANSACTION;
                    UPDATE InventoryItems SET SellerID = NULL WHERE SellerID = @SellerID;
                    DELETE FROM Sellers WHERE SellerID = @SellerID;
                    COMMIT TRANSACTION;";

                DatabaseHelper.ExecuteNonQuery(deleteQuery, new SqlParameter("@SellerID", sellerId));
                LoadSellers();
                ShowMessage("Seller deleted successfully.", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting seller: " + ex.Message, false);
            }
        }

        protected void gvSellers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSellers.PageIndex = e.NewPageIndex;
            LoadSellers();
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = isSuccess ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }
    }
}
