<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="bank-question.aspx.cs" Inherits="KLTN.pages.bank_question" %>

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

    <div class="</div>">
        <h2 class="text-xl font-bold mb-4" id="subject_name" runat="server"></h2>

        <div class="flex-1 p-5">
            <div class="mt-5 bg-white p-4 shadow-md rounded-lg">
                <h2 class="text-xl font-bold mb-4">Danh sách câu hỏi</h2>
                <table class="w-full border-collapse border border-gray-300">
                    <thead>
                        <tr class="bg-gray-200">
                            <th class="border border-gray-300 px-4 py-2">Câu hỏi</th>
                            <th class="border border-gray-300 px-4 py-2">Chương</th>
                            <th class="border border-gray-300 px-4 py-2">Mức độ</th>
                            <th class="border border-gray-300 px-4 py-2">Thời gian tạo</th>
                            <th class="border border-gray-300 px-4 py-2">Người tạo</th>
                            <th class="border border-gray-300 px-4 py-2">Trạng thái</th>
                            <th class="border border-gray-300 px-4 py-2">Chức năng</th>
                        </tr>
                    </thead>
                    <tbody id="question_table" runat="server">
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
        <div id="question-modal" class="modal">
            <div class="modal-content">
                <div id="modal-question-form">
                    <div class="mb-4">
                        <label class="block text-lg font-medium text-gray-700">Nội dung câu hỏi</label>
                        <div class="flex items-center gap-2">
                            <input type="text" id="question-content"
                                class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                                placeholder="Nhập nội dung câu hỏi">
                        </div>
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
    <script>
        async function HandleViewQuestion(questionCode) {
            document.getElementById('save-question').style.display = 'none';
            document.getElementById('question-modal').style.display = 'block';
            document.getElementById('answer-create').style.display = 'none';

            const response = await fetch('question-created.aspx/HandleViewQuestion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ questionCode: questionCode })
            });

            const res = await response.json();

            if (res.d.status !== "200") {
                alert(res.d.message);
                return;
            }

            document.getElementById('question-content').value = res.d.question.QuestionText;
            document.getElementById('question-type').value = res.d.question.QuestionType;
            document.getElementById('question-level').value = res.d.question.QuestionLevel;

            const answer_list = document.getElementById('answer-list');
            answer_list.innerHTML = '';

            if (res.d.question && Array.isArray(res.d.question.Answers)) {
                for (let i = 0; i < res.d.question.Answers.length; i++) {
                    const answer = res.d.question.Answers[i];
                    CreateElementAnswer();
                    const lastAnswerElem = answer_list.lastElementChild;
                    if (lastAnswerElem) {
                        lastAnswerElem.querySelector('.answer-text').value = answer.AnswerText;
                        lastAnswerElem.querySelector('.answer-check').checked = answer.AnswerTrue;
                    }
                }
            }

        }

        function CancelQuestion() {
            const modal = document.getElementById("question-modal");
            modal.style.display = "none";
            document.getElementById('update-question').style.display = 'none';
            document.getElementById('answer-create').style.display = 'block';
        }

        function CreateElementAnswer() {
            const answer_list = document.getElementById('answer-list');
            const question_type = document.getElementById('question-type').value;
            var inputType = question_type === 'single' ? 'radio' : 'checkbox'

            let answerElem = document.createElement('div');
            answerElem.classList.add('flex', 'answer', 'items-center', 'gap-2', 'mt-2');
            const answer_element = `<input type="text" class="answer-text w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" placeholder="Nhập đáp án">
                            <input type="${inputType}" name="multiple" class="w-5 h-5 answer-check">
                            <input type="button" onclick="DeleteAnswer(this)" class="cursor-pointer text-red-600 hover:text-red-800" value="🗑">
                            `
            answerElem.innerHTML = answer_element;
            answer_list.appendChild(answerElem);
        }

        async function HandleQuestionApproval(questionCode) {
            if (!confirm('Bạn chắc chắn muốn duyệt câu hỏi này ?')) return;

            const response = await fetch('./bank-question.aspx/HandleQuestionApproval', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ questionCode: questionCode })
            });

            const res = await response.json();
            console.log(res);

            if (res.d.status !== "200") {
                alert(res.d.message);
            }
            else {
                alert(res.d.message);
                window.location.reload();
            }
        }

        document.addEventListener("DOMContentLoaded", async function (){
            const paginationButtons = document.querySelectorAll(".pagination-btn");

            paginationButtons.forEach(button => {
                button.addEventListener("click", function () {
                    // Xóa class active khỏi tất cả các nút
                    paginationButtons.forEach(btn => {
                        btn.classList.remove("pagination-active");
                        btn.classList.remove("bg-blue-500");
                        btn.classList.remove("text-white");
                        btn.classList.add("bg-white");
                        btn.classList.add("text-gray-700");
                    });

                    // Thêm class active cho nút được nhấp
                    this.classList.add("pagination-active");
                    this.classList.add("bg-blue-500");
                    this.classList.add("text-white");
                    this.classList.remove("bg-white");
                    this.classList.remove("text-gray-700");
                });
            });
        })

        async function NextPage(event) {
            const subjectCode = new URLSearchParams(window.location.search).get('subjectCode');

            const response = await fetch('bank-question.aspx/HandleNextPage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ pageIndex: event.value, subjectCode : subjectCode })
            });

            const res = await response.json();

            const question_table = document.getElementById('main_question_table');
            question_table.innerHTML = '';
            question_table.innerHTML = res.d.html;
        }
    </script>
</asp:Content>
