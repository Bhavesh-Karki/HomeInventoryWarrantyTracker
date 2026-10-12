<%@ Page Title="Expiring Warranties" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ExpiringWarranty.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Warranty.ExpiringWarranty" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2 class="page-header"><i class="bi bi-exclamation-triangle-fill" style="color:#F59E0B"></i> Warranty Expiry Alerts</h2>
        <p class="text-muted mb-4">Warranties expiring within the next 30 days</p>
        
        <div class="alert-section">
            <div class="grid-view table-responsive">
                <asp:GridView ID="gvExpiring" runat="server" AutoGenerateColumns="False" 
                    CssClass="grid-table table table-striped table-hover" GridLines="None"
                    EmptyDataText="No warranties are expiring within the next 30 days.">
                    
                    <Columns>
                        <asp:BoundField DataField="ProductName" HeaderText="Product" />
                        <asp:BoundField DataField="ProviderName" HeaderText="Provider" />
                        <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" DataFormatString="{0:dd MMM yyyy}" />
                        
                        <asp:TemplateField HeaderText="Remaining Days">
                            <ItemTemplate>
                                <span style="font-weight: bold; color: darkorange;">
                                    <%# GetRemainingDays(Eval("ExpiryDate")) %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:BoundField DataField="ContactInformation" HeaderText="Contact" />
                    </Columns>
                    
                </asp:GridView>
            </div>
        </div>
</asp:Content>
