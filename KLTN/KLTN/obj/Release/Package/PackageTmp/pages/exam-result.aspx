<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="exam-result.aspx.cs" Inherits="KLTN.pages.exam_result" %>

<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Kết Quả Bài Thi</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 20px;
        }

        .result-container {
            background: rgba(255, 255, 255, 0.95);
            backdrop-filter: blur(10px);
            border-radius: 20px;
            padding: 40px;
            max-width: 500px;
            width: 100%;
            box-shadow: 0 20px 40px rgba(0, 0, 0, 0.1);
            border: 1px solid rgba(255, 255, 255, 0.2);
            text-align: center;
            position: relative;
            overflow: hidden;
        }

        .result-container::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            height: 5px;
            background: linear-gradient(90deg, #667eea, #764ba2, #667eea);
            background-size: 200% 100%;
            animation: gradient-move 3s ease-in-out infinite;
        }

        @keyframes gradient-move {
            0%, 100% { background-position: 0% 50%; }
            50% { background-position: 100% 50%; }
        }

        .header {
            margin-bottom: 30px;
        }

        .exam-title {
            font-size: 28px;
            font-weight: bold;
            color: #2c3e50;
            margin-bottom: 10px;
            text-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        .subtitle {
            color: #7f8c8d;
            font-size: 16px;
            margin-bottom: 30px;
        }

        .score-circle {
            width: 150px;
            height: 150px;
            border-radius: 50%;
            background: conic-gradient(from 0deg, #4CAF50 0deg, #4CAF50 288deg, #e0e0e0 0deg);
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 30px;
            position: relative;
            animation: pulse 2s ease-in-out infinite;
        }

        .score-circle::before {
            content: '';
            width: 120px;
            height: 120px;
            background: white;
            border-radius: 50%;
            position: absolute;
        }

        .score-text {
            position: relative;
            z-index: 1;
            font-size: 24px;
            font-weight: bold;
            color: #2c3e50;
        }

        @keyframes pulse {
            0%, 100% { transform: scale(1); }
            50% { transform: scale(1.05); }
        }

        .result-details {
            display: grid;
            gap: 20px;
            margin-bottom: 30px;
        }

        .detail-item {
            background: rgba(108, 99, 255, 0.1);
            padding: 20px;
            border-radius: 15px;
            border-left: 4px solid #6c63ff;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
        }

        .detail-item:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(108, 99, 255, 0.2);
        }

        .detail-label {
            font-size: 14px;
            color: #666;
            margin-bottom: 5px;
            text-transform: uppercase;
            letter-spacing: 1px;
        }

        .detail-value {
            font-size: 18px;
            font-weight: bold;
            color: #2c3e50;
        }

        .correct-answers {
            color: #27ae60;
        }

        .score-value {
            color: #e74c3c;
            font-size: 24px;
        }

        .notes-section {
            background: rgba(52, 152, 219, 0.1);
            padding: 20px;
            border-radius: 15px;
            border-left: 4px solid #3498db;
            text-align: left;
        }

        .notes-title {
            color: #2c3e50;
            font-weight: bold;
            margin-bottom: 10px;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .notes-content {
            color: #555;
            line-height: 1.6;
        }

        .action-buttons {
            display: flex;
            gap: 15px;
            justify-content: center;
            margin-top: 30px;
        }

        .btn {
            padding: 12px 24px;
            border: none;
            border-radius: 25px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s ease;
            text-decoration: none;
            display: inline-block;
        }

        .btn-primary {
            background: linear-gradient(45deg, #667eea, #764ba2);
            color: white;
        }

        .btn-secondary {
            background: rgba(108, 99, 255, 0.1);
            color: #6c63ff;
            border: 2px solid #6c63ff;
        }

        .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.2);
        }

        .icon {
            width: 20px;
            height: 20px;
            display: inline-block;
        }

        @media (max-width: 768px) {
            .result-container {
                padding: 30px 20px;
            }
            
            .exam-title {
                font-size: 24px;
            }
            
            .score-circle {
                width: 120px;
                height: 120px;
            }
            
            .score-circle::before {
                width: 90px;
                height: 90px;
            }
            
            .score-text {
                font-size: 20px;
            }
            
            .action-buttons {
                flex-direction: column;
            }
        }
    </style>
</head>
<body>
    <div class="result-container">
        <div class="header">
            <h1 class="exam-title" id="examTitle">Bài Kiểm Tra Toán Học Lớp 10</h1>
        </div>

        <div class="score-circle">
            <div class="score-text" id="scoreDisplay">8.5</div>
        </div>

        <div class="result-details">
            <div class="detail-item">
                <div class="detail-label">Số câu đúng</div>
                <div class="detail-value correct-answers" id="correctAnswers">17/20</div>
            </div>
        </div>

        <div class="notes-section">
            <div class="notes-title">
                <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                    <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/>
                    <polyline points="14,2 14,8 20,8"/>
                    <line x1="16" y1="13" x2="8" y2="13"/>
                    <line x1="16" y1="17" x2="8" y2="17"/>
                    <polyline points="10,9 9,9 8,9"/>
                </svg>
                Ghi chú
            </div>
            <div class="notes-content" id="notes">
                Bạn đã hoàn thành bài thi với kết quả khá tốt! Cần ôn lại các dạng bài về phương trình bậc hai và hệ phương trình. Chúc mừng bạn đã vượt qua bài kiểm tra này.
            </div>
        </div>

        <div class="action-buttons">
            <button class="btn btn-secondary" onclick="goHome()">
                <svg class="icon" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" style="margin-right: 8px;">
                    <path d="M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z"/>
                    <polyline points="9 22 9 12 15 12 15 22"/>
                </svg>
                Về trang chủ
            </button>
        </div>
    </div>

    <script>
        document.addEventListener('DOMContentLoaded', async () => {
            const examSessionCode = new URLSearchParams(window.location.search).get('examSessionCode');

            const response = await fetch('exam-result.aspx/GetInfoExamResult', {
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
                console.log(res.d.exam_Result);
                updateExamResult(res.d.exam_Result);
            }
        })

        function updateExamResult(data) {
            document.getElementById('examTitle').textContent = data.ExamPaperName;
            document.getElementById('correctAnswers').textContent = `${data.CorrectAnswers}/${data.TotalQuestions}`;
            document.getElementById('scoreDisplay').textContent = data.Score;
            document.getElementById('notes').textContent = data.Note;
            
            const percentage = (data.CorrectAnswers / data.TotalQuestions) * 100;
            const scoreDegree = (percentage / 100) * 360;
            const scoreCircle = document.querySelector('.score-circle');
            
            let color = '#e74c3c';
            if (percentage >= 80) color = '#27ae60';
            else if (percentage >= 60) color = '#f39c12';
            
            scoreCircle.style.background = `conic-gradient(from 0deg, ${color} 0deg, ${color} ${scoreDegree}deg, #e0e0e0 0deg)`;
        }

        function goHome() {
            window.location.href = '/';
        }

    </script>
</body>
</html>
