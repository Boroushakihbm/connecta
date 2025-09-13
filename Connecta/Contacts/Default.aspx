<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Connecta.Contacts.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div class="container mt-4">
        <div class="row mb-3">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header">
                        <h5>جستجوی مخاطبینم</h5>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="input-group">
                                    <input type="text" id="clientSearchInput" class="form-control" placeholder="جستجو در مخاطبین..." onkeyup="filterContacts()" />
                                    <button class="btn btn-outline-secondary" onclick="filterContacts()">جستجو</button>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="input-group setleft">
                                    <input type="button" class="btn btn-primary" value="افزودن مخاطب جدید +" onclick="openAddModal()" />
                                </div>
                            </div>
                        </div>
                        <div class="table-responsive mt-4">
                            <table id="contactsTable" class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th>نام</th>
                                        <th>نام خانوادگی</th>
                                        <th>شماره تلفن</th>
                                        <th>ایمیل</th>
                                        <th>عملیات</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptContacts" runat="server">
                                        <ItemTemplate>
                                            <tr data-id='<%# Eval("Id") %>'>
                                                <td><%# Eval("FirstName") %></td>
                                                <td><%# Eval("LastName") %></td>
                                                <td><%# Eval("PhoneNumber") %></td>
                                                <td><%# Eval("Email") %></td>
                                                <td>
                                                    <button class="btn btn-sm btn-warning" onclick="return editContact(<%# Eval("Id") %>, event)">ویرایش</button>
                                                    <button class="btn btn-sm btn-danger" onclick="return deleteContact(<%# Eval("Id") %>, event)">حذف</button>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                            <div id="noContacts" class="alert alert-warning" style="display: none;">هیچ مخاطبی یافت نشد.</div>
                            <div id="noSearchResults" class="alert alert-warning" style="display: none;">هیچ نتیجه‌ای برای جستجوی شما یافت نشد.</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-3">
            <div class="col-md-12">
                <div class="card">
                    <div class="card-header" style="background-color: antiquewhite">
                        <h5>جستجوی کاربران سیستم</h5>
                    </div>
                    <div class="card-body">
                        <div class="input-group">
                            <asp:TextBox ID="txtSearchPhone" runat="server" CssClass="form-control" placeholder="شماره تلفن کاربر"></asp:TextBox>
                            <asp:Button ID="btnSearchPhone" runat="server" Text="جستجو" CssClass="btn btn-outline-primary" OnClick="btnSearchPhone_Click" />
                        </div>
                        <div id="searchResults" runat="server" class="mt-3">
                            <!-- نتایج جستجو نمایش داده می‌شود -->
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="alert alert-info" runat="server" id="limitAlert" visible="false">
            شما
            <asp:Literal ID="litContactCount" runat="server"></asp:Literal>
            از 
            <asp:Literal ID="litContactLimit" runat="server"></asp:Literal>
            مخاطب مجاز را استفاده کرده‌اید.
        </div>

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

        <!-- Modal برای ویرایش مخاطب -->
        <div class="modal fade" id="editContactModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">ویرایش مخاطب</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <input type="hidden" id="editContactId" />
                        <div class="mb-3">
                            <label for="editFirstName" class="form-label">نام</label>
                            <input type="text" id="editFirstName" class="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="editLastName" class="form-label">نام خانوادگی</label>
                            <input type="text" id="editLastName" class="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="editPhoneNumber" class="form-label">شماره تلفن</label>
                            <input type="text" id="editPhoneNumber" class="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="editEmail" class="form-label">ایمیل</label>
                            <input type="email" id="editEmail" class="form-control" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">انصراف</button>
                        <button type="button" class="btn btn-primary" onclick="saveEditedContact(); return false;">ذخیره تغییرات</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>

        function filterContacts() {
            const searchText = document.getElementById('clientSearchInput').value.toLowerCase();
            const rows = document.querySelectorAll('#contactsTable tbody tr');
            let hasVisibleRows = false;
            let hasAnyRows = rows.length > 0;

            rows.forEach(row => {
                const firstName = row.cells[0].textContent.toLowerCase();
                const lastName = row.cells[1].textContent.toLowerCase();
                const phoneNumber = row.cells[2].textContent.toLowerCase();
                const email = row.cells[3].textContent.toLowerCase();

                if (firstName.includes(searchText) ||
                    lastName.includes(searchText) ||
                    phoneNumber.includes(searchText) ||
                    email.includes(searchText)) {
                    row.style.display = '';
                    hasVisibleRows = true;
                } else {
                    row.style.display = 'none';
                }
            });

            // نمایش پیام مناسب
            document.getElementById('noContacts').style.display = 'none';
            document.getElementById('noSearchResults').style.display = 'none';

            if (!hasAnyRows) {
                document.getElementById('noContacts').style.display = 'block';
            } else if (!hasVisibleRows) {
                document.getElementById('noSearchResults').style.display = 'block';
            }
        }

        // تابع برای باز کردن مودال ویرایش
        function editContact(id, event) {
            if (event) {
                event.preventDefault();
                event.stopPropagation();
            }

            const row = document.querySelector(`tr[data-id="${id}"]`);
            if (row) {
                document.getElementById('editContactId').value = id;
                document.getElementById('editFirstName').value = row.cells[0].textContent;
                document.getElementById('editLastName').value = row.cells[1].textContent;
                document.getElementById('editPhoneNumber').value = row.cells[2].textContent;
                document.getElementById('editEmail').value = row.cells[3].textContent;

                var editModal = new bootstrap.Modal(document.getElementById('editContactModal'));
                editModal.show();
            }

            return false;
        }

        function saveEditedContact() {
            const id = document.getElementById('editContactId').value;
            const firstName = document.getElementById('editFirstName').value;
            const lastName = document.getElementById('editLastName').value;
            const phoneNumber = document.getElementById('editPhoneNumber').value;
            const email = document.getElementById('editEmail').value;

            const saveButton = document.querySelector('#editContactModal .btn-primary');
            const originalText = saveButton.textContent;
            saveButton.textContent = 'در حال ذخیره...';
            saveButton.disabled = true;

            PageMethods.UpdateContact(id, firstName, lastName, phoneNumber, email,
                function (result) {
                    if (result === "success") {
                        const row = document.querySelector(`tr[data-id="${id}"]`);
                        if (row) {
                            row.cells[0].textContent = firstName;
                            row.cells[1].textContent = lastName;
                            row.cells[2].textContent = phoneNumber;
                            row.cells[3].textContent = email;
                        }

                        var editModal = bootstrap.Modal.getInstance(document.getElementById('editContactModal'));
                        editModal.hide();

                        showAlert('مخاطب با موفقیت به‌روزرسانی شد', 'success');
                    } else {
                        showAlert('خطا در به‌روزرسانی مخاطب', 'danger');
                    }

                    saveButton.textContent = originalText;
                    saveButton.disabled = false;
                },
                function (error) {
                    showAlert('خطا در ارتباط با سرور', 'danger');
                    saveButton.textContent = originalText;
                    saveButton.disabled = false;
                }
            );
        }

        function deleteContact(id, event) {
            if (event) {
                event.preventDefault();
                event.stopPropagation();
            }

            if (confirm('آیا از حذف این مخاطب اطمینان دارید؟')) {
                PageMethods.DeleteContact(id,
                    function (result) {
                        if (result === "success") {
                            const row = document.querySelector(`tr[data-id="${id}"]`);
                            if (row) {
                                row.remove();
                            }

                            filterContacts();

                            showAlert('مخاطب با موفقیت حذف شد', 'success');
                        } else {
                            showAlert('خطا در حذف مخاطب', 'danger');
                        }
                    },
                    function (error) {
                        showAlert('خطا در ارتباط با سرور', 'danger');
                    }
                );
            }

            return false;
        }

        function openAddModal() {
            var myModal = new bootstrap.Modal(document.getElementById('addContactModal'));
            myModal.show();
            return false;
        }

        function showAlert(message, type) {
            const alertDiv = document.createElement('div');
            alertDiv.className = `alert alert-${type} alert-dismissible fade show fixed-top m-3`;
            alertDiv.style.zIndex = '9999';
            alertDiv.innerHTML = `
                                    <div class="d-flex justify-content-between align-items-center">
                                        <div>
                                            <button type="button" class="btn-close me-2" data-bs-dismiss="alert" aria-label="Close"></button>
                                        </div>
                                        <div class="flex-grow-1">
                                            ${message}
                                        </div>
                                    </div>
                                `;

            document.body.appendChild(alertDiv);

            setTimeout(() => {
                if (alertDiv.parentNode) {
                    alertDiv.remove();
                }
            }, 3000);
        }

        document.addEventListener('DOMContentLoaded', function () {
            filterContacts();
        });
    </script>
</asp:Content>
