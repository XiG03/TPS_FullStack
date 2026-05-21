import { useState, useEffect, useCallback } from 'react';
import { getStudents, createStudent, updateStudent, deleteStudent } from '../services/studentService';

export const useStudent = () => {
    const [students, setStudents] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchStudents = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getStudents();
            setStudents(res.data);
        } catch (err) {
            setError('Lỗi khi tải danh sách học viên.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchStudents();
    }, [fetchStudents]);

    const handleAddStudent = async (data) => {
        try {
            const res = await createStudent(data);
            setStudents(prev => [...prev, res.data]);
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleUpdateStudent = async (id, data) => {
        try {
            const res = await updateStudent(id, data);
            setStudents(prev => prev.map(s => s.maID === id ? { ...s, ...res.data } : s));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleDeleteStudent = async (id) => {
        try {
            await deleteStudent(id);
            setStudents(prev => prev.filter(s => s.maID !== id));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        students,
        isLoading,
        error,
        fetchStudents,
        handleAddStudent,
        handleUpdateStudent,
        handleDeleteStudent
    };
};
