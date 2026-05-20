const API_BASE_URL = '/api/auth';

export const login = async (username, password) => {
    const response = await fetch(`${API_BASE_URL}/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password, stayedSignedin: false })
    });
    if (!response.ok) {
        const errorData = await response.json().catch(() => null);
        throw new Error(errorData?.Message || 'Đăng nhập thất bại');
    }
    const data = await response.json();
    return { data };
};
