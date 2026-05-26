import { useNavigate } from 'react-router-dom';

const getInitial = (name) => {
    return name?.trim()?.charAt(0)?.toUpperCase() || 'G';
};

const TeacherDetailUI = ({ detail, isLoading, error }) => {
    const navigate = useNavigate();

    if (isLoading) {
        return (
            <div className="flex-1 flex items-center justify-center bg-surface min-h-screen">
                <p className="font-body text-on-surface-variant">Đang tải thông tin chi tiết...</p>
            </div>
        );
    }

    if (error || !detail) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center bg-surface min-h-screen gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin giảng viên'}</p>
                <button
                    onClick={() => navigate('/teachers')}
                    className="px-4 py-2 bg-surface-container-highest rounded-lg font-label text-sm hover:bg-surface-dim transition-colors"
                >
                    Quay lại danh sách
                </button>
            </div>
        );
    }

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-screen p-6 lg:p-12">
            <div className="mb-8 flex items-center gap-4">
                <button
                    onClick={() => navigate('/teachers')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                    title="Quay lại"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <h1 className="font-headline text-2xl lg:text-3xl font-extrabold text-on-surface tracking-tight">
                    Hồ sơ giảng viên
                </h1>
            </div>

            <div className="bg-surface-container-lowest rounded-2xl p-6 lg:p-8 shadow-[0_8px_24px_rgba(25,28,30,0.04)] mb-8 flex flex-col md:flex-row items-start gap-8">
                <div className="w-24 h-24 rounded-full bg-primary-container text-on-primary-container flex items-center justify-center font-headline text-3xl font-bold shadow-md flex-shrink-0">
                    {getInitial(detail.hoten)}
                </div>
                <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-y-4 gap-x-8 min-w-0">
                    <div>
                        <h2 className="font-headline text-2xl font-bold text-on-surface mb-1 break-words">
                            {detail.hoten || 'Chưa có tên'}
                        </h2>
                    </div>
                    <div className="space-y-3">
                        <div className="flex items-center gap-3 text-on-surface-variant font-body text-sm">
                            <span className="material-symbols-outlined text-outline">mail</span>
                            {detail.email || 'Chưa có email'}
                        </div>
                        <div className="flex items-center gap-3 text-on-surface-variant font-body text-sm">
                            <span className="material-symbols-outlined text-outline">call</span>
                            {detail.dienthoai || 'Chưa có số điện thoại'}
                        </div>
                        <div className="flex items-center gap-3 text-on-surface-variant font-body text-sm">
                            <span className="material-symbols-outlined text-outline">location_on</span>
                            {detail.diachi || 'Chưa có địa chỉ'}
                        </div>
                        <div className="flex items-center gap-3 text-on-surface-variant font-body text-sm">
                            <span className="material-symbols-outlined text-outline">person</span>
                            Giới tính: {detail.gioitinh || 'Chưa cập nhật'}
                        </div>
                    </div>
                </div>
            </div>

            <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                <div className="bg-surface-container-lowest rounded-xl p-6 shadow-sm border border-surface-container">
                    <div className="flex items-center gap-2 mb-4 border-b border-surface-container-highest pb-3">
                        <span className="material-symbols-outlined text-primary">menu_book</span>
                        <h3 className="font-headline font-semibold text-lg text-on-surface">Chuyên đề phụ trách</h3>
                        <span className="ml-auto bg-primary-container text-on-primary-container font-label text-xs px-2 py-0.5 rounded-full">
                            {detail.topics?.length || 0}
                        </span>
                    </div>
                    {detail.topics && detail.topics.length > 0 ? (
                        <ul className="space-y-2">
                            {detail.topics.map((topic) => (
                                <li
                                    key={topic.maID}
                                    className="flex items-center gap-3 font-body text-sm text-on-surface-variant bg-surface-container-low px-3 py-2 rounded-lg"
                                >
                                    <span className="w-1.5 h-1.5 rounded-full bg-secondary flex-shrink-0"></span>
                                    <span className="flex-1">{topic.tenChuyende || 'Chuyên đề đã phân công'}</span>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p className="font-body text-sm text-outline italic">Chưa có chuyên đề nào.</p>
                    )}
                </div>

                <div className="bg-surface-container-lowest rounded-xl p-6 shadow-sm border border-surface-container">
                    <div className="flex items-center gap-2 mb-4 border-b border-surface-container-highest pb-3">
                        <span className="material-symbols-outlined text-primary">info</span>
                        <h3 className="font-headline font-semibold text-lg text-on-surface">Tổng quan</h3>
                    </div>
                    <dl className="space-y-3 font-body text-sm">
                        <div className="flex justify-between gap-4">
                            <dt className="text-on-surface-variant">Số chuyên đề</dt>
                            <dd className="text-on-surface font-medium">{detail.topics?.length || 0}</dd>
                        </div>
                    </dl>
                </div>
            </div>
        </div>
    );
};

export default TeacherDetailUI;
