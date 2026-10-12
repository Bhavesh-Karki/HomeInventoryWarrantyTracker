using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HomeInventoryWarrantyTracker.Inventory
{
    public partial class AddItem : System.Web.UI.Page
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
                LoadSellers();
                InitializeCalendarYearDropDowns();
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

        protected void btnNext1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                mvAddItem.ActiveViewIndex = 1;
            }
        }

        protected void btnPrev2_Click(object sender, EventArgs e)
        {
            mvAddItem.ActiveViewIndex = 0;
        }

        protected void btnNext2_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                mvAddItem.ActiveViewIndex = 2;
            }
        }

        protected void btnPrev3_Click(object sender, EventArgs e)
        {
            mvAddItem.ActiveViewIndex = 1;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int userId = Convert.ToInt32(Session["UserID"]);
            bool isSaved = false;

            string connStr = DatabaseHelper.GetConnectionString();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlTransaction tran = conn.BeginTransaction())
                {
                    try
                    {
                        string itemQuery = @"
                            INSERT INTO InventoryItems (UserID, CategoryID, SellerID, ProductName, Brand, ModelNumber, 
                                                        SerialNumber, PurchaseDate, PurchasePrice, Location, ItemCondition, 
                                                        Description, WarrantyStartDate, WarrantyExpiryDate, CreatedDate, UpdatedDate, IsDeleted)
                            VALUES (@UserID, @CategoryID, @SellerID, @ProductName, @Brand, @ModelNumber, 
                                    @SerialNumber, @PurchaseDate, @PurchasePrice, @Location, @ItemCondition, 
                                    @Description, @WarrantyStartDate, @WarrantyExpiryDate, GETDATE(), GETDATE(), 0);
                            SELECT SCOPE_IDENTITY();";

                        using (SqlCommand cmd = new SqlCommand(itemQuery, conn, tran))
                        {
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

                            object wStartDate = DBNull.Value;
                            object wExpiryDate = DBNull.Value;
                            
                            if (chkHasWarranty.Checked && !string.IsNullOrEmpty(txtWarrantyStart.Text) && !string.IsNullOrEmpty(txtWarrantyExpiry.Text))
                            {
                                wStartDate = ParseDateOrDefault(txtWarrantyStart.Text);
                                wExpiryDate = ParseDateOrDefault(txtWarrantyExpiry.Text);
                            }

                            cmd.Parameters.AddWithValue("@WarrantyStartDate", wStartDate);
                            cmd.Parameters.AddWithValue("@WarrantyExpiryDate", wExpiryDate);

                            int itemId = Convert.ToInt32(cmd.ExecuteScalar());

                            if (chkHasWarranty.Checked)
                            {
                                string warrantyQuery = @"
                                    INSERT INTO Warranties (ItemID, WarrantyType, ProviderName, StartDate, ExpiryDate, 
                                                            WarrantyPeriodMonths, WarrantyTerms, ContactInformation, CreatedDate)
                                    VALUES (@ItemID, @WarrantyType, @ProviderName, @StartDate, @ExpiryDate, 
                                            @WarrantyPeriodMonths, @WarrantyTerms, @ContactInformation, GETDATE())";

                                using (SqlCommand wCmd = new SqlCommand(warrantyQuery, conn, tran))
                                {
                                    wCmd.Parameters.AddWithValue("@ItemID", itemId);
                                    
                                    string wType = ddlWarrantyType.SelectedValue;
                                    if (wType == "CUSTOM")
                                    {
                                        wType = txtCustomWarrantyType.Text.Trim();
                                        if (string.IsNullOrEmpty(wType)) wType = "Standard";
                                    }
                                    wCmd.Parameters.AddWithValue("@WarrantyType", wType);
                                    
                                    wCmd.Parameters.AddWithValue("@ProviderName", txtProvider.Text.Trim() == "" ? (object)DBNull.Value : txtProvider.Text.Trim());
                                    wCmd.Parameters.AddWithValue("@StartDate", wStartDate);
                                    wCmd.Parameters.AddWithValue("@ExpiryDate", wExpiryDate);
                                    
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
                                    wCmd.Parameters.AddWithValue("@WarrantyPeriodMonths", periodMonths);
                                    
                                    wCmd.Parameters.AddWithValue("@WarrantyTerms", txtWarrantyTerms.Text.Trim() == "" ? (object)DBNull.Value : txtWarrantyTerms.Text.Trim());
                                    wCmd.Parameters.AddWithValue("@ContactInformation", txtWarrantyContact.Text.Trim() == "" ? (object)DBNull.Value : txtWarrantyContact.Text.Trim());
                                    
                                    wCmd.ExecuteNonQuery();
                                }
                            }

                            tran.Commit();
                            isSaved = true;
                        }
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
                        lblMessage.Text = "Error adding item: " + ex.Message;
                    }
                }
            }

            if (isSaved)
            {
                Response.Redirect("~/Inventory/Inventory.aspx?msg=added");
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
