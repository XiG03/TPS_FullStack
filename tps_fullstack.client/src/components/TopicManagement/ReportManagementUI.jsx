import { useMemo, useState } from 'react';

const STATUS_OPTIONS = [
    { value: 'all', label: 'Tất cả' },
    { value: 'submitted', label: 'Đã nộp' },
    { value: 'pending', label: 'Chưa nộp' },
    { value: 'graded', label: 'Đã chấm' },
];

const ReportManagementUI = ({ reports, isLoading, error, onViewReport, onGradeReport }) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [statusFilter, setStatusFilter] = useState('all');

    const filteredReports = useMemo(() => {
        return reports.filter((report) => {
            const keyword = searchTerm.trim().toLowerCase();
            const matchesKeyword =
                report.studentName.toLowerCase().includes(keyword) ||
                report.topicName.toLowerCase().includes(keyword) ||
                report.courseName.toLowerCase().includes(keyword);

            const matchesStatus = statusFilter === 'all' || report.status === statusFilter;
            return matchesKeyword && matchesStatus;
        });
    }, [reports, searchTerm, statusFilter]);

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)]">
            <header className="flex justify-between items-end px-12 pt-10 pb-10">
                <div className="flex flex-col gap-2">
                    <h1 className="font-headline text-4xl font-extrabold text-on-surface tracking-tight">Quản lý Bài thu hoạch</h1>
                    <p className="font-body text-on-surface-variant text-base">Theo dõi tiến độ nộp bài, chấm điểm và phản hồi cho sinh viên.</p>
                </div>
            </header>

            <section className="px-12 pb-8 grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="bg-surface-container-low rounded-xl p-6 flex flex-col gap-2 shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="report-search">Tìm kiếm</label>
                    <input
                        id="report-search"
                        type="text"
                        placeholder="Tìm theo sinh viên, chuyên đề hoặc khoá học..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                    />
                </div>

                <div className="bg-surface-container-low rounded-xl p-6 flex flex-col gap-2 shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="status-filter">Trạng thái</label>
                    <select
                        id="status-filter"
                        value={statusFilter}
                        onChange={(e) => setStatusFilter(e.target.value)}
                        className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                    >
                        {STATUS_OPTIONS.map((item) => (
                            <option key={item.value} value={item.value}>{item.label}</option>
                        ))}
                    </select>
                </div>
            </section>

            <section className="px-12 pb-16 flex-1 flex flex-col">
                <div className="grid grid-cols-[2fr_2fr_2fr_1fr_1fr_auto] gap-4 px-6 pb-4 font-label text-xs font-bold text-on-surface-variant uppercase tracking-wider">
                    <div>Sinh viên</div>
                    <div>Chuyên đề</div>
                    <div>Khoá học</div>
                    <div>Hạn nộp</div>
                    <div>Điểm</div>
                    <div className="w-[120px] text-center">Thao tác</div>
                </div>

                <div className="flex flex-col gap-3">
                    {isLoading ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Đang tải dữ liệu...</div>
                    ) : error ? (
                        <div className="text-center py-10 font-body text-error">{error}</div>
                    ) : filteredReports.length === 0 ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Không có bài thu hoạch phù hợp bộ lọc.</div>
                    ) : (
                        filteredReports.map((report) => (
                            <div key={report.id} className="bg-surface-container-lowest rounded-xl p-5 grid grid-cols-[2fr_2fr_2fr_1fr_1fr_auto] gap-4 items-center shadow-[0_4px_12px_rgba(25,28,30,0.02)]">
                                <div className="font-semibold text-on-surface">{report.studentName}</div>
                                <div className="text-on-surface-variant truncate">{report.topicName}</div>
                                <div className="text-on-surface-variant truncate">{report.courseName}</div>
                                <div className="text-sm text-on-surface">{report.deadline}</div>
                                <div className="text-sm font-semibold text-on-surface">{report.score ?? '--'}</div>
                                <div className="flex items-center justify-end gap-1 w-[120px]">
                                    <button onClick={() => onViewReport(report)} className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors" title="Xem chi tiết">
                                        <span className="material-symbols-outlined text-xl">visibility</span>
                                    </button>
                                    <button onClick={() => onGradeReport(report)} className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors" title="Chấm điểm">
                                        <span className="material-symbols-outlined text-xl">task_alt</span>
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

export default ReportManagementUI;
