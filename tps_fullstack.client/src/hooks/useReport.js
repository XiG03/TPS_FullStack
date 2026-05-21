import { useCallback, useEffect, useState } from 'react';
import { getReports, gradeReport } from '../services/reportService';

export const useReport = () => {
    const [reports, setReports] = useState([]);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);

    const fetchReports = useCallback(async (filters = {}) => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getReports(filters);
            setReports(res.data);
        } catch (err) {
            setError('Lỗi khi tải danh sách bài thu hoạch.');
            console.error(err);
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchReports();
    }, [fetchReports]);

    const handleGradeReport = async ({ id, score, feedback }) => {
        try {
            await gradeReport({ id, score, feedback });
            setReports((prev) =>
                prev.map((report) =>
                    report.id === id
                        ? { ...report, score, feedback, status: 'graded' }
                        : report
                )
            );
            return true;
        } catch (err) {
            console.error(err);
            return false;
        }
    };

    return {
        reports,
        isLoading,
        error,
        fetchReports,
        handleGradeReport,
    };
};
