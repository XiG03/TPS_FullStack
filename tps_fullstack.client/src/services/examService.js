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

    return payload;
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
        message: getApiMessage(payload, 'Tao bai thu hoach thanh cong.')
    };
};
