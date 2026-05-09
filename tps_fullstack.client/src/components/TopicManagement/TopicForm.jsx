import React, { useState, useEffect } from 'react';

const generateId = () => {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
};

export default function TopicForm({ topic, onSave, onCancel }) {
  const [formData, setFormData] = useState({
    maID: '',
    ten: '',
    mota: '',
    documents: [],
    questions: []
  });

  useEffect(() => {
    if (topic) {
        // Edit mode - map existing data to our local state format
        setFormData({
            maID: topic.maID || generateId(),
            ten: topic.ten || '',
            mota: topic.mota || '',
            // In a real scenario we would fetch full details including docs & questions here
            // The GET detail API returns topic.documents and topic.questions
            documents: (topic.documents || topic.documentsDto || topic.chuyende_TailieuDtos || []).map(d => ({...d, maID: d.maID || generateId()})),
            questions: (topic.questions || topic.questionsDtos || topic.chuyende_CauhoiDtos || []).map(q => ({
                ...q, 
                maID: q.maID || generateId(),
                answersDtos: (q.answers || q.answersDtos || q.chuyende_DapanDtos || []).map(a => ({...a, maID: a.maID || generateId()}))
            }))
        });
    } else {
        setFormData({
            maID: generateId(),
            ten: '',
            mota: '',
            documents: [],
            questions: []
        });
    }
  }, [topic]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleAddDocument = () => {
    setFormData(prev => ({
        ...prev,
        documents: [
            ...prev.documents,
            { maID: generateId(), tieude: '', loaitailieu: 'PDF', kichthuoc: 0 }
        ]
    }));
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

  const handleAddQuestion = () => {
    setFormData(prev => ({
        ...prev,
        questions: [
            ...prev.questions,
            { maID: generateId(), ten: '', diem: 1, answersDtos: [] } // defaults
        ]
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
    onSave(formData);
  };

  return (
    <div className="tm-form-container">
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label htmlFor="ten">Tên chuyên đề *</label>
          <input
            type="text"
            id="ten"
            name="ten"
            className="form-control"
            value={formData.ten}
            onChange={handleInputChange}
            required
            placeholder="Nhập tên chuyên đề..."
          />
        </div>

        <div className="form-group">
          <label htmlFor="mota">Mô tả</label>
          <textarea
            id="mota"
            name="mota"
            className="form-control"
            value={formData.mota}
            onChange={handleInputChange}
            placeholder="Nhập mô tả chuyên đề..."
          />
        </div>

        {/* Documents Section */}
        <div className="sub-section">
          <div className="sub-section-header">
            <h3 className="sub-section-title">Tài liệu</h3>
            <button type="button" className="btn btn-secondary" onClick={handleAddDocument}>
              + Thêm tài liệu
            </button>
          </div>
          
          {formData.documents.length === 0 ? (
            <div className="empty-state" style={{ padding: '2rem' }}>
              <p>Chưa có tài liệu nào.</p>
            </div>
          ) : (
            <div className="item-list">
              {formData.documents.map((doc, index) => (
                <div key={doc.maID} className="item-card">
                  <button 
                    type="button" 
                    className="btn-icon item-remove-btn" 
                    onClick={() => handleRemoveDocument(index)}
                    title="Xóa tài liệu"
                    style={{ color: 'var(--danger)' }}
                  >
                     <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>
                  </button>
                  <div className="item-row">
                    <div className="form-group" style={{ marginBottom: 0 }}>
                      <label>Tiêu đề</label>
                      <input
                        type="text"
                        className="form-control"
                        value={doc.tieude || ''}
                        onChange={(e) => handleDocumentChange(index, 'tieude', e.target.value)}
                        required
                      />
                    </div>
                    <div className="form-group" style={{ marginBottom: 0 }}>
                      <label>Loại tài liệu</label>
                      <select 
                        className="form-control"
                        value={doc.loaitailieu || 'PDF'}
                        onChange={(e) => handleDocumentChange(index, 'loaitailieu', e.target.value)}
                      >
                        <option value="PDF">PDF</option>
                        <option value="Word">Word</option>
                        <option value="Video">Video</option>
                      </select>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Questions Section */}
        <div className="sub-section">
          <div className="sub-section-header">
            <h3 className="sub-section-title">Câu hỏi</h3>
            <button type="button" className="btn btn-secondary" onClick={handleAddQuestion}>
              + Thêm câu hỏi
            </button>
          </div>

          {formData.questions.length === 0 ? (
            <div className="empty-state" style={{ padding: '2rem' }}>
              <p>Chưa có câu hỏi nào.</p>
            </div>
          ) : (
            <div className="item-list">
              {formData.questions.map((q, index) => (
                <div key={q.maID} className="item-card">
                  <button 
                    type="button" 
                    className="btn-icon item-remove-btn" 
                    onClick={() => handleRemoveQuestion(index)}
                    title="Xóa câu hỏi"
                    style={{ color: 'var(--danger)' }}
                  >
                     <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>
                  </button>
                  <div className="item-row" style={{ marginBottom: '1rem' }}>
                    <div className="form-group" style={{ marginBottom: 0 }}>
                      <label>Câu hỏi</label>
                      <input
                        type="text"
                        className="form-control"
                        value={q.ten || ''}
                        onChange={(e) => handleQuestionChange(index, 'ten', e.target.value)}
                        required
                      />
                    </div>
                    <div className="form-group" style={{ marginBottom: 0 }}>
                      <label>Điểm</label>
                      <input
                        type="number"
                        step="0.5"
                        min="0"
                        className="form-control"
                        value={q.diem || 0}
                        onChange={(e) => handleQuestionChange(index, 'diem', parseFloat(e.target.value))}
                        required
                      />
                    </div>
                  </div>
                  
                  {/* Answers Section for this question */}
                  <div style={{ background: 'var(--code-bg)', padding: '1rem', borderRadius: '8px' }}>
                    <div className="sub-section-header" style={{ marginBottom: '0.5rem' }}>
                        <h4 style={{ margin: 0, fontSize: '0.9rem', color: 'var(--text-h)' }}>Các đáp án</h4>
                        <button type="button" className="btn btn-secondary" style={{ padding: '0.25rem 0.5rem', fontSize: '0.8rem' }} onClick={() => handleAddAnswer(index)}>
                          + Thêm đáp án
                        </button>
                    </div>
                    
                    {(!q.answersDtos || q.answersDtos.length === 0) ? (
                        <p style={{ fontSize: '0.85rem', color: 'var(--text)' }}>Chưa có đáp án nào.</p>
                    ) : (
                        <div style={{ display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
                            {q.answersDtos.map((a, aIndex) => (
                                <div key={a.maID} style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
                                    <input 
                                      type="radio" 
                                      name={`correct-answer-${q.maID}`}
                                      checked={a.dung}
                                      onChange={() => {
                                        // Uncheck others and check this one
                                        const newQs = [...formData.questions];
                                        newQs[index].answersDtos.forEach(ans => ans.dung = false);
                                        newQs[index].answersDtos[aIndex].dung = true;
                                        setFormData(prev => ({ ...prev, questions: newQs }));
                                      }}
                                      title="Chọn đáp án đúng"
                                    />
                                    <input 
                                      type="text"
                                      className="form-control"
                                      style={{ padding: '0.4rem 0.6rem', flex: 1 }}
                                      value={a.ten || ''}
                                      onChange={(e) => handleAnswerChange(index, aIndex, 'ten', e.target.value)}
                                      placeholder="Nội dung đáp án..."
                                      required
                                    />
                                    <button 
                                      type="button" 
                                      className="btn-icon"
                                      onClick={() => handleRemoveAnswer(index, aIndex)}
                                      title="Xóa đáp án"
                                      style={{ color: 'var(--danger)', padding: '0.25rem' }}
                                    >
                                       <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2"><line x1="18" y1="6" x2="6" y2="18"></line><line x1="6" y1="6" x2="18" y2="18"></line></svg>
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

        <div className="form-actions">
          <button type="button" className="btn btn-secondary" onClick={onCancel}>
            Hủy
          </button>
          <button type="submit" className="btn btn-primary">
            Lưu chuyên đề
          </button>
        </div>
      </form>
    </div>
  );
}
