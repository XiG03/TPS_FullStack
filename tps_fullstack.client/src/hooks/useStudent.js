import { useCallback, useEffect, useState } from 'react';
import { createStudent, deleteStudent, getStudents, updateStudent } from '../services/studentService';

export const useStudent = () => {
    const [students, setStudents] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchStudents = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getStudents();
            setStudents(Array.isArray(res.data) ? res.data : []);
        } catch (err) {
            setError('Lỗi khi tải danh sách học viên.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchStudents();
    }, [fetchStudents]);

    const handleAddStudent = async (data) => {
        try {
            await createStudent(data);
            await fetchStudents();
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleUpdateStudent = async (id, data) => {
        try {
            await updateStudent(id, data);
            await fetchStudents();
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    const handleDeleteStudent = async (id) => {
        try {
            await deleteStudent(id);
            setStudents((prev) => prev.filter((student) => student.hocvienID !== id));
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
