<%@ Page Title="Item Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemDetails.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Inventory.ItemDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header d-flex justify-content-between align-items-center">
        <h2 class="mb-0"><asp:Label ID="lblPageTitle" runat="server" Text="Item Details"></asp:Label></h2>
        <div class="d-flex gap-2 align-items-center">
            <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="btn btn-primary" OnClick="btnEdit_Click" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClientClick="return confirm('Are you sure you want to delete this item?');" OnClick="btnDelete_Click" />
            <a href="Inventory.aspx" class="btn btn-secondary">Back to Inventory</a>
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" ForeColor="Green"></asp:Label>

    <div class="detail-card">
        <div class="status-section">
            <h3>Current Status</h3>
            <asp:Label ID="lblWarrantyStatus" runat="server"></asp:Label>
        </div>

        <div class="info-grid" style="display: grid; grid-template-columns: 1fr 1fr; gap: 20px; margin-top: 20px;">
            <div class="info-section">
                <h3>Product Information</h3>
                <p><strong>Product Name:</strong> <asp:Label ID="lblProductName" runat="server"></asp:Label></p>
                <p><strong>Category:</strong> <asp:Label ID="lblCategory" runat="server"></asp:Label></p>
                <p><strong>Brand:</strong> <asp:Label ID="lblBrand" runat="server"></asp:Label></p>
                <p><strong>Model Number:</strong> <asp:Label ID="lblModelNumber" runat="server"></asp:Label></p>
                <p><strong>Serial Number:</strong> <asp:Label ID="lblSerialNumber" runat="server"></asp:Label></p>
                <p><strong>Condition:</strong> <asp:Label ID="lblCondition" runat="server"></asp:Label></p>
                <p><strong>Description:</strong> <asp:Label ID="lblDescription" runat="server"></asp:Label></p>
            </div>

            <div class="info-section">
                <h3>Purchase Information</h3>
                <p><strong>Purchase Date:</strong> <asp:Label ID="lblPurchaseDate" runat="server"></asp:Label></p>
                <p><strong>Purchase Price:</strong> <asp:Label ID="lblPurchasePrice" runat="server"></asp:Label></p>
                <p><strong>Location:</strong> <asp:Label ID="lblLocation" runat="server"></asp:Label></p>

                <h3 style="margin-top: 20px;">Seller Information</h3>
                <asp:Panel ID="pnlSellerInfo" runat="server" Visible="false">
                    <p><strong>Name:</strong> <asp:Label ID="lblSellerName" runat="server"></asp:Label></p>
                    <p><strong>Contact:</strong> <asp:Label ID="lblSellerContact" runat="server"></asp:Label></p>
                    <p><strong>Email:</strong> <asp:Label ID="lblSellerEmail" runat="server"></asp:Label></p>
                    <p><strong>Address:</strong> <asp:Label ID="lblSellerAddress" runat="server"></asp:Label></p>
                </asp:Panel>
                <asp:Label ID="lblNoSeller" runat="server" Text="No seller information available." Visible="false"></asp:Label>
            </div>
        </div>

        <div class="info-section" style="margin-top: 20px;">
            <h3>Warranty Information</h3>
            <asp:Panel ID="pnlWarrantyInfo" runat="server" Visible="false">
                <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20px;">
                    <div>
                        <p><strong>Type:</strong> <asp:Label ID="lblWarrantyType" runat="server"></asp:Label></p>
                        <p><strong>Provider:</strong> <asp:Label ID="lblWarrantyProvider" runat="server"></asp:Label></p>
                        <p><strong>Period:</strong> <asp:Label ID="lblWarrantyPeriod" runat="server"></asp:Label> months</p>
                        <p><strong>Start Date:</strong> <asp:Label ID="lblWarrantyStart" runat="server"></asp:Label></p>
                        <p><strong>Expiry Date:</strong> <asp:Label ID="lblWarrantyExpiry" runat="server"></asp:Label></p>
                    </div>
                    <div>
                        <p><strong>Contact:</strong> <asp:Label ID="lblWarrantyContact" runat="server"></asp:Label></p>
                        <p><strong>Terms:</strong> <asp:Label ID="lblWarrantyTerms" runat="server"></asp:Label></p>
                    </div>
                </div>
            </asp:Panel>
            <asp:Label ID="lblNoWarranty" runat="server" Text="No warranty information available." Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
