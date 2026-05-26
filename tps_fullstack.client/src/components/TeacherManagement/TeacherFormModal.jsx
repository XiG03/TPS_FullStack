import { useEffect, useState } from 'react';

const emptyFormData = {
    hoten: '',
    email: '',
    dienthoai: '',
    gioitinh: '',
    ngaysinh: '',
    diachi: '',
    topics: []
};

const toDateInput = (value) => {
    if (!value) return '';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';
    return date.toISOString().slice(0, 10);
};

const mapInitialData = (initialData) => {
    if (!initialData) return emptyFormData;

    return {
        hoten: initialData.hoten || '',
        email: initialData.email || '',
        dienthoai: initialData.dienthoai || '',
        gioitinh: initialData.gioitinh || '',
        ngaysinh: toDateInput(initialData.ngaysinh),
        diachi: initialData.diachi || '',
        topics: (initialData.topics || []).map((topic) => ({
            maID: topic.maID || '',
            chuyendeID: topic.chuyendeID || ''
        }))
    };
};

const TeacherFormModal = ({ isOpen, onClose, onSubmit, isLoading, initialData, topicOptions }) => {
    const [formData, setFormData] = useState(() => mapInitialData(initialData));

    useEffect(() => {
        if (!isOpen) return;
        // eslint-disable-next-line react-hooks/set-state-in-effect
        setFormData(mapInitialData(initialData));
    }, [isOpen, initialData]);

    if (!isOpen) return null;

    const selectedTopicIds = formData.topics.map((topic) => topic.chuyendeID);

    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleTopicToggle = (topicId) => {
        setFormData((prev) => {
            const existing = prev.topics.find((topic) => topic.chuyendeID === topicId);
            if (existing) {
                return {
                    ...prev,
                    topics: prev.topics.filter((topic) => topic.chuyendeID !== topicId)
                };
            }

            return {
                ...prev,
                topics: [...prev.topics, { maID: '', chuyendeID: topicId }]
            };
        });
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        const payload = {
            giangvienID: initialData?.maID || '',
            hoten: formData.hoten.trim(),
            ngaysinh: formData.ngaysinh ? new Date(formData.ngaysinh).toISOString() : null,
            gioitinh: formData.gioitinh || null,
            email: formData.email.trim(),
            diachi: formData.diachi.trim(),
            dienthoai: formData.dienthoai.trim(),
            topics: formData.topics.map((topic) => ({
                maID: topic.maID || '',
                chuyendeID: topic.chuyendeID
            }))
        };

        onSubmit(payload);
    };

    return (
        <div className="fixed inset-0 z-[100] flex items-center justify-end bg-on-surface/20 backdrop-blur-sm transition-opacity">
            <div className="bg-surface w-full max-w-md h-full shadow-2xl flex flex-col animate-slide-in-right">
                <div className="px-6 py-4 border-b border-surface-container-highest flex items-center justify-between bg-surface-container-lowest">
                    <h2 className="font-headline text-xl font-bold text-on-surface">
                        {initialData ? 'Chỉnh sửa giảng viên' : 'Thêm giảng viên mới'}
                    </h2>
                    <button
                        onClick={onClose}
                        className="p-2 text-on-surface-variant hover:text-error hover:bg-error-container rounded-full transition-colors"
                        title="Đóng"
                    >
                        <span className="material-symbols-outlined text-xl">close</span>
                    </button>
                </div>

                <form onSubmit={handleSubmit} id="teacherForm" className="flex-1 overflow-y-auto p-6 space-y-6">
                    <div className="space-y-2">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Họ và tên *</label>
                        <input
                            required
                            type="text"
                            name="hoten"
                            value={formData.hoten}
                            onChange={handleChange}
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            placeholder="VD: Nguyễn Văn A"
                        />
                    </div>

                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Email *</label>
                            <input
                                required
                                type="email"
                                name="email"
                                value={formData.email}
                                onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                placeholder="email@domain.com"
                            />
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Điện thoại *</label>
                            <input
                                required
                                type="tel"
                                name="dienthoai"
                                value={formData.dienthoai}
                                onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                placeholder="0987..."
                            />
                        </div>
                    </div>

                    <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Giới tính</label>
                            <select
                                name="gioitinh"
                                value={formData.gioitinh}
                                onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all cursor-pointer"
                            >
                                <option value="">Chọn...</option>
                                <option value="Nam">Nam</option>
                                <option value="Nữ">Nữ</option>
                            </select>
                        </div>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Ngày sinh</label>
                            <input
                                type="date"
                                name="ngaysinh"
                                value={formData.ngaysinh}
                                onChange={handleChange}
                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                            />
                        </div>
                    </div>

                    <div className="space-y-2">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">Địa chỉ</label>
                        <textarea
                            name="diachi"
                            value={formData.diachi}
                            onChange={handleChange}
                            rows="2"
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all resize-none"
                            placeholder="Nhập địa chỉ..."
                        />
                    </div>

                    <div className="space-y-3">
                        <label className="font-label text-sm font-semibold text-on-surface-variant">
                            Chuyên đề phụ trách
                        </label>
                        {topicOptions.length === 0 ? (
                            <p className="font-body text-sm text-outline italic">
                                Chưa có danh sách chuyên đề để phân công.
                            </p>
                        ) : (
                            <div className="flex flex-wrap gap-2">
                                {topicOptions.map((topic) => (
                                    <button
                                        type="button"
                                        key={topic.chuyendeID}
                                        onClick={() => handleTopicToggle(topic.chuyendeID)}
                                        className={`px-3 py-1.5 rounded-full text-xs font-semibold font-label transition-colors border ${
                                            selectedTopicIds.includes(topic.chuyendeID)
                                                ? 'bg-primary text-on-primary border-primary shadow-sm'
                                                : 'bg-surface-container-lowest text-on-surface-variant border-outline-variant hover:bg-surface-container-highest'
                                        }`}
                                    >
                                        {topic.ten}
                                    </button>
                                ))}
                            </div>
                        )}
                    </div>
                </form>

                <div className="p-6 border-t border-surface-container-highest bg-surface-container-lowest flex items-center justify-end gap-3">
                    <button
                        type="button"
                        onClick={onClose}
                        disabled={isLoading}
                        className="px-5 py-2.5 rounded-lg font-label font-medium text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors"
                    >
                        Hủy
                    </button>
                    <button
                        type="submit"
                        form="teacherForm"
                        disabled={isLoading}
                        className={`px-6 py-2.5 rounded-lg font-label font-semibold text-sm text-on-primary bg-primary shadow-[0_4px_12px_rgba(0,50,138,0.2)] hover:shadow-md transition-all ${isLoading ? 'opacity-70 cursor-not-allowed' : 'hover:-translate-y-0.5'}`}
                    >
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

export default TeacherFormModal;
