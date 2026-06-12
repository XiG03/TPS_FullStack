import { useMemo, useState } from 'react';
import { Eye, Loader2, Plus, X } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

const STATUS_OPTIONS = [
    { value: 'all', label: 'Tất cả' },
    { value: 'pending', label: 'Chưa làm' },
    { value: 'in_progress', label: 'Đang làm' },
    { value: 'submitted', label: 'Đã nộp' }
];

const STATUS_LABELS = {
    pending: 'Chưa làm',
    in_progress: 'Đang làm',
    submitted: 'Đã nộp'
};

const formatDateTime = (value) => {
    if (!value) return '--';

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return '--';

    return new Intl.DateTimeFormat('vi-VN', {
        hour: '2-digit',
        minute: '2-digit',
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    }).format(date);
};

const formatScore = (score) => {
    if (score === null || score === undefined || score === '') return '--';
    const value = Number(score);
    return Number.isNaN(value) ? score : value.toFixed(1).replace('.0', '');
};

const getStatusClass = (status) => {
    if (status === 'submitted') return 'bg-emerald-50 text-emerald-700 border-emerald-200';
    if (status === 'in_progress') return 'bg-amber-50 text-amber-700 border-amber-200';
    return 'bg-surface-container-highest text-on-surface-variant border-outline-variant';
};

const ReportDetailModal = ({ report, isLoading, error, onClose }) => {
    return (
        <div className="fixed inset-0 z-50 bg-black/40 flex items-center justify-center p-4">
            <div className="w-full max-w-5xl max-h-[90vh] overflow-hidden rounded-lg bg-surface-container-lowest shadow-xl flex flex-col">
                <div className="flex items-start justify-between gap-4 border-b border-outline-variant px-6 py-4">
                    <div>
                        <h2 className="font-headline text-xl font-bold text-on-surface mb-1">
                            Chi tiết bài thu hoạch
                        </h2>
                        {report && (
                            <p className="font-body text-sm text-on-surface-variant">
                                {report.studentName} - {report.courseName}
                            </p>
                        )}
                    </div>
                    <button
                        type="button"
                        onClick={onClose}
                        className="w-9 h-9 rounded-full flex items-center justify-center text-on-surface-variant hover:text-on-surface hover:bg-surface-container-highest transition-colors"
                        title="Đóng"
                    >
                        <X size={20} />
                    </button>
                </div>

                <div className="overflow-y-auto px-6 py-5">
                    {isLoading ? (
                        <div className="py-16 flex items-center justify-center gap-3 text-on-surface-variant">
                            <Loader2 size={20} className="animate-spin" />
                            <span>Đang tải chi tiết...</span>
                        </div>
                    ) : error ? (
                        <div className="py-16 text-center font-body text-error">{error}</div>
                    ) : !report ? (
                        <div className="py-16 text-center font-body text-on-surface-variant">
                            Không có dữ liệu chi tiết.
                        </div>
                    ) : (
                        <div className="flex flex-col gap-5">
                            <div className="grid grid-cols-1 sm:grid-cols-4 gap-3 rounded-lg bg-surface-container-low p-4">
                                <div>
                                    <div className="text-xs font-bold uppercase tracking-wider text-on-surface-variant">Bắt đầu</div>
                                    <div className="text-sm font-semibold text-on-surface">{formatDateTime(report.startedAt)}</div>
                                </div>
                                <div>
                                    <div className="text-xs font-bold uppercase tracking-wider text-on-surface-variant">Kết thúc</div>
                                    <div className="text-sm font-semibold text-on-surface">{formatDateTime(report.endedAt)}</div>
                                </div>
                                <div>
                                    <div className="text-xs font-bold uppercase tracking-wider text-on-surface-variant">Trạng thái</div>
                                    <div className="text-sm font-semibold text-on-surface">{STATUS_LABELS[report.status] || '--'}</div>
                                </div>
                                <div>
                                    <div className="text-xs font-bold uppercase tracking-wider text-on-surface-variant">Điểm</div>
                                    <div className="text-sm font-semibold text-on-surface">{formatScore(report.score)}</div>
                                </div>
                            </div>

                            {report.questions.length === 0 ? (
                                <div className="rounded-lg border border-outline-variant px-4 py-8 text-center text-on-surface-variant">
                                    Chưa có câu hỏi cho bài thu hoạch này.
                                </div>
                            ) : (
                                report.questions.map((question, index) => (
                                    <div key={question.maID || index} className="rounded-lg border border-outline-variant p-4">
                                        <div className="mb-3 font-headline font-bold text-on-surface">
                                            Câu {index + 1}: {question.questionText || 'Chưa có nội dung câu hỏi'}
                                        </div>
                                        <div className="flex flex-col gap-2">
                                            {question.answers.length === 0 ? (
                                                <div className="text-sm text-on-surface-variant">Chưa có đáp án.</div>
                                            ) : (
                                                question.answers.map((answer, answerIndex) => (
                                                    <div
                                                        key={answer.maID || answerIndex}
                                                        className={`rounded-md border px-3 py-2 text-sm ${
                                                            answer.isSelected
                                                                ? 'border-primary bg-primary/10 text-on-surface'
                                                                : 'border-outline-variant bg-surface-container-low text-on-surface-variant'
                                                        }`}
                                                    >
                                                        <div className="flex flex-col gap-1 sm:flex-row sm:items-center sm:justify-between">
                                                            <span>{answer.answerText || 'Chưa có nội dung đáp án'}</span>
                                                            <span className="flex items-center gap-2 text-xs font-semibold">
                                                                {answer.isSelected && <span className="text-primary">Đã chọn</span>}
                                                                {answer.isCorrect && <span className="text-emerald-700">Đáp án đúng</span>}
                                                            </span>
                                                        </div>
                                                    </div>
                                                ))
                                            )}
                                        </div>
                                    </div>
                                ))
                            )}
                        </div>
                    )}
                </div>
            </div>
        </div>
    );
};

