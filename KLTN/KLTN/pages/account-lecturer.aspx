<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="account-lecturer.aspx.cs" Inherits="KLTN.pages.account_lecturer" %>

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

    <div class="flex-1 p-5">
        <div class="flex justify-end">
            <input type="button" onclick="CreateLecturerAccount()" class="cursor-pointer bg-green-500 text-white py-2 px-4 rounded" value="Thêm tài khoản">
            <input type="button" onclick="CreateLecturerAccountAuto()" class="cursor-pointer bg-green-500 text-white py-2 px-4 rounded" value="Thêm tài khoản tự động">
        </div>
        <div class="mt-5 bg-white p-4 shadow-md rounded-lg">
            <h2 class="text-xl font-bold mb-4">Danh sách giảng viên</h2>
            <table class="w-full border-collapse border border-gray-300">
                <thead>
                    <tr class="bg-gray-200">
                        <th class="border border-gray-300 px-4 py-2">Mã giảng viên</th>
                        <th class="border border-gray-300 px-4 py-2">Họ tên</th>
                        <th class="border border-gray-300 px-4 py-2">Ngày sinh</th>
                        <th class="border border-gray-300 px-4 py-2">Bộ môn</th>
                        <th class="border border-gray-300 px-4 py-2">Trưởng bộ môn</th>
                        <th class="border border-gray-300 px-4 py-2">Chức năng</th>
                    </tr>
                </thead>
                <tbody id="lecturer_table" runat="server">
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

    <div id="lecturer-modal" class="modal">
        <div class="modal-content">
            <div id="modal-lecturer-form">
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Mã giảng viên</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="lecturerCode"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập mã giảng viên">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Họ tên</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="lecturerName"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập họ tên">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Chương</label>
                    <select id="question-chapter"
                        class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                    </select>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Kiểu câu hỏi</label>
                    <select id="question-type" onchange="ChangeQuestionType()"
                        class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                        <option value="single">Một câu trả lời</option>
                        <option value="multiple">Nhiều câu trả lời</option>
                    </select>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Mức độ</label>
                    <select id="question-level"
                        class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                        <option value="basic">Dễ</option>
                        <option value="medium">Trung bình</option>
                        <option value="hard">Khó</option>
                    </select>
                </div>
                <div class="answer-section">
                    <div id="answer-list" class="space-y-2"></div>
                    <input type="button" id="answer-create" onclick="CreateElementAnswer()"
                        class="cursor-pointer mt-2 flex items-center gap-1 text-blue-600 hover:text-blue-800"
                        value="➕ Thêm đáp án">
                </div>
                <div class="mt-6 flex justify-end space-x-2">
                    <input type="button" onclick="CancelQuestion()"
                        class="cursor-pointer px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300"
                        value="Hủy">
                    <input type="button" id="save-question" onclick="SaveQuestion()"
                        class="cursor-pointer px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
                        value="Thêm câu hỏi">
                    <input type="button" id="update-question"
                        class=" hidden cursor-pointer px-4 py-2 bg-yellow-600 text-white rounded hover:bg-yellow-700"
                        value="Lưu">
                </div>
            </div>
        </div>
    </div>
    </div>
</asp:Content>
