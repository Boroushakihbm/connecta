<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Connecta.Login" %>

<!DOCTYPE html>
<html lang="fa">
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>ورود به سیستم</title>
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.rtl.min.css" rel="stylesheet">

    <style>
        body {
            background-color: #f5f5f5;
            direction: rtl;
        }

        .login-container {
            max-width: 400px;
            margin: 100px auto;
            padding: 20px;
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h2 class="text-center mb-4">ورود به دفتر تلفن</h2>

            <div class="mb-3">
                <label for="txtUsername" class="form-label">نام کاربری</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label for="txtPassword" class="form-label">کلمه عبور</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            </div>

            <div class="mb-3 form-check">
                <asp:CheckBox ID="chkRemember" runat="server" CssClass="form-check-input" />
                <label class="form-check-label" for="chkRemember">مرا به خاطر بسپار</label>
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="ورود" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
            <div class="mt-3 text-center">
                <p>حساب کاربری ندارید؟ <a href="Register.aspx">ثبت‌نام کنید</a></p>
            </div>
            <div class="mt-3 text-center">
                <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
            </div>
        </div>
    </form>
    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
