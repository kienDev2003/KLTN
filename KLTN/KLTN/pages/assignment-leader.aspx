<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="assignment-leader.aspx.cs" Inherits="KLTN.pages.schedule_leader" %>

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
                    <h2 class="text-xl font-semibold mb-4 custom-green-text">Danh sách giảng viên</h2>
                    <div id="lecturer_list" runat="server" class="grid grid-cols-1 gap-3 list-height scrollbar-thin">
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function HandleChangeLeader(element, lecturerCode) {
            let status = element.checked;

            if (status) {
                AddLeader(lecturerCode);
            }
            else {
                RemoveLeader(lecturerCode);
            }
        }

        async function AddLeader(lecturerCode) {
            const response = await fetch('assignment-leader.aspx/HandleChangeLeader', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ lecturerCode: lecturerCode, status:1 })
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

        async function RemoveLeader(lecturerCode) {
            const response = await fetch('assignment-leader.aspx/HandleChangeLeader', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ lecturerCode: lecturerCode, status: 0 })
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
