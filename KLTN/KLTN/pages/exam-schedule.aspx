<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="exam-schedule.aspx.cs" Inherits="KLTN.pages.exam_schedule" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="flex-1 p-4 relative" id="main">
        <div class="bg-white rounded-lg p-5 mb-5 shadow-md">
            <h2 class="text-primary text-xl font-bold">Danh sách ca thi</h2>
        </div>
        <div id="list_examSession" runat="server" class="flex flex-wrap gap-4 max-w-6xl mx-auto">
            <a href="pages/examSession.aspx?examSessionCode=11">
                <div class="flex flex-col bg-white rounded-lg p-5 shadow-md h-full flex items-center">
                    
                </div>
            </a>
        </div>
    </div>
</asp:Content>
