import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/v1/topic';

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

const appendValue = (formData, key, value) => {
    if (value === undefined || value === null) return;
    formData.append(key, value);
};

const toTopicFormData = (payload) => {
    const formData = new FormData();

    appendValue(formData, 'ChuyendeID', payload.chuyendeID || null);
    appendValue(formData, 'Ten', payload.ten || '');
    appendValue(formData, 'Mota', payload.mota || '');

    (payload.documents || []).forEach((document, index) => {
        appendValue(formData, `Documents[${index}].TailieuID`, document.tailieuID || null);
        appendValue(formData, `Documents[${index}].Tieude`, document.tieude || '');
        appendValue(formData, `Documents[${index}].Ngaytao`, document.ngaytao || new Date().toISOString());
        appendValue(formData, `Documents[${index}].Loaitailieu`, document.loaitailieu || 'Khác');
        appendValue(formData, `Documents[${index}].Kichthuoc`, String(document.kichthuoc || 0));
        if (document.file instanceof File) {
            formData.append(`Documents[${index}].File`, document.file);
        }
    });

    (payload.questions || []).forEach((question, questionIndex) => {
        appendValue(formData, `Questions[${questionIndex}].CauhoiID`, question.cauhoiID || null);
        appendValue(formData, `Questions[${questionIndex}].Ten`, question.ten || '');

        (question.answers || []).forEach((answer, answerIndex) => {
            appendValue(formData, `Questions[${questionIndex}].Answers[${answerIndex}].DapanID`, answer.dapanID || null);
            appendValue(formData, `Questions[${questionIndex}].Answers[${answerIndex}].Ten`, answer.ten || '');
            appendValue(formData, `Questions[${questionIndex}].Answers[${answerIndex}].Dung`, String(Boolean(answer.dung)));
        });
    });

    return formData;
};

const getFileNameFromDisposition = (contentDisposition, fallback) => {
    if (!contentDisposition) return fallback;

    const utf8Match = contentDisposition.match(/filename\*=UTF-8''([^;]+)/i);
    if (utf8Match?.[1]) return decodeURIComponent(utf8Match[1]);

    const match = contentDisposition.match(/filename="?([^"]+)"?/i);
    return match?.[1] || fallback;
};

export const getTopics = async () => {
    const response = await fetch(`${API_BASE_URL}`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch topics'));
    return { data: unwrapServiceResponse(payload) || [] };
};

export const getTopicDetail = async (MaID) => {
    const response = await fetch(`${API_BASE_URL}/${MaID}`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch topic detail'));
    return { data: unwrapServiceResponse(payload) };
};

export const createTopic = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(),
        body: toTopicFormData(payload)
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to create topic'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const updateTopic = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(),
        body: toTopicFormData(payload)
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to update topic'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const deleteTopic = async (MaID) => {
    const response = await fetch(`${API_BASE_URL}/${MaID}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to delete topic'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const downloadTopicDocument = async (document) => {
    const response = await fetch(document.downloadUrl || `${API_BASE_URL}/document/${document.tailieuID}/download`, {
        headers: getHeaders()
    });

    if (!response.ok) {
        const payload = await readJson(response).catch(() => null);
        throw new Error(getApiMessage(payload, 'Failed to download document'));
    }

    const blob = await response.blob();
    const fileName = getFileNameFromDisposition(response.headers.get('content-disposition'), document.tieude || 'document');
    const url = window.URL.createObjectURL(blob);
    const link = window.document.createElement('a');
    link.href = url;
    link.download = fileName;
    window.document.body.appendChild(link);
    link.click();
    link.remove();
    window.URL.revokeObjectURL(url);
};
