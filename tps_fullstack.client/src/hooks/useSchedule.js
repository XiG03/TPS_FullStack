import { useState, useEffect, useCallback } from 'react';
import { startOfWeek, addDays, format, isSameDay } from 'date-fns';
import { getSchedules, createSchedule, updateSchedule, deleteSchedule } from '../services/scheduleService';

export const useSchedule = () => {
    const [schedules, setSchedules] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [currentDate, setCurrentDate] = useState(new Date());

    // Generate week days (Monday - Sunday) based on currentDate
    // date-fns startOfWeek with weekStartsOn: 1 means Monday
    const startOfCurrentWeek = startOfWeek(currentDate, { weekStartsOn: 1 });
    const weekDays = Array.from({ length: 7 }).map((_, i) => addDays(startOfCurrentWeek, i));

    const nextWeek = () => setCurrentDate(addDays(currentDate, 7));
    const prevWeek = () => setCurrentDate(addDays(currentDate, -7));
    const goToday = () => setCurrentDate(new Date());

    const fetchSchedules = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getSchedules();
            setSchedules(res.data);
        } catch (err) {
            setError('Lỗi khi tải danh sách lịch học.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchSchedules();
    }, [fetchSchedules]);

    const handleAddSchedule = async (data) => {
        try {
            const res = await createSchedule(data);
            setSchedules(prev => [...prev, { ...data, MaID: 'SCH_NEW' }]);
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleUpdateSchedule = async (data) => {
        try {
            await updateSchedule(data);
            setSchedules(prev => prev.map(s => s.MaID === data.MaID ? { ...s, ...data } : s));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleDeleteSchedule = async (id) => {
        try {
            await deleteSchedule(id);
            setSchedules(prev => prev.filter(s => s.MaID !== id));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        schedules,
        isLoading,
        error,
        weekDays,
        currentDate,
        nextWeek,
        prevWeek,
        goToday,
        fetchSchedules,
        handleAddSchedule,
        handleUpdateSchedule,
        handleDeleteSchedule
    };
};
