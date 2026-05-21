import { useState, useEffect, useCallback } from 'react';
import { getStudentDetail } from '../services/studentService';

export const useStudentDetail = (id) => {
    const [detail, setDetail] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchDetail = useCallback(async () => {
        if (!id) return;
        setIsLoading(true);
        setError(null);
        try {
            const res = await getStudentDetail(id);
            setDetail(res.data);
        } catch (err) {
            setError('Lỗi khi tải thông tin chi tiết học viên.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, [id]);

    useEffect(() => {
        fetchDetail();
    }, [fetchDetail]);

    return {
        detail,
        isLoading,
        error,
        refetch: fetchDetail
    };
};
