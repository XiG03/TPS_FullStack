import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/v1/exams';

const readJson = async (response) => {
    const text = await response.text();
    return text ? JSON.parse(text) : null;
};

const getApiMessage = (payload, fallback) => {
    return payload?.message || payload?.Message || fallback;
};

const unwrapServiceResponse = (payload) => {
    if (
        payload &&
        typeof payload === 'object' &&
        Object.prototype.hasOwnProperty.call(payload, 'data') &&
        Object.prototype.hasOwnProperty.call(payload, 'statusCode')
    ) {
        return payload.data;
    }

    if (
        payload &&
        typeof payload === 'object' &&
        Object.prototype.hasOwnProperty.call(payload, 'Data') &&
        Object.prototype.hasOwnProperty.call(payload, 'statusCode')
    ) {
        return payload.Data;
    }

    return payload;
};

const getValue = (source, ...keys) => {
    for (const key of keys) {
        if (source?.[key] !== undefined && source?.[key] !== null) return source[key];
    }
    return undefined;
};

const getExamStatus = (exam) => {
    const startedAt = getValue(exam, 'batdauthi', 'Batdauthi');
    const endedAt = getValue(exam, 'ketthucthi', 'Ketthucthi');

    if (endedAt) return 'submitted';
    if (startedAt) return 'in_progress';
    return 'pending';
};

const normalizeAnswer = (answer) => ({
    maID: getValue(answer, 'maID', 'MaID'),
    answerText: getValue(answer, 'ndTraloi', 'NdTraloi'),
    isCorrect: getValue(answer, 'dung', 'Dung'),
    isSelected: getValue(answer, 'chon', 'Chon')
});

const normalizeQuestion = (question) => ({
    maID: getValue(question, 'maID', 'MaID'),
    questionText: getValue(question, 'tencauhoi', 'Tencauhoi'),
    isCorrect: getValue(question, 'dung', 'Dung'),
    answers: (getValue(question, 'examAnswerDetails', 'ExamAnswerDetails') || []).map(normalizeAnswer)
});

export const normalizeExam = (exam) => ({
    maID: getValue(exam, 'maID', 'MaID'),
    courseName: getValue(exam, 'tenKhoahoc', 'TenKhoahoc') || 'Khóa học chưa có tên',
    studentName: getValue(exam, 'tenHocvien', 'TenHocvien') || 'Học viên chưa có tên',
    startedAt: getValue(exam, 'batdauthi', 'Batdauthi'),
    endedAt: getValue(exam, 'ketthucthi', 'Ketthucthi'),
    score: getValue(exam, 'diem', 'Diem'),
    status: getExamStatus(exam)
});

const normalizeExamDetail = (exam) => ({
    ...normalizeExam(exam),
    questions: (getValue(exam, 'examQuestionDetails', 'ExamQuestionDetails') || []).map(normalizeQuestion)
});

export const getExams = async () => {
    const response = await fetch(API_BASE_URL, {
        headers: getHeaders()
    });

    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch exams'));

    const data = unwrapServiceResponse(payload) || [];
    return { data: Array.isArray(data) ? data.map(normalizeExam) : [] };
};

export const getExamDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${encodeURIComponent(id)}`, {
        headers: getHeaders()
    });

    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch exam detail'));

    const data = unwrapServiceResponse(payload);
    return { data: data ? normalizeExamDetail(data) : null };
};

export const createExam = async ({ khoahocID, hocvienID }) => {
    const courseId = encodeURIComponent(khoahocID);
    const studentId = encodeURIComponent(hocvienID);

    const response = await fetch(`${API_BASE_URL}/${courseId}/${studentId}`, {
        method: 'POST',
        headers: getHeaders()
    });

    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to create exam'));

    return {
        data: unwrapServiceResponse(payload),
        message: getApiMessage(payload, 'Tạo bài thu hoạch thành công.')
    };
};
