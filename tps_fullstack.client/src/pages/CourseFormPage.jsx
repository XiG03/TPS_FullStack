import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useCourseDetail } from '../hooks/useCourseDetail';
import { createCourse, updateCourse } from '../services/courseService';
import CourseFormUI from '../components/CourseManagement/CourseFormUI';

const CourseFormPage = () => {
    const { id } = useParams();
    const isEditMode = !!id;
    const navigate = useNavigate();
    
    const { detail, isLoading: isLoadingDetail } = useCourseDetail(isEditMode ? id : null);
    
    const [isSaving, setIsSaving] = useState(false);

    const handleSave = async (formData) => {
        setIsSaving(true);
        try {
            if (isEditMode) {
                await updateCourse(formData);
                alert('Đã cập nhật cấu hình khóa học thành công!');
            } else {
                await createCourse(formData);
                alert('Khởi tạo khóa học thành công!');
            }
            navigate('/courses');
        } catch (error) {
            alert('Có lỗi xảy ra: ' + error.message);
        } finally {
            setIsSaving(false);
        }
    };

    if (isEditMode && isLoadingDetail) {
        return <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-[calc(100vh-60px)]">Đang tải dữ liệu khóa học...</div>;
    }

    return (
        <CourseFormUI 
            initialData={isEditMode ? detail : null}
            onSave={handleSave}
            isSaving={isSaving}
        />
    );
};

export default CourseFormPage;
