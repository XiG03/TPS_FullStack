import { useTopic } from '../hooks/useTopic';
import TopicManagementUI from '../components/TopicManagement/TopicManagementUI';

const TopicManagementPage = () => {
    const { topics, isLoading, error, handleDeleteTopic } = useTopic();

    const confirmDelete = async (id) => {
        if (window.confirm('Bạn có chắc chắn muốn xóa chuyên đề này? Hành động này sẽ xóa toàn bộ tài liệu và ngân hàng câu hỏi đi kèm.')) {
            const success = await handleDeleteTopic(id);
            alert(success ? 'Đã xóa thành công.' : 'Đã có lỗi xảy ra khi xóa.');
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
