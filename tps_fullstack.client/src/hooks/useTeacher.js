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
            setTeachers(Array.isArray(res.data) ? res.data : []);
        } catch (err) {
            setError('Lỗi khi tải danh sách giảng viên.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchTeachers();
    }, [fetchTeachers]);

    const handleAddTeacher = async (data) => {
        try {
            await createTeacher(data);
            await fetchTeachers();
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleUpdateTeacher = async (data) => {
        try {
            await updateTeacher(data);
            await fetchTeachers();
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleDeleteTeacher = async (id) => {
        try {
            await deleteTeacher(id);
            setTeachers((prev) => prev.filter((teacher) => teacher.maID !== id));
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
