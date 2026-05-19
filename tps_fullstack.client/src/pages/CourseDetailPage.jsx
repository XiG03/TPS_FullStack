import React from 'react';
import { useParams } from 'react-router-dom';
import { useCourseDetail } from '../hooks/useCourseDetail';
import CourseDetailUI from '../components/CourseManagement/CourseDetailUI';

const CourseDetailPage = () => {
    const { id } = useParams();
    const { detail, isLoading, error } = useCourseDetail(id);

    return (
        <CourseDetailUI 
            detail={detail} 
            isLoading={isLoading} 
            error={error} 
        />
    );
};

export default CourseDetailPage;
