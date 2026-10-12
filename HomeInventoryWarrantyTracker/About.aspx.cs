using System;
using System.Web.UI;

namespace HomeInventoryWarrantyTracker
{
    public partial class About : Page
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
                if (Application["ApplicationVisits"] != null)
                {
                    lblVisits.Text = Application["ApplicationVisits"].ToString();
                }
            }
        }
    }
}