import { useState, useEffect, useCallback } from 'react';
import { getScheduleDetail } from '../services/scheduleService';

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
            setError('Lỗi khi tải thông tin chi tiết lịch dạy học.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, [id]);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchDetail();
    }, [fetchDetail]);

    return {
        detail,
        isLoading,
        error,
        refetch: fetchDetail
    };
};
