<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Connecta.Register" %>

<!DOCTYPE html>
<html dir="rtl">
<head>
    <title>ثبت‌نام در سیستم</title>
    <link href="Content/bootstrap.rtl.min.css" rel="stylesheet" />
    <style>
        body { 
            background-color: #f5f5f5; 
            padding-top: 50px; 
            direction: rtl;
        }
        .register-container { 
            max-width: 500px; 
            margin: 0 auto; 
            background: #fff; 
            padding: 20px; 
            border-radius: 5px; 
            box-shadow: 0 0 10px rgba(0,0,0,0.1); 
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-container">
            <h2 class="text-center mb-4">ثبت‌نام در سیستم دفتر تلفن</h2>
            
            <div class="mb-3">
                <label for="txtUsername" class="form-label">نام کاربری *</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" required="true"></asp:TextBox>
            </div>
            
            <div class="mb-3">
                <label for="txtPassword" class="form-label">کلمه عبور *</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" required="true"></asp:TextBox>
            </div>
            
            <div class="mb-3">
                <label for="txtEmail" class="form-label">ایمیل *</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" required="true"></asp:TextBox>
            </div>
            
            <div class="mb-3">
                <label for="txtPhoneNumber" class="form-label">شماره تلفن *</label>
                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" required="true"></asp:TextBox>
                <small class="text-muted">این شماره برای پیدا کردن شما توسط مخاطبین استفاده می‌شود</small>
            </div>
            
            <div class="mb-3">
                <label for="txtFirstName" class="form-label">نام</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            
            <div class="mb-3">
                <label for="txtLastName" class="form-label">نام خانوادگی</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            
            <asp:Button ID="btnRegister" runat="server" Text="ثبت‌نام" CssClass="btn btn-primary w-100" OnClick="btnRegister_Click" />
            
            <div class="mt-3 text-center">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
            </div>
            
            <div class="mt-3 text-center">
                <p>قبلاً حساب کاربری دارید؟ <a href="Login.aspx">وارد شوید</a></p>
            </div>
        </div>
    </form>
    
    <script src="Scripts/bootstrap.min.js"></script>
</body>
</html>
