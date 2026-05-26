import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/v1/teacher';

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

export const getTeachers = async () => {
    const response = await fetch(API_BASE_URL, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch teachers'));
    return { data: unwrapServiceResponse(payload) || [] };
};

export const getTeacherDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch teacher detail'));
    return { data: unwrapServiceResponse(payload) };
};

export const createTeacher = async (teacherData) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(teacherData)
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to create teacher'));
    return { data: unwrapServiceResponse(payload), message: payload?.message };
};

export const updateTeacher = async (teacherData) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(teacherData)
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to update teacher'));
    return { data: unwrapServiceResponse(payload), message: payload?.message };
};

export const deleteTeacher = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to delete teacher'));
    return { data: unwrapServiceResponse(payload), message: payload?.message };
};
