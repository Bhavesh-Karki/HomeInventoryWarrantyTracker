<%@ Page Title="All Warranties" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Warranty.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Warranty.Warranty" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2 class="page-header">All Warranties</h2>
        
        <div class="grid-view table-responsive">
            <asp:GridView ID="gvWarranties" runat="server" AutoGenerateColumns="False" 
                AllowSorting="True" AllowPaging="True" PageSize="10"
                CssClass="grid-table table table-striped table-hover" GridLines="None"
                EmptyDataText="No warranty records found."
                OnSorting="gvWarranties_Sorting" OnPageIndexChanging="gvWarranties_PageIndexChanging">
                
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product" SortExpression="ProductName" />
                    <asp:BoundField DataField="Brand" HeaderText="Brand" SortExpression="Brand" />
                    <asp:BoundField DataField="WarrantyType" HeaderText="Type" SortExpression="WarrantyType" />
                    <asp:BoundField DataField="ProviderName" HeaderText="Provider" SortExpression="ProviderName" />
                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" SortExpression="StartDate" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" SortExpression="ExpiryDate" DataFormatString="{0:dd MMM yyyy}" />
                    
                    <asp:TemplateField HeaderText="Remaining Days">
                        <ItemTemplate>
                            <%# GetRemainingDays(Eval("ExpiryDate")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# GetWarrantyStatusBadge(Eval("ExpiryDate")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                
                <PagerStyle CssClass="pagination-ys" />
            </asp:GridView>
        </div>
</asp:Content>
