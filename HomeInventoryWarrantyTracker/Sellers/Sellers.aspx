<%@ Page Title="Seller Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sellers.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Sellers.Sellers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2 class="page-header">Seller Management</h2>
        
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3"></asp:Label>
        
        <div class="form-section card mb-4 p-4">
            <h4 class="mb-3">Add Seller</h4>
            <div class="row g-3">
                <div class="col-md-3">
                    <label class="form-label">Seller Name</label>
                    <asp:TextBox ID="txtSellerName" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvSellerName" runat="server" ControlToValidate="txtSellerName"
                        ErrorMessage="Seller name is required." CssClass="text-danger" ValidationGroup="AddSeller" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Contact Number</label>
                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revContact" runat="server" ControlToValidate="txtContactNumber"
                        ErrorMessage="Invalid phone format." ValidationExpression="^[\d\s\-\+\(\)]+$" CssClass="text-danger" ValidationGroup="AddSeller" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtSellerEmail" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtSellerEmail"
                        ErrorMessage="Invalid email format." ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" CssClass="text-danger" ValidationGroup="AddSeller" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Address</label>
                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-12 text-end">
                    <asp:Button ID="btnAddSeller" runat="server" Text="Add Seller" CssClass="btn btn-primary" ValidationGroup="AddSeller" OnClick="btnAddSeller_Click" />
                </div>
            </div>
        </div>
        
        <div class="grid-view table-responsive">
            <asp:GridView ID="gvSellers" runat="server" AutoGenerateColumns="False" 
                AllowPaging="True" PageSize="10" DataKeyNames="SellerID"
                CssClass="grid-table table table-striped table-hover" GridLines="None"
                EmptyDataText="No sellers found."
                OnRowEditing="gvSellers_RowEditing" OnRowCancelingEdit="gvSellers_RowCancelingEdit"
                OnRowUpdating="gvSellers_RowUpdating" OnRowDeleting="gvSellers_RowDeleting" OnPageIndexChanging="gvSellers_PageIndexChanging">
                
                <Columns>
                    <asp:BoundField DataField="SellerID" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="SellerName" HeaderText="Seller Name" />
                    <asp:BoundField DataField="ContactNumber" HeaderText="Contact Number" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                    <asp:BoundField DataField="Address" HeaderText="Address" />
                    
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle-CssClass="btn btn-sm btn-secondary mx-1" />
                </Columns>
            </asp:GridView>
        </div>
</asp:Content>
