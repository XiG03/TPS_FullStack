import { useCallback, useEffect, useState } from 'react';
import {
    getStudentMe,
    getStudentMeSchedules,
    studentCheckinSchedule
} from '../services/studentService';
import {
    getStudentExamDetail,
    getStudentExamResult,
    getStudentExams,
    submitStudentExam
} from '../services/examService';

export const useStudentWorkspace = () => {
    const [student, setStudent] = useState(null);
    const [schedules, setSchedules] = useState([]);
    const [exams, setExams] = useState([]);
    const [selectedExam, setSelectedExam] = useState(null);
    const [isLoading, setIsLoading] = useState(false);
    const [isExamLoading, setIsExamLoading] = useState(false);
    const [error, setError] = useState(null);
    const [successMessage, setSuccessMessage] = useState(null);
    const [actionId, setActionId] = useState(null);
    const [examActionId, setExamActionId] = useState(null);

    const fetchWorkspace = useCallback(async () => {
        setIsLoading(true);
        setError(null);

        try {
            const [studentResponse, schedulesResponse] = await Promise.all([
                getStudentMe(),
                getStudentMeSchedules()
            ]);

            const currentStudent = studentResponse.data;
            const studentId = currentStudent?.hocvienID || currentStudent?.maID;

            setStudent(currentStudent);
            setSchedules(Array.isArray(schedulesResponse.data) ? schedulesResponse.data : []);

            if (studentId) {
                const examsResponse = await getStudentExams(studentId);
                setExams(Array.isArray(examsResponse.data) ? examsResponse.data : []);
            } else {
                setExams([]);
            }
        } catch (err) {
            console.error(err);
            setError('Không tải được dữ liệu học viên hiện tại.');
        } finally {
            setIsLoading(false);
        }
    }, []);

    useEffect(() => {
        // eslint-disable-next-line react-hooks/set-state-in-effect
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
            setSuccessMessage('Điểm danh thành công. Lịch học đã được cập nhật.');
            return true;
        } catch (err) {
            console.error(err);
            setError('Check-in không thành công. Vui lòng thử lại.');
            return false;
        } finally {
            setActionId(null);
        }
    };

    const openExam = async (examId, options = {}) => {
        const studentId = student?.hocvienID || student?.maID;
        if (!studentId || !examId) return false;

        setIsExamLoading(true);
        setExamActionId(examId);
        setError(null);
        setSuccessMessage(null);

        try {
            let response;

            if (options.viewResult) {
                try {
                    response = await getStudentExamResult({ studentId, examId });
                } catch (resultErr) {
                    console.error(resultErr);
                    response = await getStudentExamDetail({ studentId, examId });
                    setError('Không tải được kết quả bài thu hoạch. Đang hiển thị dữ liệu bài làm thay thế.');
                }
            } else {
                response = await getStudentExamDetail({ studentId, examId });
            }

            setSelectedExam(response.data);
            return true;
        } catch (err) {
            console.error(err);
            setError(options.viewResult
                ? 'Không tải được kết quả bài thu hoạch. Vui lòng thử lại.'
                : 'Không tải được chi tiết bài thu hoạch. Vui lòng thử lại.');
            return false;
        } finally {
            setIsExamLoading(false);
            setExamActionId(null);
        }
    };

    const closeExam = () => {
        setSelectedExam(null);
    };

    const submitExam = async (examDetail, selectedAnswers) => {
        const studentId = examDetail?.hocvienID || student?.hocvienID || student?.maID;
        const examId = examDetail?.maID;

        if (!studentId || !examId) return false;

        setExamActionId(examId);
        setError(null);
        setSuccessMessage(null);

        const payload = {
            maID: examDetail.maID,
            khoahocID: examDetail.courseId,
            tenKhoahoc: examDetail.courseName,
            hocvienID: studentId,
            tenHocvien: examDetail.studentName,
            batdauthi: examDetail.startedAt || new Date().toISOString(),
            ketthucthi: new Date().toISOString(),
            diem: examDetail.score ?? null,
            answerSubmits: (examDetail.questions || []).flatMap((question) => (
                (question.answers || []).map((answer) => ({
                    maID: answer.maID,
                    baithuhoachID: answer.examId || examDetail.maID,
                    baithuhoach_CauhoiID: answer.questionId || question.maID,
                    ndTraloi: answer.answerText,
                    dung: answer.isCorrect ?? null,
                    chon: selectedAnswers?.[question.maID] === answer.maID
                }))
            ))
        };

        try {
            const result = await submitStudentExam({ studentId, examId, payload });
            await fetchWorkspace();

            try {
                const detailResponse = await getStudentExamResult({ studentId, examId });
                setSelectedExam(detailResponse.data);
                setSuccessMessage(result.message || 'Nộp bài thu hoạch thành công.');
            } catch (resultErr) {
                console.error(resultErr);
                const fallbackDetail = await getStudentExamDetail({ studentId, examId });
                setSelectedExam(fallbackDetail.data);
                setSuccessMessage('Nộp bài thu hoạch thành công, nhưng chưa tải được màn hình kết quả.');
            }

            return true;
        } catch (err) {
            console.error(err);
            setError('Nộp bài thu hoạch không thành công. Vui lòng thử lại.');
            return false;
        } finally {
            setExamActionId(null);
        }
    };

    return {
        student,
        schedules,
        exams,
        selectedExam,
        isLoading,
        isExamLoading,
        error,
        successMessage,
        actionId,
        examActionId,
        refetch: fetchWorkspace,
        checkin,
        openExam,
        closeExam,
        submitExam
    };
};
