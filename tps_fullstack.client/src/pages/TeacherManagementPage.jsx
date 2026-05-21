import React from 'react';
import { useTeacher } from '../hooks/useTeacher';
import TeacherManagementUI from '../components/TeacherManagement/TeacherManagementUI';
import TeacherFormModal from '../components/TeacherManagement/TeacherFormModal';
import { useState } from 'react';

const TeacherManagementPage = () => {
    const { teachers, isLoading, error, handleAddTeacher, handleUpdateTeacher, handleDeleteTeacher } = useTeacher();
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingTeacher, setEditingTeacher] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    const openModal = (teacher = null) => {
        setEditingTeacher(teacher);
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setTimeout(() => setEditingTeacher(null), 300); // wait for animation
    };

    const submitTeacher = async (data) => {
        setIsSubmitting(true);
        let success = false;
        
        if (editingTeacher) {
            success = await handleUpdateTeacher(editingTeacher.MaID, data);
        } else {
            success = await handleAddTeacher(data);
        }

        setIsSubmitting(false);
        if (success) {
            closeModal();
        } else {
            alert("Đã xảy ra lỗi khi lưu thông tin giáo viên.");
        }
    };

    const handleEditTeacher = (teacher) => {
        openModal(teacher);
    };

    const confirmDelete = (id) => {
        if (window.confirm("Bạn có chắc chắn muốn xoá giáo viên này?")) {
            handleDeleteTeacher(id);
            alert("Đã gửi yêu cầu xoá (đang giả lập).");
        }
    };

    return (
        <>
            <TeacherManagementUI 
                teachers={teachers}
                isLoading={isLoading}
                error={error}
                onAddTeacher={() => openModal(null)}
                onEditTeacher={handleEditTeacher}
                onDeleteTeacher={confirmDelete}
            />
            
            <TeacherFormModal 
                isOpen={isModalOpen} 
                onClose={closeModal} 
                onSubmit={submitTeacher} 
                isLoading={isSubmitting} 
                initialData={editingTeacher}
            />
        </>
    );
};

export default TeacherManagementPage;
