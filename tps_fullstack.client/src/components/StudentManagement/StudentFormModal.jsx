import { useEffect, useState } from 'react';

const emptyForm = {
    hoten: '',
    email: '',
    dienthoai: '',
    gioitinh: '',
    ngaysinh: '',
    diachi: ''
};

const toDateInput = (value) => {
    if (!value) return '';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '';
    return date.toISOString().slice(0, 10);
};

const mapInitialData = (initialData) => {
    if (!initialData) return emptyForm;

    return {
        hoten: initialData.hoten || '',
        email: initialData.email || '',
        dienthoai: initialData.dienthoai || '',
        gioitinh: initialData.gioitinh || '',
        ngaysinh: toDateInput(initialData.ngaysinh),
        diachi: initialData.diachi || ''
    };
};

const StudentFormModal = ({ isOpen, onClose, onSubmit, isLoading, initialData }) => {
    const [formData, setFormData] = useState(() => mapInitialData(initialData));

    useEffect(() => {
        if (!isOpen) return;
        // eslint-disable-next-line react-hooks/set-state-in-effect
        setFormData(mapInitialData(initialData));
    }, [isOpen, initialData]);

    if (!isOpen) return null;

    const handleChange = (event) => {
        const { name, value } = event.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = (event) => {
        event.preventDefault();
        onSubmit({
            ...formData,
            ngaysinh: formData.ngaysinh || null
        });
    };

    return (
        <div className="fixed inset-0 z-[100] flex items-center justify-end bg-on-surface/25 backdrop-blur-sm">
            <div className="flex h-full w-full max-w-lg animate-slide-in-right flex-col bg-surface shadow-2xl">
                <div className="flex items-center justify-between border-b border-surface-container-highest bg-surface-container-lowest px-6 py-4">
                    <div>
                        <h2 className="font-headline text-xl font-bold text-on-surface">
                            {initialData ? 'Chỉnh sửa học viên' : 'Thêm học viên mới'}
                        </h2>
                        <p className="mt-1 font-body text-xs text-on-surface-variant">
                            Cập nhật thông tin liên hệ và hồ sơ cá nhân.
                        </p>
                    </div>
                    <button onClick={onClose} className="rounded-full p-2 text-on-surface-variant transition-colors hover:bg-error-container hover:text-error" title="Đóng">
                        <span className="material-symbols-outlined text-xl">close</span>
                    </button>
                </div>

                <form id="studentForm" onSubmit={handleSubmit} className="flex-1 space-y-6 overflow-y-auto p-6">
                    <section className="space-y-4">
                        <h3 className="font-headline text-base font-bold text-on-surface">Thông tin cơ bản</h3>
                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Họ và tên *</label>
                            <input
                                required
                                type="text"
                                name="hoten"
                                value={formData.hoten}
                                onChange={handleChange}
                                className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all placeholder:text-outline focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30"
                                placeholder="VD: Nguyễn Văn A"
                            />
                        </div>

                        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Giới tính</label>
                                <select
                                    name="gioitinh"
                                    value={formData.gioitinh}
                                    onChange={handleChange}
                                    className="w-full cursor-pointer rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30"
                                >
                                    <option value="">Chọn giới tính</option>
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
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30"
                                />
                            </div>
                        </div>
                    </section>

                    <section className="space-y-4 border-t border-surface-container-highest pt-5">
                        <h3 className="font-headline text-base font-bold text-on-surface">Liên hệ</h3>
                        <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                            <div className="space-y-2">
                                <label className="font-label text-sm font-semibold text-on-surface-variant">Email *</label>
                                <input
                                    required
                                    type="email"
                                    name="email"
                                    value={formData.email}
                                    onChange={handleChange}
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all placeholder:text-outline focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30"
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
                                    className="w-full rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all placeholder:text-outline focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30"
                                    placeholder="0987..."
                                />
                            </div>
                        </div>

                        <div className="space-y-2">
                            <label className="font-label text-sm font-semibold text-on-surface-variant">Địa chỉ</label>
                            <textarea
                                name="diachi"
                                value={formData.diachi}
                                onChange={handleChange}
                                rows="3"
                                className="w-full resize-none rounded-md border-none bg-surface-container-highest px-4 py-3 font-body text-sm text-on-surface outline-none transition-all placeholder:text-outline focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30"
                                placeholder="Nhập địa chỉ..."
                            />
                        </div>
                    </section>
                </form>

                <div className="flex items-center justify-end gap-3 border-t border-surface-container-highest bg-surface-container-lowest p-6">
                    <button type="button" onClick={onClose} disabled={isLoading} className="rounded-lg px-5 py-2.5 font-label text-sm font-medium text-on-surface-variant transition-colors hover:bg-surface-container-highest disabled:opacity-60">
                        Hủy
                    </button>
                    <button type="submit" form="studentForm" disabled={isLoading} className="rounded-lg bg-primary px-6 py-2.5 font-label text-sm font-semibold text-on-primary shadow-md transition-all hover:-translate-y-0.5 disabled:translate-y-0 disabled:opacity-60">
                        {isLoading ? 'Đang lưu...' : 'Lưu thông tin'}
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

export default StudentFormModal;
