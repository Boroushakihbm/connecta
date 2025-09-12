<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="Connecta.Admin.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>پنل مدیریت</h2>
        
        <div class="row">
            <div class="col-md-3 mb-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">تعداد کاربران</h5>
                        <p class="card-text display-4"><asp:Literal ID="litTotalUsers" runat="server"></asp:Literal></p>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">تعداد مخاطبین</h5>
                        <p class="card-text display-4"><asp:Literal ID="litTotalContacts" runat="server"></asp:Literal></p>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">کاربران فعال</h5>
                        <p class="card-text display-4"><asp:Literal ID="litActiveUsers" runat="server"></asp:Literal></p>
                    </div>
                </div>
            </div>
            <div class="col-md-3 mb-3">
                <div class="card text-center">
                    <div class="card-body">
                        <h5 class="card-title">کاربران ادمین</h5>
                        <p class="card-text display-4"><asp:Literal ID="litAdminUsers" runat="server"></asp:Literal></p>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row mt-4">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>آمار کاربران</h5>
                    </div>
                    <div class="card-body">
                        <canvas id="usersChart" width="400" height="200"></canvas>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header">
                        <h5>آخرین کاربران ثبت‌نام شده</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="gvRecentUsers" runat="server" CssClass="table table-sm" AutoGenerateColumns="True"></asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row mt-4">
            <div class="col-12">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5>مدیریت کاربران</h5>
                        <asp:Button ID="btnAddUser" runat="server" Text="افزودن کاربر" CssClass="btn btn-sm btn-primary" OnClick="btnAddUser_Click" />
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="gvUsers" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                            OnRowEditing="gvUsers_RowEditing" OnRowDeleting="gvUsers_RowDeleting" DataKeyNames="Id"
                            OnRowUpdating="gvUsers_RowUpdating" OnRowCancelingEdit="gvUsers_RowCancelingEdit">
                            <Columns>
                                <asp:BoundField DataField="Username" HeaderText="نام کاربری" ReadOnly="true" />
                                <asp:BoundField DataField="Email" HeaderText="ایمیل" />
                                <asp:TemplateField HeaderText="ادمین">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsAdmin" runat="server" Checked='<%# Eval("IsAdmin") %>' Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chkIsAdminEdit" runat="server" Checked='<%# Bind("IsAdmin") %>' />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="محدودیت مخاطب">
                                    <ItemTemplate>
                                        <asp:Label ID="lblContactLimit" runat="server" Text='<%# Eval("ContactLimit") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtContactLimit" runat="server" Text='<%# Bind("ContactLimit") %>' CssClass="form-control"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="ویرایش" 
                                    UpdateText="ذخیره" CancelText="انصراف" ShowDeleteButton="True" DeleteText="حذف" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script>
        // ایجاد چارت کاربران
        var ctx = document.getElementById('usersChart').getContext('2d');
        var usersChart = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: ['کاربران عادی', 'کاربران ادمین', 'کل کاربران'],
                datasets: [{
                    label: 'تعداد کاربران',
                    data: [
                        <%= RegularUsersCount %>,
                        <%= AdminUsersCount %>,
                        <%= TotalUsersCount %>
                    ],
                    backgroundColor: [
                        'rgba(54, 162, 235, 0.2)',
                        'rgba(255, 99, 132, 0.2)',
                        'rgba(75, 192, 192, 0.2)'
                    ],
                    borderColor: [
                        'rgba(54, 162, 235, 1)',
                        'rgba(255, 99, 132, 1)',
                        'rgba(75, 192, 192, 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    </script>
</asp:Content>
