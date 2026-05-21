import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const generateId = () => {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
};

const TopicFormUI = ({ initialData, onSave, isSaving }) => {
    const navigate = useNavigate();
    const fileInputRef = React.useRef(null);
    const [formData, setFormData] = useState({
        maID: generateId(),
        ten: '',
        mota: '',
        documents: [],
        questions: []
    });

    useEffect(() => {
        if (initialData) {
            setFormData({
                maID: initialData.maID || generateId(),
                ten: initialData.ten || '',
                mota: initialData.mota || '',
                documents: (initialData.documents || []).map(d => ({ ...d, maID: d.maID || generateId() })),
                questions: (initialData.questions || []).map(q => ({
                    ...q,
                    maID: q.maID || generateId(),
                    // Backend TopicDetailDto returns "answers", form uses "answersDtos" internally
                    answersDtos: (q.answersDtos || q.answers || []).map(a => ({ ...a, maID: a.maID || generateId() }))
                }))
            });
        }
    }, [initialData]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    // --- Documents Logic ---
    const handleFileSelect = (e) => {
        const files = Array.from(e.target.files);
        if (files.length === 0) return;

        const newDocs = files.map(file => {
            let loaitailieu = 'Khác';
            if (file.type.includes('pdf')) loaitailieu = 'PDF';
            else if (file.type.includes('word') || file.name.endsWith('.doc') || file.name.endsWith('.docx')) loaitailieu = 'Word';
            else if (file.type.includes('video')) loaitailieu = 'Video';

            return {
                maID: generateId(),
                tieude: file.name,
                loaitailieu: loaitailieu,
                kichthuoc: Math.round(file.size / 1024), // KB
                file: file // Store the actual file object for later upload if needed
            };
        });

        setFormData(prev => ({
            ...prev,
            documents: [...prev.documents, ...newDocs]
        }));
        
        // Reset file input
        if (fileInputRef.current) {
            fileInputRef.current.value = '';
        }
    };

    const handleDocumentChange = (index, field, value) => {
        const newDocs = [...formData.documents];
        newDocs[index][field] = value;
        setFormData(prev => ({ ...prev, documents: newDocs }));
    };
    const handleRemoveDocument = (index) => {
        setFormData(prev => ({
            ...prev,
            documents: prev.documents.filter((_, i) => i !== index)
        }));
    };

    // --- Questions Logic ---
    const handleAddQuestion = () => {
        setFormData(prev => ({
            ...prev,
            questions: [...prev.questions, { maID: generateId(), ten: '', diem: 1, answersDtos: [] }]
        }));
    };
    const handleQuestionChange = (index, field, value) => {
        const newQs = [...formData.questions];
        newQs[index][field] = value;
        setFormData(prev => ({ ...prev, questions: newQs }));
    };
    const handleRemoveQuestion = (index) => {
        setFormData(prev => ({
            ...prev,
            questions: prev.questions.filter((_, i) => i !== index)
        }));
    };

    // --- Answers Logic ---
    const handleAddAnswer = (qIndex) => {
        const newQs = [...formData.questions];
        if (!newQs[qIndex].answersDtos) newQs[qIndex].answersDtos = [];
        newQs[qIndex].answersDtos.push({ maID: generateId(), ten: '', dung: false });
        setFormData(prev => ({ ...prev, questions: newQs }));
    };
    const handleAnswerChange = (qIndex, aIndex, field, value) => {
        const newQs = [...formData.questions];
        newQs[qIndex].answersDtos[aIndex][field] = value;
        setFormData(prev => ({ ...prev, questions: newQs }));
    };
    const handleRemoveAnswer = (qIndex, aIndex) => {
        const newQs = [...formData.questions];
        newQs[qIndex].answersDtos = newQs[qIndex].answersDtos.filter((_, i) => i !== aIndex);
        setFormData(prev => ({ ...prev, questions: newQs }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        
        // Transform formData keys to match backend DTO property names.
        // The backend uses different DTO shapes for Create vs Update,
        // but the caller (TopicFormPage) determines which API to call based on isEditMode.
        // We provide both shapes: the caller's service function handles routing.
        
        const isEdit = !!initialData; // If initialData exists, it's edit mode

        if (isEdit) {
            // TopicUpdateDto shape
            const payload = {
                maID: formData.maID,
                ten: formData.ten,
                mota: formData.mota,
                documentsDto: formData.documents.map(d => ({
                    maID: d.maID,
                    tieude: d.tieude,
                    ngaytao: d.ngaytao || new Date().toISOString(),
                    loaitailieu: d.loaitailieu,
                    kichthuoc: d.kichthuoc
                })),
                questionsDtos: formData.questions.map(q => ({
                    maID: q.maID,
                    ten: q.ten,
                    diem: q.diem,
                    answersDtos: (q.answersDtos || []).map(a => ({
                        maID: a.maID,
                        cauhoiID: q.maID,
                        ten: a.ten,
                        dung: a.dung
                    }))
                }))
            };
            onSave(payload);
        } else {
            // Chuyende_ChitietDto shape (Create)
            const payload = {
                maID: formData.maID,
                ten: formData.ten,
                mota: formData.mota,
                chuyende_TailieuDtos: formData.documents.map(d => ({
                    maID: d.maID,
                    chuyendeID: formData.maID,
                    tieude: d.tieude,
                    loaitailieu: d.loaitailieu,
                    kichthuoc: d.kichthuoc
                })),
                chuyende_CauhoiDtos: formData.questions.map(q => ({
                    maID: q.maID,
                    chuyendeID: formData.maID,
                    ten: q.ten,
                    diem: q.diem,
                    chuyende_DapanDtos: (q.answersDtos || []).map(a => ({
                        maID: a.maID,
                        chuyende_CauhoiID: q.maID,
                        ten: a.ten,
                        dung: a.dung
                    }))
                }))
            };
            onSave(payload);
        }
    };

    return (
        <div className="flex-1 overflow-y-auto bg-surface relative z-0 flex flex-col min-h-[calc(100vh-60px)] p-8 lg:p-12">
            <div className="mb-8 flex items-center gap-4">
                <button 
                    type="button"
                    onClick={() => navigate('/topics')}
                    className="w-10 h-10 rounded-full flex items-center justify-center bg-surface-container-lowest shadow-sm hover:bg-surface-container-highest transition-colors text-on-surface"
                >
                    <span className="material-symbols-outlined text-xl">arrow_back</span>
                </button>
                <h1 className="font-headline text-3xl font-extrabold text-on-surface tracking-tight">
                    {initialData ? 'Chỉnh sửa Chuyên đề' : 'Thêm Chuyên đề mới'}
                </h1>
            </div>

            <form onSubmit={handleSubmit} className="flex flex-col gap-8 max-w-5xl mx-auto w-full">
                {/* Thông tin chung */}
                <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                    <h2 className="font-headline text-xl font-bold text-on-surface mb-6 flex items-center gap-2">
                        <span className="material-symbols-outlined text-primary">info</span>
                        Thông tin chung
                    </h2>
                    <div className="space-y-4">
                        <div>
                            <label className="block font-label text-sm font-semibold text-on-surface-variant mb-1">Tên chuyên đề *</label>
                            <input required type="text" name="ten" value={formData.ten} onChange={handleInputChange}
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all"
                                placeholder="Nhập tên chuyên đề..." />
                        </div>
                        <div>
                            <label className="block font-label text-sm font-semibold text-on-surface-variant mb-1">Mô tả</label>
                            <textarea name="mota" value={formData.mota} onChange={handleInputChange} rows="3"
                                className="w-full bg-surface-container-highest text-on-surface placeholder:text-outline border-none rounded-md px-4 py-3 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all resize-y"
                                placeholder="Nhập mô tả chuyên đề..." />
                        </div>
                    </div>
                </div>

                {/* Tài liệu */}
                <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                    <div className="flex items-center justify-between mb-6">
                        <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">library_books</span>
                            Tài liệu đính kèm
                        </h2>
                        <input 
                            type="file" 
                            multiple 
                            ref={fileInputRef} 
                            style={{ display: 'none' }} 
                            onChange={handleFileSelect} 
                        />
                        <button type="button" onClick={() => fileInputRef.current?.click()} className="flex items-center gap-1 bg-surface-container-highest text-on-surface-variant px-4 py-2 rounded-lg font-label text-sm font-bold hover:bg-surface-container-highest hover:text-primary transition-colors">
                            <span className="material-symbols-outlined text-[18px]">upload_file</span> Chọn file từ máy
                        </button>
                    </div>

                    {formData.documents.length === 0 ? (
                        <p className="font-body text-sm text-outline italic text-center py-6 bg-surface-container-low rounded-xl">Chưa có tài liệu nào.</p>
                    ) : (
                        <div className="space-y-4">
                            {formData.documents.map((doc, index) => (
                                <div key={doc.maID} className="flex items-start gap-4 bg-surface-container-low p-4 rounded-xl relative group">
                                    <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-4">
                                        <div>
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">Tiêu đề</label>
                                            <input required type="text" value={doc.tieude} onChange={(e) => handleDocumentChange(index, 'tieude', e.target.value)}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                                        </div>
                                        <div>
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">Loại tài liệu</label>
                                            <select value={doc.loaitailieu} onChange={(e) => handleDocumentChange(index, 'loaitailieu', e.target.value)}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all appearance-none cursor-pointer">
                                                <option value="PDF">PDF</option>
                                                <option value="Word">Word</option>
                                                <option value="Video">Video</option>
                                            </select>
                                        </div>
                                    </div>
                                    <button type="button" onClick={() => handleRemoveDocument(index)} className="mt-5 text-on-surface-variant hover:text-error p-1.5 rounded-md hover:bg-error-container transition-colors" title="Xóa tài liệu">
                                        <span className="material-symbols-outlined text-lg">delete</span>
                                    </button>
                                </div>
                            ))}
                        </div>
                    )}
                </div>

                {/* Câu hỏi trắc nghiệm */}
                <div className="bg-surface-container-lowest rounded-2xl p-8 shadow-sm border border-surface-container">
                    <div className="flex items-center justify-between mb-6">
                        <h2 className="font-headline text-xl font-bold text-on-surface flex items-center gap-2">
                            <span className="material-symbols-outlined text-primary">quiz</span>
                            Ngân hàng câu hỏi
                        </h2>
                        <button type="button" onClick={handleAddQuestion} className="flex items-center gap-1 bg-surface-container-highest text-on-surface-variant px-4 py-2 rounded-lg font-label text-sm font-bold hover:bg-surface-container-highest hover:text-primary transition-colors">
                            <span className="material-symbols-outlined text-[18px]">add</span> Thêm câu hỏi
                        </button>
                    </div>

                    {formData.questions.length === 0 ? (
                        <p className="font-body text-sm text-outline italic text-center py-6 bg-surface-container-low rounded-xl">Chưa có câu hỏi nào.</p>
                    ) : (
                        <div className="space-y-6">
                            {formData.questions.map((q, qIndex) => (
                                <div key={q.maID} className="bg-surface-container-low p-5 rounded-xl border border-surface-container relative">
                                    <div className="flex items-start gap-4 mb-4">
                                        <div className="flex-1">
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">Nội dung câu hỏi</label>
                                            <input required type="text" value={q.ten} onChange={(e) => handleQuestionChange(qIndex, 'ten', e.target.value)}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                                        </div>
                                        <div className="w-24">
                                            <label className="block font-label text-xs font-semibold text-on-surface-variant mb-1">Điểm</label>
                                            <input required type="number" step="0.5" min="0" value={q.diem} onChange={(e) => handleQuestionChange(qIndex, 'diem', parseFloat(e.target.value))}
                                                className="w-full bg-surface-container-highest text-on-surface border-none rounded-md px-3 py-2 font-body text-sm focus:bg-surface-container-lowest focus:ring-2 focus:ring-primary/30 outline-none transition-all" />
                                        </div>
                                        <button type="button" onClick={() => handleRemoveQuestion(qIndex)} className="mt-5 text-on-surface-variant hover:text-error p-1.5 rounded-md hover:bg-error-container transition-colors" title="Xóa câu hỏi">
                                            <span className="material-symbols-outlined text-lg">delete</span>
                                        </button>
                                    </div>
                                    
                                    {/* Đáp án */}
                                    <div className="bg-surface-container-highest p-4 rounded-lg mt-2">
                                        <div className="flex items-center justify-between mb-3">
                                            <h4 className="font-label text-sm font-bold text-on-surface-variant">Các đáp án</h4>
                                            <button type="button" onClick={() => handleAddAnswer(qIndex)} className="text-primary text-xs font-bold hover:underline flex items-center">
                                                <span className="material-symbols-outlined text-[14px] mr-1">add</span> Thêm đáp án
                                            </button>
                                        </div>
                                        {(!q.answersDtos || q.answersDtos.length === 0) ? (
                                            <p className="font-body text-xs text-outline italic">Chưa có đáp án. Vui lòng thêm đáp án và chọn đáp án đúng.</p>
                                        ) : (
                                            <div className="space-y-2">
                                                {q.answersDtos.map((a, aIndex) => (
                                                    <div key={a.maID} className="flex items-center gap-3">
                                                        <input type="radio" name={`correct-answer-${q.maID}`} checked={a.dung} title="Đánh dấu là đáp án đúng"
                                                            onChange={() => {
                                                                const newQs = [...formData.questions];
                                                                newQs[qIndex].answersDtos.forEach(ans => ans.dung = false);
                                                                newQs[qIndex].answersDtos[aIndex].dung = true;
                                                                setFormData(prev => ({ ...prev, questions: newQs }));
                                                            }}
                                                            className="w-4 h-4 text-primary focus:ring-primary cursor-pointer" />
                                                        <input required type="text" value={a.ten} placeholder="Nội dung đáp án..."
                                                            onChange={(e) => handleAnswerChange(qIndex, aIndex, 'ten', e.target.value)}
                                                            className="flex-1 bg-surface text-on-surface border-none rounded-md px-3 py-1.5 font-body text-sm focus:ring-1 focus:ring-primary/50 outline-none transition-all" />
                                                        <button type="button" onClick={() => handleRemoveAnswer(qIndex, aIndex)} className="text-on-surface-variant hover:text-error p-1 rounded-md transition-colors" title="Xóa đáp án">
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

                {/* Actions Footer */}
                <div className="flex justify-end gap-4 pb-12">
                    <button type="button" onClick={() => navigate('/topics')} disabled={isSaving} className="px-6 py-3 rounded-lg font-label font-bold text-sm text-on-surface-variant hover:bg-surface-container-highest transition-colors">
                        Hủy bỏ
                    </button>
                    <button type="submit" disabled={isSaving} className={`px-8 py-3 rounded-lg font-label font-bold text-sm text-on-primary bg-primary shadow-md hover:shadow-lg transition-all ${isSaving ? 'opacity-70 cursor-not-allowed' : 'hover:-translate-y-0.5'}`}>
                        {isSaving ? 'Đang lưu...' : 'Lưu chuyên đề'}
                    </button>
                </div>
            </form>
        </div>
    );
};

export default TopicFormUI;
