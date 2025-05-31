<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="examSession.aspx.cs" Inherits="KLTN.pages.examSession" %>

<!DOCTYPE html>
<html lang="vi">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Quản lý Sinh viên và Ca thi</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <style>
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
            font-family: Arial, sans-serif;
        }

        body {
            background-color: #f5f5f5;
            padding: 20px;
        }

        .container {
            display: flex;
            min-height: 100vh;
            gap: 20px;
        }

        .left-panel {
            flex: 2;
            display: flex;
            flex-direction: column;
            gap: 20px;
        }

        .right-panel {
            flex: 1;
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            padding: 20px;
            overflow: auto;
        }

        .exam-info-section {
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            padding: 20px;
        }

        .add-student-section {
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            padding: 20px;
        }

        .student-table-section {
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            padding: 20px;
            flex: 1;
            overflow: auto;
        }

        h2 {
            font-size: 20px;
            margin-bottom: 20px;
            color: #333;
        }

        h3 {
            font-size: 16px;
            margin-bottom: 10px;
            color: #333;
        }

        table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

        th,
        td {
            border: 1px solid #ddd;
            padding: 10px;
            text-align: left;
        }

        th {
            background-color: #f2f2f2;
            font-weight: bold;
        }

        tr:hover {
            background-color: #f9f9f9;
        }

        .form-group {
            display: grid;
            grid-template-columns: 1fr 1fr;
            gap: 15px;
            margin-bottom: 15px;
        }

        .input-group {
            margin-bottom: 10px;
        }

        label {
            display: block;
            margin-bottom: 5px;
            font-size: 14px;
        }

        input[type="text"] {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 4px;
        }

        button {
            padding: 8px 16px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

            button:hover {
                background-color: #45a049;
            }

        .logout-btn {
            background-color: green;
            margin-bottom: 5px;
        }

            .logout-btn:hover {
                background-color: green;
            }

        .submit-examPaper-btn {
            background-color: #f44336;
        }

            .submit-examPaper-btn:hover {
                background-color: #f44336;
            }

        .exam-info {
            display: flex;
            flex-direction: column;
            gap: 12px;
        }

        .exam-field {
            display: flex;
            flex-direction: column;
            gap: 5px;
        }

            .exam-field label {
                font-weight: bold;
                color: #555;
            }

        .exam-info {
            display: flex;
            flex-direction: column;
            gap: 10px;
        }

        .exam-field {
            line-height: 1.5;
        }

            .exam-field label {
                font-weight: bold;
                color: #333;
                margin-right: 5px;
            }

        .info-text {
            color: #666;
        }

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

        .notification-item {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 12px;
            border: 1px solid #e5e7eb;
            border-radius: 6px;
            margin-bottom: 10px;
            background-color: #f9fafb;
        }

        .notification-content {
            flex: 1;
            margin-right: 10px;
        }
    </style>
</head>

<body>
    <form runat="server">
        <div class="container">
            <!-- Phần bên trái -->
            <div class="left-panel">
                <!-- Thêm sinh viên -->
                <div class="add-student-section">
                    <h3>Thêm sinh viên vào ca thi</h3>
                    <form id="add-student-form">
                        <div class="form-group">
                            <div class="input-group">
                                <label for="student-id">Mã SV</label>
                                <input type="text" id="student-id">
                            </div>
                        </div>
                        <div class="flex flex-wrap gap-2">
                            <input type="button" class="cursor-pointer px-3 py-2 bg-green-600 text-white rounded hover:bg-green-700" id="AddStudent" runat="server" onclick="addStudent()" value="Thêm sinh viên">
                            <input type="button" class="cursor-pointer px-3 py-2 bg-green-600 text-white rounded hover:bg-green-700" id="AddStudentAuto" runat="server" onclick="CreateStudentAccountAuto()" value="Thêm sinh viên tự động">
                            <input type="button" class="hidden cursor-pointer px-3 py-2 bg-yellow-600 text-white rounded hover:bg-yellow-700" id="ExportTestScores" onclick="ExportListScores()" runat="server" value="Xuất danh sách điểm thi">
                        </div>
                    </form>
                </div>

                <!-- Bảng sinh viên -->
                <div class="student-table-section">
                    <h2>Thông tin sinh viên</h2>
                    <input type="button" class="cursor-pointer px-2 mt-3 mb-3 bg-green-600 text-white rounded hover:bg-green-700" onclick="GetStudent()" value="Reload">
                    <table>
                        <thead>
                            <tr>
                                <th>Mã SV</th>
                                <th>Họ tên</th>
                                <th>Ngày sinh</th>
                                <th>Lớp</th>
                                <th>Trạng thái</th>
                                <th>Chức năng</th>
                            </tr>
                        </thead>
                        <tbody id="student-table-body">
                        </tbody>
                    </table>
                </div>
            </div>

            <!-- Phần bên phải -->
            <div class="right-panel">
                <!-- Thông tin ca thi -->
                <div class="exam-info-section mb-5">
                    <h2>Thông tin ca thi</h2>
                    <div class="exam-info" id="exam_info" runat="server">
                    </div>
                </div>

                <!-- Bảng thông báo -->
                <div>
                    <h2>Thông báo</h2>
                    <div id="notifications-container">
                    </div>
                </div>
            </div>
        </div>

        <!-- Modal thêm sinh viên tự động -->
        <div id="student-modal-auto" class="modal">
            <div class="modal-content">
                <div id="modal-student-form-auto">
                    <div class="mb-4">
                        <a class="bg-yellow-300 w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            href="/pages/public/Student.xlsx">Tải file mẫu tại đây !</a>
                    </div>
                    <div class="mb-4">
                        <label class="block text-lg font-medium text-gray-700">Tải file lên</label>
                        <div class="flex items-center gap-2">
                            <input type="file" id="file"
                                class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300">
                        </div>
                    </div>
                    <div class="mt-6 flex justify-end space-x-2">
                        <input type="button" onclick="CancelStudent()"
                            class="cursor-pointer px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300"
                            value="Hủy">
                        <input type="button" id="save-student-auto" onclick="SaveStudentAuto()"
                            class="cursor-pointer px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
                            value="Thêm các giảng viên">
                    </div>
                </div>
            </div>
        </div>

        <script src="https://unpkg.com/xlsx/dist/xlsx.full.min.js"></script>
        <script>
            function formatMsDate(dateString) {
                const match = dateString.match(/\/Date\((\d+)\)\//);

                if (!match || match.length < 2) {
                    console.error("Định dạng ngày không hợp lệ:", dateString);
                    return "Ngày không hợp lệ";
                }

                const timestamp = parseInt(match[1], 10);
                const date = new Date(timestamp);

                const hours = date.getHours().toString().padStart(2, '0');
                const minutes = date.getMinutes().toString().padStart(2, '0');
                const seconds = date.getSeconds().toString().padStart(2, '0');

                const day = date.getDate().toString().padStart(2, '0');
                const month = (date.getMonth() + 1).toString().padStart(2, '0');
                const year = date.getFullYear();

                return `${hours}:${minutes}:${seconds} ${day}/${month}/${year}`;
            }

            async function ExportListScores() {
                const examSessionCode = new URLSearchParams(location.search).get("examSessionCode");

                const response = await fetch('examSession.aspx/HandleExportListScroes', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ examSessionCode: examSessionCode })
                });

                const res = await response.json();

                if (res.d.status !== '200') {
                    alert(res.d.message);
                }
                else {
                    RenderFileExcel(res.d.examSubmitteds);
                }
            }

            function RenderFileExcel(examSubmitteds) {
                const worksheet = XLSX.utils.json_to_sheet(examSubmitteds, {
                    header: [
                        "SubjectName", "ExamSessionCode", "ExamPaperCode", "StudentCode", "StudentName", "StudentDateOfBrith",
                        "StudentClassName", "SubmittedDate", "Score", "Note"
                    ]
                });

                // Đổi tiêu đề tiếng Việt
                const headerMap = [
                    { col: "A1", label: "Môn học" },
                    { col: "B1", label: "Mã ca thi" },
                    { col: "C1", label: "Mã đề thi" },
                    { col: "D1", label: "Mã sinh viên" },
                    { col: "E1", label: "Họ tên" },
                    { col: "F1", label: "Ngày sinh" },
                    { col: "G1", label: "Lớp" },
                    { col: "H1", label: "Thời gian nộp bài" },
                    { col: "I1", label: "Điểm" },
                    { col: "J1", label: "Ghi chú" },
                ];

                // Căn giữa toàn bộ dữ liệu trước
                const range = XLSX.utils.decode_range(worksheet['!ref']);
                for (let R = 0; R <= range.e.r; ++R) {
                    for (let C = 0; C <= range.e.c; ++C) {
                        const cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
                        if (worksheet[cellAddress]) {
                            worksheet[cellAddress].s = {
                                alignment: {
                                    horizontal: "center",
                                    vertical: "center"
                                }
                            };
                        }
                    }
                }

                headerMap.forEach(({ col, label }) => {
                    worksheet[col] = {
                        v: label,
                        t: 's',
                        s: {
                            font: { bold: true },
                            alignment: {
                                horizontal: "center",
                                vertical: "middle"
                            }
                        }
                    };
                });

                const colWidths = headerMap.map(({ label }) => ({ wch: label.length }));

                const allKeys = [
                    "SubjectName", "ExamSessionCode", "ExamPaperCode", "StudentCode", "StudentName", "StudentDateOfBrith",
                    "StudentClassName", "SubmittedDate", "Score", "Note"
                ];

                examSubmitteds.forEach(row => {
                    allKeys.forEach((key, idx) => {
                        const val = row[key];
                        const len = String(val || "").length;
                        if (len > colWidths[idx].wch) {
                            colWidths[idx].wch = len + 2;
                        }
                    });
                });

                worksheet["!cols"] = colWidths;

                const workbook = XLSX.utils.book_new();
                XLSX.utils.book_append_sheet(workbook, worksheet, "DanhSach");
                XLSX.writeFile(workbook, "Danh_Sach_Diem_Thi.xlsx");
            }


            async function GetExamSessionWarring() {
                const examSessionCode = new URLSearchParams(location.search).get("examSessionCode");
                const container = document.getElementById('notifications-container');
                container.innerHTML = '';

                const response = await fetch('examSession.aspx/HandleGetExamSessionWarring', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ examSessionCode: examSessionCode })
                });

                const res = await response.json();

                if (res.d.status !== '200') {
                    return;
                }
                else {
                    const examSessionWarrings = res.d.examSessionWarrings;

                    for (examSessionWarring of examSessionWarrings) {
                        const notificationHTML = `
                                                    <div class="notification-item">
                                                        <div class="notification-content">
                                                            <p class="text-sm font-medium text-gray-800">Sinh viên mã ${examSessionWarring.StudentCode} đã ẩn màn hình thi lúc ${formatMsDate(examSessionWarring.DateWarring)}</p>
                                                        </div>
                                                        <button type="button" class="px-3 py-1 bg-blue-600 text-white text-xs rounded hover:bg-blue-700" onclick="CheckedWarring('${examSessionWarring.StudentCode}',${examSessionWarring.ExamSessionCode})">
                                                            Đã kiểm tra
                                                        </button>
                                                    </div>
                                                `;
                        container.insertAdjacentHTML('afterbegin', notificationHTML);
                    }
                }
            }

            async function CheckedWarring(studentCode, examSessionCode) {
                const response = await fetch('examSession.aspx/HandleCheckedWarring', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ studentCode: studentCode, examSessionCode: examSessionCode })
                });

                const res = await response.json();

                if (res.d.status === '200') {
                    GetExamSessionWarring();
                }
                else {
                    alert(res.d.message);
                }
            }

            async function GetStudent() {
                const examSessionCode = new URLSearchParams(location.search).get("examSessionCode");

                const response = await fetch('examSession.aspx/GetListStudent', {
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
                else {
                    LoadTableStudent(res.d.examSession_Students);
                }
            }

            async function LoadTableStudent(examSession_Students) {
                const tableStudent = document.getElementById('student-table-body');
                tableStudent.innerHTML = '';

                for (var examSession_Student of examSession_Students) {

                    const response = await fetch('examSession.aspx/GetInfoStudentByStudentCode', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify({ studentCode: examSession_Student.StudentCode })
                    });

                    const res = await response.json();

                    if (res.d.status !== '200') {
                        alert(res.d.message);
                        return;
                    }

                    const html = `<tr>
                                <td>${res.d.student.StudentCode}</td>
                                <td>${res.d.student.FullName}</td>
                                <td>${convertDatetime(res.d.student.DateOfBirth)}</td>
                                <td>${res.d.student.ClassName}</td>
                                <td>${(examSession_Student.StudentHaveEntered ? 'Đã vào' : "Chưa vào")}</td>
                                <td>
                                    <input type="button" class="${(examSession_Student.StudentHaveEntered ? '' : "hidden")} mb-1 cursor-pointer px-2 py-1 bg-green-600 text-white rounded hover:bg-green-700" onclick="LogoutStudent('${res.d.student.StudentCode}')" value="Cấp quyền vào lại">
                                    <input type="button" class="${(examSession_Student.StudentHaveEntered ? '' : "hidden")} mb-1 cursor-pointer px-2 py-1 bg-red-600 text-white rounded hover:bg-red-700" onclick="SubmissionRequirements('${res.d.student.StudentCode}')" value="Yêu cầu nộp bài">
                                </td>
                            </tr>`;

                    tableStudent.innerHTML += html;
                }
            }

            async function SubmissionRequirements(studentCode) {
                const examSessionCode = new URLSearchParams(location.search).get("examSessionCode");

                let noteSubmissionRequirements = prompt("Nhập lý do yêu cầu nộp bài:");

                if (noteSubmissionRequirements === null) {
                    noteSubmissionRequirements = ''
                };

                const response = await fetch('examSession.aspx/HandleSubmissionRequirements', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ studentCode: studentCode, examSessionCode: examSessionCode, noteSubmissionRequirements: noteSubmissionRequirements })
                });

                const res = await response.json();

                if (res.d.status !== '200') {
                    alert(res.d.message);
                    return;
                }
                else {
                    alert(res.d.message);
                }
            }

            async function LogoutStudent(studentCode) {
                const examSessionCode = new URLSearchParams(location.search).get("examSessionCode");

                const response = await fetch('examSession.aspx/HandleLogoutStudent', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ studentCode: studentCode, examSessionCode: examSessionCode })
                });

                const res = await response.json();

                if (res.d.status !== '200') {
                    alert(res.d.message);
                    return;
                }
                else {
                    alert(res.d.message);
                    GetStudent();
                }
            }

            function CreateStudentAccountAuto() {
                document.getElementById("student-modal-auto").style.display = 'block';;
                document.getElementById('save-student-auto').style.display = 'block';
            }

            function CancelStudent() {
                document.getElementById("student-modal-auto").style.display = "none";;
                document.getElementById('update-student-auto').style.display = 'none';
            }

            function convertDatetime(DateString) {
                const timestamp = parseInt(DateString.match(/\d+/)[0], 10);
                const date = new Date(timestamp);
                return date.toLocaleDateString('vi-VN');
            }

            async function addStudent() {
                const studentCode = document.getElementById('student-id').value;
                const examSessionCode = new URLSearchParams(location.search).get("examSessionCode");

                const res = await InsertStudent(studentCode, examSessionCode)

                if (res > 0) {
                    alert('Them student thanh cong');
                    addNotification(`Đã thêm sinh viên ${studentCode} vào ca thi`);
                    document.getElementById('student-id').value = '';
                    GetStudent();
                }
                else {
                    alert('Loi');
                }
            }

            async function SaveStudentAuto() {
                const numberstudent = await readExcelFile();

                if (numberstudent !== 0) {
                    alert(`Thêm thành công ${numberstudent} sinh viên !`);
                    addNotification(`Đã thêm ${numberstudent} sinh viên từ file Excel`);
                    CancelStudent();
                    GetStudent();
                } else {
                    alert("Lỗi thêm sinh viên");
                }
            }

            async function readExcelFile() {
                let numberstudent = 0;
                const examSessionCode = new URLSearchParams(location.search).get("examSessionCode");
                const fileInput = document.getElementById('file');
                const file = fileInput.files[0];

                if (file) {
                    const reader = new FileReader();

                    return new Promise((resolve, reject) => {
                        reader.onload = async function (event) {
                            try {
                                const data = new Uint8Array(event.target.result);
                                const workbook = XLSX.read(data, { type: 'array' });
                                const firstSheetName = workbook.SheetNames[0];
                                const worksheet = workbook.Sheets[firstSheetName];
                                const users = XLSX.utils.sheet_to_json(worksheet);

                                for (const user of users) {
                                    if (user.DateOfBirth) {
                                        const date = XLSX.SSF.parse_date_code(user.DateOfBirth);
                                        const day = date.d < 10 ? '0' + date.d : date.d;
                                        const month = date.m < 10 ? '0' + date.m : date.m;
                                        const year = date.y;
                                        user.DateOfBirth = `${day}/${month}/${year}`;
                                    }

                                    const result = await InsertStudent(user.StudentCode, examSessionCode);
                                    numberstudent += result;
                                }

                                resolve(numberstudent);
                            } catch (err) {
                                reject(0);
                            }
                        };

                        reader.onerror = function () {
                            document.getElementById('output').innerText = "Lỗi đọc file.";
                            reject(0);
                        };

                        reader.readAsArrayBuffer(file);
                    });
                } else {
                    alert('Vui lòng chọn một file Excel.');
                    return 0;
                }
            }

            async function InsertStudent(studentCode, examSessionCode) {
                const response = await fetch('examSession.aspx/InsertStudentInExamSession_Student', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ studentCode: studentCode, examSessionCode: examSessionCode })
                });

                const res = await response.json();

                return res.d.status === '200' ? 1 : 0;
            }

            function CheckEndExamSession() {

            }

            function HandleWindowLoaded() {
                GetStudent();
                CheckEndExamSession();
                setInterval(GetExamSessionWarring, 10000);
                GetExamSessionWarring();
            }

            window.addEventListener('DOMContentLoaded', HandleWindowLoaded);
        </script>
    </form>
</body>

</html>
