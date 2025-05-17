<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="exam.aspx.cs" Inherits="KLTN.pages.exam" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="flex-1 p-5">
        <div class="mt-5 bg-white p-4 shadow-md rounded-lg">
            <h2 class="text-xl font-bold mb-4">Danh sách đề thi đã tạo</h2>
            <table class="w-full border-collapse border border-gray-300">
                <thead>
                    <tr class="bg-gray-200">
                        <th class="border border-gray-300 px-4 py-2">Môn học</th>
                        <th class="border border-gray-300 px-4 py-2">Tên đề thi</th>
                        <th class="border border-gray-300 px-4 py-2">Thời gian thi</th>
                        <th class="border border-gray-300 px-4 py-2">Người tạo</th>
                        <th class="border border-gray-300 px-4 py-2">Thời gian tạo</th>
                        <th class="border border-gray-300 px-4 py-2">Trạng thái</th>
                        <th class="border border-gray-300 px-4 py-2">Chức năng</th>
                    </tr>
                </thead>
                <tbody id="examPaper_table" runat="server">
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

    <script>
        function HandleViewExam(examPaperCode) {
            window.location.href = `/pages/view-exam.aspx?examPaperCode=${examPaperCode}&IsApproved=true`
        }
    </script>
</asp:Content>
