import { useCourse } from '../hooks/useCourse';
import CourseManagementUI from '../components/CourseManagement/CourseManagementUI';

const CourseManagementPage = () => {
    const { courses, isLoading, error, handleDeleteCourse } = useCourse();

    const confirmDelete = async (id) => {
        if (window.confirm('Bạn có chắc chắn muốn xóa khóa học này? Hành động này sẽ gỡ cấu hình bài thi và lịch học liên quan.')) {
            const success = await handleDeleteCourse(id);
            alert(success ? 'Đã xóa khóa học thành công.' : 'Đã có lỗi xảy ra khi xóa.');
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
