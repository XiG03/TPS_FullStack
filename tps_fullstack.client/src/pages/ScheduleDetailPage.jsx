import { useParams } from 'react-router-dom';
import { useScheduleDetail } from '../hooks/useScheduleDetail';
import ScheduleDetailUI from '../components/ScheduleManagement/ScheduleDetailUI';

const ScheduleDetailPage = () => {
    const { id } = useParams();
    const { detail, isLoading, error } = useScheduleDetail(id);

    return (
        <ScheduleDetailUI
            detail={detail}
            isLoading={isLoading}
            error={error}
        />
    );
};

export default ScheduleDetailPage;
