<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="assessment.aspx.cs" Inherits="KLTN.pages.assessment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="flex-1 p-5">
        <div id="assessments" runat="server" class="flex flex-wrap lg:flex-nowrap gap-4 max-w-6xl mx-auto">
            <div
                class="w-full lg:w-1/4 bg-white rounded-lg shadow-md p-6 text-center hover:shadow-lg transition-shadow duration-300 flex flex-col items-center">
                <a id="exam-link">
                    <img src="./h.jpg"
                        class="w-16 h-16 mb-4">
                    <h3 class="text-gray-800 font-medium mb-2">Danh sách đề thi</h3>
                </a>
            </div>
            <div
                class="w-full lg:w-1/4 bg-white rounded-lg shadow-md p-6 text-center hover:shadow-lg transition-shadow duration-300 flex flex-col items-center">
                <a id="question-link">
                    <img src="./j.jpg"
                        class="w-16 h-16 mb-4">
                    <h3 class="text-gray-800 font-medium mb-2">Ngân hàng câu hỏi</h3>
                </a>
            </div>
        </div>
    </div>

    <script>
        const subjectCode = new URLSearchParams(window.location.search).get('subjectCode');

        if (subjectCode) {
            const examLink = document.getElementById('exam-link');
            const questionLink = document.getElementById("question-link")

            examLink.href = `/pages/exam-created.aspx?subjectCode=${subjectCode}`;
            questionLink.href = `/pages/bank-question.aspx?subjectCode=${subjectCode}`;
        }
    </script>
</asp:Content>
