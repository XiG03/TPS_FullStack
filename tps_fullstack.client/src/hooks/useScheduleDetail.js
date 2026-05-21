import { useState, useEffect, useCallback } from 'react';
import { getScheduleDetail, saveAttendance } from '../services/scheduleService';

export const useScheduleDetail = (id) => {
    const [detail, setDetail] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchDetail = useCallback(async () => {
        if (!id) return;
        setIsLoading(true);
        setError(null);
        try {
            const res = await getScheduleDetail(id);
            setDetail(res.data);
        } catch (err) {
            setError('Lỗi khi tải thông tin chi tiết điểm danh.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, [id]);

    useEffect(() => {
        fetchDetail();
    }, [fetchDetail]);

    const handleSaveAttendance = async (attendanceData) => {
        try {
            await saveAttendance(id, attendanceData);
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        detail,
        setDetail, // allow local optimistic updates
        isLoading,
        error,
        refetch: fetchDetail,
        handleSaveAttendance
    };
};
