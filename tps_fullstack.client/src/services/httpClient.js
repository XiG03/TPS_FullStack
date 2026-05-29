const TOKEN_KEY = 'auth_token';
const REFRESH_TOKEN_KEY = 'refresh_token';

export const getToken = () => localStorage.getItem(TOKEN_KEY) || sessionStorage.getItem(TOKEN_KEY);

export const setToken = (token, rememberMe = true) => {
    const storage = rememberMe ? localStorage : sessionStorage;
    const otherStorage = rememberMe ? sessionStorage : localStorage;

    storage.setItem(TOKEN_KEY, token);
    otherStorage.removeItem(TOKEN_KEY);
};

export const setRefreshToken = (token, rememberMe = true) => {
    const storage = rememberMe ? localStorage : sessionStorage;
    const otherStorage = rememberMe ? sessionStorage : localStorage;

    storage.setItem(REFRESH_TOKEN_KEY, token);
    otherStorage.removeItem(REFRESH_TOKEN_KEY);
};

export const removeToken = () => {
    localStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    sessionStorage.removeItem(REFRESH_TOKEN_KEY);
};

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
