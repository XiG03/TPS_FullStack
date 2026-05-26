import { useNavigate } from 'react-router-dom';

const formatDocumentDate = (value) => {
    if (!value) return 'Chưa có ngày tạo';

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return 'Chưa có ngày tạo';

    return date.toLocaleDateString('vi-VN');
};

const TopicDetailUI = ({ detail, isLoading, error }) => {
    const navigate = useNavigate();

    if (isLoading) {
        return (
            <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-screen">
                Đang tải thông tin chuyên đề...
            </div>
        );
    }

    if (error || !detail) {
        return (
            <div className="flex-1 flex flex-col items-center justify-center min-h-screen gap-4">
                <p className="font-body text-error">{error || 'Không tìm thấy thông tin chuyên đề'}</p>
                <button
                    onClick={() => navigate('/topics')}
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
                    onClick={() => navigate('/topics')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                    title="Quay lại"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <h1 className="font-headline text-2xl lg:text-3xl font-extrabold text-on-surface tracking-tight">
                    Chi tiết chuyên đề
                </h1>
                <button
                    onClick={() => navigate(`/topics/${detail.chuyendeID}/edit`)}
                    className="ml-auto flex items-center gap-2 bg-primary-container text-on-primary-container px-4 py-2 rounded-lg font-label font-bold hover:bg-primary hover:text-on-primary transition-colors"
                >
                    <span className="material-symbols-outlined text-[18px]">edit</span>
                    Chỉnh sửa
                </button>
            </div>

            <div className="flex flex-col gap-8 max-w-5xl mx-auto w-full">
                <div className="bg-surface-container-lowest rounded-2xl p-6 lg:p-8 shadow-sm border border-surface-container flex flex-col md:flex-row items-start gap-8">
                    <div className="w-20 h-20 rounded-2xl bg-secondary-container text-on-secondary-container flex items-center justify-center flex-shrink-0">
                        <span className="material-symbols-outlined text-4xl">local_library</span>
                    </div>
                    <div className="flex-1 min-w-0">
                        <div className="flex flex-col gap-2 mb-3">
                            <h2 className="font-headline text-2xl font-bold text-on-surface break-words">
                                {detail.ten}
                            </h2>
                        </div>
                        <p className="font-body text-on-surface-variant text-sm whitespace-pre-wrap">
                            {detail.mota || 'Không có mô tả chi tiết.'}
                        </p>
                    </div>
                </div>

                <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
                    <div className="bg-surface-container-lowest rounded-2xl p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-4 flex items-center gap-2 border-b border-surface-container-highest pb-3">
                            <span className="material-symbols-outlined text-secondary">library_books</span>
                            Tài liệu đính kèm ({detail.documents?.length || 0})
                        </h3>
                        {(!detail.documents || detail.documents.length === 0) ? (
                            <p className="font-body text-sm text-outline italic">Chưa có tài liệu.</p>
                        ) : (
                            <ul className="space-y-3">
                                {detail.documents.map((document) => (
                                    <li key={document.tailieuID} className="flex items-center gap-3 bg-surface-container-low p-3 rounded-lg">
                                        <div className="w-10 h-10 rounded bg-surface-container-highest flex items-center justify-center flex-shrink-0">
                                            <span className="material-symbols-outlined text-on-surface-variant">
                                                {document.loaitailieu === 'PDF' ? 'picture_as_pdf' : document.loaitailieu === 'Video' ? 'play_circle' : 'description'}
                                            </span>
                                        </div>
                                        <div className="flex-1 min-w-0">
                                            <p className="font-label font-bold text-sm text-on-surface truncate">
                                                {document.tieude}
                                            </p>
                                            <p className="font-body text-xs text-outline flex flex-wrap items-center gap-x-2 gap-y-1">
                                                <span>Loại: {document.loaitailieu || 'Khác'}</span>
                                                <span>Ngày tạo: {formatDocumentDate(document.ngaytao)}</span>
                                                {Number(document.kichthuoc) > 0 && <span>Kích thước: {document.kichthuoc} KB</span>}
                                            </p>
                                        </div>
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>

                    <div className="bg-surface-container-lowest rounded-2xl p-6 shadow-sm border border-surface-container">
                        <h3 className="font-headline text-lg font-bold text-on-surface mb-4 flex items-center gap-2 border-b border-surface-container-highest pb-3">
                            <span className="material-symbols-outlined text-tertiary">quiz</span>
                            Ngân hàng câu hỏi ({detail.questions?.length || 0})
                        </h3>
                        {(!detail.questions || detail.questions.length === 0) ? (
                            <p className="font-body text-sm text-outline italic">Chưa có câu hỏi.</p>
                        ) : (
                            <div className="space-y-4">
                                {detail.questions.map((question, index) => (
                                    <div key={question.cauhoiID} className="bg-surface-container-low p-4 rounded-lg">
                                        <p className="font-label font-bold text-sm text-on-surface mb-3">
                                            Câu {index + 1}: {question.ten}
                                        </p>
                                        <ul className="space-y-1">
                                            {(question.answers || []).map((answer) => (
                                                <li
                                                    key={answer.dapanID}
                                                    className={`flex items-start gap-2 text-sm font-body px-3 py-1.5 rounded-md ${answer.dung ? 'bg-primary/10 text-primary font-medium border border-primary/20' : 'text-on-surface-variant'}`}
                                                >
                                                    <span className="material-symbols-outlined text-[18px] mt-0.5 flex-shrink-0">
                                                        {answer.dung ? 'check_circle' : 'radio_button_unchecked'}
                                                    </span>
                                                    {answer.ten}
                                                </li>
                                            ))}
                                        </ul>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
};

export default TopicDetailUI;
