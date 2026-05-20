import { useMemo } from 'react';
import ReportManagementUI from '../components/TopicManagement/ReportManagementUI';
import { useReport } from '../hooks/useReport';

const mapReport = (item) => ({
    id: item.id ?? item.MaID ?? item.maID,
    studentId: item.studentId ?? item.HocvienID ?? item.hocvienID ?? '',
    studentName: item.studentName ?? item.Tenhocvien ?? item.tenhocvien ?? 'N/A',
    topicName: item.topicName ?? 'Bài thu hoạch cuối khoá',
    courseId: item.courseId ?? item.KhoahocID ?? item.khoahocID ?? '',
    courseName: item.courseName ?? item.KhoahocID ?? item.khoahocID ?? 'N/A',
    deadline: item.deadline ?? '--',
    duration: item.duration ?? item.Thoigianlambai ?? item.thoigianlambai ?? 0,
    score: item.score ?? item.Diem ?? item.diem ?? null,
    status: item.status ?? ((item.Diem ?? item.diem) !== null ? 'graded' : 'submitted'),
    feedback: item.feedback ?? '',
});

const ReportManagementPage = () => {
    const { reports, isLoading, error, handleGradeReport } = useReport();

    const normalizedReports = useMemo(() => reports.map(mapReport), [reports]);

    const handleViewReport = (report) => {
        alert(`Bài thu hoạch: ${report.id} - Học viên: ${report.studentName}`);
    };

    const onGradeReport = async (report) => {
        const durationValue = window.prompt('Nhập thời gian làm bài (phút):', String(report.duration ?? 0));
        if (durationValue === null) return;

        const duration = Number(durationValue);
        if (Number.isNaN(duration) || duration < 0) {
            alert('Thời gian không hợp lệ.');
            return;
        }

        const success = await handleGradeReport({
            id: report.id,
            courseId: report.courseId,
            studentId: report.studentId,
            duration,
            score: report.score,
        });

        if (success) {
            alert('Cập nhật điểm/thời gian thành công.');
        } else {
            alert('Không thể cập nhật. Vui lòng thử lại.');
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
