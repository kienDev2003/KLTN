<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="assignment-teaching.aspx.cs" Inherits="KLTN.pages.assignment_teaching" %>

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
    </style>
    <div class="container mx-auto px-4 py-6">
        <div class="flex flex-col md:flex-row gap-6">
            <!-- Danh sách cán bộ -->
            <div class="md:w-1/2">
                <div class="bg-white rounded-lg shadow-md p-4">
                    <h2 class="text-xl font-semibold mb-4 custom-green-text">Danh sách cán bộ</h2>
                    <div id="lecturer_list" runat="server" class="grid grid-cols-1 gap-3 list-height scrollbar-thin">
                        
                    </div>
                </div>
            </div>
            <!-- Danh sách môn học -->
            <div class="md:w-1/2">
                <div class="bg-white rounded-lg shadow-md p-4">
                    <h2 class="text-xl font-semibold mb-4 custom-green-text">Danh sách môn học</h2>
                    <div id="subject_list" runat="server" class="grid grid-cols-1 gap-3 list-height scrollbar-thin">
                        
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        async function HandleChangeSubjectTeaching(event, lecturerCode) {
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
                if (confirm(`Bạn chắc chắn muốn bỏ phân công môn học này cho ${lecturerCode} ?`)) {
                    await ChangeSubjectTeaching(mode, lecturerCode, event.id);
                }
                else {
                    event.checked = true;
                }
            }
            else {
                if (confirm(`Bạn chắc chắn muốn phân công môn học này cho ${lecturerCode}`)) {
                    await ChangeSubjectTeaching(mode, lecturerCode, event.id);
                }
                else {
                    event.checked = false;
                }
            }
        }

        async function ChangeSubjectTeaching(mode, lecturerCode, subjectCode) {

            var subjectTeaching = {
                mode: mode,
                lecturerCode: lecturerCode,
                subjectCode: subjectCode
            };

            const response = await fetch('assignment-teaching.aspx/HandleChangeSubjectTeaching', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ subjectTeaching: subjectTeaching })
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

        async function HandleSelectLecturer(event, lecturerCode) {
            const boxes = document.querySelectorAll('.btn-lecturer');
            boxes.forEach(box => {
                boxes.forEach(item => {
                    item.classList.remove('bg-green-500', 'text-white');
                    item.classList.add('bg-gray-200');
                });

                event.classList.remove('bg-gray-200');
                event.classList.add('bg-green-500', 'text-white');
            });

            const response = await fetch('assignment-teaching.aspx/HandleGetSubjectTeaching', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ lecturerCode: lecturerCode })
            });

            const res = await response.json();

            if (res.d.status !== "200") {
                alert(res.d.message);

                const container = document.getElementById("main_subject_list");
                const subjectItems = Array.from(container.children);

                subjectItems.forEach(item => {
                    const checkbox = item.querySelector('input[type="checkbox"]');

                    checkbox.setAttribute("onchange", `HandleChangeSubjectTeaching(this, '${lecturerCode}')`);
                    checkbox.checked = false;
                });
            }
            else {
                HandleArrangeSubject(res.d.subjects, lecturerCode);
            }
        }

        function HandleArrangeSubject(listSubjectTeaching, lecturerCode) {
            const container = document.getElementById("main_subject_list");
            const idList = listSubjectTeaching.map(s => String(s.SubjectCode));
            const subjectItems = Array.from(container.children);

            var prioritized = [];
            var others = [];

            subjectItems.forEach(item => {
                const checkbox = item.querySelector('input[type="checkbox"]');

                checkbox.setAttribute("onchange", `HandleChangeSubjectTeaching(this, '${lecturerCode}')`);
                checkbox.checked = false;

                if (checkbox && idList.includes(checkbox.id)) {
                    checkbox.checked = true;
                    prioritized.push(item);
                } else {
                    others.push(item);
                }
            });

            container.innerHTML = '';

            [...prioritized, ...others].forEach(item => container.appendChild(item));
        }
    </script>

</asp:Content>
