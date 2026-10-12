<%@ Page Title="Edit Item" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditItem.aspx.cs" Inherits="HomeInventoryWarrantyTracker.Inventory.EditItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>Edit Item</h2>
    </div>

    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" ForeColor="Red"></asp:Label>

    <div class="form-section">
        <h3>Product Information</h3>
        
        <div class="form-group">
            <label>Product Name: *</label>
            <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="e.g. Sony Bravia 55 inch TV"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName" ErrorMessage="Product name is required" ForeColor="Red" ValidationGroup="EditItem" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        
        <div class="form-group">
            <label>Category: *</label>
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ControlToValidate="ddlCategory" InitialValue="" ErrorMessage="Please select a category" ForeColor="Red" ValidationGroup="EditItem" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        
        <div class="form-group">
            <label>Brand:</label>
            <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control" placeholder="e.g. Sony, Apple, Samsung"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label>Model Number:</label>
            <asp:TextBox ID="txtModelNumber" runat="server" CssClass="form-control" placeholder="e.g. KD-55X80L"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label>Serial Number:</label>
            <asp:TextBox ID="txtSerialNumber" runat="server" CssClass="form-control" placeholder="e.g. SN-8947291"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label>Description:</label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="Additional details, specifications or notes..."></asp:TextBox>
        </div>
    </div>

    <div class="form-section">
        <h3>Purchase Information</h3>
        
        <div class="form-group">
            <label>Purchase Date: *</label>
            <asp:TextBox ID="txtPurchaseDate" runat="server" CssClass="form-control mb-2" placeholder="DD/MM/YYYY" style="max-width:340px;"></asp:TextBox>
            <div class="calendar-card">
                <div class="cal-nav-bar">
                    <asp:DropDownList ID="ddlPurchaseMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="calPurchaseDate_NavChanged" CssClass="form-select cal-select-month">
                        <asp:ListItem Value="1" Text="January"></asp:ListItem>
                        <asp:ListItem Value="2" Text="February"></asp:ListItem>
                        <asp:ListItem Value="3" Text="March"></asp:ListItem>
                        <asp:ListItem Value="4" Text="April"></asp:ListItem>
                        <asp:ListItem Value="5" Text="May"></asp:ListItem>
                        <asp:ListItem Value="6" Text="June"></asp:ListItem>
                        <asp:ListItem Value="7" Text="July"></asp:ListItem>
                        <asp:ListItem Value="8" Text="August"></asp:ListItem>
                        <asp:ListItem Value="9" Text="September"></asp:ListItem>
                        <asp:ListItem Value="10" Text="October"></asp:ListItem>
                        <asp:ListItem Value="11" Text="November"></asp:ListItem>
                        <asp:ListItem Value="12" Text="December"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlPurchaseYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="calPurchaseDate_NavChanged" CssClass="form-select cal-select-year">
                    </asp:DropDownList>
                </div>
                <asp:Calendar ID="calPurchaseDate" runat="server" 
                    OnSelectionChanged="calPurchaseDate_SelectionChanged"
                    OnVisibleMonthChanged="calPurchaseDate_VisibleMonthChanged"
                    TitleFormat="MonthYear"
                    NextPrevFormat="FullMonth"
                    DayNameFormat="Short"
                    FirstDayOfWeek="Sunday"
                    ShowGridLines="false"
                    CssClass="modern-calendar"
                    SelectionMode="Day">
                    <TitleStyle CssClass="cal-title" />
                    <DayHeaderStyle CssClass="cal-dayheader" />
                    <DayStyle CssClass="cal-day" />
                    <SelectedDayStyle CssClass="cal-selected" />
                    <OtherMonthDayStyle CssClass="cal-othermonth" />
                    <TodayDayStyle CssClass="cal-today" />
                    <NextPrevStyle CssClass="cal-nextprev" />
                </asp:Calendar>
            </div>
            <asp:RequiredFieldValidator ID="rfvPurchaseDate" runat="server" ControlToValidate="txtPurchaseDate" ErrorMessage="Purchase date is required" ForeColor="Red" ValidationGroup="EditItem" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        
        <div class="form-group">
            <label>Purchase Price (&#8377;): *</label>
            <asp:TextBox ID="txtPurchasePrice" runat="server" CssClass="form-control" placeholder="e.g. 45000"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPurchasePrice" ErrorMessage="Purchase price is required" ForeColor="Red" ValidationGroup="EditItem" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="rvPrice" runat="server" ControlToValidate="txtPurchasePrice" MinimumValue="0" MaximumValue="10000000" Type="Double" ErrorMessage="Invalid Price (must be 0 or greater)" ForeColor="Red" ValidationGroup="EditItem" Display="Dynamic"></asp:RangeValidator>
        </div>
        
        <div class="form-group">
            <label>Seller:</label>
            <asp:DropDownList ID="ddlSeller" runat="server" CssClass="form-select" onchange="checkCustomSeller()"></asp:DropDownList>
        </div>
        <div id="divCustomSeller" class="form-group" style="display:none; margin-top: 10px;">
            <label>Custom Seller Name: <span class="text-danger">*</span></label>
            <asp:TextBox ID="txtCustomSeller" runat="server" CssClass="form-control" placeholder="Enter seller / store name"></asp:TextBox>
        </div>
        
        <div class="form-group">
            <label>Location in Home:</label>
            <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-select">
                <asp:ListItem Text="Living Room" Value="Living Room"></asp:ListItem>
                <asp:ListItem Text="Bedroom" Value="Bedroom"></asp:ListItem>
                <asp:ListItem Text="Kitchen" Value="Kitchen"></asp:ListItem>
                <asp:ListItem Text="Office" Value="Office"></asp:ListItem>
                <asp:ListItem Text="Bathroom" Value="Bathroom"></asp:ListItem>
                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
            </asp:DropDownList>
        </div>
        
        <div class="form-group">
            <label>Item Condition:</label>
            <asp:RadioButtonList ID="rblCondition" runat="server" RepeatDirection="Horizontal" CssClass="radio-list">
                <asp:ListItem Text="New" Value="New"></asp:ListItem>
                <asp:ListItem Text="Used" Value="Used"></asp:ListItem>
                <asp:ListItem Text="Refurbished" Value="Refurbished"></asp:ListItem>
            </asp:RadioButtonList>
        </div>
    </div>

    <div class="form-section">
        <h3>Warranty Information</h3>
        
        <div class="form-group">
            <asp:CheckBox ID="chkHasWarranty" runat="server" Text=" &nbsp;This product has a warranty" AutoPostBack="true" OnCheckedChanged="chkHasWarranty_CheckedChanged" Font-Bold="true" />
        </div>
        
        <asp:Panel ID="pnlWarranty" runat="server" Visible="false">
            <div class="form-group">
                <label>Warranty Type:</label>
                <asp:DropDownList ID="ddlWarrantyType" runat="server" CssClass="form-select" onchange="checkCustomWarrantyType()">
                    <asp:ListItem Text="Manufacturer" Value="Manufacturer"></asp:ListItem>
                    <asp:ListItem Text="Extended" Value="Extended"></asp:ListItem>
                    <asp:ListItem Text="Seller" Value="Seller"></asp:ListItem>
                    <asp:ListItem Text="Third Party" Value="Third Party"></asp:ListItem>
                    <asp:ListItem Text="Custom (Type Manually)" Value="CUSTOM"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div id="divCustomWarrantyType" class="form-group" style="display:none; margin-top: 10px;">
                <label>Custom Warranty Type: <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtCustomWarrantyType" runat="server" CssClass="form-control" placeholder="Enter warranty type (e.g. Accidental Damage, Dealer Care)"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Provider Name:</label>
                <asp:TextBox ID="txtProvider" runat="server" CssClass="form-control" placeholder="e.g. Sony India, AppleCare"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Warranty Start Date:</label>
                <asp:TextBox ID="txtWarrantyStart" runat="server" CssClass="form-control mb-2" placeholder="DD/MM/YYYY" style="max-width:340px;"></asp:TextBox>
                <div class="calendar-card">
                    <div class="cal-nav-bar">
                        <asp:DropDownList ID="ddlWarrantyStartMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="calWarrantyStart_NavChanged" CssClass="form-select cal-select-month">
                            <asp:ListItem Value="1" Text="January"></asp:ListItem>
                            <asp:ListItem Value="2" Text="February"></asp:ListItem>
                            <asp:ListItem Value="3" Text="March"></asp:ListItem>
                            <asp:ListItem Value="4" Text="April"></asp:ListItem>
                            <asp:ListItem Value="5" Text="May"></asp:ListItem>
                            <asp:ListItem Value="6" Text="June"></asp:ListItem>
                            <asp:ListItem Value="7" Text="July"></asp:ListItem>
                            <asp:ListItem Value="8" Text="August"></asp:ListItem>
                            <asp:ListItem Value="9" Text="September"></asp:ListItem>
                            <asp:ListItem Value="10" Text="October"></asp:ListItem>
                            <asp:ListItem Value="11" Text="November"></asp:ListItem>
                            <asp:ListItem Value="12" Text="December"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlWarrantyStartYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="calWarrantyStart_NavChanged" CssClass="form-select cal-select-year">
                        </asp:DropDownList>
                    </div>
                    <asp:Calendar ID="calWarrantyStart" runat="server" 
                        OnSelectionChanged="calWarrantyStart_SelectionChanged"
                        OnVisibleMonthChanged="calWarrantyStart_VisibleMonthChanged"
                        TitleFormat="MonthYear"
                        NextPrevFormat="FullMonth"
                        DayNameFormat="Short"
                        FirstDayOfWeek="Sunday"
                        ShowGridLines="false"
                        CssClass="modern-calendar"
                        SelectionMode="Day">
                        <TitleStyle CssClass="cal-title" />
                        <DayHeaderStyle CssClass="cal-dayheader" />
                        <DayStyle CssClass="cal-day" />
                        <SelectedDayStyle CssClass="cal-selected" />
                        <OtherMonthDayStyle CssClass="cal-othermonth" />
                        <TodayDayStyle CssClass="cal-today" />
                        <NextPrevStyle CssClass="cal-nextprev" />
                    </asp:Calendar>
                </div>
            </div>
            
            <div class="form-group">
                <label>Warranty Period:</label>
                <asp:DropDownList ID="ddlWarrantyPeriod" runat="server" CssClass="form-select" onchange="checkCustomWarrantyPeriod()">
                    <asp:ListItem Text="6 months" Value="6"></asp:ListItem>
                    <asp:ListItem Text="12 months (1 Year)" Value="12"></asp:ListItem>
                    <asp:ListItem Text="18 months" Value="18"></asp:ListItem>
                    <asp:ListItem Text="24 months (2 Years)" Value="24"></asp:ListItem>
                    <asp:ListItem Text="36 months (3 Years)" Value="36"></asp:ListItem>
                    <asp:ListItem Text="48 months (4 Years)" Value="48"></asp:ListItem>
                    <asp:ListItem Text="60 months (5 Years)" Value="60"></asp:ListItem>
                    <asp:ListItem Text="Custom (Enter Months)" Value="CUSTOM"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <div id="divCustomWarrantyPeriod" class="form-group" style="display:none; margin-top: 10px;">
                <label>Custom Warranty Period (in Months): <span class="text-danger">*</span></label>
                <asp:TextBox ID="txtCustomWarrantyPeriod" runat="server" CssClass="form-control" TextMode="Number" min="1" max="600" placeholder="e.g. 15, 30, 72" oninput="calculateCustomExpiry()"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Warranty Expiry Date:</label>
                <asp:TextBox ID="txtWarrantyExpiry" runat="server" CssClass="form-control mb-2" placeholder="DD/MM/YYYY" style="max-width:340px;"></asp:TextBox>
                <div class="calendar-card">
                    <div class="cal-nav-bar">
                        <asp:DropDownList ID="ddlWarrantyExpiryMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="calWarrantyExpiry_NavChanged" CssClass="form-select cal-select-month">
                            <asp:ListItem Value="1" Text="January"></asp:ListItem>
                            <asp:ListItem Value="2" Text="February"></asp:ListItem>
                            <asp:ListItem Value="3" Text="March"></asp:ListItem>
                            <asp:ListItem Value="4" Text="April"></asp:ListItem>
                            <asp:ListItem Value="5" Text="May"></asp:ListItem>
                            <asp:ListItem Value="6" Text="June"></asp:ListItem>
                            <asp:ListItem Value="7" Text="July"></asp:ListItem>
                            <asp:ListItem Value="8" Text="August"></asp:ListItem>
                            <asp:ListItem Value="9" Text="September"></asp:ListItem>
                            <asp:ListItem Value="10" Text="October"></asp:ListItem>
                            <asp:ListItem Value="11" Text="November"></asp:ListItem>
                            <asp:ListItem Value="12" Text="December"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlWarrantyExpiryYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="calWarrantyExpiry_NavChanged" CssClass="form-select cal-select-year">
                        </asp:DropDownList>
                    </div>
                    <asp:Calendar ID="calWarrantyExpiry" runat="server" 
                        OnSelectionChanged="calWarrantyExpiry_SelectionChanged"
                        OnVisibleMonthChanged="calWarrantyExpiry_VisibleMonthChanged"
                        TitleFormat="MonthYear"
                        NextPrevFormat="FullMonth"
                        DayNameFormat="Short"
                        FirstDayOfWeek="Sunday"
                        ShowGridLines="false"
                        CssClass="modern-calendar"
                        SelectionMode="Day">
                        <TitleStyle CssClass="cal-title" />
                        <DayHeaderStyle CssClass="cal-dayheader" />
                        <DayStyle CssClass="cal-day" />
                        <SelectedDayStyle CssClass="cal-selected" />
                        <OtherMonthDayStyle CssClass="cal-othermonth" />
                        <TodayDayStyle CssClass="cal-today" />
                        <NextPrevStyle CssClass="cal-nextprev" />
                    </asp:Calendar>
                </div>
            </div>
            
            <div class="form-group">
                <label>Warranty Terms & Conditions:</label>
                <asp:TextBox ID="txtWarrantyTerms" runat="server" TextMode="MultiLine" Rows="3" CssClass="form-control" placeholder="e.g. Covers manufacturing defects only, carry-in service"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label>Support / Contact Info:</label>
                <asp:TextBox ID="txtWarrantyContact" runat="server" CssClass="form-control" placeholder="Toll-free number, website or email"></asp:TextBox>
            </div>
        </asp:Panel>
    </div>

    <div style="margin-top: 20px; display: flex; gap: 10px;">
        <asp:Button ID="btnUpdate" runat="server" Text="Update Item" CssClass="btn btn-primary" OnClick="btnUpdate_Click" ValidationGroup="EditItem" />
        <a href="Inventory.aspx" class="btn btn-secondary">Cancel</a>
    </div>

    <script type="text/javascript">
        function checkCustomSeller() {
            var ddl = document.getElementById('<%= ddlSeller.ClientID %>');
            var div = document.getElementById('divCustomSeller');
            if (ddl && div) {
                div.style.display = (ddl.value === 'CUSTOM') ? 'block' : 'none';
            }
        }
        function checkCustomWarrantyType() {
            var ddl = document.getElementById('<%= ddlWarrantyType.ClientID %>');
            var div = document.getElementById('divCustomWarrantyType');
            if (ddl && div) {
                div.style.display = (ddl.value === 'CUSTOM') ? 'block' : 'none';
            }
        }
        function checkCustomWarrantyPeriod() {
            var ddl = document.getElementById('<%= ddlWarrantyPeriod.ClientID %>');
            var div = document.getElementById('divCustomWarrantyPeriod');
            if (ddl && div) {
                div.style.display = (ddl.value === 'CUSTOM') ? 'block' : 'none';
                if (ddl.value === 'CUSTOM') {
                    calculateCustomExpiry();
                }
            }
        }
        function calculateCustomExpiry() {
            var ddl = document.getElementById('<%= ddlWarrantyPeriod.ClientID %>');
            if (!ddl || ddl.value !== 'CUSTOM') return;
            var txtMonths = document.getElementById('<%= txtCustomWarrantyPeriod.ClientID %>');
            var txtStart = document.getElementById('<%= txtWarrantyStart.ClientID %>');
            var txtExpiry = document.getElementById('<%= txtWarrantyExpiry.ClientID %>');
            if (!txtMonths || !txtStart || !txtExpiry || !txtMonths.value || !txtStart.value) return;
            
            var parts = txtStart.value.split('/');
            if (parts.length === 3) {
                var d = parseInt(parts[0], 10);
                var m = parseInt(parts[1], 10) - 1;
                var y = parseInt(parts[2], 10);
                var months = parseInt(txtMonths.value, 10);
                if (!isNaN(d) && !isNaN(m) && !isNaN(y) && !isNaN(months) && months > 0) {
                    var startDate = new Date(y, m, d);
                    startDate.setMonth(startDate.getMonth() + months);
                    var day = ('0' + startDate.getDate()).slice(-2);
                    var month = ('0' + (startDate.getMonth() + 1)).slice(-2);
                    var year = startDate.getFullYear();
                    txtExpiry.value = day + '/' + month + '/' + year;
                }
            }
        }
        document.addEventListener('DOMContentLoaded', function () {
            checkCustomSeller();
            checkCustomWarrantyType();
            checkCustomWarrantyPeriod();
        });
        if (typeof Sys !== 'undefined' && Sys.WebForms && Sys.WebForms.PageRequestManager) {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                checkCustomSeller();
                checkCustomWarrantyType();
                checkCustomWarrantyPeriod();
            });
        }
    </script>

</asp:Content>
