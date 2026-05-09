import React from 'react';

export default function TopicList({ topics, onView, onEdit, onDelete, onCreateNew }) {
  if (!topics || topics.length === 0) {
    return (
      <div className="tm-card loading-state">
        <div className="empty-icon">📚</div>
        <h2>Chưa có chuyên đề nào</h2>
        <p>Bắt đầu bằng cách tạo chuyên đề đầu tiên của bạn.</p>
        <button className="btn btn-primary" onClick={onCreateNew} style={{ marginTop: '1rem' }}>
          <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          Thêm chuyên đề mới
        </button>
      </div>
    );
  }

  return (
    <div className="tm-card">
      <div className="tm-table-container">
        <table className="tm-table">
          <thead>
            <tr>
              <th>Tên chuyên đề</th>
              <th>Mô tả</th>
              <th style={{ width: '150px', textAlign: 'right' }}>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {topics.map((topic) => (
              <tr key={topic.maID}>
                <td>
                  <div className="topic-title">{topic.ten}</div>
                </td>
                <td>
                  <div className="topic-desc" title={topic.mota}>
                    {topic.mota || 'Không có mô tả'}
                  </div>
                </td>
                <td style={{ textAlign: 'right' }}>
                  <div className="action-cell" style={{ justifyContent: 'flex-end' }}>
                    <button 
                      className="btn-icon" 
                      onClick={() => onView(topic)}
                      title="Xem chi tiết"
                    >
                      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
                        <circle cx="12" cy="12" r="3"></circle>
                      </svg>
                    </button>
                    <button 
                      className="btn-icon" 
                      onClick={() => onEdit(topic)}
                      title="Chỉnh sửa"
                    >
                      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                        <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
                      </svg>
                    </button>
                    <button 
                      className="btn-icon" 
                      onClick={() => onDelete(topic.maID)}
                      title="Xóa"
                      style={{ color: 'var(--danger)' }}
                    >
                      <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <polyline points="3 6 5 6 21 6"></polyline>
                        <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                        <line x1="10" y1="11" x2="10" y2="17"></line>
                        <line x1="14" y1="11" x2="14" y2="17"></line>
                      </svg>
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
