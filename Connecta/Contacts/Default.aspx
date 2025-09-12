<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Connecta.Contacts.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>مخاطبین من</h2>
        
        <div class="row mb-3">
            <div class="col-md-6">
                <div class="input-group">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="جستجو مخاطب..."></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="جستجو" CssClass="btn btn-outline-secondary" OnClick="btnSearch_Click" />
                </div>
            </div>
            <div class="col-md-6 text-end">
                <asp:Button ID="btnAddContact" runat="server" Text="افزودن مخاطب جدید" CssClass="btn btn-primary" 
                    OnClientClick="openAddModal()" />
                <input type="button" class="btn btn-primary" value="افزودن مخاطب" onclick="openAddModal()" />

            </div>
        </div>
        
        <div class="alert alert-info" runat="server" id="limitAlert" visible="false">
            شما <asp:Literal ID="litContactCount" runat="server"></asp:Literal> از 
            <asp:Literal ID="litContactLimit" runat="server"></asp:Literal> مخاطب مجاز را استفاده کرده‌اید.
        </div>
        
        <asp:GridView ID="gvContacts" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False"
            OnRowEditing="gvContacts_RowEditing" OnRowDeleting="gvContacts_RowDeleting" DataKeyNames="Id"
            OnRowUpdating="gvContacts_RowUpdating" OnRowCancelingEdit="gvContacts_RowCancelingEdit">
            <Columns>
                <asp:TemplateField HeaderText="نام">
                    <ItemTemplate>
                        <asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFirstName" runat="server" Text='<%# Bind("FirstName") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="نام خانوادگی">
                    <ItemTemplate>
                        <asp:Label ID="lblLastName" runat="server" Text='<%# Eval("LastName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtLastName" runat="server" Text='<%# Bind("LastName") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="شماره تلفن">
                    <ItemTemplate>
                        <asp:Label ID="lblPhoneNumber" runat="server" Text='<%# Eval("PhoneNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtPhoneNumber" runat="server" Text='<%# Bind("PhoneNumber") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ایمیل">
                    <ItemTemplate>
                        <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="form-control"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:CommandField ShowEditButton="True" ButtonType="Button" EditText="ویرایش" 
                    UpdateText="ذخیره" CancelText="انصراف" ShowDeleteButton="True" DeleteText="حذف" />
            </Columns>
            <EmptyDataTemplate>
                <div class="alert alert-warning">هیچ مخاطبی یافت نشد.</div>
            </EmptyDataTemplate>
        </asp:GridView>
        
        <!-- Modal برای افزودن مخاطب جدید -->
        <div class="modal fade" id="addContactModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">افزودن مخاطب جدید</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="mb-3">
                            <label for="txtNewFirstName" class="form-label">نام</label>
                            <asp:TextBox ID="txtNewFirstName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtNewLastName" class="form-label">نام خانوادگی</label>
                            <asp:TextBox ID="txtNewLastName" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtNewPhone" class="form-label">شماره تلفن</label>
                            <asp:TextBox ID="txtNewPhone" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label for="txtNewEmail" class="form-label">ایمیل</label>
                            <asp:TextBox ID="txtNewEmail" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">انصراف</button>
                        <asp:Button ID="btnSaveContact" runat="server" Text="ذخیره" CssClass="btn btn-primary" OnClick="btnSaveContact_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    

    <script>
        function openAddModal() {
            var myModal = new bootstrap.Modal(document.getElementById('addContactModal'));
            myModal.show();
        }
    </script>
</asp:Content>

