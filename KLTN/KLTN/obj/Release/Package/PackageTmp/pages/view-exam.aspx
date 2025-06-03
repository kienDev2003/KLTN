<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view-exam.aspx.cs" Inherits="KLTN.pages.view_exam" %>

<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Đề Thi</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <style>
        body {
            font-family: 'Inter', sans-serif; 
        }
        .readonly-input {
            background-color: #f9fafb; 
            cursor: default;
        }

            .readonly-input:focus {
                outline: none;
                box-shadow: none;
                border-color: #d1d5db; 
            }
    </style>
</head>
<body class="bg-slate-100">
    <div class="container mx-auto p-6">
        <div class="flex flex-col md:flex-row gap-6">
            <div class="w-full md:w-1/3 bg-white p-6 rounded-lg shadow-lg">
                <h2 class="text-2xl font-semibold text-slate-700 mb-6">Thông tin bài thi</h2>
                <div class="space-y-4">
                    <div>
                        <label for="examTitle" class="block text-sm font-medium text-slate-600 mb-1">Mã đề thi</label>
                        <input type="text" id="examTitle" readonly
                            class="readonly-input mt-1 block w-full p-3 border border-gray-300 rounded-md shadow-sm text-slate-700">
                    </div>
                    <div>
                        <label for="examTime" class="block text-sm font-medium text-slate-600 mb-1">Thời gian làm bài (phút)</label>
                        <input type="text" id="examTime" readonly
                            class="readonly-input mt-1 block w-full p-3 border border-gray-300 rounded-md shadow-sm text-slate-700">
                    </div>
                    <div>
                        <p class="text-sm font-medium text-slate-600">Số câu hỏi: <span id="questionCount" class="font-bold text-slate-700">0</span></p>
                    </div>
                    <div class="mb-4">
                        <button type="button" id="btn-IsApproved" class="hidden w-full bg-green-600 text-white py-2 rounded-md hover:bg-green-700"
                            onclick="ExamApproved()">
                            Duyệt đề thi</button>
                    </div>
                </div>
            </div>

            <div class="w-full md:w-2/3 bg-white p-6 rounded-lg shadow-lg">
                <h2 class="text-2xl font-semibold text-slate-700 mb-6">Các Câu Hỏi</h2>
                <div id="questionsContainer" class="space-y-8 overflow-y-auto max-h-[calc(100vh-12rem)] pr-2">
                </div>
            </div>
        </div>
    </div>

    <script>
        async function ExamApproved() {
            const examPaperCode = new URLSearchParams(window.location.search).get('examPaperCode');

            const response = await fetch('exam-created.aspx/ExamPaperApproved', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ examPaperCode: examPaperCode })
            });

            const res = await response.json();

            if (res.d.status == "200") {
                alert(res.d.message);
                window.location.href = '/pages/exam.aspx';
            }
            else {
                alert(res.d.message);
            }
        }

        async function HandleGetExamPaper() {
            const examPaperCode = new URLSearchParams(window.location.search).get('examPaperCode');

            const response = await fetch('exam-created.aspx/GetExamPaper', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ examPaperCode: examPaperCode })
            });

            const res = await response.json();

            return res.d.exam;
        }

        document.addEventListener('DOMContentLoaded', async () => {
            const isApproved = new URLSearchParams(window.location.search).get('IsApproved');
            if (isApproved === 'true') {
                document.getElementById('btn-IsApproved').style.display = 'block';
            }

            const exam = await HandleGetExamPaper();

            document.getElementById('examTitle').value = exam.ExamPaperCode || 'N/A';
            document.getElementById('examTime').value = exam.ExamTime || 'N/A';
            document.getElementById('questionCount').textContent = exam.questions ? exam.questions.length : 0;

            const questionsContainer = document.getElementById('questionsContainer');
            if (exam.questions && exam.questions.length > 0) {
                exam.questions.forEach((question, index) => {
                    const questionDiv = document.createElement('div');
                    questionDiv.className = 'question-item p-4 border border-gray-200 rounded-md shadow-sm bg-white';

                    const questionText = document.createElement('p');
                    questionText.className = 'text-md font-medium text-slate-800 mb-3';
                    questionText.innerHTML = `<strong>Câu ${index + 1}:</strong> ${escapeHTML(question.QuestionText)}`;
                    questionDiv.appendChild(questionText);

                    const answersDiv = document.createElement('div');
                    answersDiv.className = 'answers-container space-y-2 pl-4';

                    if (question.Answers && question.Answers.length > 0) {
                        question.Answers.forEach(answer => {
                            const answerWrapper = document.createElement('div');
                            answerWrapper.className = 'flex items-center';

                            const input = document.createElement('input');
                            input.id = `answer-${answer.AnswerCode}`;
                            input.value = answer.AnswerCode;
                            input.className = 'h-4 w-4 text-indigo-600 border-gray-300 focus:ring-indigo-500 mr-3';

                            if (question.QuestionType === 'single') {
                                input.type = 'radio';
                                input.name = `question-${question.QuestionCode}`;
                            } else if (question.QuestionType === 'multiple') {
                                input.type = 'checkbox';
                                input.name = `answer-checkbox-${answer.AnswerCode}`;
                            }

                            if (answer.AnswerTrue === true) {
                                input.checked = true;
                            }

                            input.disabled = true;

                            const label = document.createElement('label');
                            label.htmlFor = `answer-${answer.AnswerCode}`;
                            label.className = 'text-sm text-slate-700 cursor-pointer';
                            label.textContent = answer.AnswerText;

                            answerWrapper.appendChild(input);
                            answerWrapper.appendChild(label);
                            answersDiv.appendChild(answerWrapper);
                        });
                    } else {
                        const noAnswersText = document.createElement('p');
                        noAnswersText.className = 'text-sm text-slate-500 italic';
                        noAnswersText.textContent = 'Không có đáp án cho câu hỏi này.';
                        answersDiv.appendChild(noAnswersText);
                    }

                    questionDiv.appendChild(answersDiv);
                    questionsContainer.appendChild(questionDiv);
                });
            } else {
                questionsContainer.textContent = 'Không có câu hỏi nào trong đề thi này.';
                questionsContainer.className += ' text-slate-500 italic';
            }
        });

        function escapeHTML(text) {
            return text
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
        }
    </script>
</body>
</html>
