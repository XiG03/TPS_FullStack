const API_BASE_URL = '/api/auth';

const readJson = async (response) => {
    try {
        return await response.json();
    } catch {
        return null;
    }
};

const getApiMessage = (payload, fallback) => payload?.message || payload?.Message || fallback;

const unwrapServiceResponse = (payload) => {
    if (
        payload &&
        typeof payload === 'object' &&
        Object.prototype.hasOwnProperty.call(payload, 'statusCode') &&
        (Object.prototype.hasOwnProperty.call(payload, 'data') || Object.prototype.hasOwnProperty.call(payload, 'Data'))
    ) {
        return {
            data: payload.data ?? payload.Data ?? null,
            message: getApiMessage(payload, ''),
            statusCode: payload.statusCode,
        };
    }

    return {
        data: payload,
        message: '',
        statusCode: null,
    };
};

export const login = async (username, password, stayedSignedin = false) => {
    const response = await fetch(`${API_BASE_URL}/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password, stayedSignedin })
    });

    const payload = await readJson(response);
    const result = unwrapServiceResponse(payload);

    if (!response.ok) {
        throw new Error(result.message || 'Đăng nhập thất bại');
    }

    return result;
};
