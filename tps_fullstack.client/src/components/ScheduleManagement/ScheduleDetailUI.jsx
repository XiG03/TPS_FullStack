import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const ScheduleDetailUI = ({ detail, setDetail, isLoading, error, onSaveAttendance }) => {
    const navigate = useNavigate();
    const [isSaving, setIsSaving] = useState(false);

    if (isLoading) {
        return <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-[calc(100vh-60px)]">Đang tải thông tin điểm danh...</div>;
    }

    if (error || !detail || !detail.info) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center min-h-[calc(100vh-60px)] gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin lịch học'}</p>
                <button onClick={() => navigate('/schedules')} className="px-4 py-2 bg-surface-container-highest rounded-lg font-label text-sm hover:bg-surface-dim transition-colors">Quay lại Lịch</button>
            </div>
        );
    }

    const { info, teacherAttendance, studentAttendance } = detail;

    const toggleTeacher = (index) => {
        const newArr = [...teacherAttendance];
        newArr[index].IsPresent = !newArr[index].IsPresent;
        setDetail(prev => ({ ...prev, teacherAttendance: newArr }));
    };

    const toggleStudent = (index) => {
        const newArr = [...studentAttendance];
        newArr[index].IsPresent = !newArr[index].IsPresent;
        setDetail(prev => ({ ...prev, studentAttendance: newArr }));
    };

    const handleSave = async () => {
        setIsSaving(true);
        const success = await onSaveAttendance({
            teacherAttendance,
            studentAttendance
        });
        setIsSaving(false);
        if (success) alert('Đã lưu điểm danh thành công!');
        else alert('Có lỗi xảy ra khi lưu điểm danh.');
    };

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-8 lg:p-12">
            <div className="mb-8 flex items-center justify-between">
                <div className="flex items-center gap-4">
                    <button onClick={() => navigate('/schedules')} className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface">
                        <span className="material-symbols-outlined text-xl">arrow_back</span>
                    </button>
                    <div className="flex flex-col">
                        <h1 className="font-headline text-3xl font-extrabold text-on-surface tracking-tight">Điểm danh Lớp học</h1>
                        <span className="font-label text-sm text-on-surface-variant font-bold">Khóa học: {info.TenKhoahoc} | ID Lịch: {info.MaID}</span>
                    </div>
                </div>
                <button onClick={handleSave} disabled={isSaving} className="flex items-center gap-2 bg-primary text-on-primary px-6 py-2.5 rounded-lg font-label font-bold shadow hover:shadow-md transition-all hover:-translate-y-0.5 disabled:opacity-70">
                    <span className="material-symbols-outlined text-[18px]">save</span>
                    {isSaving ? 'Đang lưu...' : 'Lưu Điểm danh'}
                </button>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8 max-w-6xl mx-auto w-full">
                
                {/* Bảng Giảng viên */}
                <div className="bg-surface-container-lowest rounded-3xl p-6 shadow-sm border border-surface-container flex flex-col h-full">
                    <div className="flex items-center justify-between mb-4 border-b border-surface-container-highest pb-3">
                        <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">record_voice_over</span> Giảng viên
                        </h3>
                        <span className="font-label text-xs font-bold text-primary bg-primary/10 px-3 py-1 rounded-full">
                            Có mặt: {teacherAttendance.filter(t => t.IsPresent).length}/{teacherAttendance.length}
                        </span>
                    </div>
                    {teacherAttendance.length === 0 ? <p className="font-body text-sm text-outline italic">Không có giảng viên phụ trách.</p> :
                    <div className="flex flex-col gap-2">
                        {teacherAttendance.map((t, index) => (
                            <label key={t.MaID} className="flex items-center justify-between bg-surface-container-low p-3 rounded-xl cursor-pointer hover:bg-surface-container-highest transition-colors">
                                <div className="flex items-center gap-3">
                                    <div className="w-8 h-8 rounded-full bg-primary-container text-on-primary-container flex items-center justify-center text-xs font-bold">{t.Ten?.substring(0, 2).toUpperCase() || 'GV'}</div>
                                    <div>
                                        <p className="font-body font-semibold text-sm">{t.Ten}</p>
                                        <p className="font-body text-[11px] text-on-surface-variant">ID: {t.GiangvienID}</p>
                                    </div>
                                </div>
                                <div className="flex items-center gap-2">
                                    <span className={`font-label text-xs font-bold ${t.IsPresent ? 'text-primary' : 'text-error'}`}>{t.IsPresent ? 'Có mặt' : 'Vắng'}</span>
                                    <input type="checkbox" checked={t.IsPresent} onChange={() => toggleTeacher(index)} className="w-5 h-5 accent-primary cursor-pointer" />
                                </div>
                            </label>
                        ))}
                    </div>}
                </div>

                {/* Bảng Học viên */}
                <div className="bg-surface-container-lowest rounded-3xl p-6 shadow-sm border border-surface-container flex flex-col h-full">
                    <div className="flex items-center justify-between mb-4 border-b border-surface-container-highest pb-3">
                        <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-secondary">groups</span> Học viên
                        </h3>
                        <span className="font-label text-xs font-bold text-secondary bg-secondary/10 px-3 py-1 rounded-full">
                            Có mặt: {studentAttendance.filter(s => s.IsPresent).length}/{studentAttendance.length}
                        </span>
                    </div>
                    {studentAttendance.length === 0 ? <p className="font-body text-sm text-outline italic">Không có học viên nào.</p> :
                    <div className="flex flex-col gap-2 max-h-[500px] overflow-y-auto pr-2">
                        {studentAttendance.map((s, index) => (
                            <label key={s.MaID} className="flex items-center justify-between bg-surface-container-low p-3 rounded-xl cursor-pointer hover:bg-surface-container-highest transition-colors">
                                <div className="flex items-center gap-3">
                                    <div className="w-8 h-8 rounded-full bg-secondary-container text-on-secondary-container flex items-center justify-center text-xs font-bold">{s.Ten?.substring(0, 2).toUpperCase() || 'HV'}</div>
                                    <div>
                                        <p className="font-body font-semibold text-sm">{s.Ten}</p>
                                        <p className="font-body text-[11px] text-on-surface-variant">ID: {s.HocvienID}</p>
                                    </div>
                                </div>
                                <div className="flex items-center gap-2">
                                    <span className={`font-label text-xs font-bold ${s.IsPresent ? 'text-secondary' : 'text-error'}`}>{s.IsPresent ? 'Có mặt' : 'Vắng'}</span>
                                    <input type="checkbox" checked={s.IsPresent} onChange={() => toggleStudent(index)} className="w-5 h-5 accent-secondary cursor-pointer" />
                                </div>
                            </label>
                        ))}
                    </div>}
                </div>

            </div>
        </div>
    );
};

export default ScheduleDetailUI;
