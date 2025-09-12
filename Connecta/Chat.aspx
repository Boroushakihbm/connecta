<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Chat.aspx.cs" Inherits="Connecta.Chat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>سیستم چت</h2>
        
        <div class="row">
            <!-- سایدبار مخاطبین -->
            <div class="col-md-4">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5>مخاطبین</h5>
                        <button class="btn btn-sm btn-primary" data-bs-toggle="modal" data-bs-target="#searchModal">
                            <i class="fas fa-search"></i> جستجو
                        </button>
                    </div>
                    <div class="card-body">
                        <div class="list-group" id="contactsList">
                            
                        </div>
                    </div>
                </div>
            </div>
            
            <!-- بخش چت -->
            <div class="col-md-8">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 id="chatTitle">انتخاب مخاطب</h5>
                        <div>
                            <span id="onlineStatus" class="badge bg-secondary">آفلاین</span>
                            <button id="btnUserInfo" class="btn btn-sm btn-info ms-2" style="display:none;">
                                <i class="fas fa-info-circle"></i>
                            </button>
                        </div>
                    </div>
                    <div class="card-body">
                        <div id="chatMessages" style="height: 300px; overflow-y: auto; margin-bottom: 15px; padding: 10px; border: 1px solid #ddd; border-radius: 5px;">
                            <div class="text-center text-muted">یک مخاطب را برای شروع چت انتخاب کنید</div>
                        </div>
                        
                        <div class="input-group" id="messageInput" style="display: none;">
                            <textarea id="txtMessage" class="form-control" placeholder="پیام خود را وارد کنید..." rows="2"></textarea>
                            <button id="btnSend" class="btn btn-primary">ارسال</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- مودال جستجوی کاربران -->
    <div class="modal fade" id="searchModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">جستجوی کاربران</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="input-group mb-3">
                        <input type="text" id="txtSearch" class="form-control" placeholder="نام کاربری یا ایمیل">
                        <button id="btnSearch" class="btn btn-primary">جستجو</button>
                    </div>
                    <div id="searchResults" class="mt-3">
                        <!-- نتایج جستجو在这里 نمایش داده می‌شود -->
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- اسکریپت‌ها -->
    <script src="/Scripts/jquery-3.6.0.min.js"></script>
    <script src="/Scripts/jquery.signalR-2.4.3.min.js"></script>
    <script src="/signalr/hubs"></script>
    <script src="https://kit.fontawesome.com/your-fontawesome-kit.js" crossorigin="anonymous"></script>
    
    <script type="text/javascript">
        var chatHub = $.connection.chatHub;
        var currentChatUserId = 0;

        // شروع اتصال
        $.connection.hub.qs = { 'userId': '<%= Session["UserId"] %>' };
        $.connection.hub.start().done(function () {
            console.log('اتصال به چت برقرار شد');
            loadUserContacts();
        });

        // دریافت پیام جدید
        chatHub.client.receiveMessage = function (senderId, message, timestamp, messageId) {
            if (senderId == currentChatUserId) {
                addMessage(senderId, message, timestamp, false);
                // علامت گذاری به عنوان خوانده شده
                chatHub.server.markAsRead(messageId);
            }
        };

        // تغییر وضعیت آنلاین/آفلاین
        chatHub.client.userStatusChanged = function (userId, isOnline) {
            if (userId == currentChatUserId) {
                updateUserStatus(isOnline);
            }
            updateContactStatus(userId, isOnline);
        };

        // نمایش نتایج جستجو
        chatHub.client.showSearchResults = function (users) {
            var html = '';
            if (users.length > 0) {
                html += '<div class="list-group">';
                users.forEach(function (user) {
                    html += '<div class="list-group-item d-flex justify-content-between align-items-center">';
                    html += '<div>';
                    html += '<strong>' + user.Username + '</strong><br>';
                    html += '<small class="text-muted">' + user.Email + '</small>';
                    html += '</div>';
                    html += '<button class="btn btn-sm btn-success add-contact" data-userid="' + user.Id + '">';
                    html += '<i class="fas fa-plus"></i> افزودن';
                    html += '</button>';
                    html += '</div>';
                });
                html += '</div>';
            } else {
                html = '<div class="alert alert-info">هیچ کاربری یافت نشد.</div>';
            }
            $('#searchResults').html(html);
        };

        // بارگذاری مخاطبین کاربر
        function loadUserContacts() {
            $.ajax({
                url: 'Chat.aspx/GetUserContacts',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var contacts = response.d;
                    var html = '';

                    if (contacts.length > 0) {
                        contacts.forEach(function (contact) {
                            var statusClass = contact.IsOnline ? 'bg-success' : 'bg-secondary';
                            var statusText = contact.IsOnline ? 'آنلاین' : 'آفلاین';

                            html += '<a href="#" class="list-group-item list-group-item-action contact-item" data-userid="' + contact.Id + '">';
                            html += '<div class="d-flex justify-content-between align-items-center">';
                            html += '<div>';
                            html += '<strong>' + (contact.Nickname || contact.Username) + '</strong>';
                            html += '</div>';
                            html += '<span class="badge ' + statusClass + '">' + statusText + '</span>';
                            html += '</div>';
                            html += '</a>';
                        });
                    } else {
                        html = '<div class="text-center text-muted">هیچ مخاطبی وجود ندارد</div>';
                    }

                    $('#contactsList').html(html);
                }
            });
        }

        // شروع چت با یک کاربر
        function startChat(userId, username) {
            currentChatUserId = userId;
            $('#chatTitle').text('چت با ' + username);
            $('#messageInput').show();
            $('#btnUserInfo').show();

            // بارگذاری تاریخچه چت
            loadChatHistory(userId);

            // دریافت وضعیت آنلاین بودن
            getUserStatus(userId);
        }

        // بارگذاری تاریخچه چت
        function loadChatHistory(userId) {
            $.ajax({
                url: 'Chat.aspx/GetChatHistory',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ userId: userId }),
                dataType: 'json',
                success: function (response) {
                    $('#chatMessages').empty();
                    var messages = response.d;

                    if (messages.length > 0) {
                        messages.forEach(function (message) {
                            var isMyMessage = message.SenderId == '<%= Session["UserId"] %>';
                            addMessage(message.SenderId, message.Content, message.Timestamp, isMyMessage);
                        });
                    } else {
                        $('#chatMessages').html('<div class="text-center text-muted">هیچ پیامی وجود ندارد</div>');
                    }
                }
            });
        }
        
        // افزودن پیام به صفحه
        function addMessage(senderId, message, timestamp, isMyMessage) {
            var messageClass = isMyMessage ? 'alert-primary text-left' : 'alert-secondary text-right';
            var time = new Date(parseInt(timestamp.substr(6))).toLocaleTimeString('fa-IR');
            
            var messageHtml = '<div class="alert ' + messageClass + ' mb-2">';
            messageHtml += '<div class="message-content">' + message + '</div>';
            messageHtml += '<small class="text-muted">' + time + '</small>';
            messageHtml += '</div>';
            
            $('#chatMessages').append(messageHtml);
            $('#chatMessages').scrollTop($('#chatMessages')[0].scrollHeight);
        }
        
        // ارسال پیام
        function sendMessage() {
            var message = $('#txtMessage').val().trim();
            if (message && currentChatUserId) {
                chatHub.server.sendMessage(currentChatUserId, message);
                addMessage('<%= Session["UserId"] %>', message, new Date(), true);
                $('#txtMessage').val('');
            }
        }

        // دریافت وضعیت کاربر
        function getUserStatus(userId) {
            $.ajax({
                url: 'Chat.aspx/GetUserStatus',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ userId: userId }),
                dataType: 'json',
                success: function (response) {
                    updateUserStatus(response.d);
                }
            });
        }

        // به‌روزرسانی وضعیت کاربر
        function updateUserStatus(isOnline) {
            var status = isOnline ? 'آنلاین' : 'آفلاین';
            var badgeClass = isOnline ? 'bg-success' : 'bg-secondary';
            $('#onlineStatus').removeClass('bg-success bg-secondary').addClass(badgeClass).text(status);
        }

        // به‌روزرسانی وضعیت مخاطب در لیست
        function updateContactStatus(userId, isOnline) {
            $('.contact-item[data-userid="' + userId + '"] .badge')
                .removeClass('bg-success bg-secondary')
                .addClass(isOnline ? 'bg-success' : 'bg-secondary')
                .text(isOnline ? 'آنلاین' : 'آفلاین');
        }

        // رویدادها
        $(document).on('click', '.contact-item', function (e) {
            e.preventDefault();
            var userId = $(this).data('userid');
            var username = $(this).find('strong').text();
            startChat(userId, username);
        });

        $(document).on('click', '#btnSearch', function () {
            var query = $('#txtSearch').val();
            if (query) {
                chatHub.server.searchUsers(query);
            }
        });

        $(document).on('click', '.add-contact', function () {
            var userId = $(this).data('userid');
            chatHub.server.addContact(userId);
        });

        $('#btnSend').click(sendMessage);

        $('#txtMessage').keypress(function (e) {
            if (e.which == 13 && !e.shiftKey) {
                e.preventDefault();
                sendMessage();
            }
        });


         // افزودن کاربر به لیست چت
        function addToChatContacts(userId) {
            chatHub.server.addContact(userId).done(function () {
                alert('کاربر با موفقیت به لیست چت شما اضافه شد');
                $('#searchResults').html('<div class="alert alert-success">کاربر با موفقیت به لیست چت شما اضافه شد</div>');

                // اطلاع به کاربر اضافه شده
                chatHub.server.notifyContactJoined(userId);
            }).fail(function (error) {
                alert('خطا در افزودن کاربر: ' + error);
            });
        }

        // دریافت通知 پیوستن مخاطب به سیستم
        chatHub.client.contactJoinedSystem = function (contactId, contactName) {
            showNotification(contactName + ' به سیستم دفتر تلفن پیوست! اکنون می‌توانید با او چت کنید.');

            // به‌روزرسانی خودکار لیست مخاطبین
            refreshContactsList();
        };

        // نمایش notification
        function showNotification(message) {
            // ایجاد یک notification موقت
            var notification = $('<div class="alert alert-info alert-dismissible fade show" role="alert">' +
                message +
                '<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>' +
                '</div>');

            $('body').prepend(notification);

            // حذف خودکار بعد از 5 ثانیه
            setTimeout(function () {
                notification.alert('close');
            }, 5000);
        }

        // به‌روزرسانی لیست مخاطبین
        function refreshContactsList() {
            // این تابع باید لیست مخاطبین را به‌روزرسانی کند
            console.log('لیست مخاطبین should be refreshed here');
        }
    </script>
</asp:Content>
