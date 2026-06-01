import { useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';

const generateId = () => {
    if (window.crypto?.randomUUID) return window.crypto.randomUUID();

    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, (char) => {
        const random = Math.random() * 16 | 0;
        const value = char === 'x' ? random : (random & 0x3) | 0x8;
        return value.toString(16);
    });
};

const toInputDateTime = (value) => {
    if (!value) return new Date().toISOString().slice(0, 16);

    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return new Date().toISOString().slice(0, 16);

    return date.toISOString().slice(0, 16);
};

const toApiDateTime = (value) => {
    const date = value ? new Date(value) : new Date();
    return Number.isNaN(date.getTime()) ? new Date().toISOString() : date.toISOString();
};

const createEmptyAnswer = () => ({
    clientId: generateId(),
    dapanID: '',
    ten: '',
    dung: false
});

const createEmptyQuestion = () => ({
    clientId: generateId(),
    cauhoiID: '',
    ten: '',
    answers: [createEmptyAnswer()]
});

const createEmptyDocument = () => ({
    clientId: generateId(),
    tailieuID: '',
    tieude: '',
    ngaytao: toInputDateTime(),
    loaitailieu: 'PDF',
    kichthuoc: 0,
    file: null
});

const buildFormData = (initialData) => {
    if (!initialData) {
        return {
            chuyendeID: '',
            ten: '',
            mota: '',
            documents: [],
            questions: []
        };
    }

    return {
        chuyendeID: initialData.chuyendeID || '',
        ten: initialData.ten || '',
        mota: initialData.mota || '',
        documents: (initialData.documents || []).map((document) => ({
            clientId: document.tailieuID || generateId(),
            tailieuID: document.tailieuID || '',
            tieude: document.tieude || '',
            ngaytao: toInputDateTime(document.ngaytao),
            loaitailieu: document.loaitailieu || 'Khác',
            kichthuoc: document.kichthuoc ?? 0,
            file: null
        })),
        questions: (initialData.questions || []).map((question) => ({
            clientId: question.cauhoiID || generateId(),
            cauhoiID: question.cauhoiID || '',
            ten: question.ten || '',
            answers: (question.answers || []).map((answer) => ({
                clientId: answer.dapanID || generateId(),
                dapanID: answer.dapanID || '',
                ten: answer.ten || '',
                dung: Boolean(answer.dung)
            }))
        }))
    };
};

