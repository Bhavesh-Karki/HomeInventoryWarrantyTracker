<%@ Page Title="Reports & Analytics" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Reports.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2 class="page-header">Reports & Analytics</h2>

        <div class="row">
            <div class="col-md-6 mb-4">
                <div class="form-section card p-4 h-100">
                    <h4 class="mb-4">Inventory Statistics</h4>
                    <div class="d-flex justify-content-between mb-2">
                        <span class="card-label">Total Items:</span>
                        <asp:Label ID="lblTotalItems" runat="server" CssClass="fw-bold fs-5"></asp:Label>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                        <span class="card-label">Total Inventory Value:</span>
                        <asp:Label ID="lblTotalValue" runat="server" CssClass="fw-bold fs-5 text-success"></asp:Label>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                        <span class="card-label">Average Item Price:</span>
                        <asp:Label ID="lblAvgPrice" runat="server" CssClass="fw-bold fs-5 text-primary"></asp:Label>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                        <span class="card-label">Most Common Category:</span>
                        <asp:Label ID="lblTopCategory" runat="server" CssClass="fw-bold fs-5 text-info"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="col-md-6 mb-4">
                <div class="form-section card p-4 h-100">
                    <h4 class="mb-4">Warranty Statistics</h4>
                    <div class="d-flex justify-content-between mb-2">
                        <span class="card-label">Active Warranties:</span>
                        <asp:Label ID="lblActiveWarranties" runat="server" CssClass="fw-bold fs-5 text-success"></asp:Label>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                        <span class="card-label">Expiring Warranties (30 Days):</span>
                        <asp:Label ID="lblExpiringWarranties" runat="server" CssClass="fw-bold fs-5 text-warning"></asp:Label>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                        <span class="card-label">Expired Warranties:</span>
                        <asp:Label ID="lblExpiredWarranties" runat="server" CssClass="fw-bold fs-5 text-danger"></asp:Label>
                    </div>
                    <div class="d-flex justify-content-between mb-2">
                        <span class="card-label">Items Without Warranty:</span>
                        <asp:Label ID="lblNoWarranty" runat="server" CssClass="fw-bold fs-5 text-secondary"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-6 mb-4">
                <div class="form-section card p-4 h-100">
                    <h4 class="mb-3">Category Statistics</h4>
                    <div class="grid-view table-responsive">
                        <asp:GridView ID="gvCategoryStats" runat="server" AutoGenerateColumns="False" 
                            CssClass="grid-table table table-sm table-striped" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                <asp:BoundField DataField="ItemCount" HeaderText="Number of Items" />
                                <asp:BoundField DataField="TotalValue" HeaderText="Total Value (&#8377;)" DataFormatString="&#8377;{0:#,##0.00}" HtmlEncode="false" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="col-md-6 mb-4">
                <div class="form-section card p-4 h-100">
                    <h4 class="mb-3">Location Statistics</h4>
                    <div class="grid-view table-responsive">
                        <asp:GridView ID="gvLocationStats" runat="server" AutoGenerateColumns="False" 
                            CssClass="grid-table table table-sm table-striped" GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="Location" HeaderText="Location" />
                                <asp:BoundField DataField="ItemCount" HeaderText="Number of Items" />
                                <asp:BoundField DataField="TotalValue" HeaderText="Total Value (&#8377;)" DataFormatString="&#8377;{0:#,##0.00}" HtmlEncode="false" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>

</asp:Content>
