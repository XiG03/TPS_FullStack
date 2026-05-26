import { useParams } from 'react-router-dom';
import { useTopicDetail } from '../hooks/useTopicDetail';
import TopicDetailUI from '../components/TopicManagement/TopicDetailUI';

const TopicDetailPage = () => {
    const { id } = useParams();
    const { detail, isLoading, error } = useTopicDetail(id);

    return (
        <TopicDetailUI 
            detail={detail} 
            isLoading={isLoading} 
            error={error} 
        />
    );
};

export default TopicDetailPage;
