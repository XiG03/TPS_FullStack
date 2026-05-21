import React, { useState, useEffect } from 'react';
import { getCourses } from '../../services/courseService';

const ScheduleFormModal = ({ isOpen, onClose, onSubmit, isLoading, initialData }) => {
    const [courses, setCourses] = useState([]);
    const [formData, setFormData] = useState({
        KhoahocID: '',
        Ngaydukien: '',
        Batdaudukien: '',
        Ketthucdukien: ''
    });

    useEffect(() => {
        getCourses().then(res => setCourses(res.data));
    }, []);

    useEffect(() => {
        if (isOpen) {
            if (initialData) {
                setFormData({
                    KhoahocID: initialData.KhoahocID || '',
                    Ngaydukien: initialData.Ngaydukien?.substring(0, 10) || '',
                    Batdaudukien: initialData.Batdaudukien ? new Date(initialData.Batdaudukien).toISOString().substring(11, 16) : '',
                    Ketthucdukien: initialData.Ketthucdukien ? new Date(initialData.Ketthucdukien).toISOString().substring(11, 16) : ''
                });
            } else {
                setFormData({ KhoahocID: '', Ngaydukien: '', Batdaudukien: '', Ketthucdukien: '' });
            }
        }
    }, [isOpen, initialData]);

    if (!isOpen) return null;

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        // Convert back to ISO string for backend payload
        const payload = {
            ...formData,
            Ngaydukien: `${formData.Ngaydukien}T00:00:00Z`,
            Batdaudukien: `${formData.Ngaydukien}T${formData.Batdaudukien}:00Z`,
            Ketthucdukien: `${formData.Ngaydukien}T${formData.Ketthucdukien}:00Z`,
            MaID: initialData?.MaID
        };
        onSubmit(payload);
    };

    return (
        <div className="fixed inset-0 z-[100] flex items-center justify-end bg-on-surface/20 backdrop-blur-sm transition-opacity">
            <div className="bg-surface w-full max-w-md h-full shadow-2xl flex flex-col animate-slide-in-right">
                <div className="px-6 py-4 border-b border-surface-container-highest flex items-center justify-between bg-surface-container-lowest">
                    <h2 className="font-headline text-xl font-bold text-on-surface">
                        {initialData ? 'Chỉnh sửa Buổi học' : 'Thêm Buổi học mới'}
                    </h2>
                    <button onClick={onClose} className="p-2 text-on-surface-variant hover:text-error hover:bg-error-container rounded-full transition-colors">
                        <span className="material-symbols-outlined text-xl">close</span>
                    </button>
                </div>

                <form id="scheduleForm" onSubmit={handleSubmit} className="flex-1 overflow-y-auto p-6 space-y-6">
                    <div className="space-y-2">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Thuộc Khóa học *</label>
                        <select required name="KhoahocID" value={formData.KhoahocID} onChange={handleChange}
                            className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all cursor-pointer">
                            <option value="">-- Chọn khóa học --</option>
                            {courses.map(c => <option key={c.maID} value={c.maID}>{c.ten}</option>)}
                        </select>
                    </div>

                    <div className="space-y-2">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày học dự kiến *</label>
                        <input required type="date" name="Ngaydukien" value={formData.Ngaydukien} onChange={handleChange}
                            className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                    </div>

                    <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Giờ bắt đầu *</label>
                            <input required type="time" name="Batdaudukien" value={formData.Batdaudukien} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Giờ kết thúc *</label>
                            <input required type="time" name="Ketthucdukien" value={formData.Ketthucdukien} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                        </div>
                    </div>
                </form>

                <div className="p-6 border-t border-surface-container-highest bg-surface-container-lowest flex items-center justify-end gap-3">
                    <button type="button" onClick={onClose} disabled={isLoading} className="px-5 py-2.5 rounded-lg font-label font-medium text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors">Hủy</button>
                    <button type="submit" form="scheduleForm" disabled={isLoading} className="px-6 py-2.5 rounded-lg font-label font-semibold text-sm text-on-primary bg-primary shadow-md hover:-translate-y-0.5 transition-all">
                        {isLoading ? 'Đang lưu...' : 'Lưu lịch học'}
                    </button>
                </div>
            </div>
            <style jsx="true">{`
                @keyframes slideInRight {
                    from { transform: translateX(100%); }
                    to { transform: translateX(0); }
                }
                .animate-slide-in-right { animation: slideInRight 0.3s cubic-bezier(0.16, 1, 0.3, 1) forwards; }
            `}</style>
        </div>
    );
};

export default ScheduleFormModal;
