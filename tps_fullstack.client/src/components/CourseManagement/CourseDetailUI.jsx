import { useNavigate } from 'react-router-dom';

const formatDate = (value) => {
    if (!value) return 'Chưa cập nhật';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return 'Chưa cập nhật';
    return date.toLocaleDateString('vi-VN');
};

const formatStudyDays = (value) => {
    return value ? value.replace(/#/g, ', ') : 'Chưa cấu hình';
};

const CourseDetailUI = ({ detail, isLoading, error }) => {
    const navigate = useNavigate();

    if (isLoading) {
        return (
            <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-[calc(100vh-60px)]">
                Đang tải thông tin...
            </div>
        );
    }

    if (error || !detail) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center min-h-[calc(100vh-60px)] gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin khóa học'}</p>
                <button
                    onClick={() => navigate('/courses')}
                    className="px-4 py-2 bg-surface-container-highest rounded-lg font-label text-sm hover:bg-surface-dim transition-colors"
                >
                    Quay lại danh sách
                </button>
            </div>
        );
    }

    const topics = detail.topics || [];
    const teachers = detail.teachers || [];
    const students = detail.students || [];

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-6 lg:p-12">
            <div className="mb-8 flex items-center gap-4">
                <button
                    onClick={() => navigate('/courses')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                    title="Quay lại"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <div className="flex flex-col min-w-0">
                    <h1 className="font-headline text-2xl lg:text-3xl font-extrabold text-on-surface tracking-tight truncate">
                        {detail.ten}
                    </h1>
                </div>
                <button
                    onClick={() => navigate(`/courses/${detail.khoahocID}/edit`)}
                    className="ml-auto flex items-center gap-2 bg-primary-container text-on-primary-container px-4 py-2 rounded-lg font-label font-bold hover:bg-primary hover:text-on-primary transition-colors"
                >
                    <span className="material-symbols-outlined text-[18px]">edit</span>
                    Chỉnh sửa
                </button>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8 max-w-7xl mx-auto w-full">
                <div className="lg:col-span-1 flex flex-col gap-6">
                    <div className="bg-surface-container-lowest rounded-2xl p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-3">Mô tả khóa học</h3>
                        <p className="font-body text-sm text-on-surface-variant whitespace-pre-wrap">
                            {detail.mota || 'Không có mô tả chi tiết.'}
                        </p>
                    </div>

                    <div className="bg-surface-container-lowest rounded-2xl p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-4">Cấu hình đào tạo</h3>
                        <div className="space-y-4">
                            <div className="flex items-center gap-3">
                                <div className="w-8 h-8 rounded bg-primary/10 text-primary flex items-center justify-center">
                                    <span className="material-symbols-outlined text-sm">calendar_month</span>
                                </div>
                                <div>
                                    <p className="font-label text-[11px] font-bold text-on-surface-variant uppercase">Lịch học</p>
                                    <p className="font-body text-sm text-on-surface">{detail.sobuoihoc} buổi, thứ {formatStudyDays(detail.thu)}</p>
                                    <p className="font-body text-xs text-on-surface-variant">Bắt đầu: {formatDate(detail.ngaybatdau)}</p>
                                </div>
                            </div>
                            <div className="flex items-center gap-3">
                                <div className="w-8 h-8 rounded bg-secondary/10 text-secondary flex items-center justify-center">
                                    <span className="material-symbols-outlined text-sm">timer</span>
                                </div>
                                <div>
                                    <p className="font-label text-[11px] font-bold text-on-surface-variant uppercase">Thời lượng</p>
                                    <p className="font-body text-sm text-on-surface">{detail.thoiluonghoc} phút/buổi</p>
                                </div>
                            </div>
                            <div className="flex items-center gap-3">
                                <div className="w-8 h-8 rounded bg-tertiary/10 text-tertiary flex items-center justify-center">
                                    <span className="material-symbols-outlined text-sm">quiz</span>
                                </div>
                                <div>
                                    <p className="font-label text-[11px] font-bold text-on-surface-variant uppercase">Bài thi cuối khóa</p>
                                    <p className="font-body text-sm text-on-surface">{detail.socauhoi} câu hỏi, {detail.thoiluongthi} phút</p>
                                    <p className="font-body text-xs text-on-surface-variant mt-1">Điều kiện đạt: {detail.diemdat} điểm</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="bg-surface-container-lowest rounded-2xl p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-4">Giảng viên phụ trách</h3>
                        {teachers.length === 0 ? (
                            <p className="font-body text-sm text-outline italic">Chưa phân công giảng viên.</p>
                        ) : (
                            <div className="space-y-3">
                                {teachers.map((teacher, index) => (
                                    <div key={teacher.maID} className="flex items-center gap-3 bg-surface-container-low p-2 rounded-lg">
                                        <div className="w-8 h-8 rounded-full bg-surface-variant flex items-center justify-center text-xs font-bold">GV</div>
                                        <div className="min-w-0">
                                            <p className="font-body text-sm font-semibold truncate">Giảng viên {index + 1}</p>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </div>

                <div className="lg:col-span-2 flex flex-col gap-6">
                    <div className="bg-surface-container-lowest rounded-2xl p-6 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between mb-4 border-b border-surface-container-highest pb-3">
                            <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-primary">menu_book</span>
                                Chương trình học ({topics.length} chuyên đề)
                            </h3>
                        </div>
                        {topics.length === 0 ? (
                            <p className="font-body text-sm text-outline italic">Chưa có chuyên đề.</p>
                        ) : (
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                {topics.map((topic, index) => (
                                    <div key={topic.maID} className="bg-surface-container-low p-4 rounded-xl border border-surface-container-highest">
                                        <h4 className="font-label font-bold text-sm mb-2">Chuyên đề {index + 1}</h4>
                                        <p className="font-body text-xs text-on-surface-variant mt-1 text-primary">
                                            Trích {topic.socauhoi} câu hỏi vào đề thi
                                        </p>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>

                    <div className="bg-surface-container-lowest rounded-2xl p-6 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between mb-4 border-b border-surface-container-highest pb-3">
                            <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-secondary">groups</span>
                                Danh sách lớp ({students.length} học viên)
                            </h3>
                        </div>
                        {students.length === 0 ? (
                            <p className="font-body text-sm text-outline italic">Chưa có học viên đăng ký.</p>
                        ) : (
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-3 max-h-64 overflow-y-auto pr-2">
                                {students.map((student, index) => (
                                    <div key={student.maID} className="flex items-center justify-between bg-surface-container-low p-3 rounded-lg">
                                        <div className="flex items-center gap-3 min-w-0">
                                            <div className="w-8 h-8 rounded-full bg-surface-variant flex items-center justify-center text-xs font-bold">HV</div>
                                            <div className="min-w-0">
                                                <p className="font-body text-sm font-semibold truncate">Học viên {index + 1}</p>
                                                <p className="font-body text-[11px] text-on-surface-variant">Điểm: {student.diem ?? 0}, điều chỉnh: {student.dieuchinh ?? 0}</p>
                                            </div>
                                        </div>
                                        <span className="material-symbols-outlined text-outline text-sm">chevron_right</span>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CourseDetailUI;
