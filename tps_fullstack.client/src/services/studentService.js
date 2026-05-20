const API_BASE_URL = '/api/admin/student';

export const getStudents = async () => {
    const response = await fetch(`${API_BASE_URL}/students`);
    if (!response.ok) throw new Error('Failed to fetch students');
    const data = await response.json();
    return { data };
};

export const getStudentDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`);
    if (!response.ok) throw new Error('Failed to fetch student detail');
    const data = await response.json();
    return { data };
};

export const createStudent = async (payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
    });
    if (!response.ok) throw new Error('Failed to create student');
    const data = await response.json();
    return { data };
};

export const updateStudent = async (id, payload) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ ...payload, maID: id })
    });
    if (!response.ok) throw new Error('Failed to update student');
    const data = await response.json();
    return { data };
};

export const deleteStudent = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE'
    });
    if (!response.ok) throw new Error('Failed to delete student');
    return { success: true };
};
