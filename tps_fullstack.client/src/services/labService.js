import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/v1/lab';

const readJson = async (response) => {
    const text = await response.text();
    return text ? JSON.parse(text) : null;
};

const getApiMessage = (payload, fallback) => payload?.message || payload?.Message || fallback;

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

export const createLab = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(payload)
    });
    const data = await readJson(response);
    if (!response.ok) throw new Error(getApiMessage(data, 'Failed to create lab'));
    return { data: unwrapServiceResponse(data), message: getApiMessage(data, 'Tạo buổi thực hành thành công') };
};
