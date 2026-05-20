const API_BASE_URL = '/api/v1/admin/exams';

const unwrapResponse = (json) => {
    if (Array.isArray(json)) return json;
    if (Array.isArray(json?.data)) return json.data;
    if (Array.isArray(json?.Data)) return json.Data;
    if (Array.isArray(json?.result?.data)) return json.result.data;
    if (Array.isArray(json?.result)) return json.result;
    return [];
};

export const getReports = async ({ courseId = '' } = {}) => {
    const endpoint = courseId?.trim()
        ? `${API_BASE_URL}/lists/${courseId.trim()}`
        : `${API_BASE_URL}/lists`;

    const response = await fetch(endpoint);
    if (!response.ok) throw new Error('Failed to fetch reports');

    const json = await response.json();
    return { data: unwrapResponse(json) };
};

export const gradeReport = async ({ id, courseId, studentId, duration }) => {
    const response = await fetch(`${API_BASE_URL}/update-score`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            maID: id,
            khoahocID: courseId,
            hocvienID: studentId,
            thoigianlambai: Number(duration ?? 0),
        }),
    });

    if (!response.ok) throw new Error('Failed to grade report');

    const json = await response.json().catch(() => null);
    return { data: json };
};
