<%@ Page Title="Expired Warranties" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExpiredWarranty.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Warranty.ExpiredWarranty" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2 class="page-header">Expired Warranties</h2>
        
        <div class="grid-view table-responsive">
            <asp:GridView ID="gvExpired" runat="server" AutoGenerateColumns="False" 
                CssClass="grid-table table table-striped table-hover" GridLines="None"
                EmptyDataText="No expired warranties found.">
                
                <Columns>
                    <asp:BoundField DataField="ProductName" HeaderText="Product" />
                    <asp:BoundField DataField="ProviderName" HeaderText="Warranty Provider" />
                    <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:dd MMM yyyy}" />
                    
                    <asp:TemplateField HeaderText="Days Expired">
                        <ItemTemplate>
                            <span style="color: red;">
                                <%# GetDaysExpired(Eval("ExpiryDate")) %> days
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:BoundField DataField="ContactInformation" HeaderText="Contact" />
                </Columns>
                
            </asp:GridView>
        </div>
</asp:Content>
