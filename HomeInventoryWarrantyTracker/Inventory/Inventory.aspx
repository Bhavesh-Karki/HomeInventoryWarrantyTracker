<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Inventory.Inventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header d-flex justify-content-between align-items-center">
        <h2 class="mb-0">Inventory</h2>
        <a href="AddItem.aspx" class="btn btn-primary">Add New Item</a>
    </div>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" ForeColor="Green"></asp:Label>

    <div class="filter-bar">
        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control search-input" placeholder="Search product, brand or serial number"></asp:TextBox>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
        
        <asp:DropDownList ID="ddlCategoryFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged" CssClass="form-select filter-select">
            <asp:ListItem Text="All Categories" Value=""></asp:ListItem>
        </asp:DropDownList>
        
        <asp:DropDownList ID="ddlWarrantyFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged" CssClass="form-select filter-select">
            <asp:ListItem Text="All Warranties" Value=""></asp:ListItem>
            <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
            <asp:ListItem Text="Expiring Soon" Value="Expiring"></asp:ListItem>
            <asp:ListItem Text="Expired" Value="Expired"></asp:ListItem>
            <asp:ListItem Text="No Warranty" Value="None"></asp:ListItem>
        </asp:DropDownList>
        
        <asp:DropDownList ID="ddlLocationFilter" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_SelectedIndexChanged" CssClass="form-select filter-select">
            <asp:ListItem Text="All Locations" Value=""></asp:ListItem>
            <asp:ListItem Text="Living Room" Value="Living Room"></asp:ListItem>
            <asp:ListItem Text="Bedroom" Value="Bedroom"></asp:ListItem>
            <asp:ListItem Text="Kitchen" Value="Kitchen"></asp:ListItem>
            <asp:ListItem Text="Office" Value="Office"></asp:ListItem>
            <asp:ListItem Text="Bathroom" Value="Bathroom"></asp:ListItem>
            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
        </asp:DropDownList>
    </div>

    <div style="display: flex; gap: 20px;">
        <div style="flex: 1;">
            <div class="grid-view">
                <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="false" AllowSorting="true" AllowPaging="true" PageSize="8"
                    CssClass="grid-table" DataKeyNames="ItemID" 
                    OnSorting="gvInventory_Sorting" 
                    OnPageIndexChanging="gvInventory_PageIndexChanging" 
                    OnRowCommand="gvInventory_RowCommand" 
                    OnRowDeleting="gvInventory_RowDeleting"
                    OnSelectedIndexChanged="gvInventory_SelectedIndexChanged"
                    EmptyDataText="No inventory items found. Add your first item!">
                    
                    <PagerStyle CssClass="grid-pager" />
                    <HeaderStyle CssClass="grid-header" />
                    
                    <Columns>
                        <asp:BoundField DataField="ProductName" SortExpression="ProductName" HeaderText="Product" />
                        <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                        <asp:BoundField DataField="Brand" HeaderText="Brand" />
                        <asp:BoundField DataField="PurchasePrice" DataFormatString="&#8377;{0:N2}" SortExpression="PurchasePrice" HeaderText="Price (&#8377;)" HtmlEncode="false" />
                        <asp:BoundField DataField="PurchaseDate" DataFormatString="{0:dd MMM yyyy}" SortExpression="PurchaseDate" HeaderText="Purchase Date" />
                        <asp:BoundField DataField="Location" HeaderText="Location" />
                        
                        <asp:TemplateField HeaderText="Warranty Status">
                            <ItemTemplate>
                                <%# GetWarrantyStatusBadge(Eval("WarrantyExpiryDate")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-nowrap">
                            <ItemTemplate>
                                <div class="d-flex gap-1 align-items-center">
                                    <asp:LinkButton ID="btnView" runat="server" CommandName="ViewItem" CommandArgument='<%# Eval("ItemID") %>' CssClass="btn btn-sm btn-outline-primary py-1 px-2" Text="View"></asp:LinkButton>
                                    <a href='EditItem.aspx?id=<%# Eval("ItemID") %>' class="btn btn-sm btn-outline-secondary py-1 px-2">Edit</a>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("ItemID") %>' CssClass="btn btn-sm btn-outline-danger py-1 px-2" Text="Delete" OnClientClick="return confirm('Are you sure you want to remove this item?');"></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        
        <div style="width: 250px; border-left: 1px solid #ccc; padding-left: 15px;">
            <h3>Categories</h3>
            <asp:TreeView ID="tvCategories" runat="server" NodeIndent="15">
                <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                <NodeStyle Font-Names="Tahoma" Font-Size="8pt" ForeColor="Black" HorizontalPadding="2px" NodeSpacing="0px" VerticalPadding="2px" />
                <ParentNodeStyle Font-Bold="False" />
                <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px" VerticalPadding="0px" />
            </asp:TreeView>
        </div>
    </div>
</asp:Content>
