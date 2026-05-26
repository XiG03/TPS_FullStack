import { useState, useEffect, useCallback } from 'react';
import { startOfWeek, addDays } from 'date-fns';
import { getSchedules, createSchedule, updateSchedule, deleteSchedule } from '../services/scheduleService';

export const useSchedule = () => {
    const [schedules, setSchedules] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [currentDate, setCurrentDate] = useState(new Date());

    const startOfCurrentWeek = startOfWeek(currentDate, { weekStartsOn: 1 });
    const weekDays = Array.from({ length: 7 }).map((_, index) => addDays(startOfCurrentWeek, index));

    const nextWeek = () => setCurrentDate((prev) => addDays(prev, 7));
    const prevWeek = () => setCurrentDate((prev) => addDays(prev, -7));
    const goToday = () => setCurrentDate(new Date());

    const fetchSchedules = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getSchedules();
            setSchedules(Array.isArray(res.data) ? res.data : []);
        } catch (err) {
            setError('Lỗi khi tải danh sách lịch dạy học.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchSchedules();
    }, [fetchSchedules]);

    const handleAddSchedule = async (data) => {
        try {
            await createSchedule(data);
            await fetchSchedules();
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleUpdateSchedule = async (data) => {
        try {
            await updateSchedule(data);
            await fetchSchedules();
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleDeleteSchedule = async (id) => {
        try {
            await deleteSchedule(id);
            setSchedules((prev) => prev.filter((schedule) => schedule.lichhocID !== id));
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
