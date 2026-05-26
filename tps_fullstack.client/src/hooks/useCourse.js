import { useState, useEffect, useCallback } from 'react';
import { getCourses, deleteCourse } from '../services/courseService';

export const useCourse = () => {
    const [courses, setCourses] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchCourses = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getCourses();
            setCourses(Array.isArray(res.data) ? res.data : []);
        } catch (err) {
            setError('Lỗi khi tải danh sách khóa học.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
        fetchCourses();
    }, [fetchCourses]);

    const handleDeleteCourse = async (id) => {
        try {
            await deleteCourse(id);
            setCourses((prev) => prev.filter((course) => course.khoahocID !== id));
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        courses,
        isLoading,
        error,
        fetchCourses,
        handleDeleteCourse
    };
};
