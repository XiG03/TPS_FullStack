import { useEffect, useState } from 'react';
import { useTeacher } from '../hooks/useTeacher';
import { getTopics } from '../services/topicService';
import { getTeacherDetail } from '../services/teacherService';
import TeacherManagementUI from '../components/TeacherManagement/TeacherManagementUI';
import TeacherFormModal from '../components/TeacherManagement/TeacherFormModal';

const TeacherManagementPage = () => {
    const { teachers, isLoading, error, handleAddTeacher, handleUpdateTeacher, handleDeleteTeacher } = useTeacher();
    const [topicOptions, setTopicOptions] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingTeacher, setEditingTeacher] = useState(null);
    const [isSubmitting, setIsSubmitting] = useState(false);

    useEffect(() => {
        const fetchTopics = async () => {
            try {
                const res = await getTopics();
                setTopicOptions(Array.isArray(res.data) ? res.data : []);
            } catch (err) {
                console.error(err);
                setTopicOptions([]);
            }
        };

        fetchTopics();
    }, []);

    const openModal = async (teacher = null) => {
        if (!teacher) {
            setEditingTeacher(null);
            setIsModalOpen(true);
            return;
        }

        setIsModalOpen(true);
        setEditingTeacher(teacher);
        try {
            const res = await getTeacherDetail(teacher.maID);
            setEditingTeacher(res.data || teacher);
        } catch (err) {
            console.error(err);
        }
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setEditingTeacher(null);
    };

    const submitTeacher = async (data) => {
        setIsSubmitting(true);
        const success = editingTeacher
            ? await handleUpdateTeacher(data)
            : await handleAddTeacher(data);

        setIsSubmitting(false);
        if (success) {
            closeModal();
        } else {
            alert('Đã xảy ra lỗi khi lưu thông tin giảng viên.');
        }
    };

    const confirmDelete = async (id) => {
        if (!window.confirm('Bạn có chắc chắn muốn xóa giảng viên này?')) return;

        const success = await handleDeleteTeacher(id);
        alert(success ? 'Đã xóa giảng viên thành công.' : 'Đã xảy ra lỗi khi xóa giảng viên.');
    };

    return (
        <>
            <TeacherManagementUI
                teachers={teachers}
                isLoading={isLoading}
                error={error}
                topicOptions={topicOptions}
                onAddTeacher={() => openModal(null)}
                onEditTeacher={openModal}
                onDeleteTeacher={confirmDelete}
            />

            <TeacherFormModal
                isOpen={isModalOpen}
                onClose={closeModal}
                onSubmit={submitTeacher}
                isLoading={isSubmitting}
                initialData={editingTeacher}
                topicOptions={topicOptions}
            />
        </>
    );
};

export default TeacherManagementPage;
