import React from 'react';
import { useParams } from 'react-router-dom';
import { useTeacherDetail } from '../hooks/useTeacherDetail';
import TeacherDetailUI from '../components/TeacherManagement/TeacherDetailUI';

const TeacherDetailPage = () => {
    const { id } = useParams();
    const { detail, isLoading, error } = useTeacherDetail(id);

    return (
        <TeacherDetailUI 
            detail={detail} 
            isLoading={isLoading} 
            error={error} 
        />
    );
};

export default TeacherDetailPage;
