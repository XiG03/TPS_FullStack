import { useState, useEffect, useCallback } from 'react';
import { getTopicDetail } from '../services/topicService';

export const useTopicDetail = (id) => {
    const [detail, setDetail] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchDetail = useCallback(async () => {
        if (!id) return;
        setIsLoading(true);
        setError(null);
        try {
            const res = await getTopicDetail(id);
            setDetail(res.data);
        } catch (err) {
            setError('Lỗi khi tải thông tin chi tiết chuyên đề.');
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
