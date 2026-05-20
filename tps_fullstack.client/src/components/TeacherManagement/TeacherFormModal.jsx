import React, { useState, useEffect } from 'react';

const TeacherFormModal = ({ isOpen, onClose, onSubmit, isLoading, initialData }) => {
    if (!isOpen) return null;

    const mockTopics = [
        { id: 'TP01', name: 'Data Architecture' },
        { id: 'TP02', name: 'UI/UX Design' },
        { id: 'TP03', name: 'Leadership & Ethics' },
        { id: 'TP04', name: 'Systems Engineering' }
    ];

    const [formData, setFormData] = useState({
        Hoten: '',
        Email: '',
        Dienthoai: '',
        Gioitinh: '',
        Ngaysinh: '',
        Diachi: '',
        Topics: []
    });

    useEffect(() => {
        if (isOpen) {
            if (initialData) {
                setFormData({
                    Hoten: initialData.Hoten || '',
                    Email: initialData.Email || '',
                    Dienthoai: initialData.Dienthoai || '',
                    Gioitinh: initialData.Gioitinh || '',
                    Ngaysinh: initialData.Ngaysinh || '',
                    Diachi: initialData.Diachi || '',
                    Topics: initialData.Topics || mockTopics.filter(t => t.name === initialData.Specialization).map(t => t.id)
                });
            } else {
                setFormData({
                    Hoten: '',
                    Email: '',
                    Dienthoai: '',
                    Gioitinh: '',
                    Ngaysinh: '',
                    Diachi: '',
                    Topics: []
                });
            }
        }
    }, [isOpen, initialData]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleTopicToggle = (topicId) => {
        setFormData(prev => {
            const isSelected = prev.Topics.includes(topicId);
            if (isSelected) {
                return { ...prev, Topics: prev.Topics.filter(id => id !== topicId) };
            } else {
                return { ...prev, Topics: [...prev.Topics, topicId] };
            }
        });
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        // Format payload theo cấu trúc TeacherCreateDto
        const payload = {
            ...formData,
            topics: mockTopics.map(t => ({
                MaID: t.id,
                Picked: formData.Topics.includes(t.id)
            }))
        };
        onSubmit(payload);
    };

    return (
        <div className="fixed inset-0 z-[100] flex items-center justify-end bg-on-surface/20 backdrop-blur-sm transition-opacity">
            {/* Drawer */}
            <div className="bg-surface w-full max-w-md h-full shadow-2xl flex flex-col animate-slide-in-right">
                {/* Header */}
                <div className="px-6 py-4 border-b border-surface-container-highest flex items-center justify-between bg-surface-container-lowest">
                    <h2 className="font-headline text-xl font-bold text-on-surface">
                        {initialData ? 'Chỉnh sửa Giáo viên' : 'Thêm Giáo viên mới'}
                    </h2>
                    <button onClick={onClose} className="p-2 text-on-surface-variant hover:text-error hover:bg-error-container rounded-full transition-colors">
                        <span className="material-symbols-outlined text-xl">close</span>
                    </button>
                </div>

                {/* Form Content */}
                <form onSubmit={handleSubmit} id="teacherForm" className="flex-1 overflow-y-auto p-6 space-y-6">
                    {/* Họ tên */}
                    <div className="space-y-2">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Họ và tên *</label>
                        <input required type="text" name="Hoten" value={formData.Hoten} onChange={handleChange}
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            placeholder="VD: Nguyễn Văn A" />
                    </div>

                    <div className="grid grid-cols-2 gap-4">
                        {/* Email */}
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Email *</label>
                            <input required type="email" name="Email" value={formData.Email} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                placeholder="email@domain.com" />
                        </div>
                        {/* Điện thoại */}
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Điện thoại *</label>
                            <input required type="tel" name="Dienthoai" value={formData.Dienthoai} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                placeholder="0987..." />
                        </div>
                    </div>

                    <div className="grid grid-cols-2 gap-4">
                        {/* Giới tính */}
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Giới tính</label>
                            <select name="Gioitinh" value={formData.Gioitinh} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all cursor-pointer appearance-none">
                                <option value="">Chọn...</option>
                                <option value="Nam">Nam</option>
                                <option value="Nữ">Nữ</option>
                            </select>
                        </div>
                        {/* Ngày sinh */}
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày sinh</label>
                            <input type="date" name="Ngaysinh" value={formData.Ngaysinh} onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                        </div>
                    </div>

                    {/* Địa chỉ */}
                    <div className="space-y-2">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Địa chỉ</label>
                        <textarea name="Diachi" value={formData.Diachi} onChange={handleChange} rows="2"
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all resize-none"
                            placeholder="Nhập địa chỉ..." />
                    </div>

                    {/* Topics (Chuyên đề) */}
                    <div className="space-y-3">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Chuyên đề phụ trách</label>
                        <div className="flex flex-wrap gap-2">
                            {mockTopics.map(topic => (
                                <button type="button" key={topic.id} onClick={() => handleTopicToggle(topic.id)}
                                    className={`px-3 py-1.5 rounded-full text-xs font-semibold font-label transition-colors border ${
                                        formData.Topics.includes(topic.id) 
                                        ? 'bg-primary text-on-primary border-primary shadow-sm' 
                                        : 'bg-surface-container-lowest text-on-surface-variant border-outline-variant hover:bg-surface-container-highest'
                                    }`}
                                >
                                    {topic.name}
                                </button>
                            ))}
                        </div>
                    </div>
                </form>

                {/* Footer Actions */}
                <div className="p-6 border-t border-surface-container-highest bg-surface-container-lowest flex items-center justify-end gap-3">
                    <button type="button" onClick={onClose} disabled={isLoading}
                        className="px-5 py-2.5 rounded-lg font-label font-medium text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors">
                        Hủy
                    </button>
                    <button type="submit" form="teacherForm" disabled={isLoading}
                        className={`px-6 py-2.5 rounded-lg font-label font-semibold text-sm text-on-primary bg-primary shadow-[0_4px_12px_rgba(0,50,138,0.2)] hover:shadow-md transition-all ${isLoading ? 'opacity-70 cursor-not-allowed' : 'hover:-translate-y-0.5'}`}>
                        {isLoading ? 'Đang lưu...' : 'Lưu thông tin'}
                    </button>
                </div>
            </div>
            {/* Custom Animation Style */}
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

export default TeacherFormModal;
