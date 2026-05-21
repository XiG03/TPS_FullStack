import React, { useState } from 'react';
import { useStudent } from '../hooks/useStudent';
import StudentManagementUI from '../components/StudentManagement/StudentManagementUI';
import StudentFormModal from '../components/StudentManagement/StudentFormModal';

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
        let success = false;
        
        if (editingStudent) {
            success = await handleUpdateStudent(editingStudent.maID, formData);
        } else {
            success = await handleAddStudent(formData);
        }
        
        setIsSaving(false);
        if (success) {
            closeModal();
            alert(editingStudent ? "Cập nhật thành công!" : "Thêm mới thành công!");
        } else {
            alert("Đã có lỗi xảy ra. Vui lòng thử lại.");
        }
    };

    const confirmDelete = async (id) => {
        if (window.confirm("Bạn có chắc chắn muốn xoá học viên này?")) {
            const success = await handleDeleteStudent(id);
            if (success) {
                alert("Đã xoá thành công.");
            } else {
                alert("Đã có lỗi xảy ra khi xoá.");
            }
        }
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
