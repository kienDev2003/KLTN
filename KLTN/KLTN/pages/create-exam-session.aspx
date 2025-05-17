<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="create-exam-session.aspx.cs" Inherits="KLTN.pages.create_exam_session" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <style>
        .modal {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 1000;
            overflow: auto;
        }

        .modal-content {
            position: relative;
            background-color: #fff;
            margin: 10% auto;
            padding: 20px;
            border-radius: 8px;
            max-width: 600px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            animation: modalAppear 0.3s ease;
        }

        @keyframes modalAppear {
            from {
                transform: translateY(-50px);
                opacity: 0;
            }

            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        .close-button {
            position: absolute;
            top: 10px;
            right: 10px;
            font-size: 20px;
            cursor: pointer;
        }
    </style>

    <div class="flex-1 p-4 relative">
        <div class="flex-1 p-5">
            <div class="flex justify-end">
                <input type="button" onclick="CreateExamSession()" class="cursor-pointer bg-green-500 text-white py-2 px-4 rounded" value="Tạo ca thi">
            </div>
            <div class="mt-5 bg-white p-4 shadow-md rounded-lg">
                <h2 class="text-xl font-bold mb-4">Danh sách ca thi</h2>
                <table class="w-full border-collapse border border-gray-300">
                    <thead>
                        <tr class="bg-gray-200">
                            <th class="border border-gray-300 px-4 py-2">Môn học</th>
                            <th class="border border-gray-300 px-4 py-2">Thời gian bắt đầu</th>
                            <th class="border border-gray-300 px-4 py-2">Thời gian kết thúc</th>
                            <th class="border border-gray-300 px-4 py-2">Đề thi</th>
                            <th class="border border-gray-300 px-4 py-2">Mật khẩu ca thi</th>
                            <th class="border border-gray-300 px-4 py-2">Trạng thái phân công</th>
                            <th class="border border-gray-300 px-4 py-2">Chức năng</th>
                        </tr>
                    </thead>
                    <tbody id="examSession_table" runat="server">
                    </tbody>
                </table>
                <div class="flex justify-center mt-6">
                    <div class="inline-flex space-x-1" id="pagination" runat="server">
                        <input type="button" class="pagination-btn pagination-active px-4 py-2 border border-gray-300 bg-blue-500 text-white text-sm font-medium rounded hover:bg-blue-600" value="1">
                        <input type="button" class="pagination-btn px-4 py-2 border border-gray-300 bg-white text-sm font-medium text-gray-700 rounded hover:bg-gray-50" value="2">
                        <input type="button" class="pagination-btn px-4 py-2 border border-gray-300 bg-white text-sm font-medium text-gray-700 rounded hover:bg-gray-50" value="3">
                        <input type="button" class="pagination-btn px-4 py-2 border border-gray-300 bg-white text-sm font-medium text-gray-700 rounded hover:bg-gray-50" value="4">
                        <input type="button" class="pagination-btn px-4 py-2 border border-gray-300 bg-white text-sm font-medium text-gray-700 rounded hover:bg-gray-50" value="5">
                    </div>
                </div>
            </div>
        </div>
        <div id="examSession-modal" class="modal">
            <div class="modal-content">
                <div id="modal-examSession-form">
                    <div class="mb-4">
                        <label class="block text-lg font-medium text-gray-700">Môn học</label>
                        <select id="subject" onchange="ChangeSubject()"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                        </select>
                    </div>
                    <div class="mb-4">
                        <label class="block text-lg font-medium text-gray-700">Đề thi</label>
                        <select id="exam"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                        </select>
                    </div>
                    <div class="mb-4">
                        <label class="block text-lg font-medium text-gray-700">Thời gian bắt đầu</label>
                        <input type="datetime-local" id="start-exam-date"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                    </div>
                    <div class="mb-4">
                        <label class="block text-lg font-medium text-gray-700">Thời gian kết thúc</label>
                        <input type="datetime-local" id="end-exam-date"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                    </div>
                    <div class="mb-4">
                        <label class="block text-lg font-medium text-gray-700">Mật khẩu ca thi</label>
                        <input type="text" id="password-exam"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập mật khẩu ca thi">
                    </div>
                    <div class="mt-6 flex justify-end space-x-2">
                        <input type="button" onclick="CancelExamSession()"
                            class="cursor-pointer px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300"
                            value="Hủy">
                        <input type="button" id="save-examSession" onclick="SaveExamSession()"
                            class="cursor-pointer px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
                            value="Tạo ca thi">
                        <input type="button" id="update-examSession"
                            class=" hidden cursor-pointer px-4 py-2 bg-yellow-600 text-white rounded hover:bg-yellow-700"
                            value="Lưu">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        async function SaveExamSession() {
            const subjectCode = document.getElementById('subject').value;
            const examPaperCode = document.getElementById('exam').value;
            const start_exam_date = document.getElementById('start-exam-date').value;
            const end_exam_date = document.getElementById('end-exam-date').value;
            const password_examSession = document.getElementById('password-exam').value;

            const examSession = {
                SubjectCode: subjectCode,
                ExamPaperCode: examPaperCode,
                StartExamDate: start_exam_date,
                EndExamDate: end_exam_date,
                ExamSessionPassword: password_examSession
            };

            const response = await fetch('create-exam-session.aspx/HandleInsertExamSession', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ examSession: examSession })
            });

            const res = await response.json();

            if (res.d.status !== "200") {
                alert(res.d.message);
                return;
            }
            else {
                alert(res.d.message);
                window.location.reload();
            }
        }

        async function CreateExamSession() {
            const modal = document.getElementById("examSession-modal");
            modal.style.display = 'block';
            document.getElementById('save-examSession').style.display = 'block';

            await HandleGetSubjects();
            await HandleGetExamBySubject();
        }

        function CancelExamSession() {
            const modal = document.getElementById("examSession-modal");
            modal.style.display = "none";

            document.getElementById('subject').innerHTML = '';
            document.getElementById('exam').innerHTML = '';
        }

        async function HandleGetSubjects() {
            const response = await fetch('create-exam-session.aspx/HandleGetSubjects', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({})
            });

            const res = await response.json();

            if (res.d.status !== "200") {
                alert(res.d.message);
                return;
            }

            const subjectHTML = document.getElementById('subject');

            if (res.d.subjects && Array.isArray(res.d.subjects)) {
                for (let i = 0; i < res.d.subjects.length; i++) {
                    const subject = res.d.subjects[i];

                    const html = `<option value="${subject.SubjectCode}">${subject.SubjectName}</option>`;

                    subjectHTML.innerHTML += html;
                }
            }
        }

        async function HandleGetExamBySubject() {
            const subjectCode = document.getElementById('subject').value;

            const response = await fetch('create-exam-session.aspx/HandleGetExamBySubject', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ subjectCode: subjectCode })
            });

            const res = await response.json();

            if (res.d.status !== "200") {
                alert(res.d.message);
                return;
            }

            const examHTML = document.getElementById('exam');

            if (res.d.exams && Array.isArray(res.d.exams)) {
                for (let i = 0; i < res.d.exams.length; i++) {
                    const exam = res.d.exams[i];

                    const html = `<option value="${exam.ExamCode}">${exam.ExamName}</option>`;

                    examHTML.innerHTML += html;
                }
            }
        }

        async function ChangeSubject() {
            await HandleGetExamBySubject();
        }
    </script>
</asp:Content>
