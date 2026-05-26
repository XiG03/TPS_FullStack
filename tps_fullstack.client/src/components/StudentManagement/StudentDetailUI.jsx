import { useNavigate } from 'react-router-dom';

const getInitials = (name) => {
    if (!name) return 'HV';
    const parts = name.trim().split(/\s+/).filter(Boolean);
    if (parts.length >= 2) return `${parts[0][0]}${parts[parts.length - 1][0]}`.toUpperCase();
    return name.substring(0, 2).toUpperCase();
};

const formatDate = (value) => {
    if (!value) return 'Chưa cập nhật';
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return 'Chưa cập nhật';
    return date.toLocaleDateString('vi-VN');
};

const StudentDetailUI = ({ detail, isLoading, error }) => {
    const navigate = useNavigate();

    if (isLoading) {
        return (
            <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-[calc(100vh-60px)]">
                Đang tải thông tin...
            </div>
        );
    }

    if (error || !detail) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center min-h-[calc(100vh-60px)] gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin học viên'}</p>
                <button onClick={() => navigate('/students')} className="px-4 py-2 bg-surface-container-highest rounded-lg font-label text-sm hover:bg-surface-dim transition-colors">
                    Quay lại danh sách
                </button>
            </div>
        );
    }

    const certificates = detail.certificates || [];

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-6 lg:p-12">
            <div className="mb-8 flex items-center gap-4">
                <button
                    onClick={() => navigate('/students')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                    title="Quay lại"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <div className="min-w-0">
                    <h1 className="font-headline text-2xl lg:text-3xl font-extrabold text-on-surface tracking-tight">
                        Hồ sơ học viên
                    </h1>
                    <p className="mt-1 truncate font-body text-sm text-on-surface-variant">
                        {detail.hoten || 'Chưa cập nhật tên'}
                    </p>
                </div>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-[360px_minmax(0,1fr)] gap-8 max-w-6xl mx-auto w-full">
                <div className="flex flex-col gap-6">
                    <div className="bg-surface-container-lowest rounded-lg p-6 shadow-sm border border-surface-container">
                        <div className="flex flex-col items-center text-center">
                            <div className="w-28 h-28 rounded-full bg-primary-container text-on-primary-container flex items-center justify-center text-3xl font-headline font-bold shadow-sm mb-4">
                                {getInitials(detail.hoten)}
                            </div>
                            <h2 className="font-headline text-2xl font-bold text-on-surface">{detail.hoten || 'Chưa cập nhật tên'}</h2>
                            <p className="mt-1 font-body text-sm text-on-surface-variant">{detail.email || 'Chưa cập nhật email'}</p>
                        </div>
                    </div>

                    <div className="bg-surface-container-lowest rounded-lg p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-4">Thông tin cá nhân</h3>
                        <div className="space-y-3">
                            <div className="flex items-start gap-3 rounded-lg bg-surface-container-low p-3">
                                <span className="material-symbols-outlined text-secondary text-xl">call</span>
                                <div className="min-w-0">
                                    <p className="font-label text-xs font-bold text-on-surface-variant">Điện thoại</p>
                                    <p className="font-body text-sm text-on-surface">{detail.dienthoai || 'Chưa cập nhật'}</p>
                                </div>
                            </div>
                            <div className="grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-1">
                                <div className="flex items-start gap-3 rounded-lg bg-surface-container-low p-3">
                                    <span className="material-symbols-outlined text-primary text-xl">cake</span>
                                    <div className="min-w-0">
                                        <p className="font-label text-xs font-bold text-on-surface-variant">Ngày sinh</p>
                                        <p className="font-body text-sm text-on-surface">{formatDate(detail.ngaysinh)}</p>
                                    </div>
                                </div>
                                <div className="flex items-start gap-3 rounded-lg bg-surface-container-low p-3">
                                    <span className="material-symbols-outlined text-primary text-xl">{detail.gioitinh === 'Nữ' ? 'female' : 'person'}</span>
                                    <div className="min-w-0">
                                        <p className="font-label text-xs font-bold text-on-surface-variant">Giới tính</p>
                                        <p className="font-body text-sm text-on-surface">{detail.gioitinh || 'Chưa cập nhật'}</p>
                                    </div>
                                </div>
                            </div>
                            <div className="flex items-start gap-3 rounded-lg bg-surface-container-low p-3">
                                <span className="material-symbols-outlined text-error text-xl">location_on</span>
                                <div className="min-w-0">
                                    <p className="font-label text-xs font-bold text-on-surface-variant">Địa chỉ</p>
                                    <p className="font-body text-sm text-on-surface">{detail.diachi || 'Chưa cập nhật'}</p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="bg-surface-container-lowest rounded-lg p-6 lg:p-8 shadow-sm border border-surface-container">
                    <div className="flex items-center justify-between gap-4 mb-6 pb-4 border-b border-surface-container-highest">
                        <h3 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">workspace_premium</span>
                            Chứng chỉ
                        </h3>
                        <span className="rounded-full bg-surface-container-high px-3 py-1 font-label text-xs font-bold text-on-surface-variant">
                            {certificates.length}
                        </span>
                    </div>

                    {certificates.length === 0 ? (
                        <div className="flex min-h-[280px] flex-col items-center justify-center rounded-lg border border-dashed border-outline-variant bg-surface-container-low p-8 text-center">
                            <span className="material-symbols-outlined mb-4 text-6xl text-outline">school</span>
                            <p className="font-headline font-bold text-on-surface">Chưa có chứng chỉ</p>
                            <p className="mt-1 font-body text-sm text-on-surface-variant">Khi học viên có chứng chỉ, thông tin sẽ hiển thị tại đây.</p>
                        </div>
                    ) : (
                        <div className="grid grid-cols-1 gap-4 xl:grid-cols-2">
                            {certificates.map((certificate, index) => (
                                <div key={certificate.maID || index} className="relative overflow-hidden rounded-lg border border-surface-container-highest bg-surface-container-low p-5 transition-colors hover:border-primary/30">
                                    <div className="absolute right-3 top-3 opacity-10">
                                        <span className="material-symbols-outlined text-6xl text-primary">military_tech</span>
                                    </div>
                                    <div className="relative z-10">
                                        <h4 className="font-headline text-base font-bold text-on-surface">{certificate.tenChungchi || 'Chứng chỉ chưa đặt tên'}</h4>
                                        <p className="mt-2 line-clamp-3 font-body text-sm text-on-surface-variant">
                                            {certificate.mota || 'Chưa có mô tả.'}
                                        </p>
                                        <div className="mt-4 space-y-2 border-t border-outline-variant/30 pt-4 font-body text-xs">
                                            <div className="flex justify-between gap-3">
                                                <span className="font-label font-bold text-on-surface-variant">Đơn vị cấp</span>
                                                <span className="text-right text-on-surface">{certificate.donvicap || 'Chưa cập nhật'}</span>
                                            </div>
                                            <div className="flex justify-between gap-3">
                                                <span className="font-label font-bold text-on-surface-variant">Ngày cấp</span>
                                                <span className="text-right text-on-surface">{formatDate(certificate.ngaycap)}</span>
                                            </div>
                                            <div className="flex justify-between gap-3">
                                                <span className="font-label font-bold text-on-surface-variant">Ngày hết hạn</span>
                                                <span className="text-right text-on-surface">{formatDate(certificate.ngayhethan)}</span>
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
    );
};

export default StudentDetailUI;
