using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeInventoryWarrantyTracker.Inventory
{
    public partial class EditItem : System.Web.UI.Page
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
                int itemId;
                if (!int.TryParse(Request.QueryString["id"], out itemId))
                {
                    Response.Redirect("~/Inventory/Inventory.aspx");
                    return;
                }
                
                InitializeCalendarYearDropDowns();
                LoadCategories();
                LoadSellers();
                LoadItemData(itemId, Convert.ToInt32(Session["UserID"]));
            }
        }

        private void InitializeCalendarYearDropDowns()
        {
            int currentYear = DateTime.Now.Year;
            for (int y = currentYear - 10; y <= currentYear + 10; y++)
            {
                string yr = y.ToString();
                ddlPurchaseYear.Items.Add(new ListItem(yr, yr));
                ddlWarrantyStartYear.Items.Add(new ListItem(yr, yr));
                ddlWarrantyExpiryYear.Items.Add(new ListItem(yr, yr));
            }

            ddlPurchaseYear.SelectedValue = currentYear.ToString();
            ddlPurchaseMonth.SelectedValue = DateTime.Now.Month.ToString();
            ddlWarrantyStartYear.SelectedValue = currentYear.ToString();
            ddlWarrantyStartMonth.SelectedValue = DateTime.Now.Month.ToString();
            ddlWarrantyExpiryYear.SelectedValue = (currentYear + 1).ToString();
            ddlWarrantyExpiryMonth.SelectedValue = DateTime.Now.Month.ToString();
        }

        private void LoadCategories()
        {
            string query = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
            DataTable dt = DatabaseHelper.GetDataTable(query);
            ddlCategory.DataSource = dt;
            ddlCategory.DataTextField = "CategoryName";
            ddlCategory.DataValueField = "CategoryID";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("-- Select Category --", ""));
        }

        private void LoadSellers()
        {
            string query = "SELECT SellerID, SellerName FROM Sellers ORDER BY SellerName";
            DataTable dt = DatabaseHelper.GetDataTable(query);
            ddlSeller.DataSource = dt;
            ddlSeller.DataTextField = "SellerName";
            ddlSeller.DataValueField = "SellerID";
            ddlSeller.DataBind();
            ddlSeller.Items.Insert(0, new ListItem("-- Select Seller --", ""));
            ddlSeller.Items.Add(new ListItem("Custom (Type Manually)", "CUSTOM"));
        }

        private void LoadItemData(int itemId, int userId)
        {
            string query = @"
                SELECT i.*, w.WarrantyID, w.WarrantyType, w.ProviderName, 
                       w.StartDate as WStartDate, w.ExpiryDate as WExpiryDate, 
                       w.WarrantyPeriodMonths, w.WarrantyTerms, w.ContactInformation
                FROM InventoryItems i
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
                            txtProductName.Text = reader["ProductName"].ToString();
                            if (reader["CategoryID"] != DBNull.Value)
                            {
                                string catId = reader["CategoryID"].ToString();
                                if (ddlCategory.Items.FindByValue(catId) == null)
                                {
                                    string catNameQuery = "SELECT CategoryName FROM Categories WHERE CategoryID = @CatID";
                                    object cName = DatabaseHelper.ExecuteScalar(catNameQuery, new SqlParameter("@CatID", catId));
                                    if (cName != null)
                                    {
                                        ddlCategory.Items.Add(new ListItem(cName.ToString() + " (Discontinued)", catId));
                                    }
                                }
                                ddlCategory.SelectedValue = catId;
                            }
                            txtBrand.Text = reader["Brand"].ToString();
                            txtModelNumber.Text = reader["ModelNumber"].ToString();
                            txtSerialNumber.Text = reader["SerialNumber"].ToString();
                            txtDescription.Text = reader["Description"].ToString();
                            
                            if (reader["PurchaseDate"] != DBNull.Value)
                            {
                                DateTime pDate = Convert.ToDateTime(reader["PurchaseDate"]);
                                txtPurchaseDate.Text = pDate.ToString("dd/MM/yyyy");
                                calPurchaseDate.SelectedDate = pDate;
                                calPurchaseDate.VisibleDate = pDate;
                                ddlPurchaseMonth.SelectedValue = pDate.Month.ToString();
                                if (ddlPurchaseYear.Items.FindByValue(pDate.Year.ToString()) != null)
                                    ddlPurchaseYear.SelectedValue = pDate.Year.ToString();
                            }
                            
                            txtPurchasePrice.Text = reader["PurchasePrice"].ToString();
                            
                            if (reader["SellerID"] != DBNull.Value)
                            {
                                string sId = reader["SellerID"].ToString();
                                if (ddlSeller.Items.FindByValue(sId) != null)
                                    ddlSeller.SelectedValue = sId;
                            }
                                
                            if (reader["Location"] != DBNull.Value)
                                ddlLocation.SelectedValue = reader["Location"].ToString();
                                
                            if (reader["ItemCondition"] != DBNull.Value)
                                rblCondition.SelectedValue = reader["ItemCondition"].ToString();

                            if (reader["WarrantyID"] != DBNull.Value)
                            {
                                chkHasWarranty.Checked = true;
                                pnlWarranty.Visible = true;
                                
                                string wType = reader["WarrantyType"].ToString();
                                if (ddlWarrantyType.Items.FindByValue(wType) != null)
                                {
                                    ddlWarrantyType.SelectedValue = wType;
                                }
                                else if (!string.IsNullOrEmpty(wType))
                                {
                                    ddlWarrantyType.SelectedValue = "CUSTOM";
                                    txtCustomWarrantyType.Text = wType;
                                }

                                txtProvider.Text = reader["ProviderName"].ToString();
                                
                                if (reader["WStartDate"] != DBNull.Value)
                                {
                                    DateTime wStart = Convert.ToDateTime(reader["WStartDate"]);
                                    txtWarrantyStart.Text = wStart.ToString("dd/MM/yyyy");
                                    calWarrantyStart.SelectedDate = wStart;
                                    calWarrantyStart.VisibleDate = wStart;
                                    ddlWarrantyStartMonth.SelectedValue = wStart.Month.ToString();
                                    if (ddlWarrantyStartYear.Items.FindByValue(wStart.Year.ToString()) != null)
                                        ddlWarrantyStartYear.SelectedValue = wStart.Year.ToString();
                                }
                                
                                if (reader["WExpiryDate"] != DBNull.Value)
                                {
                                    DateTime wExpiry = Convert.ToDateTime(reader["WExpiryDate"]);
                                    txtWarrantyExpiry.Text = wExpiry.ToString("dd/MM/yyyy");
                                    calWarrantyExpiry.SelectedDate = wExpiry;
                                    calWarrantyExpiry.VisibleDate = wExpiry;
                                    ddlWarrantyExpiryMonth.SelectedValue = wExpiry.Month.ToString();
                                    if (ddlWarrantyExpiryYear.Items.FindByValue(wExpiry.Year.ToString()) != null)
                                        ddlWarrantyExpiryYear.SelectedValue = wExpiry.Year.ToString();
                                }
                                
                                if (reader["WarrantyPeriodMonths"] != DBNull.Value)
                                {
                                    string pMonths = reader["WarrantyPeriodMonths"].ToString();
                                    if (ddlWarrantyPeriod.Items.FindByValue(pMonths) != null)
                                    {
                                        ddlWarrantyPeriod.SelectedValue = pMonths;
                                    }
                                    else
                                    {
                                        ddlWarrantyPeriod.SelectedValue = "CUSTOM";
                                        txtCustomWarrantyPeriod.Text = pMonths;
                                    }
                                }
                                    
                                txtWarrantyTerms.Text = reader["WarrantyTerms"].ToString();
                                txtWarrantyContact.Text = reader["ContactInformation"].ToString();
                            }
                            else
                            {
                                chkHasWarranty.Checked = false;
                                pnlWarranty.Visible = false;
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

        protected void calPurchaseDate_NavChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlPurchaseMonth.SelectedValue, out int m) && int.TryParse(ddlPurchaseYear.SelectedValue, out int y))
            {
                calPurchaseDate.VisibleDate = new DateTime(y, m, 1);
            }
        }

        protected void calPurchaseDate_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            ddlPurchaseMonth.SelectedValue = e.NewDate.Month.ToString();
            if (ddlPurchaseYear.Items.FindByValue(e.NewDate.Year.ToString()) != null)
            {
                ddlPurchaseYear.SelectedValue = e.NewDate.Year.ToString();
            }
        }

        protected void calPurchaseDate_SelectionChanged(object sender, EventArgs e)
        {
            txtPurchaseDate.Text = calPurchaseDate.SelectedDate.ToString("dd/MM/yyyy");
            ddlPurchaseMonth.SelectedValue = calPurchaseDate.SelectedDate.Month.ToString();
            if (ddlPurchaseYear.Items.FindByValue(calPurchaseDate.SelectedDate.Year.ToString()) != null)
            {
                ddlPurchaseYear.SelectedValue = calPurchaseDate.SelectedDate.Year.ToString();
            }
        }

        protected void calWarrantyStart_NavChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlWarrantyStartMonth.SelectedValue, out int m) && int.TryParse(ddlWarrantyStartYear.SelectedValue, out int y))
            {
                calWarrantyStart.VisibleDate = new DateTime(y, m, 1);
            }
        }

        protected void calWarrantyStart_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            ddlWarrantyStartMonth.SelectedValue = e.NewDate.Month.ToString();
            if (ddlWarrantyStartYear.Items.FindByValue(e.NewDate.Year.ToString()) != null)
            {
                ddlWarrantyStartYear.SelectedValue = e.NewDate.Year.ToString();
            }
        }

        protected void calWarrantyStart_SelectionChanged(object sender, EventArgs e)
        {
            txtWarrantyStart.Text = calWarrantyStart.SelectedDate.ToString("dd/MM/yyyy");
            ddlWarrantyStartMonth.SelectedValue = calWarrantyStart.SelectedDate.Month.ToString();
            if (ddlWarrantyStartYear.Items.FindByValue(calWarrantyStart.SelectedDate.Year.ToString()) != null)
            {
                ddlWarrantyStartYear.SelectedValue = calWarrantyStart.SelectedDate.Year.ToString();
            }
        }

        protected void calWarrantyExpiry_NavChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlWarrantyExpiryMonth.SelectedValue, out int m) && int.TryParse(ddlWarrantyExpiryYear.SelectedValue, out int y))
            {
                calWarrantyExpiry.VisibleDate = new DateTime(y, m, 1);
            }
        }

        protected void calWarrantyExpiry_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            ddlWarrantyExpiryMonth.SelectedValue = e.NewDate.Month.ToString();
            if (ddlWarrantyExpiryYear.Items.FindByValue(e.NewDate.Year.ToString()) != null)
            {
                ddlWarrantyExpiryYear.SelectedValue = e.NewDate.Year.ToString();
            }
        }

        protected void calWarrantyExpiry_SelectionChanged(object sender, EventArgs e)
        {
            txtWarrantyExpiry.Text = calWarrantyExpiry.SelectedDate.ToString("dd/MM/yyyy");
            ddlWarrantyExpiryMonth.SelectedValue = calWarrantyExpiry.SelectedDate.Month.ToString();
            if (ddlWarrantyExpiryYear.Items.FindByValue(calWarrantyExpiry.SelectedDate.Year.ToString()) != null)
            {
                ddlWarrantyExpiryYear.SelectedValue = calWarrantyExpiry.SelectedDate.Year.ToString();
            }
        }

        protected void chkHasWarranty_CheckedChanged(object sender, EventArgs e)
        {
            pnlWarranty.Visible = chkHasWarranty.Checked;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int itemId;
            if (!int.TryParse(Request.QueryString["id"], out itemId)) return;
            
            int userId = Convert.ToInt32(Session["UserID"]);
            bool isUpdated = false;

            string connStr = DatabaseHelper.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        object wStartDate = DBNull.Value;
                        object wExpiryDate = DBNull.Value;
                        
                        if (chkHasWarranty.Checked && !string.IsNullOrEmpty(txtWarrantyStart.Text) && !string.IsNullOrEmpty(txtWarrantyExpiry.Text))
                        {
                            wStartDate = ParseDateOrDefault(txtWarrantyStart.Text);
                            wExpiryDate = ParseDateOrDefault(txtWarrantyExpiry.Text);
                        }

                        string updateItemQuery = @"
                            UPDATE InventoryItems 
                            SET CategoryID = @CategoryID, SellerID = @SellerID, ProductName = @ProductName, 
                                Brand = @Brand, ModelNumber = @ModelNumber, SerialNumber = @SerialNumber, 
                                PurchaseDate = @PurchaseDate, PurchasePrice = @PurchasePrice, Location = @Location, 
                                ItemCondition = @ItemCondition, Description = @Description, 
                                WarrantyStartDate = @WarrantyStartDate, WarrantyExpiryDate = @WarrantyExpiryDate, 
                                UpdatedDate = GETDATE()
                            WHERE ItemID = @ItemID AND UserID = @UserID AND IsDeleted = 0";

                        using (SqlCommand cmd = new SqlCommand(updateItemQuery, conn, tran))
                        {
                            cmd.Parameters.AddWithValue("@ItemID", itemId);
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@CategoryID", Convert.ToInt32(ddlCategory.SelectedValue));
                            
                            object sellerId = DBNull.Value;
                            if (ddlSeller.SelectedValue == "CUSTOM")
                            {
                                string customSeller = txtCustomSeller.Text.Trim();
                                if (!string.IsNullOrEmpty(customSeller))
                                {
                                    using (SqlCommand checkCmd = new SqlCommand("SELECT SellerID FROM Sellers WHERE LOWER(SellerName) = LOWER(@SName)", conn, tran))
                                    {
                                        checkCmd.Parameters.AddWithValue("@SName", customSeller);
                                        object existingId = checkCmd.ExecuteScalar();
                                        if (existingId != null && existingId != DBNull.Value)
                                        {
                                            sellerId = Convert.ToInt32(existingId);
                                        }
                                        else
                                        {
                                            using (SqlCommand insCmd = new SqlCommand("INSERT INTO Sellers (SellerName, CreatedDate) VALUES (@SName, GETDATE()); SELECT SCOPE_IDENTITY();", conn, tran))
                                            {
                                                insCmd.Parameters.AddWithValue("@SName", customSeller);
                                                sellerId = Convert.ToInt32(insCmd.ExecuteScalar());
                                            }
                                        }
                                    }
                                }
                            }
                            else if (!string.IsNullOrEmpty(ddlSeller.SelectedValue))
                            {
                                sellerId = Convert.ToInt32(ddlSeller.SelectedValue);
                            }
                            cmd.Parameters.AddWithValue("@SellerID", sellerId);

                            cmd.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Brand", txtBrand.Text.Trim() == "" ? (object)DBNull.Value : txtBrand.Text.Trim());
                            cmd.Parameters.AddWithValue("@ModelNumber", txtModelNumber.Text.Trim() == "" ? (object)DBNull.Value : txtModelNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@SerialNumber", txtSerialNumber.Text.Trim() == "" ? (object)DBNull.Value : txtSerialNumber.Text.Trim());
                            cmd.Parameters.AddWithValue("@PurchaseDate", ParseDateOrDefault(txtPurchaseDate.Text));
                            cmd.Parameters.AddWithValue("@PurchasePrice", Convert.ToDecimal(txtPurchasePrice.Text));
                            cmd.Parameters.AddWithValue("@Location", ddlLocation.SelectedValue);
                            cmd.Parameters.AddWithValue("@ItemCondition", rblCondition.SelectedValue);
                            cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim() == "" ? (object)DBNull.Value : txtDescription.Text.Trim());
                            cmd.Parameters.AddWithValue("@WarrantyStartDate", wStartDate);
                            cmd.Parameters.AddWithValue("@WarrantyExpiryDate", wExpiryDate);

                            cmd.ExecuteNonQuery();
                        }

                        // Handle Warranty
                        string checkWarranty = "SELECT COUNT(*) FROM Warranties WHERE ItemID = @ItemID";
                        int warrantyCount = 0;
                        using (SqlCommand chkCmd = new SqlCommand(checkWarranty, conn, tran))
                        {
                            chkCmd.Parameters.AddWithValue("@ItemID", itemId);
                            warrantyCount = Convert.ToInt32(chkCmd.ExecuteScalar());
                        }

                        if (chkHasWarranty.Checked)
                        {
                            string wType = ddlWarrantyType.SelectedValue;
                            if (wType == "CUSTOM")
                            {
                                wType = txtCustomWarrantyType.Text.Trim();
                                if (string.IsNullOrEmpty(wType)) wType = "Standard";
                            }

                            int periodMonths = 12;
                            if (ddlWarrantyPeriod.SelectedValue == "CUSTOM")
                            {
                                if (int.TryParse(txtCustomWarrantyPeriod.Text.Trim(), out int customPeriod) && customPeriod > 0)
                                {
                                    periodMonths = customPeriod;
                                }
                            }
                            else if (int.TryParse(ddlWarrantyPeriod.SelectedValue, out int p))
                            {
                                periodMonths = p;
                            }

                            if (warrantyCount > 0)
                            {
                                // Update existing warranty
                                string updateWarr = @"
                                    UPDATE Warranties 
                                    SET WarrantyType = @WarrantyType, ProviderName = @ProviderName, 
                                        StartDate = @StartDate, ExpiryDate = @ExpiryDate, 
                                        WarrantyPeriodMonths = @WarrantyPeriodMonths, WarrantyTerms = @WarrantyTerms, 
                                        ContactInformation = @ContactInformation
                                    WHERE ItemID = @ItemID";
                                    
                                using (SqlCommand uCmd = new SqlCommand(updateWarr, conn, tran))
                                {
                                    uCmd.Parameters.AddWithValue("@ItemID", itemId);
                                    uCmd.Parameters.AddWithValue("@WarrantyType", wType);
                                    uCmd.Parameters.AddWithValue("@ProviderName", txtProvider.Text.Trim() == "" ? (object)DBNull.Value : txtProvider.Text.Trim());
                                    uCmd.Parameters.AddWithValue("@StartDate", wStartDate);
                                    uCmd.Parameters.AddWithValue("@ExpiryDate", wExpiryDate);
                                    uCmd.Parameters.AddWithValue("@WarrantyPeriodMonths", periodMonths);
                                    uCmd.Parameters.AddWithValue("@WarrantyTerms", txtWarrantyTerms.Text.Trim() == "" ? (object)DBNull.Value : txtWarrantyTerms.Text.Trim());
                                    uCmd.Parameters.AddWithValue("@ContactInformation", txtWarrantyContact.Text.Trim() == "" ? (object)DBNull.Value : txtWarrantyContact.Text.Trim());
                                    uCmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // Insert new warranty
                                string insertWarr = @"
                                    INSERT INTO Warranties (ItemID, WarrantyType, ProviderName, StartDate, ExpiryDate, 
                                                            WarrantyPeriodMonths, WarrantyTerms, ContactInformation, CreatedDate)
                                    VALUES (@ItemID, @WarrantyType, @ProviderName, @StartDate, @ExpiryDate, 
                                            @WarrantyPeriodMonths, @WarrantyTerms, @ContactInformation, GETDATE())";
                                            
                                using (SqlCommand iCmd = new SqlCommand(insertWarr, conn, tran))
                                {
                                    iCmd.Parameters.AddWithValue("@ItemID", itemId);
                                    iCmd.Parameters.AddWithValue("@WarrantyType", wType);
                                    iCmd.Parameters.AddWithValue("@ProviderName", txtProvider.Text.Trim() == "" ? (object)DBNull.Value : txtProvider.Text.Trim());
                                    iCmd.Parameters.AddWithValue("@StartDate", wStartDate);
                                    iCmd.Parameters.AddWithValue("@ExpiryDate", wExpiryDate);
                                    iCmd.Parameters.AddWithValue("@WarrantyPeriodMonths", periodMonths);
                                    iCmd.Parameters.AddWithValue("@WarrantyTerms", txtWarrantyTerms.Text.Trim() == "" ? (object)DBNull.Value : txtWarrantyTerms.Text.Trim());
                                    iCmd.Parameters.AddWithValue("@ContactInformation", txtWarrantyContact.Text.Trim() == "" ? (object)DBNull.Value : txtWarrantyContact.Text.Trim());
                                    iCmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else if (warrantyCount > 0)
                        {
                            // User unchecked warranty, delete existing
                            string deleteWarr = "DELETE FROM Warranties WHERE ItemID = @ItemID";
                            using (SqlCommand dCmd = new SqlCommand(deleteWarr, conn, tran))
                            {
                                dCmd.Parameters.AddWithValue("@ItemID", itemId);
                                dCmd.ExecuteNonQuery();
                            }
                        }

                        tran.Commit();
                        isUpdated = true;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            if (tran != null && tran.Connection != null)
                            {
                                tran.Rollback();
                            }
                        }
                        catch { }
                        lblMessage.Text = "Error updating item: " + ex.Message;
                    }
                }
            }

            if (isUpdated)
            {
                Response.Redirect($"~/Inventory/ItemDetails.aspx?id={itemId}&msg=updated");
            }
        }

        private DateTime ParseDateOrDefault(string dateStr)
        {
            DateTime result;
            string[] formats = new string[] { "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy", "yyyy-MM-dd", "yyyy/MM/dd" };
            if (DateTime.TryParseExact(dateStr.Trim(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out result))
            {
                return result;
            }
            return Convert.ToDateTime(dateStr.Trim());
        }
    }
}
