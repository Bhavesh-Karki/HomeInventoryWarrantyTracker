using System;
using HomeInventoryWarrantyTracker.App_Code;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

namespace HomeInventoryWarrantyTracker.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check for Remember Me cookie
                if (Request.Cookies["UserEmail"] != null)
                {
                    txtEmail.Text = Request.Cookies["UserEmail"].Value;
                    chkRememberMe.Checked = true;
                }
                
                // If already logged in, redirect
                if (Session["UserID"] != null)
                {
                    Response.Redirect("~/Dashboard.aspx");
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string hashedPassword = HashPassword(password);

            string query = "SELECT UserID, FullName, Email FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash AND IsActive = 1";
            
            try
            {
                string connStr = DatabaseHelper.GetConnectionString();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Create session
                                Session["UserID"] = Convert.ToInt32(reader["UserID"]);
                                Session["UserName"] = reader["FullName"].ToString();
                                Session["UserEmail"] = reader["Email"].ToString();
                                
                                // Handle Remember Me cookie
                                if (chkRememberMe.Checked)
                                {
                                    HttpCookie cookie = new HttpCookie("UserEmail", email);
                                    cookie.Expires = DateTime.Now.AddDays(30);
                                    Response.Cookies.Add(cookie);
                                }
                                else
                                {
                                    // Remove cookie if exists
                                    if (Request.Cookies["UserEmail"] != null)
                                    {
                                        HttpCookie cookie = new HttpCookie("UserEmail");
                                        cookie.Expires = DateTime.Now.AddDays(-1);
                                        Response.Cookies.Add(cookie);
                                    }
                                }
                                
                                Response.Redirect("~/Dashboard.aspx");
                            }
                            else
                            {
                                lblError.Text = "Invalid email or password.";
                                lblError.Visible = true;
                            }
                        }
                    }
                }
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
