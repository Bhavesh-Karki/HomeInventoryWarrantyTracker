<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="HomeInventoryWarrantyTracker.About" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="content-area">
        <h2 class="page-header">About</h2>

        <div class="detail-card">
            <h3>Home Inventory & Warranty Tracker</h3>
            <p><strong>Version:</strong> 1.0.0</p>
            <p>
                Home Inventory & Warranty Tracker helps you manage household products, track purchases, 
                monitor warranty status, and get alerts for expiring warranties.
            </p>

            <h4>Technology Stack</h4>
            <ul>
                <li>ASP.NET Web Forms</li>
                <li>C#</li>
                <li>ADO.NET</li>
                <li>SQL Server</li>
                <li>.NET Framework 4.7.2</li>
            </ul>

            <div style="margin-top: 20px;">
                <p><strong>Application Visits:</strong> <asp:Label ID="lblVisits" runat="server" Text="0"></asp:Label></p>
            </div>

            <div style="margin-top: 20px;">
                <h4>Developer Info</h4>
                <p>Developed as a sample project.</p>
            </div>
        </div>
    </div>
</asp:Content>
