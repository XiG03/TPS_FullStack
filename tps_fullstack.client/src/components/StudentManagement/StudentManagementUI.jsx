import { useState } from 'react';
import { useNavigate } from 'react-router-dom';

const getInitials = (name) => {
    if (!name) return 'HV';
    const parts = name.trim().split(/\s+/).filter(Boolean);
    if (parts.length >= 2) return `${parts[0][0]}${parts[parts.length - 1][0]}`.toUpperCase();
    return name.substring(0, 2).toUpperCase();
};

const StudentManagementUI = ({ students, isLoading, error, onAddStudent, onEditStudent, onDeleteStudent }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const navigate = useNavigate();

    const normalizedSearch = searchTerm.trim().toLowerCase();
    const filteredStudents = students.filter((student) => {
        if (!normalizedSearch) return true;

        return (
            student.hoten?.toLowerCase().includes(normalizedSearch) ||
            student.email?.toLowerCase().includes(normalizedSearch) ||
            student.dienthoai?.toLowerCase().includes(normalizedSearch) ||
            student.gioitinh?.toLowerCase().includes(normalizedSearch) ||
            student.diachi?.toLowerCase().includes(normalizedSearch)
        );
    });

    const maleCount = students.filter((student) => student.gioitinh === 'Nam').length;
    const femaleCount = students.filter((student) => student.gioitinh === 'Nữ').length;

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)]">
            <header className="flex flex-col gap-6 px-6 pt-8 pb-8 lg:flex-row lg:items-end lg:justify-between lg:px-12 lg:pt-10">
                <div className="flex flex-col gap-2">
                    <h1 className="font-headline text-3xl lg:text-4xl font-extrabold text-on-surface tracking-tight">
                        Quản lý học viên
                    </h1>
                    <p className="font-body text-on-surface-variant text-base">
                        Theo dõi hồ sơ, liên hệ và chứng chỉ của học viên.
                    </p>
                </div>
                <button
                    onClick={onAddStudent}
                    className="flex items-center justify-center gap-2 bg-primary text-on-primary px-6 py-3 rounded-lg font-label font-bold shadow-sm hover:bg-primary/90 transition-all"
                >
                    <span className="material-symbols-outlined text-xl">person_add</span>
                    Thêm học viên
                </button>
            </header>

            <section className="px-6 pb-6 lg:px-12">
                <div className="grid grid-cols-1 gap-3 md:grid-cols-3">
                    <div className="flex items-center gap-4 rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-4 shadow-sm">
                        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-blue-50 text-primary">
                            <span className="material-symbols-outlined">groups</span>
                        </div>
                        <div>
                            <p className="font-label text-[10px] font-extrabold uppercase text-outline">Tổng học viên</p>
                            <span className="font-headline text-xl font-extrabold text-on-surface">{students.length}</span>
                        </div>
                    </div>
                    <div className="flex items-center gap-4 rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-4 shadow-sm">
                        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-surface-container-high text-on-surface-variant">
                            <span className="material-symbols-outlined">male</span>
                        </div>
                        <div>
                            <p className="font-label text-[10px] font-extrabold uppercase text-outline">Nam</p>
                            <span className="font-headline text-xl font-extrabold text-on-surface">{maleCount}</span>
                        </div>
                    </div>
                    <div className="flex items-center gap-4 rounded-lg border border-outline-variant/30 bg-surface-container-lowest p-4 shadow-sm">
                        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-surface-container-high text-on-surface-variant">
                            <span className="material-symbols-outlined">female</span>
                        </div>
                        <div>
                            <p className="font-label text-[10px] font-extrabold uppercase text-outline">Nữ</p>
                            <span className="font-headline text-xl font-extrabold text-on-surface">{femaleCount}</span>
                        </div>
                    </div>
                </div>
            </section>

            <section className="px-6 pb-8 lg:px-12">
                <div className="bg-surface-container-low rounded-lg p-5 shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="student-search">
                        Tìm kiếm
                    </label>
                    <div className="relative mt-2 max-w-xl">
                        <span className="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-outline">search</span>
                        <input
                            id="student-search"
                            type="text"
                            placeholder="Nhập tên, email, điện thoại hoặc địa chỉ..."
                            value={searchTerm}
                            onChange={(event) => setSearchTerm(event.target.value)}
                            className="w-full rounded-md border-none bg-surface-container-highest py-3 pl-10 pr-4 font-body text-sm text-on-surface outline-none transition-all placeholder:text-outline focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30"
                        />
                    </div>
                </div>
            </section>

            <section className="flex-1 px-6 pb-16 lg:px-12">
                <div className="hidden grid-cols-[2fr_1.4fr_1fr_1fr_140px] gap-4 px-6 pb-4 font-label text-xs font-bold uppercase tracking-wider text-on-surface-variant md:grid">
                    <div>Học viên</div>
                    <div>Email</div>
                    <div>Điện thoại</div>
                    <div>Giới tính</div>
                    <div className="text-center">Thao tác</div>
                </div>

                <div className="flex flex-col gap-3">
                    {isLoading ? (
                        <div className="rounded-lg bg-surface-container-lowest py-10 text-center font-body text-on-surface-variant">Đang tải dữ liệu...</div>
                    ) : error ? (
                        <div className="rounded-lg bg-error-container py-10 text-center font-body text-error">{error}</div>
                    ) : filteredStudents.length === 0 ? (
                        <div className="rounded-lg bg-surface-container-lowest py-10 text-center font-body text-on-surface-variant">Không tìm thấy học viên nào.</div>
                    ) : (
                        filteredStudents.map((student) => (
                            <div
                                key={student.hocvienID}
                                className="grid grid-cols-1 gap-4 rounded-lg bg-surface-container-lowest p-5 shadow-[0_4px_12px_rgba(25,28,30,0.02)] transition-transform hover:-translate-y-0.5 hover:shadow-[0_8px_24px_rgba(25,28,30,0.04)] md:grid-cols-[2fr_1.4fr_1fr_1fr_140px] md:items-center"
                            >
                                <div className="flex min-w-0 items-center gap-4">
                                    <div className="flex h-10 w-10 flex-shrink-0 items-center justify-center rounded-full bg-surface-variant font-headline text-sm font-bold text-on-surface-variant shadow-sm">
                                        {getInitials(student.hoten)}
                                    </div>
                                    <div className="min-w-0">
                                        <p className="truncate font-headline text-base font-bold text-on-surface">{student.hoten || 'Chưa cập nhật tên'}</p>
                                        <p className="mt-0.5 truncate font-body text-xs text-on-surface-variant">{student.diachi || 'Chưa cập nhật địa chỉ'}</p>
                                    </div>
                                </div>
                                <div className="truncate font-body text-sm text-on-surface-variant">{student.email || 'Chưa cập nhật'}</div>
                                <div className="truncate font-body text-sm text-on-surface-variant">{student.dienthoai || 'Chưa cập nhật'}</div>
                                <div>
                                    <span className="inline-flex rounded-full bg-surface-container-highest px-3 py-1 font-label text-xs font-bold text-on-surface-variant">
                                        {student.gioitinh || 'Chưa cập nhật'}
                                    </span>
                                </div>
                                <div className="flex items-center justify-end gap-1">
                                    <button onClick={() => navigate(`/students/${student.hocvienID}`)} className="rounded-md p-2 text-on-surface-variant transition-colors hover:bg-surface-container-highest hover:text-primary" title="Xem chi tiết">
                                        <span className="material-symbols-outlined text-xl">visibility</span>
                                    </button>
                                    <button onClick={() => onEditStudent(student)} className="rounded-md p-2 text-on-surface-variant transition-colors hover:bg-surface-container-highest hover:text-primary" title="Chỉnh sửa">
                                        <span className="material-symbols-outlined text-xl">edit</span>
                                    </button>
                                    <button onClick={() => onDeleteStudent(student.hocvienID)} className="rounded-md p-2 text-on-surface-variant transition-colors hover:bg-error-container hover:text-error" title="Xóa">
                                        <span className="material-symbols-outlined text-xl">delete</span>
                                    </button>
                                </div>
                            </div>
                        ))
                    )}
                </div>
            </section>
        </div>
    );
};

export default StudentManagementUI;
