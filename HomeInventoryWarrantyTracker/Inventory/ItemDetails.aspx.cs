using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data.SqlClient;
using System.Web.UI;

namespace HomeInventoryWarrantyTracker.Inventory
{
    public partial class ItemDetails : System.Web.UI.Page
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
                if (Request.QueryString["msg"] == "updated")
                {
                    lblMessage.Text = "Item successfully updated.";
                }

                int itemId;
                if (int.TryParse(Request.QueryString["id"], out itemId))
                {
                    LoadItemDetails(itemId, Convert.ToInt32(Session["UserID"]));
                }
                else
                {
                    Response.Redirect("~/Inventory/Inventory.aspx");
                }
            }
        }

        private void LoadItemDetails(int itemId, int userId)
        {
            string query = @"
                SELECT i.*, c.CategoryName, 
                       s.SellerName, s.ContactNumber, s.Email as SellerEmail, s.Address as SellerAddress,
                       w.WarrantyType, w.ProviderName, w.StartDate as WStartDate, w.ExpiryDate as WExpiryDate, 
                       w.WarrantyPeriodMonths, w.WarrantyTerms, w.ContactInformation as WContactInfo
                FROM InventoryItems i
                LEFT JOIN Categories c ON i.CategoryID = c.CategoryID
                LEFT JOIN Sellers s ON i.SellerID = s.SellerID
                LEFT JOIN Warranties w ON i.ItemID = w.ItemID
                WHERE i.ItemID = @ItemID AND i.UserID = @UserID AND i.IsDeleted = 0";

            string connStr = DatabaseHelper.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ItemID", itemId);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string productName = reader["ProductName"].ToString();
                            lblPageTitle.Text = productName;
                            lblProductName.Text = productName;
                            lblCategory.Text = reader["CategoryName"].ToString();
                            lblBrand.Text = reader["Brand"] == DBNull.Value ? "N/A" : reader["Brand"].ToString();
                            lblModelNumber.Text = reader["ModelNumber"] == DBNull.Value ? "N/A" : reader["ModelNumber"].ToString();
                            lblSerialNumber.Text = reader["SerialNumber"] == DBNull.Value ? "N/A" : reader["SerialNumber"].ToString();
                            lblCondition.Text = reader["ItemCondition"] == DBNull.Value ? "N/A" : reader["ItemCondition"].ToString();
                            lblDescription.Text = reader["Description"] == DBNull.Value ? "N/A" : reader["Description"].ToString();
                            
                            if (reader["PurchaseDate"] != DBNull.Value)
                                lblPurchaseDate.Text = Convert.ToDateTime(reader["PurchaseDate"]).ToString("dd MMM yyyy");
                            
                            if (reader["PurchasePrice"] != DBNull.Value)
                                lblPurchasePrice.Text = string.Format("\u20B9{0:N2}", Convert.ToDecimal(reader["PurchasePrice"]));
                                
                            lblLocation.Text = reader["Location"] == DBNull.Value ? "N/A" : reader["Location"].ToString();

                            if (reader["SellerID"] != DBNull.Value)
                            {
                                pnlSellerInfo.Visible = true;
                                lblSellerName.Text = reader["SellerName"].ToString();
                                lblSellerContact.Text = reader["ContactNumber"] == DBNull.Value ? "N/A" : reader["ContactNumber"].ToString();
                                lblSellerEmail.Text = reader["SellerEmail"] == DBNull.Value ? "N/A" : reader["SellerEmail"].ToString();
                                lblSellerAddress.Text = reader["SellerAddress"] == DBNull.Value ? "N/A" : reader["SellerAddress"].ToString();
                            }
                            else
                            {
                                lblNoSeller.Visible = true;
                            }

                            if (reader["WExpiryDate"] != DBNull.Value)
                            {
                                pnlWarrantyInfo.Visible = true;
                                lblWarrantyType.Text = reader["WarrantyType"].ToString();
                                lblWarrantyProvider.Text = reader["ProviderName"] == DBNull.Value ? "N/A" : reader["ProviderName"].ToString();
                                lblWarrantyPeriod.Text = reader["WarrantyPeriodMonths"].ToString();
                                lblWarrantyStart.Text = Convert.ToDateTime(reader["WStartDate"]).ToString("dd MMM yyyy");
                                
                                DateTime expiryDate = Convert.ToDateTime(reader["WExpiryDate"]);
                                lblWarrantyExpiry.Text = expiryDate.ToString("dd MMM yyyy");
                                
                                lblWarrantyContact.Text = reader["WContactInfo"] == DBNull.Value ? "N/A" : reader["WContactInfo"].ToString();
                                lblWarrantyTerms.Text = reader["WarrantyTerms"] == DBNull.Value ? "N/A" : reader["WarrantyTerms"].ToString();
                                
                                int daysRemaining = (expiryDate - DateTime.Now).Days;
                                if (daysRemaining < 0)
                                {
                                    lblWarrantyStatus.Text = "<span class='status-badge status-expired'>Expired (" + Math.Abs(daysRemaining) + " days ago)</span>";
                                }
                                else if (daysRemaining <= 30)
                                {
                                    lblWarrantyStatus.Text = "<span class='status-badge status-expiring'>Expiring Soon (" + daysRemaining + " days left)</span>";
                                }
                                else
                                {
                                    lblWarrantyStatus.Text = "<span class='status-badge status-active'>Active (" + daysRemaining + " days left)</span>";
                                }
                            }
                            else
                            {
                                lblNoWarranty.Visible = true;
                                lblWarrantyStatus.Text = "<span class='status-badge status-none'>No Warranty</span>";
                            }
                        }
                        else
                        {
                            Response.Redirect("~/Inventory/Inventory.aspx");
                        }
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string itemId = Request.QueryString["id"];
            Response.Redirect($"~/Inventory/EditItem.aspx?id={itemId}");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int itemId;
            if (int.TryParse(Request.QueryString["id"], out itemId))
            {
                int userId = Convert.ToInt32(Session["UserID"]);
                string query = @"
                    BEGIN TRANSACTION;
                    DELETE FROM Warranties WHERE ItemID = @ItemID;
                    DELETE FROM InventoryItems WHERE ItemID = @ItemID AND UserID = @UserID;
                    COMMIT TRANSACTION;";
                
                int rows = DatabaseHelper.ExecuteNonQuery(query, 
                    new SqlParameter("@ItemID", itemId),
                    new SqlParameter("@UserID", userId));

                if (rows > 0)
                {
                    Response.Redirect("~/Inventory/Inventory.aspx");
                }
            }
        }
    }
}
