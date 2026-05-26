import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/v1/schedule';

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

export const getSchedules = async () => {
    const response = await fetch(API_BASE_URL, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch schedules'));
    return { data: unwrapServiceResponse(payload) || [] };
};

export const getScheduleDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        headers: getHeaders()
    });
    const payload = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(payload, 'Failed to fetch schedule detail'));
    return { data: unwrapServiceResponse(payload) };
};

export const createSchedule = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to create schedule'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const updateSchedule = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to update schedule'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const deleteSchedule = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to delete schedule'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};
