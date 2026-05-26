const getValue = (source, ...keys) => {
    for (const key of keys) {
        if (source?.[key] !== undefined && source?.[key] !== null) return source[key];
    }
    return undefined;
};

const toDate = (value) => {
    if (!value) return null;
    const date = new Date(value);
    return Number.isNaN(date.getTime()) ? null : date;
};

const formatDate = (value) => {
    const date = toDate(value);
    if (!date) return 'Chưa cập nhật';
    return date.toLocaleDateString('vi-VN', {
        weekday: 'long',
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
};

const formatTime = (value) => {
    const date = toDate(value);
    if (!date) return '--:--';
    return date.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
};

const DetailRow = ({ label, value }) => (
    <div className="flex items-start justify-between gap-4 rounded-lg bg-surface-container-low px-4 py-3">
        <dt className="font-label text-sm font-semibold text-on-surface-variant">{label}</dt>
        <dd className="max-w-[60%] text-right font-body text-sm font-semibold text-on-surface">{value}</dd>
    </div>
);

const ScheduleDetailModal = ({ schedule, onClose, onEditSchedule, onDeleteSchedule }) => {
    if (!schedule) return null;

    const id = getValue(schedule, 'lichhocID', 'LichhocID', 'maID', 'MaID');
    const courseName = getValue(schedule, 'tenKhoahoc', 'TenKhoahoc') || 'Khóa học chưa đặt tên';
    const topicName = getValue(schedule, 'tenChuyende', 'TenChuyende') || 'Chưa chọn chuyên đề';
    const teacherName = getValue(schedule, 'tenGiangvien', 'TenGiangvien') || 'Chưa phân công';
    const expectedDate = getValue(schedule, 'ngaydukien', 'Ngaydukien');
    const expectedStart = getValue(schedule, 'batdaudukien', 'Batdaudukien');
    const expectedEnd = getValue(schedule, 'ketthucdukien', 'Ketthucdukien', 'kethucdukien', 'Kethucdukien');
    const actualDate = getValue(schedule, 'ngaythucte', 'Ngaythucte');
    const actualStart = getValue(schedule, 'batdauthucte', 'Batdauthucte');
    const actualEnd = getValue(schedule, 'ketthucthucte', 'Ketthucthucte', 'kethucthucte', 'Kethucthucte');

    const handleEdit = () => {
        onClose();
        onEditSchedule(schedule);
    };

    const handleDelete = () => {
        onClose();
        onDeleteSchedule(id);
    };

    return (
        <div className="fixed inset-0 z-[110] flex items-center justify-center bg-on-surface/30 p-4 backdrop-blur-sm">
            <div className="flex max-h-[90vh] w-full max-w-2xl flex-col overflow-hidden rounded-lg bg-surface shadow-2xl">
                <div className="flex items-start justify-between gap-4 border-b border-surface-container-highest bg-surface-container-lowest px-6 py-5">
                    <div className="min-w-0">
                        <div className="mb-2 inline-flex rounded-full bg-surface-container-high px-3 py-1 font-label text-[11px] font-bold uppercase text-on-surface-variant">
                            Chi tiết lịch học
                        </div>
                        <h2 className="m-0 truncate font-headline text-2xl font-extrabold text-on-surface">
                            {topicName}
                        </h2>
                        <p className="mt-1 truncate font-body text-sm text-on-surface-variant">{courseName}</p>
                    </div>
                    <button
                        type="button"
                        onClick={onClose}
                        className="rounded-full p-2 text-on-surface-variant transition-colors hover:bg-error-container hover:text-error"
                        title="Đóng"
                    >
                        <span className="material-symbols-outlined text-xl">close</span>
                    </button>
                </div>

                <div className="space-y-6 overflow-y-auto p-6">
                    <section>
                        <h3 className="mb-3 flex items-center gap-2 font-headline text-base font-bold text-on-surface">
                            <span className="material-symbols-outlined text-primary">school</span>
                            Thông tin buổi học
                        </h3>
                        <dl className="space-y-3">
                            <DetailRow label="Khóa học" value={courseName} />
                            <DetailRow label="Chuyên đề" value={topicName} />
                            <DetailRow label="Giảng viên" value={teacherName} />
                        </dl>
                    </section>

                    <section>
                        <h3 className="mb-3 flex items-center gap-2 font-headline text-base font-bold text-on-surface">
                            <span className="material-symbols-outlined text-primary">schedule</span>
                            Thời gian
                        </h3>
                        <dl className="space-y-3">
                            <DetailRow label="Ngày dự kiến" value={formatDate(expectedDate)} />
                            <DetailRow label="Giờ dự kiến" value={`${formatTime(expectedStart)} - ${formatTime(expectedEnd)}`} />
                            <DetailRow label="Ngày thực tế" value={formatDate(actualDate)} />
                            <DetailRow label="Giờ thực tế" value={`${formatTime(actualStart)} - ${formatTime(actualEnd)}`} />
                        </dl>
                    </section>
                </div>

                <div className="flex flex-col-reverse gap-3 border-t border-surface-container-highest bg-surface-container-lowest p-5 sm:flex-row sm:justify-end">
                    <button
                        type="button"
                        onClick={handleDelete}
                        className="rounded-lg px-5 py-2.5 font-label text-sm font-bold text-error transition-colors hover:bg-error-container"
                    >
                        Xóa
                    </button>
                    <button
                        type="button"
                        onClick={handleEdit}
                        className="rounded-lg bg-surface-container-highest px-5 py-2.5 font-label text-sm font-bold text-on-surface-variant transition-colors hover:text-primary"
                    >
                        Chỉnh sửa
                    </button>
                    <button
                        type="button"
                        onClick={onClose}
                        className="rounded-lg bg-primary px-6 py-2.5 font-label text-sm font-bold text-on-primary shadow-sm transition-all hover:-translate-y-0.5"
                    >
                        Đóng
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ScheduleDetailModal;
