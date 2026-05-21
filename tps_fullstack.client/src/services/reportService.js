const API_BASE_URL = '/api/admin/report';

const unwrapResponse = (json) => {
    if (Array.isArray(json)) return json;
    if (Array.isArray(json?.data)) return json.data;
    if (Array.isArray(json?.result?.data)) return json.result.data;
    if (Array.isArray(json?.result)) return json.result;
    return [];
};

export const getReports = async ({ keyword = '', status = 'all' } = {}) => {
    const query = new URLSearchParams();
    if (keyword.trim()) query.append('keyword', keyword.trim());
    if (status && status !== 'all') query.append('status', status);

    const response = await fetch(`${API_BASE_URL}?${query.toString()}`);
    if (!response.ok) throw new Error('Failed to fetch reports');

    const json = await response.json();
    return { data: unwrapResponse(json) };
};

export const gradeReport = async ({ id, score, feedback }) => {
    const response = await fetch(`${API_BASE_URL}/${id}/grade`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ score, feedback }),
    });

    if (!response.ok) throw new Error('Failed to grade report');

    const json = await response.json().catch(() => null);
    return { data: json };
};
