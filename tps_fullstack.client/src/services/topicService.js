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
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to create topic'));
    return { data: unwrapServiceResponse(data), message: data?.message };
};

export const updateTopic = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
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
