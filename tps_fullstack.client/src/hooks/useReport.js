import { useCallback, useEffect, useState } from 'react';
import { getExamDetail, getExams } from '../services/examService';

export const useReport = () => {
    const [reports, setReports] = useState([]);
    const [selectedReport, setSelectedReport] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [isLoadingDetail, setIsLoadingDetail] = useState(false);
    const [error, setError] = useState(null);
    const [detailError, setDetailError] = useState(null);

    const fetchReports = useCallback(async () => {
        setIsLoading(true);
        setError(null);
        try {
            const res = await getExams();
            setReports(Array.isArray(res.data) ? res.data : []);
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

    const handleViewReport = async (report) => {
        if (!report?.maID) return;

        setIsLoadingDetail(true);
        setDetailError(null);
        setSelectedReport(null);

        try {
            const res = await getExamDetail(report.maID);
            setSelectedReport(res.data ? { ...report, ...res.data, score: res.data.score ?? report.score } : report);
        } catch (err) {
            setDetailError('Không tải được chi tiết bài thu hoạch.');
            console.error(err);
        } finally {
            setIsLoadingDetail(false);
        }
    };

    const closeReportDetail = () => {
        setSelectedReport(null);
        setDetailError(null);
    };

    return {
        reports,
        selectedReport,
        isLoading,
        isLoadingDetail,
        error,
        detailError,
        fetchReports,
        handleViewReport,
        closeReportDetail
    };
};
