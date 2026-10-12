using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace HomeInventoryWarrantyTracker.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] != null)
                {
                    Response.Redirect("~/Dashboard.aspx");
                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string hashedPassword = HashPassword(password);

            try
            {
                // Check if email already exists
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                object result = DatabaseHelper.ExecuteScalar(checkQuery, 
                    new SqlParameter("@Email", email));
                
                if (Convert.ToInt32(result) > 0)
                {
                    lblError.Text = "An account with this email already exists.";
                    lblError.Visible = true;
                    lblSuccess.Visible = false;
                    return;
                }

                // Insert new user
                string insertQuery = @"INSERT INTO Users (FullName, Email, PasswordHash, CreatedDate, IsActive) 
                                      VALUES (@FullName, @Email, @PasswordHash, GETDATE(), 1)";
                
                DatabaseHelper.ExecuteNonQuery(insertQuery,
                    new SqlParameter("@FullName", fullName),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@PasswordHash", hashedPassword));

                lblSuccess.Text = "✓ Registration successful! Redirecting to login...";
                lblSuccess.Visible = true;
                lblError.Visible = false;
                
                // Clear form
                txtFullName.Text = "";
                txtEmail.Text = "";
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                
                // Redirect after short delay using client script
                ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                    "setTimeout(function(){ window.location.href='Login.aspx'; }, 2000);", true);
            }
            catch (SqlException)
            {
                lblError.Text = "A database error occurred. Please try again.";
                lblError.Visible = true;
            }
            catch (Exception)
            {
                lblError.Text = "An unexpected error occurred. Please try again.";
                lblError.Visible = true;
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("X2"));
                return sb.ToString();
            }
        }
    }
}
