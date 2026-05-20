import { useState, useEffect, useCallback } from 'react';
import { getTopics, deleteTopic } from '../services/topicService';

export const useTopic = () => {
    const [topics, setTopics] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchTopics = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getTopics();
            setTopics(res.data);
        } catch (err) {
            setError('Lỗi khi tải danh sách chuyên đề.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchTopics();
    }, [fetchTopics]);

    const handleDeleteTopic = async (id) => {
        try {
            await deleteTopic(id);
            setTopics(prev => prev.filter(t => t.maID !== id));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        topics,
        isLoading,
        error,
        fetchTopics,
        handleDeleteTopic
    };
};
