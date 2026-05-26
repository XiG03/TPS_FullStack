import { useNavigate } from 'react-router-dom';

const getValue = (source, ...keys) => {
    for (const key of keys) {
        if (source?.[key] !== undefined && source?.[key] !== null) return source[key];
    }
    return undefined;
};

const formatDate = (value) => {
    if (!value) return 'Chưa cập nhật';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return 'Chưa cập nhật';
    return date.toLocaleDateString('vi-VN');
};

const formatTime = (value) => {
    if (!value) return 'Chưa cập nhật';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return 'Chưa cập nhật';
    return date.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
};

const ScheduleDetailUI = ({ detail, isLoading, error }) => {
    const navigate = useNavigate();

    if (isLoading) {
        return (
            <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-[calc(100vh-60px)]">
                Đang tải thông tin lịch dạy học...
            </div>
        );
    }

    if (error || !detail) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center min-h-[calc(100vh-60px)] gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin lịch dạy học'}</p>
                <button onClick={() => navigate('/schedules')} className="px-4 py-2 bg-surface-container-highest rounded-lg font-label text-sm hover:bg-surface-dim transition-colors">
                    Quay lại lịch
                </button>
            </div>
        );
    }

    const courseName = getValue(detail, 'tenKhoahoc', 'TenKhoahoc') || (getValue(detail, 'khoahocID', 'KhoahocID') ? 'Đã chọn khóa học' : 'Chưa chọn');
    const topicName = getValue(detail, 'tenChuyende', 'TenChuyende') || (getValue(detail, 'chuyendeID', 'ChuyendeID') ? 'Đã chọn chuyên đề' : 'Chưa chọn');
    const teacherName = getValue(detail, 'tenGiangvien', 'TenGiangvien') || (getValue(detail, 'giangvienID', 'GiangvienID') ? 'Đã phân công' : 'Chưa chọn');
    const expectedDate = getValue(detail, 'ngaydukien', 'Ngaydukien');
    const expectedStart = getValue(detail, 'batdaudukien', 'Batdaudukien');
    const expectedEnd = getValue(detail, 'ketthucdukien', 'Ketthucdukien', 'kethucdukien', 'Kethucdukien');
    const actualDate = getValue(detail, 'ngaythucte', 'Ngaythucte');
    const actualStart = getValue(detail, 'batdauthucte', 'Batdauthucte');
    const actualEnd = getValue(detail, 'ketthucthucte', 'Ketthucthucte', 'kethucthucte', 'Kethucthucte');

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-6 lg:p-12">
            <div className="mb-8 flex items-center gap-4">
                <button
                    onClick={() => navigate('/schedules')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                    title="Quay lại"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <div className="flex flex-col min-w-0">
                    <h1 className="font-headline text-2xl lg:text-3xl font-extrabold text-on-surface tracking-tight">
                        Chi tiết lịch dạy học
                    </h1>
                    <p className="mt-1 truncate font-body text-sm text-on-surface-variant">
                        {courseName}
                    </p>
                </div>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 max-w-5xl mx-auto w-full">
                <div className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                    <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                        <span className="material-symbols-outlined text-primary">event</span>
                        Thông tin buổi học
                    </h2>
                    <dl className="space-y-4 font-body text-sm">
                        <div className="flex justify-between gap-4">
                            <dt className="text-on-surface-variant">Khóa học</dt>
                            <dd className="text-on-surface font-medium text-right">{courseName}</dd>
                        </div>
                        <div className="flex justify-between gap-4">
                            <dt className="text-on-surface-variant">Chuyên đề</dt>
                            <dd className="text-on-surface font-medium text-right">{topicName}</dd>
                        </div>
                        <div className="flex justify-between gap-4">
                            <dt className="text-on-surface-variant">Giảng viên</dt>
                            <dd className="text-on-surface font-medium text-right">{teacherName}</dd>
                        </div>
                    </dl>
                </div>

                <div className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                    <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                        <span className="material-symbols-outlined text-secondary">schedule</span>
                        Thời gian
                    </h2>
                    <dl className="space-y-4 font-body text-sm">
                        <div className="flex justify-between gap-4">
                            <dt className="text-on-surface-variant">Ngày dự kiến</dt>
                            <dd className="text-on-surface font-medium text-right">{formatDate(expectedDate)}</dd>
                        </div>
                        <div className="flex justify-between gap-4">
                            <dt className="text-on-surface-variant">Giờ dự kiến</dt>
                            <dd className="text-on-surface font-medium text-right">{formatTime(expectedStart)} - {formatTime(expectedEnd)}</dd>
                        </div>
                        <div className="flex justify-between gap-4">
                            <dt className="text-on-surface-variant">Ngày thực tế</dt>
                            <dd className="text-on-surface font-medium text-right">{formatDate(actualDate)}</dd>
                        </div>
                        <div className="flex justify-between gap-4">
                            <dt className="text-on-surface-variant">Giờ thực tế</dt>
                            <dd className="text-on-surface font-medium text-right">{formatTime(actualStart)} - {formatTime(actualEnd)}</dd>
                        </div>
                    </dl>
                </div>
            </div>
        </div>
    );
};

export default ScheduleDetailUI;
