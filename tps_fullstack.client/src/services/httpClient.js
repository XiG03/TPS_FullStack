const TOKEN_KEY = 'auth_token';

export const getToken = () => localStorage.getItem(TOKEN_KEY);

export const setToken = (token) => localStorage.setItem(TOKEN_KEY, token);

export const removeToken = () => localStorage.removeItem(TOKEN_KEY);

/**
 * Trả về headers cơ bản kèm Authorization nếu có token.
 * @param {boolean} hasBody - true nếu request có body JSON (thêm Content-Type)
 */
export const getHeaders = (hasBody = false) => {
    const token = getToken();
    const headers = {};

    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    if (hasBody) {
        headers['Content-Type'] = 'application/json';
    }

    return headers;
};
