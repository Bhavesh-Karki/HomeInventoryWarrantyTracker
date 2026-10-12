using System;
using System.Web.UI;

namespace HomeInventoryWarrantyTracker
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // If user is already logged in, redirect to Dashboard
            if (Session["UserID"] != null)
            {
                Response.Redirect("~/Dashboard.aspx");
            }
        }
    }
}