const ReportManagementUI = ({
    reports,
    selectedReport,
    isLoading,
    isLoadingDetail,
    error,
    detailError,
    onViewReport,
    onCloseDetail
}) => {
    const [searchTerm, setSearchTerm] = useState('');
    const [statusFilter, setStatusFilter] = useState('all');
    const navigate = useNavigate();

    const filteredReports = useMemo(() => {
        const keyword = searchTerm.trim().toLowerCase();

        return reports.filter((report) => {
            const matchesKeyword = !keyword ||
                report.studentName.toLowerCase().includes(keyword) ||
                report.courseName.toLowerCase().includes(keyword);

            const matchesStatus = statusFilter === 'all' || report.status === statusFilter;
            return matchesKeyword && matchesStatus;
        });
    }, [reports, searchTerm, statusFilter]);

    const showDetailModal = isLoadingDetail || detailError || selectedReport;

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)]">
            <header className="flex flex-col gap-6 px-6 pt-8 pb-8 lg:flex-row lg:items-end lg:justify-between lg:px-12 lg:pt-10">
                <div className="flex flex-col gap-2">
                    <h1 className="font-headline text-3xl lg:text-4xl font-extrabold text-on-surface tracking-tight">
                        Quản lý bài thu hoạch
                    </h1>
                    <p className="font-body text-on-surface-variant text-base">
                        Theo dõi bài thu hoạch theo khóa học, học viên, thời gian làm bài và điểm.
                    </p>
                </div>
                <button
                    onClick={() => navigate('/reports/new')}
                    className="flex items-center justify-center gap-2 bg-gradient-to-br from-primary to-primary-container text-on-primary px-6 py-3.5 rounded-lg font-body font-semibold shadow-[0px_12px_32px_rgba(25,28,30,0.06)] hover:shadow-md transition-all hover:-translate-y-0.5"
                >
                    <Plus size={20} />
                    Thêm bài thu hoạch
                </button>
            </header>

            <section className="px-6 pb-8 grid grid-cols-1 md:grid-cols-2 gap-6 lg:px-12">
                <div className="bg-surface-container-low rounded-lg p-6 flex flex-col gap-2 shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="report-search">
                        Tìm kiếm
                    </label>
                    <input
                        id="report-search"
                        type="text"
                        placeholder="Tìm theo học viên hoặc khóa học..."
                        value={searchTerm}
                        onChange={(event) => setSearchTerm(event.target.value)}
                        className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                    />
                </div>

                <div className="bg-surface-container-low rounded-lg p-6 flex flex-col gap-2 shadow-[inset_0_2px_4px_rgba(255,255,255,0.4)]">
                    <label className="font-label text-sm font-semibold text-on-surface-variant" htmlFor="status-filter">
                        Trạng thái
                    </label>
                    <select
                        id="status-filter"
                        value={statusFilter}
                        onChange={(event) => setStatusFilter(event.target.value)}
                        className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                    >
                        {STATUS_OPTIONS.map((item) => (
                            <option key={item.value} value={item.value}>{item.label}</option>
                        ))}
                    </select>
                </div>
            </section>

            <section className="px-6 pb-16 flex-1 flex flex-col lg:px-12">
                <div className="hidden lg:grid grid-cols-[1.4fr_1.7fr_120px_150px_150px_80px_80px] gap-4 px-6 pb-4 font-label text-xs font-bold text-on-surface-variant uppercase tracking-wider">
                    <div>Học viên</div>
                    <div>Khóa học</div>
                    <div>Trạng thái</div>
                    <div>Bắt đầu</div>
                    <div>Kết thúc</div>
                    <div>Điểm</div>
                    <div className="text-center">Xem</div>
                </div>

                <div className="flex flex-col gap-3">
                    {isLoading ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">Đang tải dữ liệu...</div>
                    ) : error ? (
                        <div className="text-center py-10 font-body text-error">{error}</div>
                    ) : filteredReports.length === 0 ? (
                        <div className="text-center py-10 font-body text-on-surface-variant">
                            Không có bài thu hoạch phù hợp bộ lọc.
                        </div>
                    ) : (
                        filteredReports.map((report) => (
                            <div
                                key={report.maID}
                                className="bg-surface-container-lowest rounded-lg p-5 grid grid-cols-1 gap-3 shadow-[0_4px_12px_rgba(25,28,30,0.02)] lg:grid-cols-[1.4fr_1.7fr_120px_150px_150px_80px_80px] lg:items-center"
                            >
                                <div>
                                    <div className="lg:hidden text-xs font-bold uppercase tracking-wider text-on-surface-variant mb-1">Học viên</div>
                                    <div className="font-semibold text-on-surface truncate">{report.studentName}</div>
                                </div>
                                <div>
                                    <div className="lg:hidden text-xs font-bold uppercase tracking-wider text-on-surface-variant mb-1">Khóa học</div>
                                    <div className="text-on-surface-variant truncate">{report.courseName}</div>
                                </div>
                                <div>
                                    <div className="lg:hidden text-xs font-bold uppercase tracking-wider text-on-surface-variant mb-1">Trạng thái</div>
                                    <span className={`inline-flex rounded-full border px-3 py-1 text-xs font-semibold ${getStatusClass(report.status)}`}>
                                        {STATUS_LABELS[report.status] || '--'}
                                    </span>
                                </div>
                                <div>
                                    <div className="lg:hidden text-xs font-bold uppercase tracking-wider text-on-surface-variant mb-1">Bắt đầu</div>
                                    <div className="text-sm text-on-surface">{formatDateTime(report.startedAt)}</div>
                                </div>
                                <div>
                                    <div className="lg:hidden text-xs font-bold uppercase tracking-wider text-on-surface-variant mb-1">Kết thúc</div>
                                    <div className="text-sm text-on-surface">{formatDateTime(report.endedAt)}</div>
                                </div>
                                <div>
                                    <div className="lg:hidden text-xs font-bold uppercase tracking-wider text-on-surface-variant mb-1">Điểm</div>
                                    <div className="text-sm font-semibold text-on-surface">{formatScore(report.score)}</div>
                                </div>
                                <div className="flex items-center justify-start lg:justify-center">
                                    <button
                                        onClick={() => onViewReport(report)}
                                        className="text-on-surface-variant hover:text-primary hover:bg-surface-container-highest p-2 rounded-md transition-colors"
                                        title="Xem chi tiết"
                                    >
                                        <Eye size={20} />
                                    </button>
                                </div>
                            </div>
                        ))
                    )}
                </div>
            </section>

            {showDetailModal && (
                <ReportDetailModal
                    report={selectedReport}
                    isLoading={isLoadingDetail}
                    error={detailError}
                    onClose={onCloseDetail}
                />
            )}
        </div>
    );
};

export default ReportManagementUI;
