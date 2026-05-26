import { useState } from 'react';
import { useSchedule } from '../hooks/useSchedule';
import ScheduleManagementUI from '../components/ScheduleManagement/ScheduleManagementUI';
import ScheduleFormModal from '../components/ScheduleManagement/ScheduleFormModal';
import ScheduleDetailModal from '../components/ScheduleManagement/ScheduleDetailModal';

const ScheduleManagementPage = () => {
    const {
        schedules,
        isLoading,
        error,
        handleAddSchedule,
        handleUpdateSchedule,
        handleDeleteSchedule,
        weekDays,
        currentDate,
        nextWeek,
        prevWeek,
        goToday
    } = useSchedule();

    const [isModalOpen, setIsModalOpen] = useState(false);
    const [editingSchedule, setEditingSchedule] = useState(null);
    const [viewingSchedule, setViewingSchedule] = useState(null);
    const [isSaving, setIsSaving] = useState(false);

    const openAddModal = () => {
        setEditingSchedule(null);
        setIsModalOpen(true);
    };

    const openEditModal = (schedule) => {
        setEditingSchedule(schedule);
        setIsModalOpen(true);
    };

    const openDetailModal = (schedule) => {
        setViewingSchedule(schedule);
    };

    const closeDetailModal = () => {
        setViewingSchedule(null);
    };

    const closeModal = () => {
        setIsModalOpen(false);
        setEditingSchedule(null);
    };

    const handleSubmit = async (formData) => {
        setIsSaving(true);
        const success = editingSchedule
            ? await handleUpdateSchedule(formData)
            : await handleAddSchedule(formData);

        setIsSaving(false);
        if (success) {
            closeModal();
            alert(editingSchedule ? 'Cập nhật lịch dạy học thành công!' : 'Thêm mới lịch dạy học thành công!');
        } else {
            alert('Đã có lỗi xảy ra. Vui lòng thử lại.');
        }
    };

    const confirmDelete = async (id) => {
        if (!window.confirm('Bạn có chắc chắn muốn xóa lịch dạy học này?')) return;

        const success = await handleDeleteSchedule(id);
        alert(success ? 'Đã xóa lịch dạy học thành công.' : 'Đã có lỗi xảy ra khi xóa.');
    };

    return (
        <>
            <ScheduleManagementUI
                schedules={schedules}
                isLoading={isLoading}
                error={error}
                onAddSchedule={openAddModal}
                onViewSchedule={openDetailModal}
                onEditSchedule={openEditModal}
                onDeleteSchedule={confirmDelete}
                weekDays={weekDays}
                currentDate={currentDate}
                nextWeek={nextWeek}
                prevWeek={prevWeek}
                goToday={goToday}
            />

            <ScheduleFormModal
                isOpen={isModalOpen}
                onClose={closeModal}
                onSubmit={handleSubmit}
                isLoading={isSaving}
                initialData={editingSchedule}
            />

            <ScheduleDetailModal
                schedule={viewingSchedule}
                onClose={closeDetailModal}
                onEditSchedule={openEditModal}
                onDeleteSchedule={confirmDelete}
            />
        </>
    );
};

export default ScheduleManagementPage;
