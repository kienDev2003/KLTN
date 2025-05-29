<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testing.aspx.cs" Inherits="KLTN.pages.testing" %>

<!DOCTYPE html>
<html lang="vi">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Bài thi trắc nghiệm</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <style>
        .question-item {
            scroll-margin-top: 100px;
        }

        .popup-overlay {
            backdrop-filter: blur(5px);
        }

        .popup-content {
            animation: fadeInUp 0.3s ease-out;
        }

        @keyframes fadeInUp {
            from {
                opacity: 0;
                transform: translateY(20px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }
    </style>
</head>

<body class="bg-gray-50">
    <form runat="server">
        <!-- Popup thông tin đề thi -->
        <div id="examInfoPopup"
            class="fixed inset-0 bg-black bg-opacity-50 popup-overlay flex items-center justify-center z-50">
            <div class="popup-content bg-white rounded-xl shadow-2xl max-w-md w-full mx-4 p-8">
                <div class="text-center">
                    <!-- Icon -->
                    <div class="mx-auto flex items-center justify-center w-16 h-16 bg-blue-100 rounded-full mb-6">
                        <svg class="w-8 h-8 text-blue-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z">
                            </path>
                        </svg>
                    </div>

                    <!-- Tiêu đề -->
                    <h2 class="text-2xl font-bold text-gray-900 mb-2">Thông tin đề thi</h2>
                    <p class="text-gray-600 mb-6">Vui lòng đọc kỹ thông tin trước khi bắt đầu làm bài</p>

                    <!-- Thông tin chi tiết -->
                    <div class="bg-gray-50 rounded-lg p-6 mb-6 text-left">
                        <div class="space-y-3">
                            <div class="flex justify-between">
                                <span class="text-gray-600">Tên đề thi:</span>
                                <span id="examPaperName" class="font-medium text-gray-900">examName</span>
                            </div>
                            <div class="flex justify-between">
                                <span class="text-gray-600">Số câu hỏi:</span>
                                <span id="numberQuestion" class="font-medium text-gray-900">numberQuestion</span>
                            </div>
                            <div class="flex justify-between">
                                <span class="text-gray-600">Thời gian:</span>
                                <span id="examPaperTime" class="font-medium text-red-600">examPaperTime</span>
                            </div>
                        </div>
                    </div>

                    <!-- Lưu ý -->
                    <div class="bg-yellow-50 border border-yellow-200 rounded-lg p-4 mb-6">
                        <div class="flex items-start">
                            <svg class="w-5 h-5 text-yellow-600 mt-0.5 mr-2" fill="currentColor" viewBox="0 0 20 20">
                                <path fill-rule="evenodd"
                                    d="M8.257 3.099c.765-1.36 2.722-1.36 3.486 0l5.58 9.92c.75 1.334-.213 2.98-1.742 2.98H4.42c-1.53 0-2.493-1.646-1.743-2.98l5.58-9.92zM11 13a1 1 0 11-2 0 1 1 0 012 0zm-1-8a1 1 0 00-1 1v3a1 1 0 002 0V6a1 1 0 00-1-1z"
                                    clip-rule="evenodd">
                                </path>
                            </svg>
                            <div class="text-left">
                                <h4 class="text-sm font-medium text-yellow-800 mb-1">Lưu ý quan trọng:</h4>
                                <ul class="text-sm text-yellow-700 space-y-1">
                                    <li>• Thời gian làm bài sẽ bắt đầu khi bạn nhấn "Bắt đầu"</li>
                                    <li>• Không thể tạm dừng sau khi bắt đầu</li>
                                    <li>• Hãy kiểm tra kỹ trước khi nộp bài</li>
                                </ul>
                            </div>
                        </div>
                    </div>

                    <!-- Nút bắt đầu -->
                    <button type="button" onclick="startExam()"
                        class="w-full bg-blue-600 hover:bg-blue-700 text-white font-medium py-3 px-6 rounded-lg transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2">
                        <svg class="w-5 h-5 inline-block mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="M14 5l7 7m0 0l-7 7m7-7H3">
                            </path>
                        </svg>
                        Bắt đầu làm bài
                    </button>
                </div>
            </div>
        </div>
        <!-- Nội dung chính của đề thi (ẩn ban đầu) -->
        <div id="examContent" style="display: none;">
            <div class="fixed top-0 left-0 right-0 bg-white shadow-md z-50 px-6 py-4">
                <div class="flex justify-between items-center">
                    <div class="text-lg font-semibold text-gray-800">
                        Thí sinh: <span runat="server" id="student_name" class="text-blue-600"></span>
                    </div>
                    <div class="text-xl font-bold text-red-600" id="timer">25:00</div>
                    <button type="button" onclick="btn_submitExam()"
                        class="bg-red-500 hover:bg-red-600 text-white px-6 py-2 rounded-lg font-medium transition-colors">
                        Nộp bài
                    </button>
                </div>
            </div>
            <div class="pt-20 flex min-h-screen">
                <!-- Phần câu hỏi -->
                <div class="flex-1 p-6 pr-3">
                    <div class="bg-white rounded-lg shadow-sm p-6 space-y-8" id="question-list"></div>
                </div>
                <!-- Sơ đồ câu hỏi -->
                <div class="w-80 p-6 pl-3">
                    <div class="bg-white rounded-lg shadow-sm p-6 sticky top-24">
                        <h3 class="text-lg font-semibold text-gray-800 mb-4">Sơ đồ câu hỏi</h3>
                        <div class="grid grid-cols-5 gap-3" id="question-map"></div>
                        <div class="mt-6 space-y-2 text-sm">
                            <div class="flex items-center space-x-2">
                                <div class="w-4 h-4 border-2 border-gray-300 rounded"></div>
                                <span class="text-gray-600">Chưa làm</span>
                            </div>
                            <div class="flex items-center space-x-2">
                                <div class="w-4 h-4 bg-blue-500 rounded"></div>
                                <span class="text-gray-600">Đã làm</span>
                            </div>
                        </div>
                        <div class="mt-6 pt-4 border-t">
                            <div class="text-sm text-gray-600 space-y-1">
                                <div>Tổng số câu: <span class="font-medium" id="total-question">0</span></div>
                                <div>Đã làm: <span class="font-medium text-blue-600" id="answered-count">0</span></div>
                                <div>Còn lại: <span class="font-medium text-red-600" id="remaining-count">0</span></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script>
            let timeLeft = 180;
            let answeredQuestions = new Set();
            let timerInterval;
            let submitExam = [];
            let numberHiddenDoc = 0;
            let note = '';

            let examObject = {};

            function startExam() {
                document.getElementById('examInfoPopup').style.display = 'none';
                document.getElementById('examContent').style.display = 'block';

                timeLeft = convertSpanTimeToSeconds();

                renderQuestions();
                renderQuestionMap();
                timerInterval = setInterval(updateTimer, 1000);

                document.addEventListener('visibilitychange', CheckHiddenWindow);
            }

            function CheckHiddenWindow() {
                if (document.hidden) {
                    pendingWarning = true;
                    InsertWarringHiddenWindow();
                }
            }

            async function InsertWarringHiddenWindow() {
                const examSessionCode = new URLSearchParams(window.location.search).get('examSessionCode');

                const response = await fetch('testing.aspx/InsertWarringHiddenWindow', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ examSessionCode: examSessionCode })
                });

                const res = await response.json();

                if (res.d.status !== '200') {
                    alert(res.d.message);
                    return;
                }
            }

            function updateTimer() {
                const minutes = Math.floor(timeLeft / 60);
                const seconds = timeLeft % 60;
                document.getElementById('timer').textContent = `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
                if (timeLeft <= 0) {
                    submitExamFunction();
                    return;
                }
                timeLeft--;
            }

            function convertSpanTimeToSeconds() {
                const timeSpan = document.getElementById('examPaperTime');

                if (timeSpan) {
                    const timeText = timeSpan.textContent;

                    const match = timeText.match(/\d+/);

                    if (match && match[0]) {
                        const minutes = parseInt(match[0], 10);

                        const totalSeconds = minutes * 60;

                        return totalSeconds;
                    } else {
                        return 300;
                    }
                } else {
                    return 300;
                }
            }

            function renderQuestions() {
                const list = document.getElementById('question-list');
                list.innerHTML = '';
                shuffleArray(examObject.questions);

                examObject.questions.forEach((q, index) => {
                    const qDiv = document.createElement('div');
                    qDiv.className = 'question-item border-t pt-8';
                    qDiv.id = `question-${index + 1}`;
                    const single = q.QuestionType === 'single';
                    let html = `<div class="flex items-start space-x-3 mb-4">
                        <span class="bg-blue-100 text-blue-800 px-3 py-1 rounded-full text-sm font-medium">Câu ${index + 1}</span>
                        <span class="${single ? 'bg-green-100 text-green-800' : 'bg-orange-100 text-orange-800'} px-2 py-1 rounded text-xs">${single ? 'Một đáp án' : 'Nhiều đáp án'}</span>
                    </div>
                    <h3 class="text-lg font-medium text-gray-800 mb-4">${escapeHTML(q.QuestionText)}</h3>
                    <div class="space-y-3">`;
                    q.Answers.forEach(a => {
                        html += `<label class="flex items-center space-x-3 cursor-pointer hover:bg-gray-50 p-2 rounded">
                        <input type="${single ? 'radio' : 'checkbox'}" name="q${q.QuestionCode}" value="${a.AnswerCode}" data-question-code="${q.QuestionCode}" class="text-blue-600">
                        <span>${escapeHTML(a.AnswerText)}</span>
                    </label>`;
                    });
                    html += '</div>';
                    qDiv.innerHTML = html;
                    list.appendChild(qDiv);
                });
                document.querySelectorAll('input').forEach(input => input.addEventListener('change', handleAnswerChange));
                document.getElementById('total-question').textContent = examObject.questions.length;
                document.getElementById('remaining-count').textContent = examObject.questions.length;
            }

            function shuffleArray(array) {
                let currentIndex = array.length, randomIndex;

                while (currentIndex !== 0) {
                    randomIndex = Math.floor(Math.random() * currentIndex);
                    currentIndex--;

                    [array[currentIndex], array[randomIndex]] = [
                        array[randomIndex], array[currentIndex]];
                }

                return array;
            }

            function escapeHTML(text) {
                return text
                    .replace(/</g, '&lt;')
                    .replace(/>/g, '&gt;');
            }

            function renderQuestionMap() {
                const map = document.getElementById('question-map');
                map.innerHTML = '';
                examObject.questions.forEach((q, i) => {
                    const btn = document.createElement('button');
                    btn.type = 'button';
                    btn.id = `btn-${i + 1}`;
                    btn.className = 'question-btn w-12 h-12 rounded-lg border-2 border-gray-300 hover:border-blue-500 flex items-center justify-center font-medium transition-colors';
                    btn.textContent = i + 1;
                    btn.onclick = () => document.getElementById(`question-${i + 1}`).scrollIntoView({ behavior: 'smooth' });
                    map.appendChild(btn);
                });
            }

            function handleAnswerChange() {
                const newAnswered = new Set();
                submitExam = [];
                examObject.questions.forEach(q => {
                    const inputs = document.querySelectorAll(`input[name="q${q.QuestionCode}"]:checked`);
                    if (inputs.length > 0) {
                        const answers = Array.from(inputs).map(input => ({ AnswerCode: parseInt(input.value) }));
                        submitExam.push({ QuestionCode: q.QuestionCode, Answers: answers });
                        newAnswered.add(q.QuestionCode);
                    }
                });
                answeredQuestions = newAnswered;
                document.getElementById('answered-count').textContent = answeredQuestions.size;
                document.getElementById('remaining-count').textContent = examObject.questions.length - answeredQuestions.size;
                examObject.questions.forEach((_, i) => {
                    const btn = document.getElementById(`btn-${i + 1}`);
                    if (answeredQuestions.has(examObject.questions[i].QuestionCode)) {
                        btn.classList.add('bg-blue-500', 'text-white', 'border-blue-500');
                        btn.classList.remove('border-gray-300');
                    } else {
                        btn.classList.remove('bg-blue-500', 'text-white', 'border-blue-500');
                        btn.classList.add('border-gray-300');
                    }
                });
            }

            async function btn_submitExam() {
                const confirmSubmit = confirm('Bạn có chắc chắn muốn nộp bài không?');
                if (confirmSubmit) {
                    if (submitExam === null || (Array.isArray(submitExam) && submitExam.length === 0)) {
                        const comf = confirm('Chưa làm câu nào, Bạn chắc chắn muốn nộp bài?');

                        if (comf === null || comf === false) {
                            return;
                        }
                    }

                    await submitExamFunction();
                }
            }

            async function submitExamFunction() {
                const examPaperCode = new URLSearchParams(window.location.search).get('examPaperCode');
                const examSessionCode = new URLSearchParams(window.location.search).get('examSessionCode');

                var examSubmit = {
                    examSessionCode: examSessionCode,
                    examPaperCode: examPaperCode,
                    note: note,
                    questions: submitExam,
                    questionSubmitJson: JSON.stringify(submitExam)
                };

                const response = await fetch('testing.aspx/ExamSubmit', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ examSubmit: examSubmit })
                });

                const res = await response.json();

                if (res.d.status !== '200') {
                    alert(res.d.message);
                    return;
                }
                else {
                    window.location.href = `/pages/exam-result.aspx?examSessionCode=${examSessionCode}`;
                }

                clearInterval(timerInterval);
            }

            async function LoadContentExamPaper() {
                const examPaperCode = new URLSearchParams(window.location.search).get('examPaperCode');

                const response = await fetch('testing.aspx/GetExamPaperByExamPaperCode', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ examPaperCode: examPaperCode })
                });

                const res = await response.json();

                if (res.d.status !== '200') {
                    alert(res.d.message);
                    return;
                }

                const examPaper = res.d.examPaper;

                document.getElementById('examPaperName').innerText = examPaper.ExamPaperText;
                document.getElementById('examPaperTime').innerText = examPaper.ExamTime + ' phút';
                document.getElementById('numberQuestion').innerText = examPaper.questions.length + ' câu';

                examObject = examPaper;
            }

            async function CheckSubmissionRequirements() {
                const examSessionCode = new URLSearchParams(window.location.search).get('examSessionCode');

                const response = await fetch('testing.aspx/CheckSubmissionRequirements', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ examSessionCode: examSessionCode })
                });

                const res = await response.json();

                if (res.d.status === true) {
                    note = res.d.note;
                    alert(`Bạn được yêu cầu nộp bài !. Lý do: ${note}`)
                    submitExamFunction();
                }
            }

            window.addEventListener('DOMContentLoaded', async () => {
                await LoadContentExamPaper();
                setInterval(CheckSubmissionRequirements, 5000);
            });

        </script>
    </form>
</body>

</html>
