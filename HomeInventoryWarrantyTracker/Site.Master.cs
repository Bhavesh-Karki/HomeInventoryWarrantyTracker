using System;
using System.Web;
using System.Web.UI;

namespace HomeInventoryWarrantyTracker
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                pnlLoggedIn.Visible = true;
                lblUserName.Text = Session["UserName"]?.ToString() ?? "User";
                pnlAdRotator.Visible = true;
            }
            else
            {
                pnlLoggedIn.Visible = false;
                pnlAdRotator.Visible = false;
            }
        }

        protected void AdRotator1_AdCreated(object sender, System.Web.UI.WebControls.AdCreatedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.AlternateText))
            {
                lblTipText.Text = e.AlternateText;
                lblTipText.Visible = true;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear session
            Session.Clear();
            Session.Abandon();
            
            // Remove cookies
            if (Request.Cookies["UserEmail"] != null)
            {
                HttpCookie cookie = new HttpCookie("UserEmail");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}