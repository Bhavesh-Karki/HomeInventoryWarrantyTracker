<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Account.Register" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register - Home Inventory & Warranty Tracker</title>
    <link href="../Content/Site.css?v=3.5" rel="stylesheet" type="text/css" />
    <style>
        .auth-page { display: flex; justify-content: center; align-items: center; min-height: 100vh; background-color: #F4F7FA; margin: 0; font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif; }
        .auth-card { background: white; padding: 36px; border-radius: 12px; box-shadow: 0 4px 20px rgba(10,28,51,0.1); width: 100%; max-width: 420px; box-sizing: border-box; }
        .auth-card h2 { color: #142D4E; margin-bottom: 20px; font-weight: 600; }
        .form-group { margin-bottom: 18px; }
        .form-group label { display: block; margin-bottom: 6px; font-weight: 500; color: #334155; font-size: 0.9rem; }
        .form-control { width: 100%; height: 42px; padding: 8px 14px; border: 1px solid #CBD5E1; border-radius: 6px; box-sizing: border-box; font-size: 0.95rem; }
        .form-control:focus { border-color: #0D9488; outline: none; box-shadow: 0 0 0 3px rgba(13,148,136,0.2); }
        .btn { width: 100%; height: 42px; padding: 0 15px; border: none; border-radius: 6px; cursor: pointer; font-size: 1rem; font-weight: 600; display: inline-flex; align-items: center; justify-content: center; }
        .btn-primary { background-color: #0D9488; color: white; }
        .btn-primary:hover { background-color: #0F766E; }
        .msg-error { color: #EF4444; display: block; margin-bottom: 15px; font-size: 0.9rem; }
        .msg-success { color: #10B981; display: block; margin-bottom: 15px; font-size: 0.9rem; }
        .text-center { text-align: center; }
        .mt-3 { margin-top: 15px; }
        a { color: #0D9488; text-decoration: none; font-weight: 500; }
        a:hover { color: #0F766E; text-decoration: underline; }
    </style>
</head>
<body class="auth-page">
    <form id="form1" runat="server">
        <div class="auth-card">
            <h2 class="text-center">Create Account</h2>
            
            <asp:ValidationSummary ID="vsRegister" runat="server" ForeColor="Red" DisplayMode="BulletList" />
            <asp:Label ID="lblSuccess" runat="server" CssClass="msg-success" Visible="false"></asp:Label>
            <asp:Label ID="lblError" runat="server" CssClass="msg-error" Visible="false"></asp:Label>
            
            <div class="form-group">
                <label>Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full Name is required" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email format" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ForeColor="Red" Display="None"></asp:RegularExpressionValidator>
            </div>
            
            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label>Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm Password is required" ForeColor="Red" Display="None"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ErrorMessage="Passwords do not match" ForeColor="Red" Display="None"></asp:CompareValidator>
            </div>
            
            <div class="form-group">
                <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-primary" style="width: 100%;" OnClick="btnRegister_Click" />
            </div>
            
            <div class="text-center mt-3">
                <p>Already have an account? <a href="Login.aspx">Login here</a></p>
            </div>
        </div>
    </form>
</body>
</html>
