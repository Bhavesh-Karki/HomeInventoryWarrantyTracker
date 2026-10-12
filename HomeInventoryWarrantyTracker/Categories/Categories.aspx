<%@ Page Title="Category Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Categories.Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
        <h2 class="page-header">Category Management</h2>
        
        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3"></asp:Label>
        
        <div class="form-section card mb-4 p-4">
            <h4 class="mb-3">Add Category</h4>
            <div class="row g-3">
                <div class="col-md-3">
                    <label class="form-label">Category Name *</label>
                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="e.g. Garden & Outdoor"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ControlToValidate="txtCategoryName"
                        ErrorMessage="Category name is required." CssClass="text-danger" ValidationGroup="AddCategory" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4">
                    <label class="form-label">Description</label>
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" placeholder="Optional category details..."></asp:TextBox>
                </div>
                <div class="col-md-3">
                    <label class="form-label">Status</label>
                    <asp:DropDownList ID="ddlAddStatus" runat="server" CssClass="form-select">
                        <asp:ListItem Value="1" Text="Active (Available for Shopping)" Selected="True"></asp:ListItem>
                        <asp:ListItem Value="0" Text="Discontinued (Last Shopping Done)"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2 d-flex align-items-end">
                    <asp:Button ID="btnAddCategory" runat="server" Text="Add Category" CssClass="btn btn-primary w-100" ValidationGroup="AddCategory" OnClick="btnAddCategory_Click" />
                </div>
            </div>
        </div>
        
        <div class="d-flex justify-content-between align-items-center mb-3">
            <h4 class="mb-0">Categories List</h4>
            <div class="d-flex align-items-center gap-2">
                <label class="form-label mb-0 fw-semibold">Filter Status:</label>
                <asp:DropDownList ID="ddlFilterStatus" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlFilterStatus_SelectedIndexChanged" style="width: 190px;">
                    <asp:ListItem Value="ALL" Text="All Categories"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Active Only"></asp:ListItem>
                    <asp:ListItem Value="0" Text="Discontinued Only"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        
        <div class="grid-view table-responsive">
            <asp:GridView ID="gvCategories" runat="server" AutoGenerateColumns="False" 
                AllowPaging="True" PageSize="10" DataKeyNames="CategoryID"
                CssClass="grid-table table table-striped table-hover" GridLines="None"
                EmptyDataText="No categories found."
                OnRowEditing="gvCategories_RowEditing" OnRowCancelingEdit="gvCategories_RowCancelingEdit"
                OnRowUpdating="gvCategories_RowUpdating" OnRowDeleting="gvCategories_RowDeleting"
                OnRowCommand="gvCategories_RowCommand" OnRowDataBound="gvCategories_RowDataBound"
                OnPageIndexChanging="gvCategories_PageIndexChanging">
                
                <Columns>
                    <asp:BoundField DataField="CategoryID" HeaderText="ID" ReadOnly="True" ItemStyle-Width="60px" />
                    
                    <asp:TemplateField HeaderText="Category Name">
                        <ItemTemplate>
                            <span class="fw-semibold"><%# Eval("CategoryName") %></span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditCategoryName" runat="server" Text='<%# Bind("CategoryName") %>' CssClass="form-control form-control-sm"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEditCat" runat="server" ControlToValidate="txtEditCategoryName" ErrorMessage="Name is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <span><%# Eval("Description") %></span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtEditDescription" runat="server" Text='<%# Bind("Description") %>' CssClass="form-control form-control-sm"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="160px">
                        <ItemTemplate>
                            <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "status-badge status-active" : "status-badge status-discontinued" %>'
                                  title='<%# Convert.ToBoolean(Eval("IsActive")) ? "Active - Available for shopping" : "Discontinued - No new items can be added" %>'>
                                <%# Convert.ToBoolean(Eval("IsActive")) ? "ACTIVE" : "DISCONTINUED" %>
                            </span>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlEditStatus" runat="server" CssClass="form-select form-select-sm" style="width: 140px;">
                                <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                <asp:ListItem Value="0" Text="Discontinued"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Actions" ItemStyle-Width="260px">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnToggleStatus" runat="server" 
                                CommandName="ToggleStatus" 
                                CommandArgument='<%# Eval("CategoryID") %>' 
                                CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "btn btn-sm btn-outline-warning" : "btn btn-sm btn-outline-success" %>'
                                ToolTip='<%# Convert.ToBoolean(Eval("IsActive")) ? "Mark as Discontinued (Last shopping done)" : "Reactivate Category" %>'>
                                <%# Convert.ToBoolean(Eval("IsActive")) ? "<i class=\"bi bi-pause-circle\"></i> Discontinue" : "<i class=\"bi bi-play-circle\"></i> Activate" %>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" CssClass="btn btn-sm btn-secondary mx-1">
                                <i class="bi bi-pencil"></i> Edit
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="btn btn-sm btn-danger" 
                                OnClientClick="return confirm('Are you sure you want to delete this category and all its items?');">
                                <i class="bi bi-trash"></i> Delete
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" CssClass="btn btn-sm btn-primary mx-1">
                                <i class="bi bi-check-lg"></i> Update
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" CssClass="btn btn-sm btn-secondary">
                                <i class="bi bi-x-lg"></i> Cancel
                            </asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
</asp:Content>
