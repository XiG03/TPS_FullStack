import React from 'react';
import { useParams } from 'react-router-dom';
import { useScheduleDetail } from '../hooks/useScheduleDetail';
import ScheduleDetailUI from '../components/ScheduleManagement/ScheduleDetailUI';

const ScheduleDetailPage = () => {
    const { id } = useParams();
    const { detail, setDetail, isLoading, error, handleSaveAttendance } = useScheduleDetail(id);

    return (
        <ScheduleDetailUI 
            detail={detail} 
            setDetail={setDetail}
            isLoading={isLoading} 
            error={error} 
            onSaveAttendance={handleSaveAttendance}
        />
    );
};

export default ScheduleDetailPage;
