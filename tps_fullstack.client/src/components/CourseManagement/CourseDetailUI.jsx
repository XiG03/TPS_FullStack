import React from 'react';
import { useNavigate } from 'react-router-dom';

const CourseDetailUI = ({ detail, isLoading, error }) => {
    const navigate = useNavigate();

    if (isLoading) {
        return <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-[calc(100vh-60px)]">Đang tải thông tin...</div>;
    }

    if (error || !detail || !detail.courseInfo) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center min-h-[calc(100vh-60px)] gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin khóa học'}</p>
                <button onClick={() => navigate('/courses')} className="px-4 py-2 bg-surface-container-highest rounded-lg font-label text-sm hover:bg-surface-dim transition-colors">Quay lại danh sách</button>
            </div>
        );
    }

    const { courseInfo, courseTopics = [], courseTeachers = [], courseStudents = [], courseSchedules = [] } = detail;

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-8 lg:p-12">
            {/* Header */}
            <div className="mb-8 flex items-center gap-4">
                <button onClick={() => navigate('/courses')} className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface">
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <div className="flex flex-col">
                    <h1 className="font-headline text-3xl font-extrabold text-on-surface tracking-tight">{courseInfo.Ten}</h1>
                    <span className="font-label text-sm text-on-surface-variant font-bold">Mã Khóa học: {courseInfo.MaID}</span>
                </div>
                <button onClick={() => navigate(`/courses/${courseInfo.MaID}/edit`)} className="ml-auto flex items-center gap-2 bg-primary-container text-on-primary-container px-4 py-2 rounded-lg font-label font-bold hover:bg-primary hover:text-on-primary transition-colors">
                    <span className="material-symbols-outlined text-[18px]">edit</span>
                    Chỉnh sửa
                </button>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8 max-w-7xl mx-auto w-full">
                
                {/* Cột trái: Thông tin tổng quan (Chiếm 1 phần) */}
                <div className="lg:col-span-1 flex flex-col gap-6">
                    {/* Thẻ mô tả */}
                    <div className="bg-surface-container-lowest rounded-3xl p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-3">Mô tả Khóa học</h3>
                        <p className="font-body text-sm text-on-surface-variant whitespace-pre-wrap">{courseInfo.Mota || 'Không có mô tả chi tiết.'}</p>
                    </div>

                    {/* Thẻ cấu hình hệ thống */}
                    <div className="bg-surface-container-lowest rounded-3xl p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-4">Cấu hình Đào tạo</h3>
                        <div className="space-y-4">
                            <div className="flex items-center gap-3">
                                <div className="w-8 h-8 rounded bg-primary/10 text-primary flex items-center justify-center"><span className="material-symbols-outlined text-sm">calendar_month</span></div>
                                <div>
                                    <p className="font-label text-[11px] font-bold text-on-surface-variant uppercase">Lịch học</p>
                                    <p className="font-body text-sm text-on-surface">{courseInfo.Sobuoihoc} buổi • Học thứ {courseInfo.Thu}</p>
                                </div>
                            </div>
                            <div className="flex items-center gap-3">
                                <div className="w-8 h-8 rounded bg-secondary/10 text-secondary flex items-center justify-center"><span className="material-symbols-outlined text-sm">timer</span></div>
                                <div>
                                    <p className="font-label text-[11px] font-bold text-on-surface-variant uppercase">Thời lượng</p>
                                    <p className="font-body text-sm text-on-surface">{courseInfo.Thoiluonghoc} phút/buổi</p>
                                </div>
                            </div>
                            <div className="flex items-center gap-3">
                                <div className="w-8 h-8 rounded bg-tertiary/10 text-tertiary flex items-center justify-center"><span className="material-symbols-outlined text-sm">quiz</span></div>
                                <div>
                                    <p className="font-label text-[11px] font-bold text-on-surface-variant uppercase">Bài thi cuối khóa</p>
                                    <p className="font-body text-sm text-on-surface">{courseInfo.Socauhoi} câu hỏi • {courseInfo.Thoiluongthi} phút</p>
                                    <p className="font-body text-xs text-on-surface-variant mt-1">Điều kiện đạt: {courseInfo.Diemdat} điểm</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* Thẻ giảng viên phụ trách */}
                    <div className="bg-surface-container-lowest rounded-3xl p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-4">Giảng viên phụ trách</h3>
                        {courseTeachers.length === 0 ? <p className="font-body text-sm text-outline italic">Chưa phân công giảng viên</p> : 
                        <div className="space-y-3">
                            {courseTeachers.map(teacher => (
                                <div key={teacher.MaID} className="flex items-center gap-3 bg-surface-container-low p-2 rounded-lg">
                                    <div className="w-8 h-8 rounded-full bg-surface-variant flex items-center justify-center text-xs font-bold">{teacher.Hoten?.substring(0, 2).toUpperCase() || 'GV'}</div>
                                    <p className="font-body text-sm font-semibold">{teacher.Hoten}</p>
                                </div>
                            ))}
                        </div>}
                    </div>
                </div>

                {/* Cột phải: Tabs quản lý chi tiết (Chiếm 2 phần) */}
                <div className="lg:col-span-2 flex flex-col gap-6">
                    {/* Danh sách chuyên đề */}
                    <div className="bg-surface-container-lowest rounded-3xl p-6 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between mb-4 border-b border-surface-container-highest pb-3">
                            <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-primary">menu_book</span> Chương trình học ({courseTopics.length} chuyên đề)
                            </h3>
                        </div>
                        {courseTopics.length === 0 ? <p className="font-body text-sm text-outline italic">Chưa có chuyên đề.</p> :
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            {courseTopics.map(topic => (
                                <div key={topic.MaID} className="bg-surface-container-low p-4 rounded-xl border border-surface-container-highest">
                                    <h4 className="font-label font-bold text-sm mb-2">{topic.Ten}</h4>
                                    <p className="font-body text-xs text-on-surface-variant">ID: {topic.ChuyendeID}</p>
                                    <p className="font-body text-xs text-on-surface-variant mt-1 text-primary">Trích {topic.SoCauhoi} câu hỏi vào đề thi</p>
                                </div>
                            ))}
                        </div>}
                    </div>

                    {/* Danh sách học viên */}
                    <div className="bg-surface-container-lowest rounded-3xl p-6 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between mb-4 border-b border-surface-container-highest pb-3">
                            <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-secondary">groups</span> Danh sách Lớp ({courseStudents.length} học viên)
                            </h3>
                        </div>
                        {courseStudents.length === 0 ? <p className="font-body text-sm text-outline italic">Chưa có học viên đăng ký.</p> :
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-3 max-h-64 overflow-y-auto pr-2">
                            {courseStudents.map(student => (
                                <div key={student.MaID} className="flex items-center justify-between bg-surface-container-low p-3 rounded-lg">
                                    <div className="flex items-center gap-3">
                                        <div className="w-8 h-8 rounded-full bg-surface-variant flex items-center justify-center text-xs font-bold">{student.Hoten?.substring(0, 2).toUpperCase() || 'HV'}</div>
                                        <div>
                                            <p className="font-body text-sm font-semibold">{student.Hoten}</p>
                                            <p className="font-body text-[11px] text-on-surface-variant">{student.HocvienID}</p>
                                        </div>
                                    </div>
                                    <span className="material-symbols-outlined text-outline text-sm">chevron_right</span>
                                </div>
                            ))}
                        </div>}
                    </div>

                    {/* Lịch trình dự kiến */}
                    <div className="bg-surface-container-lowest rounded-3xl p-6 shadow-sm border border-surface-container">
                        <div className="flex items-center justify-between mb-4 border-b border-surface-container-highest pb-3">
                            <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-tertiary">edit_calendar</span> Lịch trình học tập
                            </h3>
                        </div>
                        {courseSchedules.length === 0 ? <p className="font-body text-sm text-outline italic">Chưa khởi tạo lịch học.</p> :
                        <div className="flex flex-col gap-2 max-h-64 overflow-y-auto pr-2">
                            {courseSchedules.map((schedule, idx) => (
                                <div key={schedule.MaID} className="flex items-center gap-4 bg-surface-container-low py-3 px-4 rounded-lg">
                                    <div className="font-headline font-bold text-lg text-outline w-12 text-center border-r border-outline-variant pr-4">
                                        B{idx + 1}
                                    </div>
                                    <div className="flex-1">
                                        <p className="font-body font-semibold text-sm">Ngày: {new Date(schedule.Ngaydukien).toLocaleDateString('vi-VN')}</p>
                                        <p className="font-body text-xs text-on-surface-variant">Thời gian: {new Date(schedule.Batdaudukien).toLocaleTimeString('vi-VN', {hour: '2-digit', minute:'2-digit'})} - {new Date(schedule.Ketthucdukien).toLocaleTimeString('vi-VN', {hour: '2-digit', minute:'2-digit'})}</p>
                                    </div>
                                </div>
                            ))}
                        </div>}
                    </div>

                </div>
            </div>
        </div>
    );
};

export default CourseDetailUI;
