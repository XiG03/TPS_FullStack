import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const TeacherManagementUI = ({
    teachers,
    isLoading,
    error,
    topicOptions,
    onAddTeacher,
    onEditTeacher,
    onDeleteTeacher
}) => {
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    const normalizedSearch = searchTerm.trim().toLowerCase();
    const filteredTeachers = teachers.filter((teacher) => {
        if (!normalizedSearch) return true;

        return (
            teacher.hoten?.toLowerCase().includes(normalizedSearch) ||
            teacher.email?.toLowerCase().includes(normalizedSearch) ||
            teacher.dienthoai?.toLowerCase().includes(normalizedSearch) ||
            teacher.diachi?.toLowerCase().includes(normalizedSearch)
        );
    });

    const getInitials = (name) => {
        if (!name) return 'GV';
        const parts = name.trim().split(/\s+/);
        if (parts.length >= 2) return `${parts[0][0]}${parts[parts.length - 1][0]}`.toUpperCase();
        return name.substring(0, 2).toUpperCase();
    };

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)]">
            <header className="flex flex-col gap-6 px-6 pt-8 pb-8 lg:flex-row lg:items-end lg:justify-between lg:px-12 lg:pt-10">
                <div className="flex flex-col gap-2">
                    <h1 className="font-headline text-3xl lg:text-4xl font-extrabold text-on-surface tracking-tight">
                        Quản lý Giảng viên
                    </h1>
                    <p className="font-body text-on-surface-variant text-base">
                        Danh sách giảng viên và chuyên đề phụ trách.
                    </p>
                </div>
                <button
                    onClick={onAddTeacher}
                    className="flex items-center justify-center gap-2 bg-gradient-to-br from-primary to-primary-container text-on-primary px-6 py-3.5 rounded-lg font-body font-semibold shadow-[0px_12px_32px_rgba(25,28,30,0.06)] hover:shadow-md transition-all hover:-translate-y-0.5"
                >
                    <span className="material-symbols-outlined text-xl">person_add</span>
                    Thêm giảng viên
                </button>
            </header>

            <section className="px-6 pb-8 lg:px-12">
                <div className="bg-surface-container-low rounded-xl p-6 flex flex-col gap-2 max-w-md shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="search">
                        Tìm kiếm
                    </label>
                    <div className="relative">
                        <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-outline">
                            search
                        </span>
                        <input
                            id="search"
                            type="text"
                            placeholder="Nhập tên, email, số điện thoại hoặc mã..."
                            value={searchTerm}
                            onChange={(event) => setSearchTerm(event.target.value)}
                            className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md pl-10 pr-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                        />
                    </div>
                </div>
            </section>

            <section className="px-6 pb-16 flex-1 flex flex-col lg:px-12">
                <div className="hidden md:grid grid-cols-[2fr_1.5fr_1.2fr_140px] gap-4 px-6 pb-4 font-label text-xs font-bold text-on-surface-variant uppercase tracking-wider">
                    <div>Giảng viên / Email</div>
                    <div>Liên hệ</div>
                    <div>Địa chỉ</div>
                    <div className="text-center">Thao tác</div>
                </div>

                <div className="flex flex-col gap-3">
                    {isLoading ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Đang tải dữ liệu...</div>
                    ) : error ? (
                        <div className="text-center py-10 font-body text-error">{error}</div>
                    ) : filteredTeachers.length === 0 ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">
                            Không tìm thấy giảng viên nào.
                        </div>
                    ) : (
                        filteredTeachers.map((teacher) => (
                            <div
                                key={teacher.maID}
                                className="bg-surface-container-lowest rounded-xl p-5 grid grid-cols-1 gap-4 shadow-[0_4px_12px_rgba(25,28,30,0.02)] transition-transform hover:-translate-y-0.5 hover:shadow-[0_8px_24px_rgba(25,28,30,0.04)] md:grid-cols-[2fr_1.5fr_1.2fr_140px] md:items-center"
                            >
                                <div className="flex items-center gap-4 min-w-0">
                                    <div className="w-12 h-12 rounded-full bg-surface-variant flex items-center justify-center text-on-surface-variant font-headline font-bold text-lg shadow-sm flex-shrink-0">
                                        {getInitials(teacher.hoten)}
                                    </div>
                                    <div className="flex flex-col overflow-hidden">
                                        <span className="font-headline font-bold text-on-surface text-lg truncate">
                                            {teacher.hoten || 'Chưa có tên'}
                                        </span>
                                        <span className="font-body text-sm text-on-surface-variant truncate">
                                            {teacher.email || 'Chưa có email'}
                                        </span>
                                    </div>
                                </div>

                                <div className="font-body text-sm text-on-surface-variant space-y-1">
                                    <div className="flex items-center gap-2">
                                        <span className="material-symbols-outlined text-[18px] text-outline">call</span>
                                        <span>{teacher.dienthoai || 'Chưa có SĐT'}</span>
                                    </div>
                                    <div className="flex items-center gap-2">
                                        <span className="material-symbols-outlined text-[18px] text-outline">person</span>
                                        <span>{teacher.gioitinh || 'Chưa rõ giới tính'}</span>
                                    </div>
                                </div>

                                <div className="font-body text-sm text-on-surface-variant line-clamp-2" title={teacher.diachi}>
                                    {teacher.diachi || 'Chưa có địa chỉ'}
                                </div>

                                <div className="flex items-center justify-end gap-1">
                                    <button
                                        onClick={() => navigate(`/teachers/${teacher.maID}`)}
                                        className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors"
                                        title="Xem chi tiết"
                                    >
                                        <span className="material-symbols-outlined text-xl">visibility</span>
                                    </button>
                                    <button
                                        onClick={() => onEditTeacher(teacher)}
                                        className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors"
                                        title="Chỉnh sửa"
                                    >
                                        <span className="material-symbols-outlined text-xl">edit</span>
                                    </button>
                                    <button
                                        onClick={() => onDeleteTeacher(teacher.maID)}
                                        className="text-on-surface-variant hover:text-error hover:bg-error-container p-2 rounded-md transition-colors"
                                        title="Xóa"
                                    >
                                        <span className="material-symbols-outlined text-xl">delete</span>
                                    </button>
                                </div>
                            </div>
                        ))
                    )}
                </div>

                {topicOptions.length === 0 && !isLoading && (
                    <p className="mt-6 font-body text-xs text-outline">
                        Chưa tải được danh sách chuyên đề cho form phân công.
                    </p>
                )}
            </section>
        </div>
    );
};

export default TeacherManagementUI;
