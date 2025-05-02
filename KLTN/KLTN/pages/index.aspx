<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="KLTN.pages.index" %>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Hệ thống thi trắc nghiệm trực tuyến</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.3/css/all.min.css" rel="stylesheet" />
    <style>
        footer {
            position: fixed;
            bottom: 0;
            width: 100%;
        }
    </style>
</head>

<body class="bg-gray-100">
    <form runat="server">
        <header class="bg-green-600 text-white p-4">
            <div class="container mx-auto flex items-center justify-center">
                <i class="fas fa-home mr-2"></i>
                <h1 class="text-lg font-bold">HỆ THỐNG THI TRẮC NGHIỆM TRỰC TUYẾN</h1>
            </div>
        </header>
        <main class="container mx-auto mt-4 flex justify-center">
            <div class="w-full lg:w-1/3 p-2">
                <div class="bg-green-500 text-white p-2 rounded-t">
                    <h2 class="text-lg font-bold">ĐĂNG NHẬP
                    </h2>
                </div>
                <div class="bg-white p-4 rounded-b shadow">
                    <div class="mb-4">
                        <label class="block text-gray-700 text-sm font-bold mb-2" for="username">
                            Tên đăng nhập
                        </label>
                        <input
                            class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                            id="txt_userName" runat="server" placeholder="Tên đăng nhập" type="text">
                    </div>
                    <div class="mb-4">
                        <label class="block text-gray-700 text-sm font-bold mb-2" for="password">
                            Mật khẩu
                        </label>
                        <div class="relative">
                            <input
                                class="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                                id="txt_password" runat="server" placeholder="Mật khẩu" type="password">
                            <i class="fas fa-eye absolute right-3 top-3 text-gray-500"></i>
                        </div>
                    </div>
                    <div class="mb-4">
                        <a class="text-green-600 hover:underline" href="#">Quên mật khẩu
                        </a>
                    </div>
                    <div class="mb-4">
                        <input
                            class="cursor-pointer bg-green-600 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline w-full"
                            type="button" runat="server" onserverclick="HandleLogin"
                            value="Đăng nhập">
                    </div>
                </div>
            </div>
        </main>
        <footer class="bg-green-600 text-white p-4 mt-4">
            <div class="container mx-auto text-center">
                <p class="text-sm">Copyright © 2025 HỌC VIỆN NÔNG NGHIỆP VIỆT NAM</p>
            </div>
        </footer>
        <script type="text/javascript">
            function showSuccess(msg) {
                Swal.fire({
                    icon: 'success',
                    title: 'Thành công',
                    text: msg,
                    confirmButtonText: 'OK'
                });
            }

            function showError(msg) {
                Swal.fire({
                    icon: 'error',
                    title: 'Lỗi',
                    text: msg,
                    confirmButtonText: 'OK'
                });
            }

            function showWarring(msg) {
                Swal.fire({
                    icon: 'warring',
                    title: 'Cảnh báo',
                    text: msg,
                    confirmButtonText: 'OK'
                });
            }
        </script>
    </form>
</body>
</html>
