<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Additional head content for Dashboard if needed -->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="page-header">Dashboard</h2>

        <div class="dashboard-cards" style="display: flex; gap: 20px; flex-wrap: wrap; margin-bottom: 20px; justify-content: center;">
            <div class="dashboard-card default-card">
                <div class="card-value"><asp:Label ID="lblTotalItems" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Total Items</div>
            </div>
            <div class="dashboard-card success-card">
                <div class="card-value"><asp:Label ID="lblTotalValue" runat="server" Text="&#8377;0"></asp:Label></div>
                <div class="card-label">Total Value</div>
            </div>
            <div class="dashboard-card success-card">
                <div class="card-value"><asp:Label ID="lblActiveWarranties" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Active Warranties</div>
            </div>
            <div class="dashboard-card warning-card">
                <div class="card-value"><asp:Label ID="lblExpiringSoon" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Expiring Soon</div>
            </div>
            <div class="dashboard-card danger-card">
                <div class="card-value"><asp:Label ID="lblExpired" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Expired</div>
            </div>
            <div class="dashboard-card default-card">
                <div class="card-value"><asp:Label ID="lblCategories" runat="server" Text="0"></asp:Label></div>
                <div class="card-label">Categories</div>
            </div>
        </div>

        <div class="alert-section" style="margin-bottom: 20px;">
            <h3><i class="bi bi-exclamation-triangle-fill" style="color:#F59E0B"></i> Warranty Expiring Soon</h3>
            <asp:Repeater ID="rptWarrantyAlerts" runat="server">
                <HeaderTemplate>
                    <ul style="list-style-type: none; padding-left: 0;">
                </HeaderTemplate>
                <ItemTemplate>
                    <li>
                        <strong><%# Eval("ProductName") %></strong> - Expires in <%# Eval("RemainingDays") %> days
                    </li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            <asp:Panel ID="pnlNoAlerts" runat="server" Visible="false">
                <asp:Label ID="lblNoAlerts" runat="server" Text="No warranties expiring soon."></asp:Label>
            </asp:Panel>
        </div>

        <div class="recent-inventory-section">
            <h3>Recent Inventory</h3>
            <div class="grid-view">
                <asp:GridView ID="gvRecentItems" runat="server" AutoGenerateColumns="false" CssClass="grid-table" GridLines="None" EmptyDataText="No inventory items found.">
                    <Columns>
                        <asp:BoundField DataField="ProductName" HeaderText="Product" />
                        <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                        <asp:BoundField DataField="Brand" HeaderText="Brand" />
                        <asp:BoundField DataField="PurchaseDate" HeaderText="Purchase Date" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField DataField="PurchasePrice" HeaderText="Purchase Price (&#8377;)" DataFormatString="&#8377;{0:N2}" HtmlEncode="false" />
                        <asp:BoundField DataField="WarrantyExpiryDate" HeaderText="Warranty Expiry" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <%# GetWarrantyStatus(Eval("WarrantyExpiryDate")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="text-nowrap">
                            <ItemTemplate>
                                <a href='<%# ResolveUrl("~/Inventory/ItemDetails.aspx?id=" + Eval("ItemID")) %>' class="btn btn-sm btn-outline-primary py-1 px-2">View Details</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div style="margin-top: 20px; font-size: 0.9em; color: #666;">
            Total Application Visits: <asp:Label ID="lblVisitCount" runat="server" Text="0"></asp:Label>
        </div>
</asp:Content>