const TopicFormUI = ({ initialData, onSave, isSaving }) => {
    const navigate = useNavigate();
    const fileInputRef = useRef(null);
    const [formData, setFormData] = useState(() => buildFormData(initialData));

    const handleInputChange = (event) => {
        const { name, value } = event.target;
        setFormData((prev) => ({ ...prev, [name]: value }));
    };

    const handleFileSelect = (event) => {
        const files = Array.from(event.target.files || []);
        if (files.length === 0) return;

        const newDocuments = files.map((file) => {
            let loaitailieu = 'Khác';
            if (file.type.includes('pdf')) loaitailieu = 'PDF';
            else if (file.type.includes('word') || file.name.endsWith('.doc') || file.name.endsWith('.docx')) loaitailieu = 'Word';
            else if (file.type.includes('video')) loaitailieu = 'Video';

            return {
                ...createEmptyDocument(),
                tieude: file.name,
                loaitailieu,
                kichthuoc: Math.ceil(file.size / 1024),
                file
            };
        });

        setFormData((prev) => ({
            ...prev,
            documents: [...prev.documents, ...newDocuments]
        }));

        if (fileInputRef.current) fileInputRef.current.value = '';
    };

    const handleAddDocument = () => {
        setFormData((prev) => ({
            ...prev,
            documents: [...prev.documents, createEmptyDocument()]
        }));
    };

    const handleDocumentChange = (index, field, value) => {
        setFormData((prev) => ({
            ...prev,
            documents: prev.documents.map((document, currentIndex) => (
                currentIndex === index ? { ...document, [field]: value } : document
            ))
        }));
    };

    const handleRemoveDocument = (index) => {
        setFormData((prev) => ({
            ...prev,
            documents: prev.documents.filter((_, currentIndex) => currentIndex !== index)
        }));
    };

    const handleAddQuestion = () => {
        setFormData((prev) => ({
            ...prev,
            questions: [...prev.questions, createEmptyQuestion()]
        }));
    };

    const handleQuestionChange = (index, value) => {
        setFormData((prev) => ({
            ...prev,
            questions: prev.questions.map((question, currentIndex) => (
                currentIndex === index ? { ...question, ten: value } : question
            ))
        }));
    };

    const handleRemoveQuestion = (index) => {
        setFormData((prev) => ({
            ...prev,
            questions: prev.questions.filter((_, currentIndex) => currentIndex !== index)
        }));
    };

    const handleAddAnswer = (questionIndex) => {
        setFormData((prev) => ({
            ...prev,
            questions: prev.questions.map((question, currentIndex) => (
                currentIndex === questionIndex
                    ? { ...question, answers: [...question.answers, createEmptyAnswer()] }
                    : question
            ))
        }));
    };

    const handleAnswerChange = (questionIndex, answerIndex, field, value) => {
        setFormData((prev) => ({
            ...prev,
            questions: prev.questions.map((question, currentQuestionIndex) => {
                if (currentQuestionIndex !== questionIndex) return question;

                return {
                    ...question,
                    answers: question.answers.map((answer, currentAnswerIndex) => (
                        currentAnswerIndex === answerIndex ? { ...answer, [field]: value } : answer
                    ))
                };
            })
        }));
    };

    const handleCorrectAnswerChange = (questionIndex, answerIndex) => {
        setFormData((prev) => ({
            ...prev,
            questions: prev.questions.map((question, currentQuestionIndex) => {
                if (currentQuestionIndex !== questionIndex) return question;

                return {
                    ...question,
                    answers: question.answers.map((answer, currentAnswerIndex) => ({
                        ...answer,
                        dung: currentAnswerIndex === answerIndex
                    }))
                };
            })
        }));
    };

    const handleRemoveAnswer = (questionIndex, answerIndex) => {
        setFormData((prev) => ({
            ...prev,
            questions: prev.questions.map((question, currentQuestionIndex) => {
                if (currentQuestionIndex !== questionIndex) return question;

                return {
                    ...question,
                    answers: question.answers.filter((_, currentAnswerIndex) => currentAnswerIndex !== answerIndex)
                };
            })
        }));
    };

    const handleSubmit = (event) => {
        event.preventDefault();

        const payload = {
            chuyendeID: formData.chuyendeID,
            ten: formData.ten.trim(),
            mota: formData.mota.trim(),
            documents: formData.documents.map((document) => ({
                tailieuID: document.tailieuID || '',
                tieude: document.tieude.trim(),
                ngaytao: toApiDateTime(document.ngaytao),
                loaitailieu: document.loaitailieu,
                kichthuoc: Number(document.kichthuoc) || 0,
                file: document.file || null
            })),
            questions: formData.questions.map((question) => ({
                cauhoiID: question.cauhoiID || '',
                ten: question.ten.trim(),
                answers: question.answers.map((answer) => ({
                    dapanID: answer.dapanID || '',
                    ten: answer.ten.trim(),
                    dung: Boolean(answer.dung)
                }))
            }))
        };

        onSave(payload);
    };

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-6 lg:p-12">
            <div className="mb-8 flex items-center gap-4">
                <button
                    type="button"
                    onClick={() => navigate('/topics')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                    title="Quay lại"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <h1 className="font-headline text-2xl lg:text-3xl font-extrabold text-on-surface tracking-tight">
                    {initialData ? 'Chỉnh sửa chuyên đề' : 'Thêm chuyên đề mới'}
                </h1>
            </div>

            <form onSubmit={handleSubmit} className="flex flex-col gap-8 max-w-5xl mx-auto w-full">
                <div className="bg-surface-container-lowest rounded-2xl p-6 lg:p-8 shadow-sm border border-surface-container">
                    <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                        <span className="material-symbols-outlined text-primary">info</span>
                        Thông tin chung
                    </h2>
                    <div className="space-y-4">
                        <div>
                            <label className="block font-label text-sm font-semibold text-on-surface-variant mb-1">
                                Tên chuyên đề *
                            </label>
                            <input
                                required
                                type="text"
                                name="ten"
                                value={formData.ten}
                                onChange={handleInputChange}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                placeholder="Nhập tên chuyên đề..."
                            />
                        </div>
                        <div>
                            <label className="block font-label text-sm font-semibold text-on-surface-variant mb-1">
                                Mô tả
                            </label>
                            <textarea
                                name="mota"
                                value={formData.mota}
                                onChange={handleInputChange}
                                rows="3"
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all resize-y"
                                placeholder="Nhập mô tả chuyên đề..."
                            />
                        </div>
                    </div>
                </div>

                <div className="bg-surface-container-lowest rounded-2xl p-6 lg:p-8 shadow-sm border border-surface-container">
                    <div className="flex flex-col gap-4 mb-6 sm:flex-row sm:items-center sm:justify-between">
                        <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">library_books</span>
                            Tài liệu đính kèm
                        </h2>
                        <div className="flex flex-wrap gap-2">
                            <input
                                type="file"
                                multiple
                                ref={fileInputRef}
                                className="hidden"
                                onChange={handleFileSelect}
                            />
                            <button
                                type="button"
                                onClick={handleAddDocument}
                                className="flex items-center gap-1 bg-surface-container-highest text-on-surface-variant px-4 py-2 rounded-lg font-label text-sm font-bold hover:text-primary transition-colors"
                            >
                                <span className="material-symbols-outlined text-[18px]">add</span>
                                Thêm dòng
                            </button>
                            <button
                                type="button"
                                onClick={() => fileInputRef.current?.click()}
                                className="flex items-center gap-1 bg-surface-container-highest text-on-surface-variant px-4 py-2 rounded-lg font-label text-sm font-bold hover:text-primary transition-colors"
                            >
                                <span className="material-symbols-outlined text-[18px]">upload_file</span>
                                Tải file lên
                            </button>
                        </div>
                    </div>

                    {formData.documents.length === 0 ? (
                        <p className="font-body text-sm text-outline italic text-center py-6 bg-surface-container-low rounded-xl">
                            Chưa có tài liệu nào.
                        </p>
                    ) : (
                        <div className="space-y-4">
                            {formData.documents.map((document, index) => (
                                <div key={document.clientId} className="bg-surface-container-low p-4 rounded-xl">
                                    <div className="grid grid-cols-1 md:grid-cols-[minmax(0,2fr)_1fr_1fr_120px_auto] gap-4 items-end">
                                        <div>
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">
                                                Tiêu đề *
                                            </label>
                                            <input
                                                required
                                                type="text"
                                                value={document.tieude}
                                                onChange={(event) => handleDocumentChange(index, 'tieude', event.target.value)}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                            />
                                        </div>
                                        <div>
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">
                                                Loại
                                            </label>
                                            <select
                                                value={document.loaitailieu}
                                                onChange={(event) => handleDocumentChange(index, 'loaitailieu', event.target.value)}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                            >
                                                <option value="PDF">PDF</option>
                                                <option value="Word">Word</option>
                                                <option value="Video">Video</option>
                                                <option value="Khác">Khác</option>
                                            </select>
                                        </div>
                                        <div>
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">
                                                Ngày tạo
                                            </label>
                                            <input
                                                type="datetime-local"
                                                value={document.ngaytao}
                                                onChange={(event) => handleDocumentChange(index, 'ngaytao', event.target.value)}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                            />
                                        </div>
                                        <div>
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">
                                                KB
                                            </label>
                                            <input
                                                type="number"
                                                min="0"
                                                value={document.kichthuoc}
                                                onChange={(event) => handleDocumentChange(index, 'kichthuoc', event.target.value)}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                            />
                                        </div>
                                        <button
                                            type="button"
                                            onClick={() => handleRemoveDocument(index)}
                                            className="text-on-surface-variant hover:text-error p-2 rounded-md hover:bg-error-container transition-colors"
                                            title="Xóa tài liệu"
                                        >
                                            <span className="material-symbols-outlined text-lg">delete</span>
                                        </button>
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                <div className="bg-surface-container-lowest rounded-2xl p-6 lg:p-8 shadow-sm border border-surface-container">
                    <div className="flex items-center justify-between mb-6">
                        <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">quiz</span>
                            Ngân hàng câu hỏi
                        </h2>
                        <button
                            type="button"
                            onClick={handleAddQuestion}
                            className="flex items-center gap-1 bg-surface-container-highest text-on-surface-variant px-4 py-2 rounded-lg font-label text-sm font-bold hover:text-primary transition-colors"
                        >
                            <span className="material-symbols-outlined text-[18px]">add</span>
                            Thêm câu hỏi
                        </button>
                    </div>

                    {formData.questions.length === 0 ? (
                        <p className="font-body text-sm text-outline italic text-center py-6 bg-surface-container-low rounded-xl">
                            Chưa có câu hỏi nào.
                        </p>
                    ) : (
                        <div className="space-y-6">
                            {formData.questions.map((question, questionIndex) => (
                                <div key={question.clientId} className="bg-surface-container-low p-5 rounded-xl border border-surface-container">
                                    <div className="flex items-start gap-4 mb-4">
                                        <div className="flex-1">
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">
                                                Nội dung câu hỏi *
                                            </label>
                                            <input
                                                required
                                                type="text"
                                                value={question.ten}
                                                onChange={(event) => handleQuestionChange(questionIndex, event.target.value)}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                            />
                                        </div>
                                        <button
                                            type="button"
                                            onClick={() => handleRemoveQuestion(questionIndex)}
                                            className="mt-5 text-on-surface-variant hover:text-error p-1.5 rounded-md hover:bg-error-container transition-colors"
                                            title="Xóa câu hỏi"
                                        >
                                            <span className="material-symbols-outlined text-lg">delete</span>
                                        </button>
                                    </div>

                                    <div className="bg-surface-container-highest p-4 rounded-lg mt-2">
                                        <div className="flex items-center justify-between mb-3">
                                            <h4 className="font-label text-sm font-bold text-on-surface-variant">
                                                Các đáp án
                                            </h4>
                                            <button
                                                type="button"
                                                onClick={() => handleAddAnswer(questionIndex)}
                                                className="text-primary text-xs font-bold hover:underline flex items-center"
                                            >
                                                <span className="material-symbols-outlined text-[14px] mr-1">add</span>
                                                Thêm đáp án
                                            </button>
                                        </div>
                                        {question.answers.length === 0 ? (
                                            <p className="font-body text-xs text-outline italic">
                                                Chưa có đáp án.
                                            </p>
                                        ) : (
                                            <div className="space-y-2">
                                                {question.answers.map((answer, answerIndex) => (
                                                    <div key={answer.clientId} className="flex items-center gap-3">
                                                        <input
                                                            type="radio"
                                                            name={`correct-answer-${question.clientId}`}
                                                            checked={answer.dung}
                                                            title="Đánh dấu là đáp án đúng"
                                                            onChange={() => handleCorrectAnswerChange(questionIndex, answerIndex)}
                                                            className="w-4 h-4 text-primary focus:ring-primary cursor-pointer"
                                                        />
                                                        <input
                                                            required
                                                            type="text"
                                                            value={answer.ten}
                                                            placeholder="Nội dung đáp án..."
                                                            onChange={(event) => handleAnswerChange(questionIndex, answerIndex, 'ten', event.target.value)}
                                                            className="flex-1 bg-surface text-on-surface border-none rounded-md px-3 py-1.5 font-body text-sm focus:ring-1 focus:ring-primary/50 outline-none transition-all"
                                                        />
                                                        <button
                                                            type="button"
                                                            onClick={() => handleRemoveAnswer(questionIndex, answerIndex)}
                                                            className="text-on-surface-variant hover:text-error p-1 rounded-md transition-colors"
                                                            title="Xóa đáp án"
                                                        >
                                                            <span className="material-symbols-outlined text-sm">close</span>
                                                        </button>
                                                    </div>
                                                ))}
                                            </div>
                                        )}
                                    </div>
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                <div className="flex justify-end gap-4 pb-12">
                    <button
                        type="button"
                        onClick={() => navigate('/topics')}
                        disabled={isSaving}
                        className="px-6 py-3 rounded-lg font-label font-bold text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors"
                    >
                        Hủy bỏ
                    </button>
                    <button
                        type="submit"
                        disabled={isSaving}
                        className={`px-8 py-3 rounded-lg font-label font-bold text-sm text-on-primary bg-primary shadow-md hover:shadow-lg transition-all ${isSaving ? 'opacity-70 cursor-not-allowed' : 'hover:-translate-y-0.5'}`}
                    >
                        {isSaving ? 'Đang lưu...' : 'Lưu chuyên đề'}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default TopicFormUI;
