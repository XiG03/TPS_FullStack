import React, { useState } from 'react';
import { useSchedule } from '../hooks/useSchedule';
import ScheduleManagementUI from '../components/ScheduleManagement/ScheduleManagementUI';
import ScheduleFormModal from '../components/ScheduleManagement/ScheduleFormModal';

const ScheduleManagementPage = () => {
    const { 
        schedules, isLoading, error, handleAddSchedule, handleUpdateSchedule,
        weekDays, currentDate, nextWeek, prevWeek, goToday 
    } = useSchedule();
    
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isSaving, setIsSaving] = useState(false);

    const openAddModal = () => {
        setIsModalOpen(true);
    };

    const closeModal = () => {
        setIsModalOpen(false);
    };

    const handleSubmit = async (formData) => {
        setIsSaving(true);
        let success = false;
        
        success = await handleAddSchedule(formData);
        
        setIsSaving(false);
        if (success) {
            closeModal();
            alert("Thêm mới lịch học thành công!");
        } else {
            alert("Đã có lỗi xảy ra. Vui lòng thử lại.");
        }
    };

    return (
        <>
            <ScheduleManagementUI 
                schedules={schedules}
                isLoading={isLoading}
                error={error}
                onAddSchedule={openAddModal}
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
            />
        </>
    );
};

export default ScheduleManagementPage;
