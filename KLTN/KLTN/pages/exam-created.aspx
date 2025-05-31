<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="exam-created.aspx.cs" Inherits="KLTN.pages.exam_created" %>

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
            max-width: 65%;
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
        <h2 class="text-xl font-bold mb-4" id="subject_name" runat="server"></h2>

        <div class="flex-1 p-5">
            <div class="flex justify-end">
                <input type="button" onclick="CreateExamPaper()" class="cursor-pointer bg-green-500 text-white py-2 px-4 rounded" value="Tạo đề thi">
            </div>
            <div class="mt-5 bg-white p-4 shadow-md rounded-lg">
                <h2 class="text-xl font-bold mb-4">Danh sách đề thi đã tạo</h2>
                <table class="w-full border-collapse border border-gray-300">
                    <thead>
                        <tr class="bg-gray-200">
                            <th class="border border-gray-300 px-4 py-2">Mã đề thi</th>
                            <th class="border border-gray-300 px-4 py-2">Thời gian thi</th>
                            <th class="border border-gray-300 px-4 py-2">Duyệt đề</th>
                            <th class="border border-gray-300 px-4 py-2">Giảng viên duyệt</th>
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
        <div id="examPaper-modal" class="modal">
            <div class="modal-content">
                <div id="modal-examPaper-form">
                    <div class="mb-4">
                        <label class="block text-lg font-medium text-gray-700">Thời gian làm bài</label>
                        <div class="flex items-center gap-2">
                            <input type="text" id="exam-time"
                                class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                                placeholder="Nhập thời gian làm bài">
                        </div>
                    </div>
                    <div class="chapter-section">
                        <div id="chapter-list" class="space-y-2"></div>
                        <input type="button" id="chapter-create" onclick="CreateElementChapter()"
                            class="cursor-pointer mt-2 flex items-center gap-1 text-blue-600 hover:text-blue-800"
                            value="➕ Thêm chương">
                    </div>
                    <div class="mb-4">
                        <label class="mt-3 number-question-total block text-lg font-medium text-yellow-700">Tổng số câu hỏi: </label>
                    </div>
                    <div class="mt-6 flex justify-end space-x-2">
                        <input type="button" onclick="CancelExam()"
                            class="cursor-pointer px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300"
                            value="Hủy">
                        <input type="button" id="save-exam" onclick="SaveExam()"
                            class="cursor-pointer px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
                            value="Tạo đề thi">
                        <input type="button" id="update-exam"
                            class=" hidden cursor-pointer px-4 py-2 bg-yellow-600 text-white rounded hover:bg-yellow-700"
                            value="Lưu">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        async function SaveExam() {
            const subjectCode = new URLSearchParams(location.search).get("subjectCode");
            const lecturerCode = document.getElementById('user_code').innerText;
            const examTime = document.getElementById('exam-time').value;

            const result = collectChapterData();

            const exam = {
                CreateByLectuterCode: lecturerCode,
                subjectCode: subjectCode,
                examTime: examTime,
                chapterExams: result
            };

            await InsertExam(exam);
        }

        async function InsertExam(exam) {
            const response = await fetch('exam-created.aspx/InsertExam', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ exam: exam })
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

        async function CreateElementChapter() {
            const chapter_list = document.getElementById('chapter-list');

            let chapterElem = document.createElement('div');
            chapterElem.classList.add('flex', 'chapter', 'items-center', 'gap-2', 'mt-2');
            const chapter_element = `<label class="block text-lg font-medium text-gray-700">Chương</label>
                                    <select id="exam-chapter"
                                        class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                                    </select>
                        <input type="text" class="number-basic w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" onchange="NumberQuestionChange(this)" placeholder="Số lượng đáp án dễ">
                        <input type="text" class="number-medium w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" onchange="NumberQuestionChange(this)" placeholder="Số lượng đáp án trung bình">
                        <input type="text" class="number-hard w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" onchange="NumberQuestionChange(this)" placeholder="Số lượng đáp án khó">
                        <input type="button" onclick="DeleteAnswer(this)" class="cursor-pointer text-red-600 hover:text-red-800" value="🗑">
                        <label class="number-question block text-lg font-medium text-gray-700"></label>
                        `
            chapterElem.innerHTML = chapter_element;
            chapter_list.appendChild(chapterElem);

            await HandleGetChapters(chapterElem);
        }

        function DeleteAnswer(button) {
            button.closest('.chapter').remove();
        }

        function NumberQuestionChange(element) {
            const numberQuestionBasic = Number(element.parentElement.querySelector('.number-basic').value);
            const numberQuestionMedium = Number(element.parentElement.querySelector('.number-medium').value);
            const numberQuestionHard = Number(element.parentElement.querySelector('.number-hard').value);

            const total = numberQuestionBasic + numberQuestionMedium + numberQuestionHard;

            element.parentElement.querySelector('.number-question').innerText = total;

            const totalQuestionChapters = document.querySelectorAll('.number-question');
            let totalQuestion = 0;

            for (totalQuestionChapter of totalQuestionChapters) {
                totalQuestion += Number(totalQuestionChapter.innerText);
            }

            document.querySelector('.number-question-total').innerHTML = `Tổng số câu hỏi: ${totalQuestion}`;

        }


        function CancelExam() {
            const modal = document.getElementById("examPaper-modal");
            modal.style.display = "none";
            document.getElementById('update-exam').style.display = 'none';
            document.getElementById('chapter-create').style.display = 'block';
        }

        async function CreateExamPaper() {
            const modal = document.getElementById("examPaper-modal");
            modal.style.display = 'block';
            document.getElementById('save-exam').style.display = 'block';
            document.getElementById('chapter-create').style.display = 'block';

            document.getElementById('exam-time').value = '';
            const answer_list = document.getElementById('chapter-list');
            answer_list.innerHTML = '';
        }

        async function HandleGetChapters(examChapter) {
            const subjectCode = new URLSearchParams(location.search).get("subjectCode");

            const response = await fetch('subject.aspx/HandleViewSubject', {
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

            examChapter.querySelector('#exam-chapter').innerHTML = '';

            if (res.d.subject && Array.isArray(res.d.subject.Chapters)) {
                for (let i = 0; i < res.d.subject.Chapters.length; i++) {
                    const chapter = res.d.subject.Chapters[i];

                    const html = `<option value="${chapter.ChapterCode}">${chapter.ChapterName}</option>`;

                    examChapter.querySelector('#exam-chapter').innerHTML += html;
                }
            }
        }

        function collectChapterData() {
            const chapterElements = document.querySelectorAll("#chapter-list .chapter");

            const chapterData = [];

            chapterElements.forEach((chapterElem) => {
                const selectElem = chapterElem.querySelector("select");
                const chapterCode = selectElem ? selectElem.value : null;

                const basicInput = chapterElem.querySelector(".number-basic");
                const numberBasic = basicInput && basicInput.value ? parseInt(basicInput.value, 10) : 0;

                const mediumInput = chapterElem.querySelector(".number-medium");
                const numberMedium = mediumInput && mediumInput.value ? parseInt(mediumInput.value, 10) : 0;

                const hardInput = chapterElem.querySelector(".number-hard");
                const numberHard = hardInput && hardInput.value ? parseInt(hardInput.value, 10) : 0;

                chapterData.push({
                    chapterCode,
                    numberBasic,
                    numberMedium,
                    numberHard
                });
            });

            return chapterData

        }

        function HandleViewExam(examPaperCode) {
            window.location.href = `/pages/view-exam.aspx?examPaperCode=${examPaperCode}`
        }

        async function HandleDeleteExam(examPaperCode) {
            const response = await fetch('exam-created.aspx/DeleteExam', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ examPaperCode: examPaperCode })
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

    </script>
</asp:Content>
