import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useTopicDetail } from '../hooks/useTopicDetail';
import { createTopic, updateTopic } from '../services/topicService';
import TopicFormUI from '../components/TopicManagement/TopicFormUI';

const TopicFormPage = () => {
    const { id } = useParams();
    const isEditMode = !!id;
    const navigate = useNavigate();
    
    // Nếu là EditMode, dùng hook lấy thông tin chi tiết
    const { detail, isLoading: isLoadingDetail } = useTopicDetail(isEditMode ? id : null);
    
    const [isSaving, setIsSaving] = useState(false);

    const handleSave = async (formData) => {
        setIsSaving(true);
        try {
            if (isEditMode) {
                await updateTopic(formData);
                alert('Đã cập nhật chuyên đề thành công!');
            } else {
                await createTopic(formData);
                alert('Đã tạo chuyên đề thành công!');
            }
            navigate('/topics');
        } catch (error) {
            alert('Có lỗi xảy ra: ' + error.message);
        } finally {
            setIsSaving(false);
        }
    };

    if (isEditMode && isLoadingDetail) {
        return <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-screen">Đang tải thông tin chuyên đề...</div>;
    }

    return (
        <TopicFormUI 
            initialData={isEditMode ? detail : null}
            onSave={handleSave}
            isSaving={isSaving}
        />
    );
};

export default TopicFormPage;
