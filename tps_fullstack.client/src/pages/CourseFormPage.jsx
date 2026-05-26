import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { useCourseDetail } from '../hooks/useCourseDetail';
import { createCourse, updateCourse } from '../services/courseService';
import CourseFormUI from '../components/CourseManagement/CourseFormUI';

const CourseFormPage = () => {
    const { id } = useParams();
    const isEditMode = Boolean(id);
    const navigate = useNavigate();
    const { detail, isLoading: isLoadingDetail, error: detailError } = useCourseDetail(isEditMode ? id : null);
    const [isSaving, setIsSaving] = useState(false);

    const handleSave = async (formData) => {
        setIsSaving(true);
        try {
            if (isEditMode) {
                await updateCourse({
                    ...formData,
                    KhoahocID: formData.KhoahocID || id
                });
                alert('Đã cập nhật cấu hình khóa học thành công!');
            } else {
                await createCourse(formData);
                alert('Khởi tạo khóa học thành công!');
            }
            navigate('/courses');
        } catch (error) {
            alert(`Có lỗi xảy ra: ${error.message}`);
        } finally {
            setIsSaving(false);
        }
    };

    if (isEditMode && detailError) {
        return (
            <div className="flex-1 flex items-center justify-center font-body text-error min-h-[calc(100vh-60px)]">
                {detailError}
            </div>
        );
    }

    if (isEditMode && (isLoadingDetail || !detail)) {
        return (
            <div className="flex-1 flex items-center justify-center font-body text-on-surface-variant min-h-[calc(100vh-60px)]">
                Đang tải dữ liệu khóa học...
            </div>
        );
    }

    return (
        <CourseFormUI
            key={isEditMode ? detail.khoahocID : 'new-course'}
            initialData={isEditMode ? detail : null}
            onSave={handleSave}
            isSaving={isSaving}
        />
    );
};

export default CourseFormPage;
