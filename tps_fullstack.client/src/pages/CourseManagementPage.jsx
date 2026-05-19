import React from 'react';
import { useCourse } from '../hooks/useCourse';
import CourseManagementUI from '../components/CourseManagement/CourseManagementUI';

const CourseManagementPage = () => {
    const { courses, isLoading, error, handleDeleteCourse } = useCourse();

    const confirmDelete = async (id) => {
        if (window.confirm("Bạn có chắc chắn muốn xoá khóa học này? Hành động này sẽ gỡ toàn bộ cấu hình bài thi và lịch học liên quan.")) {
            const success = await handleDeleteCourse(id);
            if (success) {
                alert("Đã xoá khóa học thành công.");
            } else {
                alert("Đã có lỗi xảy ra khi xoá.");
            }
        }
    };

    return (
        <CourseManagementUI 
            courses={courses}
            isLoading={isLoading}
            error={error}
            onDeleteCourse={confirmDelete}
        />
    );
};

export default CourseManagementPage;
