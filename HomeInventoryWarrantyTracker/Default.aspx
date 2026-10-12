<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HomeInventoryWarrantyTracker._Default" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Home Inventory &amp; Warranty Tracker</title>
    <link href="Content/Site.css?v=3.5" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />
</head>
<body class="landing-page">
    <form id="form1" runat="server">
        <div class="landing-header">
            <h1 class="landing-title"><i class="bi bi-house-door-fill"></i> Home Inventory &amp; Warranty Tracker</h1>
            <p class="landing-subtitle">Manage your household products, purchases and warranties in one place.</p>
            <div class="landing-buttons">
                <a href="Account/Login.aspx" class="landing-btn landing-btn-primary">Login</a>
                <a href="Account/Register.aspx" class="landing-btn landing-btn-secondary">Register</a>
            </div>
        </div>
        
        <div class="feature-cards">
            <div class="feature-card">
                <div class="feature-icon"><i class="bi bi-box-seam-fill" style="font-size:2.5rem;color:#0D9488"></i></div>
                <h3 class="feature-title">Inventory Management</h3>
                <p class="feature-desc">Track all your household items, their details, purchase info and current location.</p>
            </div>
            <div class="feature-card">
                <div class="feature-icon"><i class="bi bi-shield-fill-check" style="font-size:2.5rem;color:#10B981"></i></div>
                <h3 class="feature-title">Warranty Tracking</h3>
                <p class="feature-desc">Never miss a warranty claim. Track warranty periods for all your products.</p>
            </div>
            <div class="feature-card">
                <div class="feature-icon"><i class="bi bi-exclamation-triangle-fill" style="font-size:2.5rem;color:#F59E0B"></i></div>
                <h3 class="feature-title">Expiry Alerts</h3>
                <p class="feature-desc">Get alerts for warranties expiring soon so you can take timely action.</p>
            </div>
            <div class="feature-card">
                <div class="feature-icon"><i class="bi bi-bar-chart-fill" style="font-size:2.5rem;color:#8B5CF6"></i></div>
                <h3 class="feature-title">Reports &amp; Analytics</h3>
                <p class="feature-desc">View inventory statistics, category breakdowns and total value reports.</p>
        </div>
    </form>
</body>
</html>
