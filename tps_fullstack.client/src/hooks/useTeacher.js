import { useState, useEffect, useCallback } from 'react';
import { getTeachers, createTeacher, updateTeacher, deleteTeacher } from '../services/teacherService';

export const useTeacher = () => {
    const [teachers, setTeachers] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchTeachers = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getTeachers();
            setTeachers(res.data);
        } catch (err) {
            setError('Lỗi khi tải danh sách giáo viên.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchTeachers();
    }, [fetchTeachers]);

    const handleAddTeacher = async (data) => {
        try {
            const res = await createTeacher(data);
            setTeachers(prev => [...prev, res.data]);
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleUpdateTeacher = async (id, data) => {
        try {
            const res = await updateTeacher(id, data);
            setTeachers(prev => prev.map(t => t.MaID === id ? { ...t, ...res.data } : t));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleDeleteTeacher = async (id) => {
        try {
            await deleteTeacher(id);
            setTeachers(prev => prev.filter(t => t.MaID !== id));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        teachers,
        isLoading,
        error,
        fetchTeachers,
        handleAddTeacher,
        handleUpdateTeacher,
        handleDeleteTeacher
    };
};
