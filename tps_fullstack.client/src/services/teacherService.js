import { getHeaders } from './httpClient';

const API_BASE_URL = '/api/admin/teacher';

export const getTeachers = async () => {
    const response = await fetch(`${API_BASE_URL}/teachers`, {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch teachers');
    const data = await response.json();
    return { data };
};

export const getTeacherDetail = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to fetch teacher detail');
    const data = await response.json();
    return { data };
};

export const createTeacher = async (teacherData) => {
    const response = await fetch(`${API_BASE_URL}/create`, {
        method: 'POST',
        headers: getHeaders(true),
        body: JSON.stringify(teacherData)
    });
    if (!response.ok) throw new Error('Failed to create teacher');
    return { success: true };
};

export const updateTeacher = async (id, teacherData) => {
    const response = await fetch(API_BASE_URL, {
        method: 'PUT',
        headers: getHeaders(true),
        body: JSON.stringify({ ...teacherData, maID: id })
    });
    if (!response.ok) throw new Error('Failed to update teacher');
    const data = await response.json();
    return { data };
};

export const deleteTeacher = async (id) => {
    const response = await fetch(`${API_BASE_URL}/${id}`, {
        method: 'DELETE',
        headers: getHeaders()
    });
    if (!response.ok) throw new Error('Failed to delete teacher');
    return { success: true };
};
