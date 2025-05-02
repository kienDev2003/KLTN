<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="question-created.aspx.cs" Inherits="KLTN.pages.question_created" %>

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

    <h2 class="text-xl font-bold mb-4" id="subject_name" runat="server"></h2>

    <div class="flex-1 p-5">
        <div class="flex justify-end">
            <input type="button" onclick="CreateQuestion()" class="cursor-pointer bg-green-500 text-white py-2 px-4 rounded" value="Tạo câu hỏi">
        </div>
        <div class="mt-5 bg-white p-4 shadow-md rounded-lg">
            <h2 class="text-xl font-bold mb-4">Danh sách câu hỏi đã tạo</h2>
            <table class="w-full border-collapse border border-gray-300">
                <thead>
                    <tr class="bg-gray-200">
                        <th class="border border-gray-300 px-4 py-2">Câu hỏi</th>
                        <th class="border border-gray-300 px-4 py-2">Dạng</th>
                        <th class="border border-gray-300 px-4 py-2">Mức độ</th>
                        <th class="border border-gray-300 px-4 py-2">Thời gian tạo</th>
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
                    <input type="button" onclick="CreateElementAnswer()"
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
    <script>
        async function HandleDeteleQuestion(questionCode) {
            if (!confirm('Bạn chắc chắn muốn xóa câu hỏi này ?')) return;

            const response = await fetch('./question-created.aspx/HandleDeleteQuestion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ questionCode: questionCode })
            });

            const res = await response.json();

            if (res.d.status !== "200") {
                alert(res.d.message);
            }
            else {
                alert(res.d.message);
                window.location.reload();
            }
        }

        async function HandleEditQuestion(questionCode) {
            const btnEdit = document.getElementById('update-question');
            btnEdit.style.display = 'block';
            btnEdit.addEventListener('click', async () => await UpdateQuestion(questionCode));

            HandleViewQuestion(questionCode);
        }

        async function HandleViewQuestion(questionCode) {
            document.getElementById('save-question').style.display = 'none';
            document.getElementById('question-modal').style.display = 'block';

            const response = await fetch('./question-created.aspx/HandleViewQuestion', {
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

        async function UpdateQuestion(questionCode) {
            const questionContent = document.getElementById('question-content').value;
            const questionType = document.getElementById('question-type').value;
            const questionLevel = document.getElementById('question-level').value;
            const subjectCode = new URLSearchParams(location.search).get("subjectCode");
            var answers = [];

            const answer_list = document.getElementById('answer-list');
            const answer_content = document.querySelectorAll('.answer').forEach(answer => {
                const answerContent = answer.querySelector('.answer-text').value;
                const isCorrect = answer.querySelector('.answer-check').checked;

                answers.push({
                    AnswerText: answerContent,
                    AnswerTrue: isCorrect
                });
            })

            const questionRequest = {
                questionCode: questionCode,
                questionText: questionContent,
                questionLevel: questionLevel,
                questionType: questionType,
                subjectCode: subjectCode,
                answers: answers
            };

            const response = await fetch('./question-created.aspx/HandleUpdateQuestion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ questionRequest: questionRequest })
            });

            const res = await response.json();

            if (res.d.status === '200') {
                alert(res.d.message);

                window.location.reload();
            }
            else {
                alert(res.d.message);
            }
        }

        async function SaveQuestion() {
            const questionContent = document.getElementById('question-content').value;
            const questionType = document.getElementById('question-type').value;
            const questionLevel = document.getElementById('question-level').value;
            const subjectCode = new URLSearchParams(location.search).get("subjectCode");
            var answers = [];

            const answer_list = document.getElementById('answer-list');
            const answer_content = document.querySelectorAll('.answer').forEach(answer => {
                const answerContent = answer.querySelector('.answer-text').value;
                const isCorrect = answer.querySelector('.answer-check').checked;

                answers.push({
                    AnswerText: answerContent,
                    AnswerTrue: isCorrect
                });
            })

            const questionRequest = {
                questionText: questionContent,
                questionLevel: questionLevel,
                questionType: questionType,
                subjectCode: subjectCode,
                answers: answers
            };

            const response = await fetch('./question-created.aspx/HandleInsertQuestion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ questionRequest: questionRequest })
            });

            const res = await response.json();

            if (res.d.status === '200') {
                alert(res.d.message);

                window.location.reload();
            }
            else {
                alert(res.d.message);
            }
        }

        function ChangeQuestionType() {
            const question_type = document.getElementById('question-type').value;

            const answerList = document.getElementById("answer-list");
            answerList.querySelectorAll('.answer-check').forEach(input => {

                if (question_type === 'single') {
                    input.type = 'radio';
                    input.name = 'answer';
                } else {
                    input.type = 'checkbox';
                    input.removeAttribute('name');
                }
            });
        }

        function CancelQuestion() {
            const modal = document.getElementById("question-modal");
            modal.style.display = "none";
        }

        function CreateQuestion() {
            const modal = document.getElementById("question-modal");
            modal.style.display = 'block';
            document.getElementById('save-question').style.display = 'block';

            document.getElementById('question-content').value = '';
            document.getElementById('question-type').value = '';
            document.getElementById('question-level').value = '';
            const answer_list = document.getElementById('answer-list');
            answer_list.innerHTML = '';
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

        function DeleteAnswer(button) {
            button.closest('.answer').remove();
        }

        document.addEventListener("DOMContentLoaded", function () {
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
        });
    </script>
</asp:Content>
