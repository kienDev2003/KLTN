<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="account-student.aspx.cs" Inherits="KLTN.pages.account_student" %>

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

    <div class="flex-1 p-5">
        <div class="flex justify-end">
            <input type="button" onclick="CreateStudentAccount()" class="mr-[20px] cursor-pointer bg-green-500 text-white py-2 px-4 rounded" value="Thêm tài khoản">
            <input type="button" onclick="CreateStudentAccountAuto()" class="cursor-pointer bg-green-500 text-white py-2 px-4 rounded" value="Thêm tài khoản tự động">
        </div>
        <div class="mt-5 bg-white p-4 shadow-md rounded-lg">
            <h2 class="text-xl font-bold mb-4">Danh sách sinh viên</h2>
            <table class="w-full border-collapse border border-gray-300">
                <thead>
                    <tr class="bg-gray-200">
                        <th class="border border-gray-300 px-4 py-2">Mã sinh viên</th>
                        <th class="border border-gray-300 px-4 py-2">Họ tên</th>
                        <th class="border border-gray-300 px-4 py-2">Ngày sinh</th>
                        <th class="border border-gray-300 px-4 py-2">Lớp</th>
                        <th class="border border-gray-300 px-4 py-2">Email</th>
                        <th class="border border-gray-300 px-4 py-2">Chuyên ngành</th>
                        <th class="border border-gray-300 px-4 py-2">Chức năng</th>
                    </tr>
                </thead>
                <tbody id="student_table" runat="server">
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

    <div id="student-modal" class="modal">
        <div class="modal-content">
            <div id="modal-student-form">
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Mã sinh viên</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="studentCode"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập mã sinh viên">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Họ tên</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="studentName"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập họ tên">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Ngày sinh</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="dateOfBirth"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="dd/MM/yyyy">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Lớp</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="className"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập tên lớp">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Email</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="email"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Nhập email">
                    </div>
                </div>
                <div class="mb-4">
                    <label class="block text-lg font-medium text-gray-700">Chuyên ngành</label>
                    <div class="flex items-center gap-2">
                        <input type="text" id="majorName"
                            class="w-full px-3 py-2 border rounded-md focus:ring focus:ring-blue-300"
                            placeholder="Chuyên ngành">
                    </div>
                </div>
                <div class="mt-6 flex justify-end space-x-2">
                    <input type="button" onclick="CancelStudent()"
                        class="cursor-pointer px-4 py-2 bg-gray-200 text-gray-800 rounded hover:bg-gray-300"
                        value="Hủy">
                    <input type="button" id="save-student" onclick="SaveStudent()"
                        class="cursor-pointer px-4 py-2 bg-green-600 text-white rounded hover:bg-green-700"
                        value="Thêm giảng viên">
                    <input type="button" id="update-student"
                        class=" hidden cursor-pointer px-4 py-2 bg-yellow-600 text-white rounded hover:bg-yellow-700"
                        value="Lưu">
                </div>
            </div>
        </div>
    </div>

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
        async function SaveStudent() {
            const studentCode = document.getElementById('studentCode').value;
            const studentName = document.getElementById('studentName').value;
            const dateOfBirth = document.getElementById('dateOfBirth').value;
            const className = document.getElementById('className').value;
            const email = document.getElementById('email').value;
            const majorName = document.getElementById('majorName').value;

            const student = {
                studentCode: studentCode,
                fullName: studentName,
                dateOfBirth: dateOfBirth,
                majorName: majorName,
                className: className,
                email: email
            };

            const numberRes = await InsertStudent(student);

            if (numberRes > 0) {
                alert('Thêm sinh vien thành công !');

                window.location.reload();
            }
            else {
                alert('Server Error');
            }
        }

        async function SaveStudentAuto() {
            const numberstudent = await readExcelFile();

            if (numberstudent !== 0) {
                alert(`Thêm thành công ${numberstudent} sinh viên !`);
            } else {
                alert("Lỗi thêm sinh viên");
            }
        }

        async function InsertStudent(student) {
            const response = await fetch('account-student.aspx/HandleInsertStudent', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ student: student })
            });

            const res = await response.json();

            return res.d.status === '200' ? 1 : 0;
        }

        async function readExcelFile() {
            let numberstudent = 0;
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

                                console.log(user);
                                const result = await InsertStudent(user);
                                numberstudent += result;
                            }

                            resolve(numberstudent);
                        } catch (err) {
                            console.log(err);
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


        function CancelStudent() {
            document.getElementById("student-modal").style.display = "none";;
            document.getElementById('update-student').style.display = 'none';
            document.getElementById("student-modal-auto").style.display = "none";;
            document.getElementById('update-student-auto').style.display = 'none';
        }

        function CreateStudentAccount() {
            document.getElementById("student-modal").style.display = 'block';;
            document.getElementById('save-student').style.display = 'block';

            document.getElementById('studentCode').value = '';
            document.getElementById('studentName').value = '';
            document.getElementById('dateOfBirth').value = '';
            document.getElementById('className').value = '';
            document.getElementById('email').value = '';
            document.getElementById('majorName').value = '';
        }

        function CreateStudentAccountAuto() {
            document.getElementById("student-modal-auto").style.display = 'block';;
            document.getElementById('save-student-auto').style.display = 'block';
        }
    </script>
</asp:Content>
