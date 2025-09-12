<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="Connecta.Chat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>سیستم چت</h2>
        
        <div class="row">
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header">
                        <h5>مخاطبین</h5>
                    </div>
                    <div class="card-body">
                        <asp:ListBox ID="lstContacts" runat="server" CssClass="form-control" Height="300px" 
                            AutoPostBack="true" OnSelectedIndexChanged="lstContacts_SelectedIndexChanged"></asp:ListBox>
                    </div>
                </div>
            </div>
            
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 id="chatTitle">انتخاب مخاطب</h5>
                        <span id="onlineStatus" class="badge bg-secondary">آفلاین</span>
                    </div>
                    <div class="card-body">
                        <div id="chatMessages" style="height: 300px; overflow-y: scroll; margin-bottom: 15px; padding: 10px; border: 1px solid #ddd; border-radius: 5px;">
                            <!-- پیام‌ها在这里显示 خواهد شد -->
                        </div>
                        
                        <div class="input-group">
                            <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" placeholder="پیام خود را وارد کنید..."></asp:TextBox>
                            <asp:Button ID="btnSend" runat="server" Text="ارسال" CssClass="btn btn-primary" OnClick="btnSend_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- Reference the SignalR library. -->
    
    <script src="/signalr/hubs"></script>
    
    <script type="text/javascript">
        var chatHub = $.connection.chatHub;
        var currentReceiverId = 0;
        
        // دریافت پیام جدید
        chatHub.client.receiveMessage = function (senderId, message, timestamp) {
            if (senderId == currentReceiverId) {
                addMessage(senderId, message, timestamp, false);
            }
        };
        
        // تأیید ارسال پیام
        chatHub.client.messageSent = function (messageId) {
            console.log('پیام با موفقیت ارسال شد: ' + messageId);
        };
        
        // شروع اتصال
        $.connection.hub.qs = { 'userId': '<%= Session["UserId"] %>' };
        $.connection.hub.start().done(function () {
            console.log('اتصال به چت برقرار شد');
        });
        
        // افزودن پیام به صفحه
        function addMessage(senderId, message, timestamp, isMyMessage) {
            var messageClass = isMyMessage ? 'alert-primary' : 'alert-secondary';
            var alignClass = isMyMessage ? 'text-left' : 'text-right';
            var time = new Date(timestamp).toLocaleTimeString('fa-IR');
            
            var messageHtml = '<div class="alert ' + messageClass + ' ' + alignClass + '">' +
                                '<div>' + message + '</div>' +
                                '<small class="text-muted">' + time + '</small>' +
                              '</div>';
            
            $('#chatMessages').append(messageHtml);
            $('#chatMessages').scrollTop($('#chatMessages')[0].scrollHeight);
        }
        
        // انتخاب مخاطب
        function selectContact(contactId, contactName) {
            currentReceiverId = contactId;
            $('#chatTitle').text('چت با ' + contactName);
            $('#onlineStatus').removeClass('bg-secondary bg-success').addClass('bg-success').text('آنلاین');
            
            // بارگذاری تاریخچه چت
            loadChatHistory(contactId);
        }
        
        // بارگذاری تاریخچه چت
        function loadChatHistory(contactId) {
            $.ajax({
                url: 'Chat.aspx/GetChatHistory',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ contactId: contactId }),
                dataType: 'json',
                success: function (response) {
                    $('#chatMessages').empty();
                    var messages = response.d;
                    
                    for (var i = 0; i < messages.length; i++) {
                        var message = messages[i];
                        var isMyMessage = message.SenderId == '<%= Session["UserId"] %>';
                        addMessage(message.SenderId, message.Content, message.Timestamp, isMyMessage);
                    }
                }
            });
        }
        
        // ارسال پیام
        function sendMessage() {
            var message = $('#<%= txtMessage.ClientID %>').val();
            if (message && currentReceiverId) {
                chatHub.server.sendMessage(currentReceiverId, message);
                addMessage('<%= Session["UserId"] %>', message, new Date(), true);
                $('#<%= txtMessage.ClientID %>').val('');
            }
        }
        
        // رویداد کلیک دکمه ارسال
        $('#<%= btnSend.ClientID %>').click(function (e) {
            e.preventDefault();
            sendMessage();
        });
        
        // ارسال با کلید Enter
        $('#<%= txtMessage.ClientID %>').keypress(function (e) {
            if (e.which == 13) {
                e.preventDefault();
                sendMessage();
            }
        });
    </script>
</asp:Content>
