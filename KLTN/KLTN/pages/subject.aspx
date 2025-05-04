<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="subject.aspx.cs" Inherits="KLTN.pages.subject" %>

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

    <h2 class="text-xl font-bold mb-4 text-subject"></h2>

    <div class="flex-1 p-5">
        <div class="flex justify-end">
            <input type="button" onclick="CreateSubject()" class="cursor-pointer bg-green-500 text-white py-2 px-4 rounded" value="Tạo môn học">
        </div>
        <div class="mt-5 bg-white p-4 shadow-md rounded-lg">
            <h2 class="text-xl font-bold mb-4">Danh sách môn học</h2>
            <table class="w-full border-collapse border border-gray-300">
                <thead>
                    <tr class="bg-gray-200">
                        <th class="border border-gray-300 px-4 py-2">Tên môn học</th>
                        <th class="border border-gray-300 px-4 py-2">Số tín chỉ</th>
                        <th class="border border-gray-300 px-4 py-2">Số chương</th>
                        <th class="border border-gray-300 px-4 py-2">Ngày tạo</th>
                        <th class="border border-gray-300 px-4 py-2">Chức năng</th>
                    </tr>
                </thead>
                <tbody id="subject_table" runat="server">
                </tbody>
            </table>
        </div>
    </div>

    <div id="subject-modal" class="modal">
        <div class="modal-content">
            <div id="modal-subject-form">
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Mã môn học</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="subject-code"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập mã môn học">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Tên môn học</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="subject-name"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập tên môn học">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Số tín chỉ</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="subject-numberOfCredits"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập số tín chỉ">
                    </div>
                </div>
                <div class="chapter-section">
                    <div id="chapter-list" class="space-y-2"></div>
                    <input type="button" id="chapter-create" onclick="CreateElementChapter()"
                        class="cursor-pointer mt-2 flex items-center gap-1 text-blue-600 hover:text-blue-800"
                        value="➕ Thêm chương">
                </div>
                <div class="mt-6 flex justify-end space-x-2">
                    <input type="button" onclick="CancelSubject()"
                        class="cursor-pointer px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300"
                        value="Hủy">
                    <input type="button" id="save-subject" onclick="SaveSubject()"
                        class="cursor-pointer px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
                        value="Thêm môn học">
                    <input type="button" id="update-subject"
                        class=" hidden cursor-pointer px-4 py-2 bg-yellow-600 text-white rounded hover:bg-yellow-700"
                        value="Lưu">
                </div>
            </div>
        </div>
    </div>

    <script>
        async function HandleViewSubject(subjectCode) {
            document.getElementById('save-subject').style.display = 'none';
            document.getElementById('subject-modal').style.display = 'block';
            document.getElementById('chapter-create').style.display = 'none';

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

            document.getElementById('subject-code').value = res.d.subject.SubjectCode;
            document.getElementById('subject-name').value = res.d.subject.SubjectName;
            document.getElementById('subject-numberOfCredits').value = res.d.subject.NumberOfCredits;

            const chapter_list = document.getElementById('chapter-list');
            chapter_list.innerHTML = '';

            if (res.d.subject && Array.isArray(res.d.subject.Chapters)) {
                for (let i = 0; i < res.d.subject.Chapters.length; i++) {
                    const chapter = res.d.subject.Chapters[i];
                    CreateElementChapter();
                    const lastChapterElem = chapter_list.lastElementChild;
                    if (lastChapterElem) {
                        console.log(chapter);
                        lastChapterElem.querySelector('.chapter-name').value = chapter.ChapterName;
                    }
                }
            }

        }

        async function SaveSubject() {
            const subjectCode = document.getElementById('subject-code').value;
            const subjectName = document.getElementById('subject-name').value;
            const NumberOfCreadits = document.getElementById('subject-numberOfCredits').value;
            var chapters = [];

            const chapter_list = document.getElementById('chapter-list');
            const chaptersElem = document.querySelectorAll('.chapter').forEach(chapter => {
                const chapterName = chapter.querySelector('.chapter-name').value;

                chapters.push({
                    ChapterName: chapterName,
                });
            })

            const subjectRequest = {
                subjectCode: subjectCode,
                subjectName: subjectName,
                NumberOfCredits: NumberOfCreadits,
                chapters: chapters
            };

            const response = await fetch('subject.aspx/HandleInsertSubject', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ subjectRequest: subjectRequest })
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

        function CancelSubject() {
            const modal = document.getElementById("subject-modal");
            modal.style.display = "none";
            document.getElementById('update-subject').style.display = 'none';
            document.getElementById('subject-create').style.display = 'block';
        }

        function CreateSubject() {
            const modal = document.getElementById("subject-modal");
            modal.style.display = 'block';
            document.getElementById('save-subject').style.display = 'block';

            document.getElementById('subject-code').value = '';
            document.getElementById('subject-name').value = '';
            document.getElementById('subject-numberOfCredits').value = '';
            const chapter_list = document.getElementById('chapter-list');
            chapter_list.innerHTML = '';
        }

        function CreateElementChapter() {
            const chapter_list = document.getElementById('chapter-list');

            let chapterElem = document.createElement('div');
            chapterElem.classList.add('flex', 'chapter', 'items-center', 'gap-2', 'mt-2');
            const chapter_element = `<input type="text" class="chapter-name w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300" placeholder="Nhập tên chương">
                            <input type="button" onclick="DeleteChapter(this)" class="cursor-pointer text-red-600 hover:text-red-800" value="🗑">
                            `
            chapterElem.innerHTML = chapter_element;
            chapter_list.appendChild(chapterElem);
        }

        function DeleteChapter(button) {
            button.closest('.chapter').remove();
        }
    </script>
</asp:Content>
