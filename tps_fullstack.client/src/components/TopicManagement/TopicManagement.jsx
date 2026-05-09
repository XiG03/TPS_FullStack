import React, { useState, useEffect } from 'react';
import TopicList from './TopicList';
import TopicForm from './TopicForm';
import TopicDetail from './TopicDetail';
import './TopicManagement.css';

export default function TopicManagement() {
  const [topics, setTopics] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentView, setCurrentView] = useState('list'); // 'list' | 'form' | 'detail'
  const [editingTopic, setEditingTopic] = useState(null); // null for create

  // Fetch all topics
  const fetchTopics = async () => {
    try {
      setIsLoading(true);
      const response = await fetch('/api/admin/topic/getall');
      if (!response.ok) throw new Error('Failed to fetch topics');
      const data = await response.json();
      setTopics(data);
    } catch (err) {
      setError(err.message);
      console.error(err);
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    fetchTopics();
  }, []);

  // Fetch full details
  const fetchTopicDetail = async (id) => {
    const response = await fetch(`/api/admin/topic/${id}`);
    if (!response.ok) throw new Error('Failed to fetch topic details');
    return await response.json();
  };

  // Handle Edit button click
  const handleEditClick = async (topicSummary) => {
    try {
      const topicDetail = await fetchTopicDetail(topicSummary.maID);
      setEditingTopic(topicDetail);
      setCurrentView('form');
    } catch (err) {
      alert('Không thể tải chi tiết chuyên đề: ' + err.message);
    }
  };

  // Handle View Details click
  const handleViewClick = async (topicSummary) => {
    try {
      const topicDetail = await fetchTopicDetail(topicSummary.maID);
      setEditingTopic(topicDetail);
      setCurrentView('detail');
    } catch (err) {
      alert('Không thể tải chi tiết chuyên đề: ' + err.message);
    }
  };

  // Handle Delete button click
  const handleDeleteClick = async (id) => {
    if (!window.confirm('Bạn có chắc chắn muốn xóa chuyên đề này?')) return;

    try {
      const response = await fetch(`/api/admin/topic/${id}`, {
        method: 'DELETE',
      });
      if (!response.ok) throw new Error('Failed to delete topic');
      
      // Update local state
      setTopics(topics.filter(t => t.maID !== id));
      if (editingTopic && editingTopic.maID === id) {
          setCurrentView('list');
      }
    } catch (err) {
      alert('Lỗi khi xóa chuyên đề: ' + err.message);
    }
  };

  // Handle Create New button click
  const handleCreateNewClick = () => {
    setEditingTopic(null);
    setCurrentView('form');
  };

  // Handle Save (Create or Update)
  const handleSave = async (formData) => {
    try {
      const isEdit = !!editingTopic && currentView !== 'form' /* if you want to handle specific things */ && editingTopic.maID === formData.maID;
      
      // Map frontend model to backend DTO
      let payload;
      let method = '';
      
      if (isEdit || editingTopic) { // Assume if editingTopic is set, we are updating. (Since Create sets it to null)
        method = 'PUT';
        payload = {
            maID: formData.maID,
            ten: formData.ten,
            mota: formData.mota,
            documentsDto: formData.documents.map(d => ({
                maID: d.maID,
                tieude: d.tieude,
                loaitailieu: d.loaitailieu,
                kichthuoc: d.kichthuoc || 0
            })),
            questionsDtos: formData.questions.map(q => ({
                maID: q.maID,
                ten: q.ten,
                diem: q.diem,
                answersDtos: q.answersDtos || []
            }))
        };
      } else {
        method = 'POST';
        payload = {
            maID: formData.maID,
            ten: formData.ten,
            mota: formData.mota,
            chuyende_TailieuDtos: formData.documents.map(d => ({
                maID: d.maID,
                chuyendeID: formData.maID,
                tieude: d.tieude,
                loaitailieu: d.loaitailieu,
                kichthuoc: d.kichthuoc || 0
            })),
            chuyende_CauhoiDtos: formData.questions.map(q => ({
                maID: q.maID,
                chuyendeID: formData.maID,
                ten: q.ten,
                diem: q.diem,
                chuyende_DapanDtos: q.answersDtos || []
            }))
        };
      }

      const response = await fetch('/api/admin/topic', {
        method: method,
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
      });

      if (!response.ok) {
        const errorData = await response.text();
        throw new Error(errorData || 'Failed to save topic');
      }

      // Refresh list and go back
      await fetchTopics();
      setCurrentView('list');
      
    } catch (err) {
      alert('Lỗi khi lưu chuyên đề: ' + err.message);
      console.error(err);
    }
  };

  return (
    <div className="topic-management-container">
      <div className="tm-header">
        <div>
          <h1>Quản lý chuyên đề</h1>
          <p className="tm-header-subtitle">Tạo, chỉnh sửa và quản lý các chuyên đề học tập.</p>
        </div>
        {currentView === 'list' && (
          <button className="btn btn-primary" onClick={handleCreateNewClick}>
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><line x1="12" y1="5" x2="12" y2="19"></line><line x1="5" y1="12" x2="19" y2="12"></line></svg>
            Thêm chuyên đề
          </button>
        )}
      </div>

      {isLoading && currentView === 'list' ? (
        <div className="loading-state">
          <div className="empty-icon">⏳</div>
          <h2>Đang tải dữ liệu...</h2>
        </div>
      ) : error ? (
        <div className="empty-state" style={{ color: 'var(--danger)' }}>
          <div className="empty-icon">⚠️</div>
          <h2>Đã có lỗi xảy ra</h2>
          <p>{error}</p>
          <button className="btn btn-secondary" onClick={fetchTopics} style={{ marginTop: '1rem' }}>
            Thử lại
          </button>
        </div>
      ) : currentView === 'list' ? (
        <TopicList 
          topics={topics} 
          onView={handleViewClick}
          onEdit={handleEditClick} 
          onDelete={handleDeleteClick} 
          onCreateNew={handleCreateNewClick}
        />
      ) : currentView === 'detail' ? (
        <TopicDetail 
          topic={editingTopic}
          onBack={() => setCurrentView('list')}
          onEdit={(t) => setCurrentView('form')}
        />
      ) : (
        <TopicForm 
          topic={editingTopic} 
          onSave={handleSave} 
          onCancel={() => setCurrentView('list')} 
        />
      )}
    </div>
  );
}
