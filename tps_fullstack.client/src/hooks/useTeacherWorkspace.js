import { useCallback, useEffect, useState } from 'react';
import {
    getTeacherMe,
    getTeacherMeSchedules,
    teacherCheckinSchedule,
    teacherCheckoutSchedule
} from '../services/teacherService';

export const useTeacherWorkspace = () => {
    const [teacher, setTeacher] = useState(null);
    const [schedules, setSchedules] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const [actionId, setActionId] = useState(null);

    const fetchWorkspace = useCallback(async () => {
        setIsLoading(true);
        setError(null);

        try {
            const [teacherResponse, schedulesResponse] = await Promise.all([
                getTeacherMe(),
                getTeacherMeSchedules()
            ]);

            setTeacher(teacherResponse.data);
            setSchedules(Array.isArray(schedulesResponse.data) ? schedulesResponse.data : []);
        } catch (err) {
            console.error(err);
            setError('Không tải được dữ liệu giảng viên hiện tại.');
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
            await teacherCheckinSchedule(scheduleId);
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

    const checkout = async (scheduleId) => {
        setActionId(scheduleId);
        setError(null);

        try {
            await teacherCheckoutSchedule(scheduleId);
            await fetchWorkspace();
            return true;
        } catch (err) {
            console.error(err);
            setError('Check-out không thành công. Vui lòng thử lại.');
            return false;
        } finally {
            setActionId(null);
        }
    };

    return {
        teacher,
        schedules,
        isLoading,
        error,
        actionId,
        refetch: fetchWorkspace,
        checkin,
        checkout
    };
};
