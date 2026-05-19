import React from 'react';
import { useNavigate } from 'react-router-dom';

const TeacherDetailUI = ({ detail, isLoading, error }) => {
    const navigate = useNavigate();

    if (isLoading) {
        return (
            <div className="flex-1 flex items-center justify-center bg-surface min-h-screen">
                <p className="font-body text-on-surface-variant">Đang tải thông tin chi tiết...</p>
            </div>
        );
    }

    if (error || !detail) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center bg-surface min-h-screen gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin giáo viên'}</p>
                <button onClick={() => navigate('/teachers')} className="px-4 py-2 bg-surface-container-highest rounded-lg font-label text-sm hover:bg-surface-dim transition-colors">
                    Quay lại danh sách
                </button>
            </div>
        );
    }

    const { teacherInfo, teacherTopics, teacherCourses, teacherStudents } = detail;

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-screen p-8 lg:p-12">
            {/* Header / Back */}
            <div className="mb-8 flex items-center gap-4">
                <button 
                    onClick={() => navigate('/teachers')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <h1 className="font-headline text-3xl font-extrabold text-on-surface tracking-tight">Hồ sơ Giáo viên</h1>
            </div>

            {/* Main Profile Card */}
            <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-[0_8px_24px_rgba(25,28,30,0.04)] mb-8 flex flex-col md:flex-row items-start gap-8">
                <div className="w-24 h-24 rounded-full bg-primary-container text-on-primary-container flex items-center justify-center font-headline text-3xl font-bold shadow-md flex-shrink-0">
                    {teacherInfo.Hoten?.charAt(0)}
                </div>
                <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-y-4 gap-x-8">
                    <div>
                        <h2 className="font-headline text-2xl font-bold text-on-surface mb-1">{teacherInfo.Hoten}</h2>
                        <span className="inline-flex items-center gap-1 text-sm font-label text-primary bg-primary-container/20 px-3 py-1 rounded-full">
                            <span className="material-symbols-outlined text-[16px]">badge</span>
                            {teacherInfo.MaID}
                        </span>
                    </div>
                    <div className="space-y-3">
                        <div className="flex items-center gap-3 text-on-surface-variant font-body text-sm">
                            <span className="material-symbols-outlined text-outline">mail</span>
                            {teacherInfo.Email}
                        </div>
                        <div className="flex items-center gap-3 text-on-surface-variant font-body text-sm">
                            <span className="material-symbols-outlined text-outline">call</span>
                            {teacherInfo.Dienthoai}
                        </div>
                        <div className="flex items-center gap-3 text-on-surface-variant font-body text-sm">
                            <span className="material-symbols-outlined text-outline">location_on</span>
                            {teacherInfo.Diachi}
                        </div>
                        <div className="flex items-center gap-3 text-on-surface-variant font-body text-sm">
                            <span className="material-symbols-outlined text-outline">person</span>
                            Giới tính: {teacherInfo.Gioitinh}
                        </div>
                    </div>
                </div>
            </div>

            {/* Details Grid */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
                {/* Topics */}
                <div className="bg-surface-container-lowest rounded-xl p-6 shadow-sm border border-surface-container">
                    <div className="flex items-center gap-2 mb-4 border-b border-surface-container-highest pb-3">
                        <span className="material-symbols-outlined text-primary">menu_book</span>
                        <h3 className="font-headline font-semibold text-lg text-on-surface">Chuyên đề phụ trách</h3>
                    </div>
                    {teacherTopics && teacherTopics.length > 0 ? (
                        <ul className="space-y-2">
                            {teacherTopics.map(topic => (
                                <li key={topic.MaID} className="flex items-center gap-3 font-body text-sm text-on-surface-variant bg-surface-container-low px-3 py-2 rounded-lg">
                                    <span className="w-1.5 h-1.5 rounded-full bg-secondary flex-shrink-0"></span>
                                    {topic.Ten}
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p className="font-body text-sm text-outline italic">Chưa có chuyên đề nào.</p>
                    )}
                </div>

                {/* Courses */}
                <div className="bg-surface-container-lowest rounded-xl p-6 shadow-sm border border-surface-container">
                    <div className="flex items-center gap-2 mb-4 border-b border-surface-container-highest pb-3">
                        <span className="material-symbols-outlined text-primary">school</span>
                        <h3 className="font-headline font-semibold text-lg text-on-surface">Khóa học đang dạy</h3>
                    </div>
                    {teacherCourses && teacherCourses.length > 0 ? (
                        <ul className="space-y-2">
                            {teacherCourses.map(course => (
                                <li key={course.MaID} className="flex items-center justify-between font-body text-sm text-on-surface-variant bg-surface-container-low px-3 py-2 rounded-lg">
                                    <span className="truncate">{course.Ten}</span>
                                    <span className="font-label text-xs font-medium text-outline bg-surface-container-highest px-2 py-0.5 rounded">{course.MaID}</span>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p className="font-body text-sm text-outline italic">Chưa phân công khóa học.</p>
                    )}
                </div>

                {/* Students */}
                <div className="bg-surface-container-lowest rounded-xl p-6 shadow-sm border border-surface-container lg:row-span-2">
                    <div className="flex items-center gap-2 mb-4 border-b border-surface-container-highest pb-3">
                        <span className="material-symbols-outlined text-primary">groups</span>
                        <h3 className="font-headline font-semibold text-lg text-on-surface">Sinh viên quản lý</h3>
                        <span className="ml-auto bg-primary-container text-on-primary-container font-label text-xs px-2 py-0.5 rounded-full">{teacherStudents?.length || 0}</span>
                    </div>
                    {teacherStudents && teacherStudents.length > 0 ? (
                        <ul className="space-y-3">
                            {teacherStudents.map(student => (
                                <li key={student.MaID} className="flex items-center gap-3">
                                    <div className="w-8 h-8 rounded-full bg-surface-variant flex items-center justify-center text-on-surface-variant font-headline text-xs font-bold">
                                        {student.Ten.charAt(0)}
                                    </div>
                                    <div className="flex flex-col">
                                        <span className="font-headline font-semibold text-sm text-on-surface">{student.Ten}</span>
                                        <span className="font-body text-xs text-outline">{student.MaID}</span>
                                    </div>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p className="font-body text-sm text-outline italic">Chưa có sinh viên.</p>
                    )}
                </div>
            </div>
        </div>
    );
};

export default TeacherDetailUI;
