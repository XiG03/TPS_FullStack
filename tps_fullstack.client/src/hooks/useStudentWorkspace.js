import { useCallback, useEffect, useState } from 'react';
import {
    getStudentMe,
    getStudentMeSchedules,
    studentCheckinSchedule
} from '../services/studentService';

export const useStudentWorkspace = () => {
    const [student, setStudent] = useState(null);
    const [schedules, setSchedules] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [successMessage, setSuccessMessage] = useState(null);
    const [actionId, setActionId] = useState(null);

    const fetchWorkspace = useCallback(async () => {
        setIsLoading(true);
        setError(null);

        try {
            const [studentResponse, schedulesResponse] = await Promise.all([
                getStudentMe(),
                getStudentMeSchedules()
            ]);

            setStudent(studentResponse.data);
            setSchedules(Array.isArray(schedulesResponse.data) ? schedulesResponse.data : []);
        } catch (err) {
            console.error(err);
            setError('Không tải được dữ liệu học viên hiện tại.');
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchWorkspace();
    }, [fetchWorkspace]);

    useEffect(() => {
        if (!successMessage) return undefined;

        const timeoutId = window.setTimeout(() => {
            setSuccessMessage(null);
        }, 4500);

        return () => window.clearTimeout(timeoutId);
    }, [successMessage]);

    const checkin = async (scheduleId) => {
        setActionId(scheduleId);
        setError(null);
        setSuccessMessage(null);

        try {
            await studentCheckinSchedule(scheduleId);
            await fetchWorkspace();
            setSuccessMessage('\u0110i\u1ec3m danh th\u00e0nh c\u00f4ng. L\u1ecbch h\u1ecdc \u0111\u00e3 \u0111\u01b0\u1ee3c c\u1eadp nh\u1eadt.');
            return true;
        } catch (err) {
            console.error(err);
            setError('Check-in không thành công. Vui lòng thử lại.');
            return false;
        } finally {
            setActionId(null);
        }
    };

    return {
        student,
        schedules,
        isLoading,
        error,
        successMessage,
        actionId,
        refetch: fetchWorkspace,
        checkin
    };
};
