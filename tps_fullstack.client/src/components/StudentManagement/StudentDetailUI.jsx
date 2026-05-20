import React from 'react';
import { useNavigate } from 'react-router-dom';

const StudentDetailUI = ({ detail, isLoading, error }) => {
    const navigate = useNavigate();

    if (isLoading) {
        return <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-[calc(100vh-60px)]">Đang tải thông tin...</div>;
    }

    if (error || !detail) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center min-h-[calc(100vh-60px)] gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin học viên'}</p>
                <button onClick={() => navigate('/students')} className="px-4 py-2 bg-surface-container-highest rounded-lg font-label text-sm hover:bg-surface-dim transition-colors">Quay lại danh sách</button>
            </div>
        );
    }

    const { studentInfo, studentCertificate } = detail;
    const certs = studentCertificate || [];

    const getInitials = (name) => {
        if (!name) return '??';
        const parts = name.trim().split(' ');
        if (parts.length >= 2) return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
        return name.substring(0, 2).toUpperCase();
    };

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-8 lg:p-12">
            <div className="mb-8 flex items-center gap-4">
                <button onClick={() => navigate('/students')} className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface">
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <h1 className="font-headline text-3xl font-extrabold text-on-surface tracking-tight">Hồ sơ Học viên</h1>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-[1fr_2fr] gap-8 max-w-6xl mx-auto w-full">
                {/* Cột trái: Thông tin cá nhân */}
                <div className="flex flex-col gap-8">
                    <div className="bg-surface-container-lowest rounded-3xl p-8 shadow-[0_8px_32px_rgba(25,28,30,0.04)] border border-surface-container flex flex-col items-center text-center relative overflow-hidden group">
                        <div className="absolute top-0 left-0 right-0 h-32 bg-gradient-to-br from-primary/20 to-tertiary/20 group-hover:scale-105 transition-transform duration-500"></div>
                        <div className="w-32 h-32 rounded-full bg-surface shadow-md border-4 border-surface flex items-center justify-center text-4xl font-headline font-bold text-primary relative z-10 mb-4 mt-12">
                            {getInitials(studentInfo.Hoten)}
                        </div>
                        <h2 className="font-headline text-2xl font-bold text-on-surface mb-1 relative z-10">{studentInfo.Hoten}</h2>
                        <span className="font-label text-xs font-bold text-outline bg-surface-container-highest px-3 py-1 rounded-full relative z-10 mb-6">{studentInfo.MaID}</span>

                        <div className="w-full flex flex-col gap-4 text-left relative z-10">
                            <div className="flex items-center gap-3 bg-surface-container-low p-3 rounded-xl">
                                <span className="material-symbols-outlined text-secondary text-xl">mail</span>
                                <div className="flex flex-col">
                                    <span className="font-label text-xs text-on-surface-variant font-bold">Email</span>
                                    <span className="font-body text-sm text-on-surface break-all">{studentInfo.Email}</span>
                                </div>
                            </div>
                            <div className="flex items-center gap-3 bg-surface-container-low p-3 rounded-xl">
                                <span className="material-symbols-outlined text-tertiary text-xl">call</span>
                                <div className="flex flex-col">
                                    <span className="font-label text-xs text-on-surface-variant font-bold">Điện thoại</span>
                                    <span className="font-body text-sm text-on-surface">{studentInfo.Dienthoai}</span>
                                </div>
                            </div>
                            <div className="grid grid-cols-2 gap-4">
                                <div className="flex items-center gap-3 bg-surface-container-low p-3 rounded-xl">
                                    <span className="material-symbols-outlined text-primary text-xl">cake</span>
                                    <div className="flex flex-col">
                                        <span className="font-label text-xs text-on-surface-variant font-bold">Ngày sinh</span>
                                        <span className="font-body text-sm text-on-surface">{studentInfo.Ngaysinh ? new Date(studentInfo.Ngaysinh).toLocaleDateString('vi-VN') : '---'}</span>
                                    </div>
                                </div>
                                <div className="flex items-center gap-3 bg-surface-container-low p-3 rounded-xl">
                                    <span className="material-symbols-outlined text-primary text-xl">{studentInfo.Gioitinh === 'Nữ' ? 'female' : 'male'}</span>
                                    <div className="flex flex-col">
                                        <span className="font-label text-xs text-on-surface-variant font-bold">Giới tính</span>
                                        <span className="font-body text-sm text-on-surface">{studentInfo.Gioitinh || '---'}</span>
                                    </div>
                                </div>
                            </div>
                            <div className="flex items-center gap-3 bg-surface-container-low p-3 rounded-xl">
                                <span className="material-symbols-outlined text-error text-xl">location_on</span>
                                <div className="flex flex-col">
                                    <span className="font-label text-xs text-on-surface-variant font-bold">Địa chỉ</span>
                                    <span className="font-body text-sm text-on-surface">{studentInfo.Diachi || '---'}</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                {/* Cột phải: Chứng chỉ */}
                <div className="flex flex-col gap-6">
                    <div className="bg-surface-container-lowest rounded-3xl p-8 shadow-sm border border-surface-container h-full flex flex-col">
                        <div className="flex items-center justify-between mb-6 pb-4 border-b border-surface-container-highest">
                            <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                                <span className="material-symbols-outlined text-primary">workspace_premium</span>
                                Chứng chỉ ({certs.length})
                            </h3>
                        </div>

                        {certs.length === 0 ? (
                            <div className="flex-1 flex flex-col items-center justify-center text-center p-8 bg-surface-container-low rounded-2xl border border-dashed border-outline-variant">
                                <span className="material-symbols-outlined text-6xl text-outline mb-4">school</span>
                                <p className="font-body text-on-surface-variant">Học viên này chưa có chứng chỉ nào.</p>
                            </div>
                        ) : (
                            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                                {certs.map((cert) => (
                                    <div key={cert.MaID} className="bg-surface-container-low p-5 rounded-2xl border border-surface-container relative overflow-hidden group hover:border-primary/30 transition-colors">
                                        <div className="absolute top-0 right-0 p-4 opacity-10 group-hover:opacity-20 transition-opacity">
                                            <span className="material-symbols-outlined text-6xl text-primary">military_tech</span>
                                        </div>
                                        <div className="relative z-10 flex flex-col h-full">
                                            <span className="font-label text-[10px] uppercase font-bold text-primary bg-primary/10 w-max px-2 py-0.5 rounded-full mb-2">
                                                {cert.MaID}
                                            </span>
                                            <h4 className="font-headline font-bold text-base text-on-surface mb-4">{cert.Tenchungchi}</h4>
                                            
                                            <div className="mt-auto pt-4 border-t border-outline-variant/30 flex flex-col gap-1">
                                                <div className="flex justify-between items-center text-xs">
                                                    <span className="font-label font-bold text-on-surface-variant">Ngày cấp:</span>
                                                    <span className="font-body text-on-surface">{new Date(cert.Ngaycap).toLocaleDateString('vi-VN')}</span>
                                                </div>
                                                <div className="flex justify-between items-center text-xs">
                                                    <span className="font-label font-bold text-on-surface-variant">Ngày hết hạn:</span>
                                                    <span className="font-body text-on-surface">{new Date(cert.Ngayhethan).toLocaleDateString('vi-VN')}</span>
                                                </div>
                                            </div>
                                        </div>
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

export default StudentDetailUI;
