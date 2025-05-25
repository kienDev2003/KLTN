<%@ Page Title="" Language="C#" MasterPageFile="~/pages/master/home.Master" AutoEventWireup="true" CodeBehind="subject-lecturer.aspx.cs" Inherits="KLTN.pages.subject_lecturer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="main" runat="server">
    <div class="flex-1 p-4 relative" id="main">
        <div class="bg-white rounded-lg p-5 mb-5 shadow-md">
            <h2 class="text-primary text-xl font-bold">Danh sách môn học phụ trách</h2>
        </div>
        <div id="list_subject_lecturer" runat="server" class="flex flex-wrap gap-4 max-w-6xl mx-auto">
        </div>
    </div>
</asp:Content>
