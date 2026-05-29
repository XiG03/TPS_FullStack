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

    const checkin = async (scheduleId) => {
        setActionId(scheduleId);
        setError(null);

        try {
            await studentCheckinSchedule(scheduleId);
            await fetchWorkspace();
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
        actionId,
        refetch: fetchWorkspace,
        checkin
    };
};
