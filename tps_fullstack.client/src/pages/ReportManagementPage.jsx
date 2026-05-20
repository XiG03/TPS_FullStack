import { useMemo } from 'react';
import ReportManagementUI from '../components/TopicManagement/ReportManagementUI';
import { useReport } from '../hooks/useReport';

const mapReport = (item) => ({
    id: item.id ?? item.MaID ?? item.maID,
    studentName: item.studentName ?? item.HocvienTen ?? item.hocvienTen ?? 'N/A',
    topicName: item.topicName ?? item.ChuyendeTen ?? item.chuyendeTen ?? 'N/A',
    courseName: item.courseName ?? item.KhoahocTen ?? item.khoahocTen ?? 'N/A',
    deadline: item.deadline ?? item.Hannop ?? item.hannop ?? '--',
    score: item.score ?? item.Diem ?? item.diem ?? null,
    status: item.status ?? item.Trangthai ?? item.trangthai ?? 'pending',
    feedback: item.feedback ?? item.Nhanxet ?? item.nhanxet ?? '',
});

const ReportManagementPage = () => {
    const { reports, isLoading, error, handleGradeReport } = useReport();

    const normalizedReports = useMemo(() => reports.map(mapReport), [reports]);

    const handleViewReport = (report) => {
        alert(`Chi tiết bài thu hoạch: ${report.id} - ${report.studentName}`);
    };

    const onGradeReport = async (report) => {
        const scoreValue = window.prompt(`Nhập điểm cho bài ${report.id} (0-10):`, report.score ?? '');
        if (scoreValue === null) return;

        const score = Number(scoreValue);
        if (Number.isNaN(score) || score < 0 || score > 10) {
            alert('Điểm không hợp lệ. Vui lòng nhập số từ 0 đến 10.');
            return;
        }

        const feedback = window.prompt('Nhập nhận xét:', report.feedback ?? '') ?? '';

        const success = await handleGradeReport({
            id: report.id,
            score,
            feedback,
        });

        if (success) {
            alert('Chấm điểm thành công.');
        } else {
            alert('Không thể chấm điểm. Vui lòng thử lại.');
        }
    };

    return (
        <ReportManagementUI
            reports={normalizedReports}
            isLoading={isLoading}
            error={error}
            onViewReport={handleViewReport}
            onGradeReport={onGradeReport}
        />
    );
};

export default ReportManagementPage;
