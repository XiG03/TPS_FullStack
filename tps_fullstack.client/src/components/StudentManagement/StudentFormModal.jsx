import React, { useState, useEffect } from 'react';

const StudentFormModal = ({ isOpen, onClose, onSubmit, isLoading, initialData }) => {
    if (!isOpen) return null;

    const [formData, setFormData] = useState({
        Hoten: '',
        Email: '',
        Dienthoai: '',
        Gioitinh: '',
        Ngaysinh: '',
        Diachi: ''
    });

    useEffect(() => {
        if (isOpen) {
            if (initialData) {
                setFormData({
                    Hoten: initialData.hoten || '',
                    Email: initialData.email || '',
                    Dienthoai: initialData.dienthoai || '',
                    Gioitinh: initialData.gioitinh || '',
                    Ngaysinh: initialData.ngaysinh ? initialData.ngaysinh.substring(0, 10) : '',
                    Diachi: initialData.diachi || ''
                });
            } else {
                setFormData({
                    Hoten: '',
                    Email: '',
                    Dienthoai: '',
                    Gioitinh: '',
                    Ngaysinh: '',
                    Diachi: ''
                });
            }
        }
    }, [isOpen, initialData]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        const payload = {
            ...formData,
            Ngaysinh: formData.Ngaysinh === '' ? null : formData.Ngaysinh
        };
        onSubmit(payload);
    };

    return (
        <div className="fixed inset-0 z-[100] flex items-center justify-end bg-on-surface/20 backdrop-blur-sm transition-opacity">
            <div className="bg-surface w-full max-w-md h-full shadow-2xl flex flex-col animate-slide-in-right">
                <div className="px-6 py-4 border-b border-surface-container-highest flex items-center justify-between bg-surface-container-lowest">
                    <h2 className="font-headline text-xl font-bold text-on-surface">
                        {initialData ? 'Chỉnh sửa Học viên' : 'Thêm Học viên mới'}
                    </h2>
                    <button onClick={onClose} className="p-2 text-on-surface-variant hover:text-error hover:bg-error-container rounded-full transition-colors">
                        <span className="material-symbols-outlined text-xl">close</span>
                    </button>
                </div>

                <form id="studentForm" onSubmit={handleSubmit} className="flex-1 overflow-y-auto p-6 space-y-6">
                    <div className="space-y-2">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Họ và tên *</label>
                        <input required type="text" name="Hoten" value={formData.Hoten} onChange={handleChange}
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            placeholder="VD: Nguyễn Văn A" />
                    </div>

                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Email *</label>
                            <input required type="email" name="Email" value={formData.Email} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                placeholder="email@domain.com" />
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Điện thoại *</label>
                            <input required type="tel" name="Dienthoai" value={formData.Dienthoai} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                placeholder="0987..." />
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Giới tính</label>
                            <select name="Gioitinh" value={formData.Gioitinh} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all cursor-pointer appearance-none">
                                <option value="">Chọn...</option>
                                <option value="Nam">Nam</option>
                                <option value="Nữ">Nữ</option>
                            </select>
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày sinh</label>
                            <input type="date" name="Ngaysinh" value={formData.Ngaysinh} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                        </div>
                    </div>

                    <div className="space-y-2">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Địa chỉ</label>
                        <textarea name="Diachi" value={formData.Diachi} onChange={handleChange} rows="3"
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all resize-none"
                            placeholder="Nhập địa chỉ..." />
                    </div>
                </form>

                <div className="p-6 border-t border-surface-container-highest bg-surface-container-lowest flex items-center justify-end gap-3">
                    <button type="button" onClick={onClose} disabled={isLoading}
                        className="px-5 py-2.5 rounded-lg font-label font-medium text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors">
                        Hủy
                    </button>
                    <button type="submit" form="studentForm" disabled={isLoading}
                        className={`px-6 py-2.5 rounded-lg font-label font-semibold text-sm text-on-primary bg-primary shadow-[0_4px_12px_rgba(0,50,138,0.2)] hover:shadow-md transition-all ${isLoading ? 'opacity-70 cursor-not-allowed' : 'hover:-translate-y-0.5'}`}>
                        {isLoading ? 'Đang lưu...' : 'Lưu thông tin'}
                    </button>
                </div>
            </div>
            <style jsx="true">{`
                @keyframes slideInRight {
                    from { transform: translateX(100%); }
                    to { transform: translateX(0); }
                }
                .animate-slide-in-right {
                    animation: slideInRight 0.3s cubic-bezier(0.16, 1, 0.3, 1) forwards;
                }
            `}</style>
        </div>
    );
};

export default StudentFormModal;
