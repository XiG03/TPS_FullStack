import React from 'react';
import { useTopic } from '../hooks/useTopic';
import TopicManagementUI from '../components/TopicManagement/TopicManagementUI';

const TopicManagementPage = () => {
    const { topics, isLoading, error, handleDeleteTopic } = useTopic();

    const confirmDelete = async (id) => {
        if (window.confirm("Bạn có chắc chắn muốn xoá chuyên đề này? Hành động này sẽ xoá toàn bộ tài liệu và ngân hàng câu hỏi đi kèm.")) {
            const success = await handleDeleteTopic(id);
            if (success) {
                alert("Đã xoá thành công.");
            } else {
                alert("Đã có lỗi xảy ra khi xoá.");
            }
        }
    };

    return (
        <TopicManagementUI 
            topics={topics}
            isLoading={isLoading}
            error={error}
            onDeleteTopic={confirmDelete}
        />
    );
};

export default TopicManagementPage;
