<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="assignment-lecterer.aspx.cs" Inherits="KLTN.pages.assignment_lecterer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <style>
        .custom-green-text {
            color: #28b463;
        }

        .scrollbar-thin {
            scrollbar-width: thin;
        }

            .scrollbar-thin::-webkit-scrollbar {
                width: 6px;
            }

            .scrollbar-thin::-webkit-scrollbar-track {
                background: #f1f1f1;
                border-radius: 10px;
            }

            .scrollbar-thin::-webkit-scrollbar-thumb {
                background: #28b463;
                border-radius: 10px;
            }

                .scrollbar-thin::-webkit-scrollbar-thumb:hover {
                    background: #218c4e;
                }

        .list-height {
            max-height: 550px;
            overflow-y: auto;
        }

        .list-subject-height {
            max-height: 235px;
            overflow-y: auto;
        }
    </style>

    <div class="container mx-auto px-4 py-6">
        <div class="flex flex-col md:flex-row gap-6">
            <!-- Danh sách cán bộ -->
            <div class="md:w-1/2">
                <div class="bg-white rounded-lg shadow-md p-4">
                    <h2 class="text-xl font-semibold mb-4 custom-green-text">Danh sách cán bộ</h2>
                    <div id="lecturer_list" runat="server" class="grid grid-cols-1 gap-3 list-height scrollbar-thin">
                        <!-- Cán bộ 1 -->
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Hoàng Văn Văn</p>
                                </div>
                            </div>
                        </div>
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Hoàng Văn Văn</p>
                                </div>
                            </div>
                        </div>
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Hoàng Văn Văn</p>
                                </div>
                            </div>
                        </div>
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Hoàng Văn Văn</p>
                                </div>
                            </div>
                        </div>
                        <!-- Cán bộ 2 -->
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Nguyễn Thị Lan</p>
                                </div>
                            </div>
                        </div>
                        <!-- Cán bộ 3 -->
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Trần Văn Đức</p>
                                </div>
                            </div>
                        </div>
                        <!-- Cán bộ 4 -->
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Lê Thị Hương</p>
                                </div>
                            </div>
                        </div>
                        <!-- Cán bộ 5 -->
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Phạm Văn Trung</p>
                                </div>
                            </div>
                        </div>
                        <!-- Cán bộ 6 -->
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Võ Thị Mai</p>
                                </div>
                            </div>
                        </div>
                        <!-- Cán bộ 7 -->
                        <div class="border rounded-lg overflow-hidden shadow-sm">
                            <div class="p-3 border-b flex justify-between items-center">
                                <div class="flex items-center">
                                    <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                    <p class="font-medium">Đặng Văn Tú</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Danh sách môn học -->
            <div class="md:w-1/2 flex flex-col">
                <div class="bg-white rounded-lg shadow-md p-4">
                    <h2 class="text-xl font-semibold mb-4 custom-green-text">Môn học đang phụ trách</h2>
                    <div id="subject_lecturer_list" runat="server" class="grid grid-cols-1 gap-3 list-subject-height scrollbar-thin">
                    </div>
                </div>
                <div class="bg-white rounded-lg shadow-md p-4">
                    <h2 class="text-xl font-semibold mb-4 custom-green-text">Môn học chưa được phân phụ trách</h2>
                    <div id="subject_not_lecturer_list" runat="server" class="grid grid-cols-1 gap-3 list-subject-height scrollbar-thin">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        async function HandleSelectLecturer(event, lecturerCode) {
            const containerSubjectLecturer = document.getElementById("main_subject_lecturer_list");
            containerSubjectLecturer.innerHTML = '';

            const boxes = document.querySelectorAll('.btn-lecturer');
            boxes.forEach(box => {
                boxes.forEach(item => {
                    item.classList.remove('bg-green-500', 'text-white');
                    item.classList.add('bg-gray-200');
                });

                event.classList.remove('bg-gray-200');
                event.classList.add('bg-green-500', 'text-white');
            });

            const response = await fetch('assignment-lecterer.aspx/HandleGetSubjectLecturer', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ lecturerCode: lecturerCode })
            });

            const res = await response.json();

            if (res.d.status !== "200") {
                alert(res.d.message);
            }
            else {
                HandleArrangeSubject(res.d.subjects, lecturerCode, containerSubjectLecturer);
            }

            const containerSubject = document.getElementById('main_subject_not_lecturer_list');
            const subjectItems = Array.from(containerSubject.children);

            subjectItems.forEach(item => {
                const checkbox = item.querySelector('input[type="checkbox"]');

                checkbox.setAttribute("onchange", `HandleChangeSubjectLecturer(this, '${lecturerCode}')`);
                checkbox.checked = false;
            });
        }

        function HandleArrangeSubject(subjects, lecturerCode, containerSubjectLecturer) {

            subjects.forEach(subject => {
                const html = `<div class="border rounded-lg overflow-hidden shadow-sm">
                                <div class="p-3 border-b flex justify-between items-center">
                                    <div class="flex items-center">
                                        <img src="/api/placeholder/40/40" alt="subject" class="w-8 h-8 mr-3">
                                        <p class="font-medium">${subject.SubjectName}</p>
                                    </div>
                                    <div>
                                        <input type="checkbox" id="${subject.SubjectCode}"  onchange=\"HandleChangeSubjectLecturer(this,'${lecturerCode}')\" checked class="h-5 w-5 text-green-600 rounded border-gray-300 focus:ring-green-500">
                                    </div>
                                </div>
                            </div>`

                containerSubjectLecturer.innerHTML += html;
            })
        }

        async function HandleChangeSubjectLecturer(event, lecturerCode) {
            var mode = 'uncheck';

            if (event.checked) {
                mode = 'check';
            }

            if (!lecturerCode) {
                alert('Vui lòng chọn giảng viên phân công !');
                if (mode == 'uncheck') event.checked = true;
                else event.checked = false;
                return;
            }



            if (mode == 'uncheck') {
                if (confirm(`Bạn chắc chắn muốn bỏ phân công phụ trách môn học này cho ${lecturerCode} ?`)) {
                    const subjectLecturer = {
                        subjectCode: event.id,
                        lecturerCode: lecturerCode,
                        mode: mode
                    }
                    await ChangeSubjectLecturer(subjectLecturer);
                }
                else {
                    event.checked = true;
                }
            }
            else {
                if (confirm(`Bạn chắc chắn muốn phân công phụ trách môn học này cho ${lecturerCode}`)) {
                    const subjectLecturer = {
                        subjectCode: event.id,
                        lecturerCode: lecturerCode,
                        mode: mode
                    }
                    await ChangeSubjectLecturer(subjectLecturer);
                }
                else {
                    event.checked = false;
                }
            }
        }

        async function ChangeSubjectLecturer(subjectLecturerRequest) {
            const response = await fetch('assignment-lecterer.aspx/HandleChangeSubjectLecturer', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ subjectLecturer: subjectLecturerRequest })
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
    </script>
</asp:Content>
