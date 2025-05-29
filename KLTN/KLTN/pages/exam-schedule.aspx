<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="exam-schedule.aspx.cs" Inherits="KLTN.pages.exam_schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="flex-1 p-4 relative" id="main">
        <div class="bg-white rounded-lg p-5 mb-5 shadow-md">
            <h2 class="text-primary text-xl font-bold">Danh sách ca thi</h2>
        </div>
        <div id="list_examSession" runat="server" class="flex flex-wrap gap-4 max-w-6xl mx-auto">
            <a href="pages/examSession.aspx?examSessionCode=11">
                <div class="flex flex-col bg-white rounded-lg p-5 shadow-md h-full flex items-center">
                </div>
            </a>
        </div>
    </div>
    <script>
        function CheckDatetime(startExamSessionDate) {
            var now = new Date();
            var target = new Date(startExamSessionDate);

            if (now < target) {
                alert('Chưa đến thời gian bắt đầu ca thi');
                return true;
            } else {
                return true;
            }
        }

        function handleExamClick(anchorElement, passwordExamSession) {
            const examSessionCode = anchorElement.querySelector('#examSessionCode').textContent;

            CheckPassword(passwordExamSession, examSessionCode, function (allowed) {
                if (allowed) {
                    window.location.href = anchorElement.href;
                }
            });
        }

        function CheckPassword(passwordExamSession, examSessionCode, callback) {
            let inputPassword = prompt('Nhập mật khẩu ca thi được cấp !');

            if (inputPassword === null) {
                callback(false);
                return;
            }

            if (passwordExamSession === inputPassword) {
                CheckStudentHaveted(examSessionCode).then(status => {
                    if (status === true) {
                        alert('Bạn đã ở trong ca thi. Hãy xin cấp quyền vào lại !');
                        callback(false);
                    } else {
                        callback(true);
                    }
                }).catch(error => {
                    console.error("Lỗi khi kiểm tra sinh viên:", error);
                    callback(false);
                });
            } else {
                alert('Sai mật khẩu !');
                callback(false);
            }
        }

        async function CheckStudentHaveted(examSessionCode) {

            const response = await fetch('exam-schedule.aspx/CheckStudentHaveEntered', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ examSessionCode: examSessionCode })
            });

            const res = await response.json();
            return res.d.status;
        }
    </script>
</asp:Content>
