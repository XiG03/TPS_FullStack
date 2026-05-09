import React from 'react';

export default function TopicDetail({ topic, onBack, onEdit }) {
  if (!topic) return null;

  const documents = topic.documents || topic.documentsDto || topic.chuyende_TailieuDtos || [];
  const questions = topic.questions || topic.questionsDtos || topic.chuyende_CauhoiDtos || [];

  return (
    <div className="tm-form-container" style={{ animation: 'fadeIn 0.3s ease-out' }}>
      <div className="tm-header" style={{ marginBottom: '1rem' }}>
        <div>
          <h2 style={{ fontSize: '1.75rem', marginBottom: '0.5rem', color: 'var(--text-h)' }}>{topic.ten}</h2>
          <p style={{ color: 'var(--text)', whiteSpace: 'pre-wrap' }}>{topic.mota || 'Chưa có mô tả'}</p>
        </div>
        <div style={{ display: 'flex', gap: '0.5rem' }}>
            <button className="btn btn-secondary" onClick={onBack}>Quay lại</button>
            <button className="btn btn-primary" onClick={() => onEdit(topic)}>Sửa chuyên đề</button>
        </div>
      </div>

      <div className="sub-section">
        <h3 className="sub-section-title">Tài liệu ({documents.length})</h3>
        {documents.length === 0 ? (
          <p className="empty-state" style={{ padding: '1rem' }}>Không có tài liệu đính kèm.</p>
        ) : (
          <div className="tm-table-container">
            <table className="tm-table">
              <thead>
                <tr>
                  <th>Tiêu đề</th>
                  <th>Định dạng</th>
                  <th>Kích thước</th>
                </tr>
              </thead>
              <tbody>
                {documents.map((doc) => (
                  <tr key={doc.maID}>
                    <td><strong>{doc.tieude}</strong></td>
                    <td><span className="counter">{doc.loaitailieu}</span></td>
                    <td>{doc.kichthuoc} MB</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>

      <div className="sub-section">
        <h3 className="sub-section-title">Câu hỏi ({questions.length})</h3>
        {questions.length === 0 ? (
          <p className="empty-state" style={{ padding: '1rem' }}>Không có câu hỏi.</p>
        ) : (
          <div className="item-list">
            {questions.map((q, idx) => {
               const answers = q.answers || q.answersDtos || q.chuyende_DapanDtos || [];
               return (
                <div key={q.maID} className="item-card" style={{ padding: '1.5rem' }}>
                    <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: '1rem' }}>
                        <strong style={{ fontSize: '1.1rem', color: 'var(--text-h)' }}>Câu {idx + 1}: {q.ten}</strong>
                        <span style={{ color: 'var(--primary)', fontWeight: '500' }}>{q.diem} điểm</span>
                    </div>
                    
                    <div style={{ paddingLeft: '1rem', borderLeft: '3px solid var(--border)' }}>
                        {answers.length === 0 ? (
                            <p style={{ color: 'var(--text)', fontStyle: 'italic', margin: 0 }}>Chưa có đáp án</p>
                        ) : (
                            <ul style={{ listStyleType: 'none', padding: 0, margin: 0, display: 'flex', flexDirection: 'column', gap: '0.5rem' }}>
                                {answers.map(a => (
                                    <li key={a.maID} style={{ 
                                        display: 'flex', 
                                        alignItems: 'center', 
                                        gap: '0.5rem',
                                        color: a.dung ? 'var(--success)' : 'var(--text-h)',
                                        fontWeight: a.dung ? '600' : '400'
                                    }}>
                                        {a.dung ? (
                                            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"><polyline points="20 6 9 17 4 12"></polyline></svg>
                                        ) : (
                                            <div style={{ width: '18px', height: '18px', borderRadius: '50%', border: '2px solid var(--border)' }}></div>
                                        )}
                                        {a.ten}
                                    </li>
                                ))}
                            </ul>
                        )}
                    </div>
                </div>
               );
            })}
          </div>
        )}
      </div>
    </div>
  );
}
