import { useState } from 'react';
import StudentFormModal from '../components/StudentManagement/StudentFormModal';
import StudentManagementUI from '../components/StudentManagement/StudentManagementUI';
import { useStudent } from '../hooks/useStudent';

const StudentManagementPage = () => {
    const { students, isLoading, error, handleAddStudent, handleUpdateStudent, handleDeleteStudent } = useStudent();

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingStudent, setEditingStudent] = useState(null);
    const [isSaving, setIsSaving] = useState(false);

    const openAddModal = () => {
        setEditingStudent(null);
        setIsModalOpen(true);
    };

    const openEditModal = (student) => {
        setEditingStudent(student);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setEditingStudent(null);
    };

    const handleSubmit = async (formData) => {
        setIsSaving(true);
        const success = editingStudent
            ? await handleUpdateStudent(editingStudent.hocvienID, formData)
            : await handleAddStudent(formData);

        setIsSaving(false);
        if (success) {
            closeModal();
            alert(editingStudent ? 'Cập nhật học viên thành công!' : 'Thêm mới học viên thành công!');
        } else {
            alert('Đã có lỗi xảy ra. Vui lòng thử lại.');
        }
    };

    const confirmDelete = async (id) => {
        if (!window.confirm('Bạn có chắc chắn muốn xóa học viên này?')) return;

        const success = await handleDeleteStudent(id);
        alert(success ? 'Đã xóa học viên thành công.' : 'Đã có lỗi xảy ra khi xóa.');
    };

    return (
        <>
            <StudentManagementUI
                students={students}
                isLoading={isLoading}
                error={error}
                onAddStudent={openAddModal}
                onEditStudent={openEditModal}
                onDeleteStudent={confirmDelete}
            />

            <StudentFormModal
                isOpen={isModalOpen}
                onClose={closeModal}
                onSubmit={handleSubmit}
                isLoading={isSaving}
                initialData={editingStudent}
            />
        </>
    );
};

export default StudentManagementPage;
